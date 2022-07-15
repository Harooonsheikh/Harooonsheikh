using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Xml;
using VSI.CommerceLink.EcomDataModel;
using VSI.CommerceLink.EcomDataModel.Request;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.Enums.Enums.TMV;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;
using VSI.EDGEAXConnector.Web.ActionFilters;
using VSI.EDGEAXConnector.Web.Controllers;
using static VSI.EDGEAXConnector.Web.Controllers.ContactPersonController;
using static VSI.EDGEAXConnector.Web.CustomerController;

namespace VSI.EDGEAXConnector.Web
{
    /// <summary>
    /// SalesOrderController defines properties and methods for API controller for Sales order.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class SalesOrderController : ApiBaseController
    {
        const string C_ATT_TMV_MIGRATED_ORDER_NUMBER = "TMVMIGRATEDORDERNUMBER";
        const string C_ATT_TMV_MIGRATED_SALES_LINE_NUMBER = "TMVMIGRATEDSALESLINENUMBER";

        const string C_ATT_TMV_OLD_SALES_ORDER_NUMBER = "TMVOLDSALESORDERNUMBER";
        const string C_ATT_TMV_OLD_SALES_LINE_NUMBER = "TMVOLDSALESLINENUMBER";
        const string C_ATT_TMV_OLD_SALES_LINE_ACTION = "TMVOLDSALESLINEACTION";
        const string C_ATT_TMV_PARENT = "TMVPARENT";
        const string C_ATT_TMV_RESELLER = "TMVRESELLER";

        EmailSender emailSender = null;
        /// <summary>
        /// Sales Order Controller 
        /// </summary>
        public SalesOrderController()
        {
            ControllerName = "SalesOrderController";
        }

        #region API Methods

        /// <summary>
        /// CreateRealtimeSalesOrderTransaction creates sales order transaction with provided details.
        /// </summary>
        /// <param name="salesOrderTransRequest">sakes order transaction request to be created</param>
        /// <returns>SalesOrderTransResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/CreateSalesOrderTransaction")]
        public SalesOrderTransResponse CreateSalesOrderTransaction([FromBody] SalesOrderTransRequest salesOrderTransRequest)
        {
            return CreateSalesOrder(salesOrderTransRequest, MethodBase.GetCurrentMethod().Name);
        }

        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/CreateSalesOrder")]
        public SalesOrderTransResponse CreateSalesOrder([FromBody] EcomSalesOrder ecomSalesOrderRequest)
        {
            ErpSalesOrder salesOrderRequest = _mapper.Map<ErpSalesOrder>(ecomSalesOrderRequest);

            SalesOrderTransResponse salesOrderTransResponse;
            bool result = false;
            string message = string.Empty;
            bool isCheckoutProcessContractOperation = false;

            try
            {
                // if order already processed and file still exists in directory, dont process it again.
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

                if (salesOrderRequest == null)
                {
                    message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                }
                else if (integrationManager.GetErpKey(EDGEAXConnector.Enums.Entities.SaleOrder, salesOrderRequest.ChannelReferenceId) != null)
                {
                    message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40512, currentStore, salesOrderRequest.ChannelReferenceId);

                }
                else if (salesOrderRequest.CustomAttributes == null)
                {
                    message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40531, currentStore, salesOrderRequest.ChannelReferenceId);
                }
                else if (salesOrderRequest.SalesLines == null)
                {
                    message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40532, currentStore, salesOrderRequest.ChannelReferenceId);
                }
                else if (salesOrderRequest.TenderLines == null)
                {
                    message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40533, currentStore, salesOrderRequest.ChannelReferenceId);
                }

                if (!string.IsNullOrWhiteSpace(message))
                {
                    salesOrderTransResponse = new SalesOrderTransResponse(false, "", message);
                    return salesOrderTransResponse;
                }

                initializeAndAssignDefaultParameters(salesOrderRequest);

                if (salesOrderRequest.SalesLines.Any(sl => !string.IsNullOrWhiteSpace(sl.SalesLineAction)))
                {
                    isCheckoutProcessContractOperation = true;
                }

                salesOrderTransResponse = this.TMV_ValidateSalesOrderRequest(salesOrderRequest, isCheckoutProcessContractOperation);

                if (!salesOrderTransResponse.status)
                {
                    return salesOrderTransResponse;
                }

                // Check if its ProcessContractOperation request
                var processContractOperationResponse = CheckoutProcessContractOperation(salesOrderRequest);
                if (processContractOperationResponse != null)
                {
                    return processContractOperationResponse;
                }

                // EDI - Map Extension properties Indirect Customer and Reseller
                //this.MapExtensionProperties(salesOrderPrams, salesOrderTransRequest);

                #region Price Validation            
                if (salesOrderRequest.CustomAttributes != null)
                {
                    SalesOrderTransResponse response = ValidatePrices(salesOrderRequest);
                    if (!response.status)
                    {
                        return response;
                    }
                }
                #endregion

                var erpSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
                result = erpSalesOrderController.CreateRealtimeSaleOrderTransaction(salesOrderRequest, GetRequestGUID(Request));

                if (result)
                {
                    string soSuccessMessage = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40509);
                    salesOrderTransResponse = new SalesOrderTransResponse(true, salesOrderRequest.Id, soSuccessMessage);
                }
                else
                {
                    string soFailureMessage = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40508);
                    salesOrderTransResponse = new SalesOrderTransResponse(false, salesOrderRequest.Id, soFailureMessage);
                }

            }
            catch (Exception ex)
            {
                message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                salesOrderTransResponse = new SalesOrderTransResponse(false, null, message);
                return salesOrderTransResponse;
            }

            return salesOrderTransResponse;
        }

        private SalesOrderTransResponse CheckoutProcessContractOperation(ErpSalesOrder salesOrderRequest)
        {
            if (salesOrderRequest == null)
            {
                var message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new SalesOrderTransResponse(false, string.Empty, message);
            }

            if (salesOrderRequest.SalesLines != null && salesOrderRequest.SalesLines.Count > 0)
            {
                if (salesOrderRequest.SalesLines.Any(sl => !string.IsNullOrWhiteSpace(sl.SalesLineAction)))
                {
                    return PerformProcessContractOperation(salesOrderRequest);
                }
            }
            return null;
        }

        private SalesOrderTransResponse PerformProcessContractOperation(ErpSalesOrder salesOrderRequest)
        {
            var isValidateAddress = ValidateCheckoutProcessContractOperationAddress(salesOrderRequest.BillingAddress);
            if (isValidateAddress != null)
            {
                return isValidateAddress;
            }

            #region Fill ProcessContractOperationRequest Model
            var processContractOperationRequest = new ProcessContractOperationRequest
            {
                ContractLines = new List<ContractLine>(),
                //PrimaryPacLicense = salesOrderRequest.CustomAttributes.FirstOrDefault(x => x.Key == ErpSalesOrderExtensionProperties.TMVPrimaryPacLicense.ToString()).Value,
                //UseOldContractDates = salesOrderRequest.CustomAttributes.FirstOrDefault(x => x.Key == ErpSalesOrderExtensionProperties.TMVUseOldContractDates.ToString()).Value,
                CurrencyCode = salesOrderRequest.CurrencyCode,
                SalesOrigin = salesOrderRequest.CustomAttributes.FirstOrDefault(x => x.Key == ErpSalesOrderExtensionProperties.TMVSALESORIGIN.ToString()).Value,
                RequestNumber = salesOrderRequest.ChannelReferenceId,
                RequestDate = salesOrderRequest.OrderPlacedDate.ToString("yyyy-MM-dd"),
                CustomerInformation = new ERPDataModels.Custom.CustomerInformation()
                {
                    CustomerEmail = salesOrderRequest.CustomerEmail,
                    Phone = salesOrderRequest.BillingAddress?.Phone,
                    VATNumber = salesOrderRequest.CustomAttributes.FirstOrDefault(x => x.Key == ErpSalesOrderExtensionProperties.TMVVatNumber.ToString()).Value,
                    BillingAddress = salesOrderRequest.BillingAddress
                }
            };

            foreach (var saleline in salesOrderRequest.SalesLines)
            {
                processContractOperationRequest.ContractLines.Add(new ContractLine()
                {
                    ProductId = saleline.ItemId,
                    TargetPrice = saleline.TargetPrice.ToString(),
                    LineNumber = saleline.LineNumber.ToString(),
                    OldLinePacLicense = saleline.OldLinePacLicense,
                    SalesLineAction = saleline.SalesLineAction,
                    Quantity = saleline.Quantity.ToString(),
                    ParentLineNumber = saleline.CustomAttributes.FirstOrDefault(x => x.Key == ErpSalesLineExtensionProperties.TMVPARENT.ToString()).Value
                });
            }
            #endregion

            var response = ProcessContractOperation(processContractOperationRequest, (List<ErpTenderLine>)salesOrderRequest.TenderLines, true);
            return new SalesOrderTransResponse(response.status, processContractOperationRequest.RequestNumber, response.message, null, response.result?.ToString());
        }

        private SalesOrderTransResponse CreateSalesOrder(SalesOrderTransRequest salesOrderTransRequest, string methodName)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, this.currentStore, MethodBase.GetCurrentMethod().Name);
            SalesOrderTransResponse salesOrderTransResponse;

            if (salesOrderTransRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                salesOrderTransResponse = new SalesOrderTransResponse(false, null, message);
                return salesOrderTransResponse;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "salesOrderTransRequest: {0}", JsonConvert.SerializeObject(salesOrderTransRequest));

            // Extract the data from parameter
            string salesOrderJSON = salesOrderTransRequest.salesOrderJSON;

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10501, this.currentStore, salesOrderJSON);

            string soFailMessage = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40508);
            salesOrderTransResponse = new SalesOrderTransResponse(false, "", soFailMessage);

            try
            {
                ErpSalesOrder salesOrderPrams = new ErpSalesOrder();
                //TODO: Add parametters valiations if required
                try
                {
                    bool result = false;

                    if (salesOrderJSON != "")
                    {
                        salesOrderPrams = ConvertSalesOrderJsonToXML(salesOrderJSON);

                        var erpSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);

                        AddPaymentCustomProperties(salesOrderPrams);
                        SalesOrderTransResponse salesOrderRequestValidation = this.TMV_ValidateSalesOrderRequest(salesOrderPrams);

                        if (!salesOrderRequestValidation.status)
                        {
                            return salesOrderRequestValidation;
                        }


                        IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                        // if order already processed and file still exists in directory, dont process it again.
                        if (integrationManager.GetErpKey(EDGEAXConnector.Enums.Entities.SaleOrder, salesOrderPrams.ChannelReferenceId) != null)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40512, currentStore, salesOrderPrams.ChannelReferenceId);
                            salesOrderTransResponse = new SalesOrderTransResponse(false, "", message);
                            return salesOrderTransResponse;
                        }

                        // EDI - Map Extension properties Indirect Customer and Reseller
                        this.MapExtensionProperties(salesOrderPrams, salesOrderTransRequest);

                        #region Price Validation            
                        if (salesOrderPrams.CustomAttributes != null)
                        {
                            SalesOrderTransResponse response = ValidatePrices(salesOrderPrams);
                            if (!response.status)
                            {
                                return response;
                            }
                        }
                        #endregion

                        result = erpSalesOrderController.CreateRealtimeSaleOrderTransaction(salesOrderPrams, GetRequestGUID(Request));

                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, result.ToString());

                        if (result)
                        {
                            string soSuccessMessage = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40509);
                            salesOrderTransResponse = new SalesOrderTransResponse(true, salesOrderPrams.Id, soSuccessMessage);
                        }
                        else
                        {
                            string soFailureMessage = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40512);
                            salesOrderTransResponse = new SalesOrderTransResponse(false, salesOrderPrams.Id, soFailureMessage);
                        }
                    }
                    else
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "salesOrderJSON");
                        salesOrderTransResponse = new SalesOrderTransResponse(false, null, message);
                        return salesOrderTransResponse;
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), GetRequestGUID(Request));
                    throw new Exception(message);
                }

            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "salesOrderJSON");
                salesOrderTransResponse = new SalesOrderTransResponse(false, null, message);
                return salesOrderTransResponse;
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                salesOrderTransResponse = new SalesOrderTransResponse(false, null, message);
                return salesOrderTransResponse;
            }

            return salesOrderTransResponse;
        }

        /// <summary>
        /// CreateRealtimeSalesOrderTransaction creates sales order transaction with provided details.
        /// </summary>
        /// <param name="salesOrderTransRequest">sakes order transaction request to be created</param>
        /// <returns>SalesOrderTransResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/CreateMergeSalesOrderTransaction")]
        public ErpMergeSalesOrderResponse CreateMergeSalesOrderTransaction([FromBody] MergeSalesOrderTransRequest salesOrderTransRequest)
        {
            ErpMergeSalesOrderResponse salesOrderTransResponse = new ErpMergeSalesOrderResponse(false, string.Empty, null, null, null);

            try
            {
                bool createSalesOrderFlag = true;
                if (salesOrderTransRequest == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    salesOrderTransResponse = new ErpMergeSalesOrderResponse(false, message, null, null, null);
                    return salesOrderTransResponse;
                }
                else
                {
                    salesOrderTransResponse = ValidateMergeSalesOrder(salesOrderTransRequest);
                    if (salesOrderTransResponse.Success == false)
                    {
                        return salesOrderTransResponse;
                    }
                }
                CustomerResponse customerResponse = new CustomerResponse(false, null, string.Empty);
                ContactPersonResponse contactPersonResponse = new ContactPersonResponse(false, null, null);
                SalesOrderTransResponse salesOrderResponse = new SalesOrderTransResponse(false, null, null);
                ErpCustomer erpCustomer = new ErpCustomer();

                //Creating customer
                if (salesOrderTransRequest.CustomerInfo != null)
                {
                    CustomerController customerCon = new CustomerController();
                    LanguageCodesDAL languageCodes = new LanguageCodesDAL(currentStore.StoreKey);

                    // Swap Language for ERP
                    if (salesOrderTransRequest.CustomerInfo != null &&
                        salesOrderTransRequest.CustomerInfo.Customer != null &&
                        !string.IsNullOrEmpty(salesOrderTransRequest.CustomerInfo.Customer.Language) &&
                        salesOrderTransRequest.SwapLanguage)
                    {
                        salesOrderTransRequest.CustomerInfo.Customer.Language =
                            languageCodes.GetErpLanguageCode(salesOrderTransRequest.CustomerInfo.Customer.Language);
                    }

                    customerResponse = customerCon.CreateCustomerMethod(salesOrderTransRequest.CustomerInfo, MethodBase.GetCurrentMethod().Name);

                    // Swap Language for ECom
                    if (customerResponse != null &&
                        customerResponse.CustomerInfo != null)
                    {
                        ErpCustomer customer = (ErpCustomer)customerResponse.CustomerInfo;
                        customer.SwapLanguage = salesOrderTransRequest.SwapLanguage;
                        if (!string.IsNullOrEmpty(customer.Language) &&
                            salesOrderTransRequest.SwapLanguage)
                        {
                            customer.Language = languageCodes.GetEcomLanguageCode(customer.Language);
                            customerResponse.CustomerInfo = customer;
                        }
                    }

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(customerResponse));

                    if (customerResponse.Status)
                    {
                        //Creating ContactPerson
                        if (salesOrderTransRequest.ContactPersonInfo != null)
                        {
                            erpCustomer = (ErpCustomer)customerResponse.CustomerInfo;
                            ContactPersonController contactPersonController = new ContactPersonController();
                            salesOrderTransRequest.ContactPersonInfo.CustAccount = erpCustomer.AccountNumber;
                            salesOrderTransRequest.ContactPersonInfo.ContactForParty = erpCustomer.DirectoryPartyRecordId;

                            // Swap Language for ERP
                            if (salesOrderTransRequest.ContactPersonInfo != null &&
                                !string.IsNullOrEmpty(salesOrderTransRequest.ContactPersonInfo.Language) &&
                                salesOrderTransRequest.SwapLanguage)
                            {
                                salesOrderTransRequest.ContactPersonInfo.Language =
                                    languageCodes.GetErpLanguageCode(salesOrderTransRequest.ContactPersonInfo.Language);
                            }

                            contactPersonResponse = contactPersonController.CreateContactPersonMethod(salesOrderTransRequest.ContactPersonInfo, MethodBase.GetCurrentMethod().Name);

                            // Swap Language for ECom
                            if (contactPersonResponse.ContactPerson != null &&
                                !string.IsNullOrEmpty(contactPersonResponse.ContactPerson.Language) &&
                                salesOrderTransRequest.SwapLanguage)
                            {
                                salesOrderTransRequest.ContactPersonInfo.Language =
                                    languageCodes.GetEcomLanguageCode(salesOrderTransRequest.ContactPersonInfo.Language);
                            }

                            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(contactPersonResponse));
                        }

                        if (contactPersonResponse.Status)
                        {
                            createSalesOrderFlag = true;
                        }
                        else
                        {
                            //Case where we should must create sales order
                            if (contactPersonResponse.Status == false && contactPersonResponse.ErrorMessage == "Contact Person already exist for customer!")
                            {
                                createSalesOrderFlag = true;
                            }
                            //Case where we should not create sales order
                            else
                            {
                                createSalesOrderFlag = false;

                                salesOrderTransResponse = new ErpMergeSalesOrderResponse(false, customerResponse.Message, customerResponse.CustomerInfo, null, null);
                                return salesOrderTransResponse;
                            }
                        }
                    }
                    else
                    {
                        salesOrderTransResponse = new ErpMergeSalesOrderResponse(false, customerResponse.Message, null, null, null);
                        return salesOrderTransResponse;
                    }
                }

                //Creating sales order
                if (createSalesOrderFlag)
                {
                    //Create Sales Order
                    salesOrderResponse = CreateSalesOrder(salesOrderTransRequest.SalesOrder, MethodBase.GetCurrentMethod().Name);
                    if (salesOrderResponse.status)
                    {
                        salesOrderTransResponse = new ErpMergeSalesOrderResponse(true, salesOrderResponse.message, customerResponse.CustomerInfo, contactPersonResponse.ContactPerson, salesOrderResponse.salesOrderTransactionId);
                    }
                    else
                    {
                        salesOrderTransResponse = new ErpMergeSalesOrderResponse(false, salesOrderResponse.message, customerResponse.CustomerInfo, contactPersonResponse.ContactPerson, null);
                    }

                    return salesOrderTransResponse;
                }

                return salesOrderTransResponse;
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                salesOrderTransResponse = new ErpMergeSalesOrderResponse(false, message, null, null, null);
                return salesOrderTransResponse;
            }
        }

        private ErpMergeSalesOrderResponse ValidateMergeSalesOrder(MergeSalesOrderTransRequest mergeSalesOrderTransRequest)
        {
            ErpMergeSalesOrderResponse salesOrderTransResponse = null;
            //SalesOrder not exists
            if (string.IsNullOrWhiteSpace(mergeSalesOrderTransRequest.SalesOrder.ToString()) || string.IsNullOrWhiteSpace(mergeSalesOrderTransRequest.SalesOrder.salesOrderJSON))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "SalesOrder");
                salesOrderTransResponse = new ErpMergeSalesOrderResponse(false, message, null, null, null);
                return salesOrderTransResponse;
            }

            //Contact Person not exist
            if (mergeSalesOrderTransRequest.CustomerInfo != null)
            {
                if (mergeSalesOrderTransRequest.ContactPersonInfo == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ContactPersonInfo");
                    salesOrderTransResponse = new ErpMergeSalesOrderResponse(false, message, null, null, null);
                    return salesOrderTransResponse;
                }
            }
            //Customer Not exists
            else if (mergeSalesOrderTransRequest.ContactPersonInfo != null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CustomerInfo");
                salesOrderTransResponse = new ErpMergeSalesOrderResponse(false, message, null, null, null);
                return salesOrderTransResponse;
            }

            return new ErpMergeSalesOrderResponse(true, null, null, null, null);
        }
        /// <summary>
        /// Get Contract Type Sales order by customer and some other optional filters  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/GetContractSalesOrders")]
        public ContractSalesOrderResponse GetContractSalesOrders([FromBody] ContractSalesOrderRequest request)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "GetContractSalesOrders", DateTime.UtcNow);
            ContractSalesOrderResponse contractSalesOrderResponse;
            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                contractSalesOrderResponse = new ContractSalesOrderResponse(false, message);
                return contractSalesOrderResponse;
            }

            if (string.IsNullOrEmpty(request.CustomerAccount) && string.IsNullOrEmpty(request.LicenseNumber) && string.IsNullOrEmpty(request.EcomCustomerId) && string.IsNullOrEmpty(request.ChannelReferenceId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL400012, currentStore, "");
                contractSalesOrderResponse = new ContractSalesOrderResponse(false, message);
                return contractSalesOrderResponse;
            }

            // Extract the data from parameter
            IEnumerable<string> requestStatuses = request.Status;
            // CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " customerAccountNo: " + request.CustomerAccount);
            ErpCustomer customer = new ErpCustomer();
            List<ErpSalesOrder> espSalesOrders = new List<ErpSalesOrder>();
            contractSalesOrderResponse = new ContractSalesOrderResponse(false, "");

            try
            {
                try
                {
                    ContractSalesorderRequest clRequest = new ContractSalesorderRequest();
                    clRequest.CustomerAccount = request.CustomerAccount;
                    clRequest.EcomCustomerId = request.EcomCustomerId;
                    clRequest.OfferType = request.OfferType;
                    clRequest.ChannelReferenceId = request.ChannelReferenceId;
                    clRequest.SalesOrderId = request.SalesOrderId;
                    clRequest.Status = request.Status;
                    clRequest.isActive = request.isActive;
                    clRequest.LicenseNumber = request.LicenseNumber;
                    try
                    {
                        clRequest.SmallResponse = request.SmallResponse;
                    }
                    catch (Exception)
                    {
                        clRequest.SmallResponse = false;
                    }
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL600000, currentStore, GetRequestGUID(Request), "CreateSalesOrderController", DateTime.UtcNow);
                    var erpContactPersonController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL600001, currentStore, GetRequestGUID(Request), "CreateSalesOrderController", DateTime.UtcNow);
                    ERPContractSalesorderResponse clResponse = erpContactPersonController.GetContractSalesOrder(clRequest, GetRequestGUID(Request));

                    if (clResponse.Success)
                    {
                        foreach (var contract in clResponse.Contracts)
                        {
                            List<object> salesOrders = new List<object>();

                            if (request.SmallResponse)
                            {
                                foreach (ErpSalesOrder erpSalesOrder in contract.SalesOrders)
                                {

                                    ErpSalesOrderSmallResponse erpSalesOrderSmallResponse = _mapper.Map<ErpSalesOrderSmallResponse>(erpSalesOrder);
                                    salesOrders.Add(erpSalesOrderSmallResponse);
                                }
                            }
                            else
                            {
                                salesOrders = contract.SalesOrders.ToList<object>();
                            }
                            var contractModel = new Contract(contract.Customer, contract.ContactPerson, salesOrders);
                            contractSalesOrderResponse.Contracts.Add(contractModel);
                        }
                        //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(clResponse));
                        contractSalesOrderResponse.status = true;
                        contractSalesOrderResponse.message = clResponse.Message;
                    }
                    else if (!clResponse.Success)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from AX.");
                        contractSalesOrderResponse = new ContractSalesOrderResponse(false, clResponse.Message);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), GetRequestGUID(Request));
                    throw new Exception(message);
                }

            }
            catch (ArgumentException exp)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message.ToString());
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy, ControllerName);
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "GetContractSalesOrders", DateTime.UtcNow);

                return contractSalesOrderResponse;
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                contractSalesOrderResponse.message = message;
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "GetContractSalesOrders", DateTime.UtcNow);

                return contractSalesOrderResponse;
            }
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "GetContractSalesOrders", DateTime.UtcNow);

            return contractSalesOrderResponse;
        }


        /// <summary>
        /// Calculate Item Quantity by below given parameters 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/CalculateTimeQuantity")]
        public CalculateTimeQuantityResponse CalculateTimeQuantity([FromBody] CalculateTimeQuantityRequest request)
        {

            CalculateTimeQuantityResponse calculateTimeQuantityResponse;

            double timeQuantity = 0;
            // Throw error if Something null is null
            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                calculateTimeQuantityResponse = new CalculateTimeQuantityResponse(false, timeQuantity, message);
            }

            try
            {
                var erpSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
                timeQuantity = erpSalesOrderController.CalculateTimeQty(request.CalculateDateFrom, request.ValidTo, (int)request.BillingPeriod, request.IsSubscription, request.TmvAutoProlongation);
                calculateTimeQuantityResponse = new CalculateTimeQuantityResponse(true, timeQuantity, string.Empty);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                calculateTimeQuantityResponse = new CalculateTimeQuantityResponse(false, timeQuantity, message);
            }
            return calculateTimeQuantityResponse;
        }

        /// <summary>
        /// Calculate Line Amount by below given parameters 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/CalculateLineAmount")]
        public CalculateLineAmountResponse CalculateLineAmount([FromBody] CalculateLineAmountRequest request)
        {
            CalculateLineAmountResponse calculateLineAmountResponse;

            double lineAmount = 0;
            // Throw error if Something null is null
            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                calculateLineAmountResponse = new CalculateLineAmountResponse(true, lineAmount, string.Empty);
            }
            try
            {
                var erpSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
                lineAmount = erpSalesOrderController.CalculateLineAmount(request.CalculateDateFrom, request.ValidTo, (int)request.BillingPeriod, request.SalesQty, request.SalesPrice, request.DiscAmount, request.DiscPct, request.IsSubscription, request.TmvAutoProlongation);
                calculateLineAmountResponse = new CalculateLineAmountResponse(true, lineAmount, string.Empty);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                calculateLineAmountResponse = new CalculateLineAmountResponse(false, lineAmount, message);
            }
            return calculateLineAmountResponse;
        }

        /// <summary>
        /// Get Contract Invoices
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/GetContractInvoices")]
        public ContractInvoicesResponse GetContractInvoices([FromBody] ContractInvoicesRequest request)
        {
            ContractInvoicesResponse contractInvoicesResponse = null;

            try
            {
                //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

                // Throw error if all if none of CustomerAccount/SalesOrderId/InvoiceId is provided
                if (request == null || (String.IsNullOrEmpty(request.CustomerAccount) && String.IsNullOrEmpty(request.SalesOrderId) && String.IsNullOrEmpty(request.InvoiceId)))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    contractInvoicesResponse = new ContractInvoicesResponse("false", message, null);
                    return contractInvoicesResponse;
                }

                // Extract the data from parameter
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " salesOrderId: " + request.SalesOrderId);
                List<ErpCustInvoiceJour> erpCustInvoiceJourList = new List<ErpCustInvoiceJour>();
                contractInvoicesResponse = new ContractInvoicesResponse("false", "", erpCustInvoiceJourList);


                ERPDataModels.Custom.ContractInvoicesRequest contractInvoicesRequest = new ERPDataModels.Custom.ContractInvoicesRequest();
                contractInvoicesRequest.InvoiceId = request.InvoiceId;
                contractInvoicesRequest.SalesOrderId = request.SalesOrderId;
                contractInvoicesRequest.CustomerAccount = request.CustomerAccount;

                var erpContactSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
                var clResponse = erpContactSalesOrderController.GetContractInvoices(contractInvoicesRequest);

                if (clResponse.Success)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(clResponse));
                    contractInvoicesResponse = new ContractInvoicesResponse("true", clResponse.Message, clResponse.ErpCustInvoiceJourList);
                }
                else if (!clResponse.Success)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from AX.");
                    contractInvoicesResponse = new ContractInvoicesResponse("false", clResponse.Message, null);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                contractInvoicesResponse = new ContractInvoicesResponse("false", message, null);
            }

            return contractInvoicesResponse;
        }


        /// <summary>
        /// Get Contract Type Sales order by customer and some other optional filters  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/ValidateVATNumber")]
        public ErpValidateVATNumberResponse ValidateVATNumber([FromBody] ErpValidateVATNumberRequest request)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpValidateVATNumberResponse validateVATNumberResponse = null;

            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                validateVATNumberResponse = new ErpValidateVATNumberResponse(false, message, string.Empty);
                return validateVATNumberResponse;
            }

            if (string.IsNullOrWhiteSpace(request.CountryId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CountryId");
                validateVATNumberResponse = new ErpValidateVATNumberResponse(false, message, string.Empty);
                return validateVATNumberResponse;
            }

            if (string.IsNullOrWhiteSpace(request.VATNumber))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "VATNumber");
                validateVATNumberResponse = new ErpValidateVATNumberResponse(false, message, string.Empty);
                return validateVATNumberResponse;
            }

            validateVATNumberResponse = null;

            try
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(request));

                var erpSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
                validateVATNumberResponse = erpSalesOrderController.ValidateVATNumber(request, GetRequestGUID(Request));

                if (validateVATNumberResponse == null || !validateVATNumberResponse.Status)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(validateVATNumberResponse));
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                validateVATNumberResponse = new ErpValidateVATNumberResponse(false, message, string.Empty);
            }
            return validateVATNumberResponse;
        }

        /// <summary>
        /// Get list of affiliations
        /// </summary>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpGet]
        [Route("SalesOrder/GetRetailAffiliations")]
        public ErpAffiliationResponse GetRetailAffiliations()
        {

            var erpSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);

            ErpAffiliationResponse erpAffiliationResponse = null;

            try
            {
                List<ErpAffiliation> erpAffiliations = erpSalesOrderController.GetRetailAffiliations();

                erpAffiliationResponse = new ErpAffiliationResponse(true, null, erpAffiliations);

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                erpAffiliationResponse = new ErpAffiliationResponse(false, message, null);

            }

            return erpAffiliationResponse;
        }

        /// <summary>
        /// CreateProductLicense Creates License transaction with provided details.
        /// </summary>
        /// <param name="salesOrderTransRequest">sakes order transaction request to be created</param>
        /// <returns>SalesOrderTransResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/CreateProductLicense")]
        public LicenseActivationResponse CreateProductLicense([FromBody] EcomProductsLicenseCreation ecomLicenseRequest)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, this.currentStore, MethodBase.GetCurrentMethod().Name);
            LicenseActivationResponse licenseActivationResponse = new LicenseActivationResponse(false, null, null);
            List<ErpCreateActionLinkRequest> erpLicenseRequest = new List<ErpCreateActionLinkRequest>();
            if (ecomLicenseRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                licenseActivationResponse = new LicenseActivationResponse(false, message, null);
                return licenseActivationResponse;
            }

            try
            {
                ErpSalesOrder salesOrderPrams = new ErpSalesOrder();
                licenseActivationResponse = ValidateProductLicense(ecomLicenseRequest.Products);
                if (licenseActivationResponse != null)
                {
                    LicenseActivationResponse licenseActivationSingleResponse = new LicenseActivationResponse(false, licenseActivationResponse.Message, null);
                    return licenseActivationSingleResponse;
                }
                //Get Erp object from Ecom
                erpLicenseRequest = GetEcomRequestToErpRequest(ecomLicenseRequest.Products);
                var erpCreateLicense = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);

                var erpResponse = erpCreateLicense.CreateProductLicense(erpLicenseRequest, GetRequestGUID(Request));

                if (erpResponse.Success)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));
                    //Get Ecom Object From Erp
                    licenseActivationResponse = new LicenseActivationResponse(true, erpResponse.Message, MapErpResponseToEcom(erpResponse.productLicenseResponses, ecomLicenseRequest.Products));
                }
                else
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "License Couldn't created.");
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));

                    licenseActivationResponse = new LicenseActivationResponse(false, erpResponse.Message, null);

                }
            }
            catch (ArgumentException exp)
            {
                string message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                licenseActivationResponse = new LicenseActivationResponse(false, message, null);
                return licenseActivationResponse;
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                licenseActivationResponse = new LicenseActivationResponse(false, message, null);
                return licenseActivationResponse;
            }

            return licenseActivationResponse;
        }

        /// <summary>
        /// Close existing order
        /// </summary>
        /// <param name="closeExistingOrderRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/CloseExistingOrder")]
        public ErpCloseExistingOrderResponse CloseExistingOrder([FromBody] EcomCloseExistingOrderRequest closeExistingOrderRequest)
        {
            ErpCloseExistingOrderResponse erpResponse = new ErpCloseExistingOrderResponse(false, "", false);

            try
            {
                if (closeExistingOrderRequest == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    erpResponse = new ErpCloseExistingOrderResponse(false, message, false);
                    return erpResponse;
                }
                if (string.IsNullOrWhiteSpace(closeExistingOrderRequest.SalesId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "SalesId");
                    erpResponse = new ErpCloseExistingOrderResponse(false, message, false);
                    return erpResponse;
                }

                var salesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);

                erpResponse = salesOrderController.CloseExistingOrder(closeExistingOrderRequest.SalesId, closeExistingOrderRequest.PACLicense, closeExistingOrderRequest.DisablePacLicenseOfSalesLines);

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                erpResponse = new ErpCloseExistingOrderResponse(false, message, false);
            }

            return erpResponse;
        }

        /// <summary>
        /// Change Contract Payment Method
        /// </summary>
        /// <param name="changeContractPaymentMethodRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/ChangeContractPaymentMethod")]
        public ErpChangeContractPaymentMethodResponse ChangeContractPaymentMethod([FromBody] EcomChangeContractPaymentMethodRequest changeContractPaymentMethodRequest)
        {

            ErpChangeContractPaymentMethodResponse erpResponse = new ErpChangeContractPaymentMethodResponse(false, "", false);

            if (changeContractPaymentMethodRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                erpResponse = new ErpChangeContractPaymentMethodResponse(false, "Request is empty or null", false);
                return erpResponse;
            }
            if (string.IsNullOrWhiteSpace(changeContractPaymentMethodRequest.SalesId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "SalesId");
                erpResponse = new ErpChangeContractPaymentMethodResponse(false, "Invalid SalesId in request", false);
                return erpResponse;
            }

            var salesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);

            erpResponse = salesOrderController.ChangeContractPaymentMethod(changeContractPaymentMethodRequest.SalesId, changeContractPaymentMethodRequest.NewPaymentMethodRecId, changeContractPaymentMethodRequest.TenderTypeId, changeContractPaymentMethodRequest.BankAccountRecId
                );


            return erpResponse;
        }


        /// <summary>
        /// Create Customer and Reseller Customer.
        /// </summary>
        /// <param name="request">mergeCustomerResellerRequest</param>
        /// <returns>MergeCustomerResellerResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/MergeSalesOrderCustomerReseller")]
        public SalesOrderTransResponse MergeSalesOrderCustomerReseller([FromBody] MergeSalesOrderCustomerResellerRequest mergeSalesOrderCustomerResellerRequest)
        {
            try
            {
                var customerController = new CustomerController();
                var mergeCustomerResellerRequest = new MergeCustomerResellerRequest()
                {
                    Customer = mergeSalesOrderCustomerResellerRequest.Customer,
                    Reseller = mergeSalesOrderCustomerResellerRequest.Reseller,
                    UseMapping = mergeSalesOrderCustomerResellerRequest.UseMapping
                };

                var mergeCustomerResellerResponse = customerController.MergeCustomerReseller(mergeCustomerResellerRequest);

                if (!mergeCustomerResellerResponse.Status)
                {
                    throw new CommerceLinkError(mergeCustomerResellerResponse.Message);
                }

                var salesOrder = new SalesOrderTransRequest();
                salesOrder.salesOrderJSON = mergeSalesOrderCustomerResellerRequest.SalesOrder;

                var customerInfo = mergeCustomerResellerResponse?.CustomerInfo as ErpCustomer;
                var resellerInfo = mergeCustomerResellerResponse?.ResellerInfo as ErpCustomer;

                salesOrder.IndirectCustomerAccountNumber = customerInfo?.AccountNumber;
                salesOrder.ResellerAccountNumber = resellerInfo?.AccountNumber;

                if (string.IsNullOrEmpty(salesOrder.IndirectCustomerAccountNumber))
                {
                    throw new CommerceLinkError("Invalid Indirect Customer");
                }

                return this.CreateSalesOrder(salesOrder, MethodBase.GetCurrentMethod().Name); ;
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                CustomerContactPersonResponse CustomerContactPersonResponse = new CustomerContactPersonResponse(false, null, null, message);
                return new SalesOrderTransResponse(false, null, message.ToString());
            }
        }

        /// <summary>
        /// CreateRealtimeSalesOrderUpdate Update sales order with provided details.
        /// </summary>
        /// <param name="request">takes order request to be Update</param>
        /// <returns>bool</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/ProcessContractOperation")]
        public ProcessContractOperationResponse ProcessContractOperation([FromBody] ProcessContractOperationRequest request)
        {
            try
            {
                return ProcessContractOperation(request, null, false);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new ProcessContractOperationResponse(false, message, null);
            }
        }

        /// <summary>
        /// Reactivate Contract
        /// </summary>
        /// <param name="reactivateContractRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/ReactivateContract")]
        public ErpReactivateContract ReactivateContract([FromBody] ReactivateContractRequest reactivateContractRequest)
        {
            try
            {
                ErpReactivateContract reactivateContractResponse = new ErpReactivateContract(false, string.Empty, null);
                DateTime subscriptionStartDate = DateTime.Today;
                if (reactivateContractRequest == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    reactivateContractResponse = new ErpReactivateContract(false, message, null);
                    return reactivateContractResponse;
                }
                else if (string.IsNullOrWhiteSpace(reactivateContractRequest.PACLicenseList))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "PACLicenseList");
                    reactivateContractResponse = new ErpReactivateContract(false, message, null);
                    return reactivateContractResponse;
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(reactivateContractRequest.PACLicenseList, "[^,]"))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "PACLicenseList");
                    reactivateContractResponse = new ErpReactivateContract(false, message, null);
                    return reactivateContractResponse;
                }
                else if (string.IsNullOrWhiteSpace(reactivateContractRequest.SubscriptionStartDate))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "SubscriptionStartDate");
                    reactivateContractResponse = new ErpReactivateContract(false, message, null);
                    return reactivateContractResponse;
                }
                else if (!DateTime.TryParseExact(reactivateContractRequest.SubscriptionStartDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out subscriptionStartDate))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "SubscriptionStartDate");
                    reactivateContractResponse = new ErpReactivateContract(false, message, null);
                    return reactivateContractResponse;
                }

                var salesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
                reactivateContractResponse = salesOrderController.ReactivateContract(reactivateContractRequest.PACLicenseList, reactivateContractRequest.SubscriptionStartDate);
                return reactivateContractResponse;
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                ErpReactivateContract reactivateContractResponse = new ErpReactivateContract(false, message, null);
                return reactivateContractResponse;
            }
        }

        /// <summary>
        /// Update CustomerPortal Link
        /// </summary>
        /// <param name="updateCustomerPortalLinkRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/UpdateCustomerPortalLink")]
        public ErpUpdateCustomerPortalLinkResponse UpdateCustomerPortalLink([FromBody] UpdateCustomerPortalLinkRequest updateCustomerPortalLinkRequest)
        {

            ErpUpdateCustomerPortalLinkResponse erpResponse = new ErpUpdateCustomerPortalLinkResponse(false, "","");

            erpResponse = ValidateUpdateCustomerLinkRequest(updateCustomerPortalLinkRequest);

            if (erpResponse != null)
            {
                return erpResponse;
            }

            var salesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);

            erpResponse = salesOrderController.UpdateCustomerPortalLink(updateCustomerPortalLinkRequest, currentStore.StoreKey);


            return erpResponse;
        }
        #endregion

        #region PrivateMethods

        /// <summary>
        /// ValidateCustomerCreateRequest validate Create Customer Request Object.
        /// </summary>
        /// <param name="salesOrderPrams"></param>
        /// <returns></returns>
        private SalesOrderTransResponse TMV_ValidateSalesOrderRequest(ErpSalesOrder salesOrderPrams, bool isCheckoutProcessContractOperation = false)
        {
            string strTMVMigratedOrderNumber = string.Empty;
            bool bTMVMigratedSalesLineNumberCheckStatus = false;

            string strTMVOldSalesOrderNumber = string.Empty;
            bool bFoundSalesLineWithoutTMVOldSalesLineNumber = false;
            bool bFoundSalesLineWithoutTMVOldSalesLineAction = false;
            bool isTMVParentValueProvided = false;
            bool isAppleAppOrder = false;

            if (!isCheckoutProcessContractOperation && string.IsNullOrEmpty(salesOrderPrams.RequestedDeliveryDate.ToString()))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "TMVCONTRACTVALIDFROM / Valid from date / ShippingDateRequested");
                return new SalesOrderTransResponse(false, "", message);
            }

            if (salesOrderPrams.SalesLines == null || salesOrderPrams.SalesLines.Count == 0)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40506, currentStore);
                return new SalesOrderTransResponse(false, "", message);
            }

            if (salesOrderPrams.TenderLines == null || salesOrderPrams.TenderLines.Count == 0)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40507, currentStore);
                return new SalesOrderTransResponse(false, "", message);
            }

            if (salesOrderPrams.CustomAttributes != null)
            {
                foreach (var kvpCustomAttribute in salesOrderPrams.CustomAttributes)
                {
                    switch (kvpCustomAttribute.Key)
                    {
                        case C_ATT_TMV_MIGRATED_ORDER_NUMBER:
                            {
                                strTMVMigratedOrderNumber = kvpCustomAttribute.Value;
                                break;
                            }
                        case C_ATT_TMV_OLD_SALES_ORDER_NUMBER:
                            {
                                strTMVOldSalesOrderNumber = kvpCustomAttribute.Value;
                                break;
                            }
                        case C_ATT_TMV_RESELLER:
                            {
                                isAppleAppOrder = kvpCustomAttribute.Value.ToUpper().Equals(ThirdPartyResellers.APPLEAPP.ToString()) ? true : false;
                                break;
                            }
                    }
                }

                if (salesOrderPrams.SalesLines != null)
                {
                    foreach (var salesLine in salesOrderPrams.SalesLines)
                    {
                        if (salesLine.LineNumber < 1 && !isAppleAppOrder)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "SalesOrder.Product-LineItem.line-number");
                            return new SalesOrderTransResponse(false, string.Empty, message);
                        }

                        if (salesLine.CustomAttributes != null)
                        {
                            foreach (var kvpSalesLineCustomAttribute in salesLine.CustomAttributes)
                            {
                                switch (kvpSalesLineCustomAttribute.Key)
                                {
                                    case C_ATT_TMV_MIGRATED_SALES_LINE_NUMBER:
                                        {
                                            if (kvpSalesLineCustomAttribute.Value != "0")
                                            {
                                                bTMVMigratedSalesLineNumberCheckStatus = true;
                                            }
                                            break;
                                        }
                                    case C_ATT_TMV_OLD_SALES_LINE_NUMBER:
                                        {
                                            if (kvpSalesLineCustomAttribute.Value == "0")
                                            {
                                                bFoundSalesLineWithoutTMVOldSalesLineNumber = true;
                                            }
                                            break;
                                        }
                                    case C_ATT_TMV_OLD_SALES_LINE_ACTION:
                                        {
                                            if (String.IsNullOrEmpty(kvpSalesLineCustomAttribute.Value))
                                            {
                                                bFoundSalesLineWithoutTMVOldSalesLineAction = true;
                                            }
                                            break;
                                        }
                                    case C_ATT_TMV_PARENT:
                                        {
                                            isTMVParentValueProvided = string.IsNullOrWhiteSpace(kvpSalesLineCustomAttribute.Value) ? false : true;

                                            if (isTMVParentValueProvided && !isAppleAppOrder)
                                            {
                                                long TMVParentValue = long.Parse(kvpSalesLineCustomAttribute.Value);
                                                isTMVParentValueProvided = TMVParentValue < 0 ? false : true;
                                            }

                                            if (!isTMVParentValueProvided && !isAppleAppOrder)
                                            {
                                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "SalesOrder.Product-LineItem.CustomAttributes.TMVPARENT");
                                                return new SalesOrderTransResponse(false, string.Empty, message);
                                            }
                                            break;
                                        }
                                }
                            }
                        }
                    }
                }
            }

            if (strTMVMigratedOrderNumber != string.Empty && bTMVMigratedSalesLineNumberCheckStatus == false)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40526, currentStore, strTMVMigratedOrderNumber);
                return new SalesOrderTransResponse(false, string.Empty, message, null);
            }

            if (strTMVOldSalesOrderNumber != string.Empty &&
                // (bFoundSalesLineWithoutTMVOldSalesLineNumber == true || bFoundSalesLineWithoutTMVOldSalesLineAction == true)
                bFoundSalesLineWithoutTMVOldSalesLineAction == true
            )
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40527, currentStore, strTMVOldSalesOrderNumber);
                return new SalesOrderTransResponse(false, string.Empty, message);
            }

            // Validate request for Payment Information 
            var salesOrderTransResponse = ValidateSalesOrderPaymentInformation(salesOrderPrams);
            if (salesOrderTransResponse != null)
            {
                return salesOrderTransResponse;
            }
            return new SalesOrderTransResponse(true, "", "");
        }

        private SalesOrderTransResponse ValidateSalesOrderPaymentInformation(ErpSalesOrder salesOrderPrams)
        {
            var tenderLine = salesOrderPrams.TenderLines[0];
            if (tenderLine.TenderTypeId == PaymentCon.PURCHASEORDER.ToString())
            {
                if (tenderLine.Amount == null || tenderLine.Amount < 0)
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "Amount"));
                }
            }
            else if (tenderLine.TenderTypeId == PaymentCon.SEPA.ToString())
            {
                if (string.IsNullOrWhiteSpace(tenderLine.CardOrAccount))
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "CardOrAccount"));
                }
                else if (string.IsNullOrWhiteSpace(tenderLine.IBAN))
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "IBAN"));
                }
                else if (string.IsNullOrWhiteSpace(tenderLine.SwiftCode))
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "SwiftCode"));
                }
            }
            else if (tenderLine.TenderTypeId == PaymentCon.ADYEN_HPP.ToString())
            {
                if (string.IsNullOrWhiteSpace(tenderLine.PspReference))
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "PspReference"));
                }
                else if (string.IsNullOrWhiteSpace(tenderLine.CardTypeId))
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "CardTypeId"));
                }

            }
            else if (tenderLine.TenderTypeId == PaymentCon.BOLETO.ToString())
            {
                if (tenderLine.Boleto == null)
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "Boleto"));
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(tenderLine.CardOrAccount))
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "CardOrAccount"));
                }
                else if (string.IsNullOrWhiteSpace(tenderLine.TenderTypeId))
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "TenderTypeId"));
                }
                else if (tenderLine.ExpMonth == null || tenderLine.ExpMonth < 1 || tenderLine.ExpMonth > 12)
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "ExpMonth"));
                }
                else if (tenderLine.ExpYear == null || tenderLine.ExpYear < 0)
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "ExpYear"));
                }
                else if (tenderLine.Amount == null || tenderLine.Amount < 0)
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "Amount"));
                }
                else if (string.IsNullOrWhiteSpace(tenderLine.Authorization))
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "Authorization"));
                }
                else if (string.IsNullOrWhiteSpace(tenderLine.CardToken))
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "CardToken"));
                }
                else if (string.IsNullOrWhiteSpace(tenderLine.CardTypeId))
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "CardTypeId"));
                }
                else if (string.IsNullOrWhiteSpace(tenderLine.MaskedCardNumber))
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "MaskedCardNumber"));
                }
                else if (string.IsNullOrWhiteSpace(tenderLine.TenderTypeId))
                {
                    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "TenderTypeId"));
                }
                if (tenderLine.TenderTypeId == PaymentCon.ADYEN_CC.ToString())
                {
                    if (string.IsNullOrWhiteSpace(tenderLine.BankIdentificationNumberStart))
                    {
                        return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "BankIdentificationNumberStart"));
                    }
                    //else if (string.IsNullOrWhiteSpace(tenderLine.ApprovalCode))
                    //{
                    //    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "ApprovalCode"));
                    //}

                    else if (string.IsNullOrWhiteSpace(tenderLine.shopperReference))
                    {
                        return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "shopperReference"));
                    }
                    //else if (string.IsNullOrWhiteSpace(tenderLine.IssuerCountry))
                    //{
                    //    return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "IssuerCountry"));
                    //}
                }
                else if (tenderLine.TenderTypeId == PaymentCon.BASIC_CREDIT.ToString())
                {
                    if (string.IsNullOrWhiteSpace(tenderLine.ThreeDSecure))
                    {
                        return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "ThreeDSecure"));
                    }
                }
                else if (tenderLine.TenderTypeId == PaymentCon.PAYPAL_EXPRESS.ToString())
                {
                    if (string.IsNullOrWhiteSpace(tenderLine.Email))
                    {
                        return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "Email"));
                    }
                    else if (string.IsNullOrWhiteSpace(tenderLine.Note))
                    {
                        return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "Note"));
                    }
                    else if (string.IsNullOrWhiteSpace(tenderLine.ParentTransactionId))
                    {
                        return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "ParentTransactionId"));

                    }
                    else if (string.IsNullOrWhiteSpace(tenderLine.PayerId))
                    {
                        return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "PayerId"));
                    }
                }
                else if (tenderLine.TenderTypeId == PaymentCon.ALLPAGO_CC.ToString())
                {
                    if (configurationHelper.GetSetting(CUSTOMER.Default_ThreeLetterISORegionName) == "BRA")
                    {
                        if (string.IsNullOrWhiteSpace(tenderLine.LocalTaxId))
                        {
                            return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "LocalTaxId"));
                        }
                    }
                    if (string.IsNullOrWhiteSpace(tenderLine.ApprovalCode))
                    {
                        return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "ApprovalCode"));
                    }
                    if (tenderLine.NumberOfInstallments == null || tenderLine.NumberOfInstallments < 1)
                    {
                        return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "NumberOfInstallments"));
                    }
                    if (string.IsNullOrWhiteSpace(tenderLine.TransactionId))
                    {
                        return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "TransactionId"));
                    }

                    if (string.IsNullOrWhiteSpace(tenderLine.IP))
                    {
                        return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "IP"));
                    }
                    if (string.IsNullOrWhiteSpace(tenderLine.Email))
                    {
                        return new SalesOrderTransResponse(false, "", string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40535), MethodBase.GetCurrentMethod().Name, "IP"));
                    }
                }
            }

            return null;
        }

        private SalesOrderTransResponse ValidateCheckoutProcessContractOperationAddress(ErpAddress address)
        {
            if (address == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new SalesOrderTransResponse(false, "", message);
            }
            else if (!string.IsNullOrWhiteSpace(address.ZipCode) && address.ZipCode.Length > 10)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Address.ZipCode", "10");
                return new SalesOrderTransResponse(false, null, message);
            }
            else if (!string.IsNullOrWhiteSpace(address.Name) && address.Name.Length > 60)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "Address.Name", "60");
                return new SalesOrderTransResponse(false, null, message);
            }

            return null;
        }

        private ErpSalesOrder ConvertSalesOrderJsonToXML(string salesOrderJSON)
        {
            ErpSalesOrder erpSalesOrder = new ErpSalesOrder();

            try
            {
                var salesOrderXML = JsonConvert.DeserializeXmlNode(salesOrderJSON, "orders");

                //get sales order object from ecom adapter
                string InProcessOrderId = string.Empty;
                InProcessOrderId = GetOrderNumber(salesOrderXML);


                if (InProcessOrderId != null)
                {
                    using (var eComSalesOrderController = ecomAdapterFactory.CreateSalesOrderController(currentStore.StoreKey))
                    {
                        erpSalesOrder = eComSalesOrderController.GetSalesOrderFromXML(salesOrderXML);
                        if (erpSalesOrder.Id != null)
                        {
                            return erpSalesOrder;
                        }
                        else
                        {
                            string soXmlMessage = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40510);
                            CustomLogger.LogDebugInfo(string.Format(soXmlMessage + ":{0}", InProcessOrderId), currentStore.StoreId, currentStore.CreatedBy, InProcessOrderId);
                        }
                    }
                }
                else
                {
                    CustomLogger.LogDebugInfo(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40511)), currentStore.StoreId, currentStore.CreatedBy);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return erpSalesOrder;
        }

        private string GetOrderNumber(XmlDocument fileData)
        {
            string orderId = string.Empty;
            if (fileData != null)
            {
                XmlNode dataAttribute = fileData.SelectSingleNode("//orders/order/@order-no");
                if (dataAttribute != null)
                {
                    orderId = string.Format("Order No {0} ", dataAttribute.Value);
                }
            }
            return orderId;
        }

        private void MapExtensionProperties(ErpSalesOrder erpSalesOrder, SalesOrderTransRequest request)
        {
            if (!string.IsNullOrEmpty(request.IndirectCustomerAccountNumber))
            {
                if (erpSalesOrder.CustomAttributes.Any(attr => attr.Key == "TMVINDIRECTCUSTOMER"))
                {
                    erpSalesOrder.CustomAttributes.Remove(
                        erpSalesOrder.CustomAttributes.FirstOrDefault(a => a.Key == "TMVINDIRECTCUSTOMER"));
                }
                erpSalesOrder.CustomAttributes.Add(new KeyValuePair<string, string>("TMVINDIRECTCUSTOMER", request.IndirectCustomerAccountNumber));

            }

            if (!string.IsNullOrEmpty(request.ResellerAccountNumber))
            {
                if (erpSalesOrder.CustomAttributes.Any(attr => attr.Key == "TMVRESELLERACCOUNT"))
                {
                    erpSalesOrder.CustomAttributes.Remove(
                        erpSalesOrder.CustomAttributes.FirstOrDefault(a => a.Key == "TMVRESELLERACCOUNT"));
                }
                erpSalesOrder.CustomAttributes.Add(new KeyValuePair<string, string>("TMVRESELLERACCOUNT", request.ResellerAccountNumber));

            }
        }

        private ProcessContractOperationResponse ValidateCheckoutProcessContractOperationRequest(ProcessContractOperationRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new ProcessContractOperationResponse(false, message, null);
                }
                else if (String.IsNullOrWhiteSpace(request.RequestNumber))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ContractSwitchMigrateRequest.RequestNumber");
                    return new ProcessContractOperationResponse(false, message, null);
                }
                else if (request.ContractLines.Count == 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ContractSwitchMigrateRequest.ContractLines");
                    return new ProcessContractOperationResponse(false, message, null);
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new ProcessContractOperationResponse(false, message, null);
            }

            return null;
        }

        private void initializeAndAssignDefaultParameters(ErpSalesOrder salesOrderRequest)
        {

            salesOrderRequest.ReceiptEmail = salesOrderRequest.CustomerEmail;
            salesOrderRequest.Status = ErpSalesStatus.Created;

            if (salesOrderRequest.BillingAddress != null)
            {
                salesOrderRequest.BillingAddress.TwoLetterISORegionName = salesOrderRequest.BillingAddress.ThreeLetterISORegionName;
            }

            processSalesOrderHeaderAttributes(salesOrderRequest);
            AddPaymentCustomProperties(salesOrderRequest);
            processSalesOrderLineAttributes(salesOrderRequest);
        }
        private void processSalesOrderHeaderAttributes(ErpSalesOrder salesOrderRequest)
        {
            if (salesOrderRequest.CustomAttributes.Count > 0)
            {
                salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVIsAXCustomerProvided.ToString(), "true"));

                var tmvContractValidFrom = salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTVALIDFROM.ToString()));

                if (tmvContractValidFrom.Key != null)
                {
                    salesOrderRequest.RequestedDeliveryDate = DateTimeOffset.Parse(tmvContractValidFrom.Value);
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVRESELLERACCOUNT.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVRESELLERACCOUNT.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVDISTRIBUTORACCOUNT.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVDISTRIBUTORACCOUNT.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVINDIRECTCUSTOMER.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVINDIRECTCUSTOMER.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVINDIRECTCUSTOMEREMAIL.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVINDIRECTCUSTOMEREMAIL.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVMAINOFFERTYPE.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVMAINOFFERTYPE.ToString(), "0"));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVPRODUCTFAMILY.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVPRODUCTFAMILY.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVSALESORDERSUBTYPE.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVSALESORDERSUBTYPE.ToString(), "1"));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVINVOICESCHEDULECOMPLETE.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVINVOICESCHEDULECOMPLETE.ToString(), "0"));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVCONTRACTSTATUSLINE.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVCONTRACTSTATUSLINE.ToString(), "0"));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVSMMCAMPAIGNID.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVSMMCAMPAIGNID.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVPURCHORDERFORMNUM.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVPURCHORDERFORMNUM.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVPIT.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVPIT.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVQUOTATIONID.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVQUOTATIONID.ToString(), ""));
                }

                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVMIGRATEDORDERNUMBER.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVMIGRATEDORDERNUMBER.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVOLDSALESORDERNUMBER.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVOLDSALESORDERNUMBER.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVORIGINATINGCOUNTRY.ToString())).Key == null &&
                    salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals("TMVOriginatingCountry")).Key == null)

                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVORIGINATINGCOUNTRY.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVCOMMENTFORORDER.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVCOMMENTFORORDER.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVCOMMENTFOREMAIL.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVCOMMENTFOREMAIL.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVSALESORIGIN.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVSALESORIGIN.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVPARTNERID.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVPARTNERID.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVPaymentTerms.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVPaymentTerms.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVFraudReviewStatus.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVFraudReviewStatus.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVKountScore.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVKountScore.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVRESELLER.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVRESELLER.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVTransferOrderAsPerOldDate.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVTransferOrderAsPerOldDate.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVActivationLinkEmail.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVActivationLinkEmail.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVCustomerReference.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVCustomerReference.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVContactPersonId.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVContactPersonId.ToString(), ""));
                }
                if (salesOrderRequest.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVLoginEmail.ToString())).Key == null)
                {
                    salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVLoginEmail.ToString(), ""));
                }
            }
        }
        private void processSalesOrderLineAttributes(ErpSalesOrder salesOrderRequest)
        {
            foreach (var salesLine in salesOrderRequest.SalesLines)
            {
                salesLine.SalesOrderUnitOfMeasure = salesLine.UnitOfMeasureSymbol;
                salesLine.Price = salesLine.BasePrice;
                salesLine.Description = salesLine.ItemId;
                salesLine.QuantityOrdered = salesLine.Quantity;
                salesLine.ShipmentId = salesOrderRequest.ChannelReferenceId;
                salesLine.IsGiftCardLine = false;

                if (salesLine.CustomAttributes.Count > 0)
                {
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTVALIDFROM.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVCONTRACTVALIDFROM.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTCALCULATEFROM.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVCONTRACTCALCULATEFROM.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTVALIDTO.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVCONTRACTVALIDTO.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTPOSSTERMDATE.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVCONTRACTPOSSTERMDATE.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTCANCELDATE.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVCONTRACTCANCELDATE.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTPOSSCANCELDATE.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVCONTRACTPOSSCANCELDATE.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTTERMDATE.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVCONTRACTTERMDATE.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTTERMDATEEFFECTIVE.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVCONTRACTTERMDATEEFFECTIVE.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVAUTOPROLONGATION.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVAUTOPROLONGATION.ToString(), "0"));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVPURCHORDERFORMNUM.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVPURCHORDERFORMNUM.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCUSTOMERREF.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVCUSTOMERREF.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTSTATUSLINE.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVCONTRACTSTATUSLINE.ToString(), "10"));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVEULAVERSION.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVEULAVERSION.ToString(), "v1"));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVBILLINGPERIOD.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVBILLINGPERIOD.ToString(), "1"));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.PACLICENSE.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.PACLICENSE.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVORIGINALLINEAMOUNT.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVORIGINALLINEAMOUNT.ToString(), salesLine.TotalAmount.ToString()));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVLINEMODIFIED.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVLINEMODIFIED.ToString(), "1"));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVREVERSEDLINE.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVREVERSEDLINE.ToString(), "0"));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVMIGRATEDSALESLINENUMBER.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVMIGRATEDSALESLINENUMBER.ToString(), "0"));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVAFFILIATIONRECID.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVAFFILIATIONRECID.ToString(), "0"));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVOLDSALESLINENUMBER.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVOLDSALESLINENUMBER.ToString(), "0"));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVOLDSALESLINEACTION.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVOLDSALESLINEACTION.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVSOURCEID.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVSOURCEID.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVPARENT.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVPARENT.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCOUPONCODE.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVCOUPONCODE.ToString(), ""));
                    }
                    if (salesLine.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesLineExtensionProperties.TMVCUSTOMERLINENUM.ToString())).Key == null)
                    {
                        salesLine.CustomAttributes.Add(new KeyValuePair<string, string>(ErpSalesLineExtensionProperties.TMVCUSTOMERLINENUM.ToString(), ""));
                    }

                }

            }

            ProcessProductOptionsAsErpSalesLine(salesOrderRequest);
        }
        private void AddPaymentCustomProperties(ErpSalesOrder salesOrderRequest)
        {
            if (salesOrderRequest.TenderLines[0].TenderTypeId == PaymentCon.SEPA.ToString())
            {
                salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVCardHolder", salesOrderRequest.TenderLines[0].CardOrAccount));
                salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVIBAN", salesOrderRequest.TenderLines[0].IBAN));
                salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVSwiftCode", salesOrderRequest.TenderLines[0].SwiftCode));
                salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVBankName", salesOrderRequest.TenderLines[0].BankName == null ? string.Empty : salesOrderRequest.TenderLines[0].BankName));
            }
            else
            {
                salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVCardHolder", string.Empty));
                salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVIBAN", string.Empty));
                salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVSwiftCode", string.Empty));
                salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVBankName", string.Empty));
            }

            salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayPspReference", salesOrderRequest.TenderLines[0]?.PspReference ?? string.Empty));
            salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayBuyerId", salesOrderRequest.TenderLines[0].Alipay?.BuyerId ?? string.Empty));
            salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayBuyerEmail", salesOrderRequest.TenderLines[0].Alipay?.BuyerEmail ?? string.Empty));
            salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayOutTradeNo", salesOrderRequest.TenderLines[0].Alipay?.OutTradeNo ?? string.Empty));
            salesOrderRequest.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayTradeNo", salesOrderRequest.TenderLines[0].Alipay?.TradeNo ?? string.Empty));

        }
        private void ProcessProductOptionsAsErpSalesLine(ErpSalesOrder salesOrderRequest)
        {
            List<ErpSalesLine> lstNewSalesLine = new List<ErpSalesLine>();
            if (salesOrderRequest.SalesLines != null)
            {
                foreach (ErpSalesLine line in salesOrderRequest.SalesLines)
                {
                    if (line.Options != null)
                    {
                        foreach (ErpSalesLine option in line.Options)
                        {
                            if (!option.ItemId.Equals(configurationHelper.GetSetting(SALESORDER.OptionItem_None_Constant)))
                            {
                                option.ShipmentId = line.ShipmentId;
                                //Maintaining these values from parent product
                                option.RequestedDeliveryDate = line.RequestedDeliveryDate;
                                option.InventoryLocationId = line.InventoryLocationId;
                                option.CustomAttributes = line.CustomAttributes;

                                //holding option parent item variant into comment to get linked item quantity
                                option.Comment += ":" + line.ItemId.Replace(configurationHelper.GetSetting(PRODUCT.SKU_Prefix), "");

                                lstNewSalesLine.Add(option);
                            }
                        }
                        line.Options.Clear();
                    }
                }
                foreach (ErpSalesLine line in lstNewSalesLine)
                {
                    salesOrderRequest.SalesLines.Add(line);
                }
            }
        }

        private ErpUpdateCustomerPortalLinkResponse ValidateUpdateCustomerLinkRequest(UpdateCustomerPortalLinkRequest updateCustomerPortalLinkRequest)
        {
            ErpUpdateCustomerPortalLinkResponse erpResponse = new ErpUpdateCustomerPortalLinkResponse(false, "","");
            if (updateCustomerPortalLinkRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name, "Request");
                erpResponse = new ErpUpdateCustomerPortalLinkResponse(false, message, "");
                return erpResponse;
            }
            if (string.IsNullOrWhiteSpace(updateCustomerPortalLinkRequest.SalesId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "SalesId");
                erpResponse = new ErpUpdateCustomerPortalLinkResponse(false, message, "");
                return erpResponse;
            }
            if (string.IsNullOrWhiteSpace(updateCustomerPortalLinkRequest.ActivationUrl))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ActivationUrl");
                erpResponse = new ErpUpdateCustomerPortalLinkResponse(false, message, "");
                return erpResponse;
            }

            return null;
        }

        private ProcessContractOperationResponse ProcessContractOperation(ProcessContractOperationRequest request, List<ErpTenderLine> tenderLines, bool isCheckoutProcessContractOperation)
        {
            var processContractOperationResponse = ValidateCheckoutProcessContractOperationRequest(request);

            if (processContractOperationResponse != null)
            {
                return processContractOperationResponse;
            }

            var erpContactSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
            return erpContactSalesOrderController.ProcessContractOperation(request, tenderLines, isCheckoutProcessContractOperation, GetRequestGUID(Request));
        }

        /// <summary>
        /// Validate product prices
        /// </summary>
        /// <param name="salesOrderPrams"></param>
        /// <returns></returns>
        private SalesOrderTransResponse ValidatePrices(ErpSalesOrder salesOrderPrams)
        {
            var salesOrigin = salesOrderPrams.CustomAttributes.FirstOrDefault(x => x.Key == "TMVSALESORIGIN");
            var isAppleAppOrder = salesOrderPrams.CustomAttributes.FirstOrDefault(k => k.Key
                .Equals(ErpSalesOrderExtensionProperties.TMVRESELLER.ToString())).Value
                .Equals(ThirdPartyResellers.APPLEAPP.ToString());

            if (!isAppleAppOrder)
            {
                if (string.IsNullOrWhiteSpace(salesOrigin.Value))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL401201, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new SalesOrderTransResponse(false, string.Empty, message);
                }
            }

            if (PriceContractLine.PriceValidationSalesOrigin(salesOrigin.Value) && !isAppleAppOrder)
            {
                bool validateOrderPrice = configurationHelper.GetSetting(SALESORDER.ValidateOrderPrice).BoolValue();

                if (validateOrderPrice)
                {
                    if (salesOrderPrams.SalesLines != null && salesOrderPrams.SalesLines.Count > 0)
                    {
                        PriceRequest priceValidateRequest = new PriceRequest();
                        priceValidateRequest.IsValidateRequest = true;
                        priceValidateRequest.Currency = salesOrderPrams.CurrencyCode;
                        priceValidateRequest.CustomerAccount = salesOrderPrams.CustomerId;
                        if (salesOrderPrams.CustomAttributes.Count(c => c.Key.ToUpper().Equals("TMVINDIRECTCUSTOMER")) > 0)
                        {
                            priceValidateRequest.IndirectCustomerAccount = salesOrderPrams.CustomAttributes.FirstOrDefault(c => c.Key.ToUpper().Equals("TMVINDIRECTCUSTOMER")).Value.ToString();
                        }
                        if (salesOrderPrams.CustomAttributes.Count(c => c.Key.ToUpper().Equals("TMVRESELLERACCOUNT")) > 0)
                        {
                            priceValidateRequest.ResellerAccount = salesOrderPrams.CustomAttributes.FirstOrDefault(c => c.Key.ToUpper().Equals("TMVRESELLERACCOUNT")).Value.ToString();
                        }

                        priceValidateRequest.RequestDate = salesOrderPrams.OrderPlacedDate.ToString("yyyy-MM-dd");
                        foreach (var salesLine in salesOrderPrams.SalesLines)
                        {
                            Decimal targetAmount = salesLine.TotalAmount;
                            if (targetAmount == 0)
                            {
                                targetAmount = salesLine.NetAmount;
                            }

                            priceValidateRequest.ContractLines.Add(new PriceContractLine()
                            {
                                LineNumber = salesLine.LineNumber,
                                ProductId = salesLine.ItemId,
                                Quantity = salesLine.Quantity,
                                TargetPrice = targetAmount,
                            });
                        }
                        var erpSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
                        var response = erpSalesOrderController.GetOrValidatePriceInformation(priceValidateRequest);
                        if (!response.Status)
                        {
                            List<PriceValidationResponse> itemsInformation = new List<PriceValidationResponse>();
                            if (response.Result != null && response.Result.SaleLines != null && response.Result.SaleLines.Items != null)
                            {
                                foreach (var salesLine in response.Result.SaleLines.Items)
                                {
                                    var item = new PriceValidationResponse();
                                    item.LineNumber = salesLine.LineNumber;
                                    item.ItemId = salesLine.ItemId;
                                    item.VariantId = salesLine.VariantId;
                                    item.TotalAmount = salesLine.TotalAmount;
                                    item.Message = salesLine.Message;
                                    itemsInformation.Add(item);
                                }
                            }
                            return new SalesOrderTransResponse(response.Status, string.Empty, response.Message, itemsInformation);
                        }
                        else
                        {
                            return new SalesOrderTransResponse(true, string.Empty, response.Message);
                        }
                    }
                    else
                    {
                        return new SalesOrderTransResponse(true, string.Empty, CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL401202));
                    }
                }
                else
                {
                    return new SalesOrderTransResponse(true, string.Empty, CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL401203));
                }
            }
            else
            {
                return new SalesOrderTransResponse(true, string.Empty, CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL401204));
            }
        }

        #endregion

        #region SalesOrderTrans Request, Response classes

        /// <summary>
        /// Represents sales order trans request.
        /// </summary>
        public class SalesOrderTransRequest
        {
            /// <summary>
            /// salesOrderJSON for sales order trans request
            /// </summary>
            [Required]
            public String salesOrderJSON { get; set; }

            /// <summary>
            /// CustomerAccountNumber 
            /// </summary>
            [Required]
            public string IndirectCustomerAccountNumber { get; set; }

            /// <summary>
            /// ResellerAccountNumber
            /// </summary>
            [Required]
            public string ResellerAccountNumber { get; set; }
        }

        /// <summary>
        /// Represents Merge sales order Customer Contact Person trans request.
        /// </summary>
        public class MergeSalesOrderTransRequest
        {
            /// <summary>
            /// salesOrderJSON for sales order Customer Contact Person trans request
            /// </summary>
            public SalesOrderTransRequest SalesOrder { get; set; }

            /// <summary>
            /// Customer
            /// </summary>
            public EcomCustomerCreateRequest CustomerInfo { get; set; }

            /// <summary>
            /// Contact Person
            /// </summary>
            public ContactPersonController.ContactPersonRequest ContactPersonInfo { get; set; }

            /// <summary>
            /// Swap Language
            /// </summary>
            public bool SwapLanguage { get; set; }

            /// <summary>
            /// Instanciate an instance of request class
            /// </summary>
            public MergeSalesOrderTransRequest()
            {
                this.SwapLanguage = true;
            }
        }

        /// <summary>
        /// Represents sales order trans response
        /// </summary>
        public class SalesOrderTransResponse
        {

            /// <summary>
            /// Initializes a new instance of the SalesOrderTransResponse
            /// </summary>
            /// <param name="status">status of sales order trans</param>
            /// <param name="salesOrderTransactionId">eCommerce Id of sales order trans</param>
            /// /// <param name="message">message of sales order trans</param>
            /// /// <param name="result">message of sales order trans</param>
            public SalesOrderTransResponse(bool status, string salesOrderTransactionId, string message, List<PriceValidationResponse> result = null, string salesOrderId = "")
            {
                this.status = status;
                this.salesOrderTransactionId = salesOrderTransactionId;
                this.message = message;
                this.result = result;
                this.salesOrderId = salesOrderId;
            }

            /// <summary>
            /// status of sales order trans
            /// </summary>
            public bool status { get; set; }

            /// <summary>
            /// eCommerce Id of sales order trans
            /// </summary>
            public string salesOrderTransactionId { get; set; }

            /// <summary>
            /// message of sales order trans
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// result of sales order trans
            /// </summary>
            public List<PriceValidationResponse> result { get; set; }

            /// <summary>
            /// Checkout process Sales order Id
            /// </summary>
            public string salesOrderId { get; set; }
        }
        private List<ErpCreateActionLinkRequest> GetEcomRequestToErpRequest(List<EcomLicenseRequest> ecomRequest)
        {
            List<ErpCreateActionLinkRequest> erpRequest = new List<ErpCreateActionLinkRequest>();
            foreach (var request in ecomRequest)
            {
                ErpCreateActionLinkRequest erpCreateActionLinkRequest = new ErpCreateActionLinkRequest();
                erpCreateActionLinkRequest.GUID = request.GUID;
                erpCreateActionLinkRequest.ItemId = request.ItemId;
                erpCreateActionLinkRequest.Quantity = request.Quantity;
                erpRequest.Add(erpCreateActionLinkRequest);
            }
            return erpRequest;
        }
        private List<ProductLicenseResponse> MapErpResponseToEcom(List<ErpProductLicenseResponse> productLicenseResponses, List<EcomLicenseRequest> ecomLicenseRequests)
        {
            List<ProductLicenseResponse> ecomResponses = new List<ProductLicenseResponse>();
            foreach (var item in ecomLicenseRequests)
            {
                var erpProductLicense = productLicenseResponses.FirstOrDefault(p => p.GUID == item.GUID);
                if (erpProductLicense != null)
                {
                    var ecomResponse = new ProductLicenseResponse(erpProductLicense.GUID,
                        item.ItemId,
                        erpProductLicense.Quantity,
                        erpProductLicense.ActionLink);

                    ecomResponses.Add(ecomResponse);
                }
            }

            return ecomResponses;
        }
        /// <summary>
        /// 
        /// </summary>
        public class LicenseActivationResponse
        {

            /// <summary>
            /// Initializes a new instance of the LicenseActivationResponse
            /// </summary>
            /// <param name="status">status of License Activation</param>
            /// <param name="message">message of Create Lincense Transaction</param>
            /// <param name="prodcutLicenseResponses">prodcutLicenseResponses response of license request</param>
            public LicenseActivationResponse(bool status, string message, List<ProductLicenseResponse> prodcutLicenseResponses)
            {
                this.Status = status;
                this.Message = message;
                this.Products = prodcutLicenseResponses;
            }

            /// <summary>
            /// status of Create Lincense Transaction
            /// </summary>
            public bool Status { get; set; }
            /// <summary>
            /// message of Create Lincense Transaction
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Products
            /// </summary>
            public List<ProductLicenseResponse> Products { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public class ContractSalesOrderResponse
        {
            /// <summary>
            /// Initializes a new instance of the SalesOrderTransResponse
            /// </summary>
            /// <param name="status">status of sales order trans</param>
            /// <param name="">eCommerce Id of sales order trans</param>
            /// /// <param name="message">message of sales order trans</param>
            public ContractSalesOrderResponse(bool status, string message)
            {
                this.status = status;
                this.message = message;
                this.Contracts = new List<Contract>();
            }

            /// <summary>
            /// status of sales order trans
            /// </summary>
            public bool status { get; set; }


            /// <summary>
            /// message of sales order trans
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// List of Contract
            /// </summary>
            public List<Contract> Contracts { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class Contract
        {
            /// <summary>
            /// Customer Detail
            /// </summary>
            public ErpCustomer Customer { get; set; }

            /// <summary>
            /// Contact Person Detail
            /// </summary>
            public ErpContactPerson ContactPerson { get; set; }

            /// <summary>
            /// List of Sales Orders
            /// </summary>
            public List<object> SalesOrders { get; set; }

            public Contract(ErpCustomer customer, ErpContactPerson contactPerson, List<object> salesOrders)
            {
                this.Customer = customer;
                this.ContactPerson = contactPerson;
                this.SalesOrders = salesOrders;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ContractSalesOrderRequest
        {
            /// <summary>
            /// 
            /// </summary>
            [Required]
            public string CustomerAccount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Required]
            public string EcomCustomerId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Required]
            public string ChannelReferenceId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Required]
            public string OfferType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            /// 
            public bool isActive { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Required]
            public string SalesOrderId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> Status { get; set; }
            /// <summary>
            /// PACLicense number for customer.
            /// </summary>
            [Required]
            public string LicenseNumber { get; set; }
            /// <summary>
            /// Fetch smaller response
            /// </summary>
            public bool SmallResponse { get; set; }
            /// <summary>
            /// Swap Language
            /// </summary>
            public bool SwapLanguage { get; set; }
            /// <summary>
            /// Initialize an instance of Request class
            /// </summary>
            public ContractSalesOrderRequest()
            {
                // By default swap language is true for E-Com
                this.SwapLanguage = true;
            }
        }

        /// <summary>
        /// Contract Invoices Request
        /// </summary>
        public class ContractInvoicesRequest
        {
            /// <summary>
            /// Customer Id property
            /// </summary>
            [Required]
            public string CustomerAccount { get; set; }

            /// <summary>
            /// Sales Order Id property
            /// </summary>
            [Required]
            public string SalesOrderId { get; set; }
            /// <summary>
            /// Invoice Id property
            /// </summary>
            [Required]
            public string InvoiceId { get; set; }
        }

        /// <summary>
        /// Contract Invoices Response
        /// </summary>
        public class ContractInvoicesResponse
        {

            ///// <summary>
            ///// Initializes a new instance of the SalesOrderTransResponse
            ///// </summary>
            ///// <param name="status">status of sales order trans</param>
            ///// <param name="">eCommerce Id of sales order trans</param>
            ///// <param name="message">message of sales order trans</param>

            /// <summary>
            /// Initialize a new instance of the ContractInvoicesResponse 
            /// </summary>
            /// <param name="status">status of contract invoice response</param>
            /// <param name="message">message of contract invoice response</param>
            /// <param name="erpCustInvoiceJourList">erpCustInvoiceJourList of conttact invoice response</param>
            public ContractInvoicesResponse(string status, string message, List<ErpCustInvoiceJour> erpCustInvoiceJourList)
            {
                this.status = status;
                this.ErpCustInvoiceJourList = erpCustInvoiceJourList;
                this.message = message;
            }

            /// <summary>
            /// status of sales order trans
            /// </summary>
            public string status { get; set; }


            /// <summary>
            /// message of sales order trans
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// ErpCustInvoiceJour list
            /// </summary>
            public List<ErpCustInvoiceJour> ErpCustInvoiceJourList { get; set; }

        }

        #endregion

        #region Time Quantity Request Response classes

        /// <summary>
        /// Represents calculate time quantity request.
        /// </summary>
        public class CalculateTimeQuantityRequest
        {
            /// <summary>
            /// CalculateDateFrom
            /// </summary>
            public DateTime CalculateDateFrom { get; set; }
            /// <summary>
            /// ValidTo
            /// </summary>
            public DateTime ValidTo { get; set; }
            /// <summary>
            /// BillingPeriod
            /// </summary>
            public int BillingPeriod { get; set; }
            /// <summary>
            /// IsSubscription
            /// </summary>
            public bool IsSubscription { get; set; }
            /// <summary>
            /// TmvAutoProlongation
            /// </summary>
            public bool TmvAutoProlongation { get; set; }
        }
        /// <summary>
        /// Represents Calculate Time Quantity response
        /// </summary>
        public class CalculateTimeQuantityResponse
        {

            /// <summary>
            /// Initializes a new instance of the TimeQuantityResponse
            /// </summary>
            /// <param name="status">status of sales order trans</param>
            /// <param name="timeQuantity">eCommerce Id of sales order trans</param>
            /// /// <param name="message">message of sales order trans</param>
            public CalculateTimeQuantityResponse(bool status, double timeQuantity, string message)
            {
                this.Status = status;
                this.TimeQuantity = timeQuantity;
                this.Message = message;
            }

            /// <summary>
            /// status
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// timeQuantity
            /// </summary>
            public double TimeQuantity { get; set; }

            /// <summary>
            /// message
            /// </summary>
            public string Message { get; set; }
        }

        /// <summary>
        /// Represents Calculate Line Amount request.
        /// </summary>
        public class CalculateLineAmountRequest
        {
            /// <summary>
            /// CalculateDateFrom
            /// </summary>
            public DateTime CalculateDateFrom { get; set; }
            /// <summary>
            /// ValidTo
            /// </summary>
            public DateTime ValidTo { get; set; }
            /// <summary>
            /// BillingPeriod
            /// </summary>
            public int BillingPeriod { get; set; }
            /// <summary>
            /// SalesQty
            /// </summary>
            public int SalesQty { get; set; }
            /// <summary>
            /// SalesPrice
            /// </summary>
            public double SalesPrice { get; set; }
            /// <summary>
            /// DiscAmount
            /// </summary>
            public double DiscAmount { get; set; }
            /// <summary>
            /// DiscPct
            /// </summary>
            public double DiscPct { get; set; }
            /// <summary>
            /// IsSubscription
            /// </summary>
            public bool IsSubscription { get; set; }
            /// <summary>
            /// TmvAutoProlongation
            /// </summary>
            public bool TmvAutoProlongation { get; set; }
        }
        /// <summary>
        /// Represents Calculate Line Amount response
        /// </summary>
        public class CalculateLineAmountResponse
        {

            /// <summary>
            /// Initializes a new instance of the TimeQuantityResponse
            /// </summary>
            /// <param name="status">status of sales order trans</param>
            /// <param name="lineAmount">eCommerce Id of sales order trans</param>
            /// /// <param name="message">message of sales order trans</param>
            public CalculateLineAmountResponse(bool status, double lineAmount, string message)
            {
                this.Status = status;
                this.LineAmount = lineAmount;
                this.Message = message;
            }

            /// <summary>
            /// status
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// LineAmount
            /// </summary>
            public double LineAmount { get; set; }

            /// <summary>
            /// message
            /// </summary>
            public string Message { get; set; }
        }

        #endregion
        #region ValidationRequests
        /// <summary>
        /// ValidateProductLicense validates ecomLicenseRequest Object. 
        /// </summary>
        /// <param name="ecomLicenseRequest"></param>
        /// <returns></returns>
        //private CLContactPersonResponse ValidateContactPerson(EcomCustomerContactPersonCreateRequest contactPersonRequest, bool isUpdate)
        private LicenseActivationResponse ValidateProductLicense(List<EcomLicenseRequest> ecomLicenseRequest)
        {
            List<LicenseActivationResponse> licenseActivationResponse = new List<LicenseActivationResponse>();
            if (ecomLicenseRequest == null)
            {
                return null;
            }
            else
            {
                foreach (var item in ecomLicenseRequest)
                {

                    if (string.IsNullOrWhiteSpace(item.GUID))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ecomLicenseRequest.GUID");
                        LicenseActivationResponse licenseActivationValidationResponse = new LicenseActivationResponse(false, message, null);
                        return licenseActivationValidationResponse;
                    }
                    else if (string.IsNullOrWhiteSpace(item.ItemId))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ecomLicenseRequest.ItemId");
                        LicenseActivationResponse licenseActivationValidationResponse = new LicenseActivationResponse(false, message, null);
                        return licenseActivationValidationResponse;
                    }
                    else if (item.Quantity == 0 || item.Quantity < 0)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ecomLicenseRequest.Quantity");
                        LicenseActivationResponse licenseActivationValidationResponse = new LicenseActivationResponse(false, message, null);
                        return licenseActivationValidationResponse;
                    }

                    if (!(base.ValidateCustomerLicenseIDLength(item.GUID)))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL400011, currentStore, "");
                        LicenseActivationResponse licenseActivationValidationResponse = new LicenseActivationResponse(false, message, null);
                        return licenseActivationValidationResponse;
                    }
                }

            }
            return null;
        }
        #endregion

        public class ProductLicenseResponse
        {
            /// <param name="GUID">GUID of Create Lincense Transaction</param>
            /// <param name="itemId">item of Create Lincense Transaction</param>
            /// <param name="quantity">quantity of Create Lincense Transaction</param>
            /// <param name="activationLink">Activation Link for Main Product </param>
            public ProductLicenseResponse(string GUID, string itemId, decimal quantity, string activationLink)
            {
                this.guid = GUID;
                this.itemId = itemId;
                this.quantity = quantity;
                this.activationLink = activationLink;
            }
            /// <summary>
            /// GUID for Main Product
            /// </summary>
            public string guid { get; set; }
            /// <summary>
            /// itemId of Main Product
            /// </summary>
            public string itemId { get; set; }
            /// <summary>
            /// Quantity of Main Product
            /// </summary>
            public decimal quantity { get; set; }
            /// <summary>
            /// Activation Link for Main Product
            /// </summary>
            public string activationLink { get; set; }
        }

        /// <summary>
        /// Reactivate Contract Request.
        /// </summary>
        #region Reactivate Conntract 
        public class ReactivateContractRequest
        {
            /// <summary>
            /// PACLicenseList
            /// </summary>
            [Required]
            public string PACLicenseList { get; set; }
            /// <summary>
            /// SubscriptionStartDate
            /// </summary>
            [Required]
            public string SubscriptionStartDate { get; set; }
        }

        #endregion

        #region PriceValidation Response

        public class PriceValidationResponse
        {
            public string LineNumber { get; set; }
            public string ItemId { get; set; }
            public string VariantId { get; set; }
            public decimal TotalAmount { get; set; }
            public string Message { get; set; }
        }
        #endregion
    }
}