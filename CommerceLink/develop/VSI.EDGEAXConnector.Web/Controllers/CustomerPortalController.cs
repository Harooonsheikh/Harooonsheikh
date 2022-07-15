using NewRelic.Api.Agent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Xml.Serialization;
using VSI.CommerceLink.EcomDataModel.Request;
using VSI.CommerceLink.EcomDataModel.Response;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.AosClasses;
using VSI.EDGEAXConnector.ERPDataModels.CalculateContract;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <summary>
    /// CustomerPortalController defines properties and methods for API controller for Customer Portal
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class CustomerPortalController : ApiBaseController
    {
        /// <summary>
        /// Customer Portal Controller
        /// </summary>
        /// <param name="_erpAdapterFactory"></param>
        /// <param name="_ecomAdapterFactory"></param>
        public CustomerPortalController()
        {
            ControllerName = "CustomerPortalController";
        }

        #region API Methods

        [RequestResponseLog]
        [HttpPost]
        [Route("CustomerPortal/Test")]
        public BaseResponse<UserSessionInfo> TestCall()
        {
            try
            {
                var subscrController = erpAdapterFactory.CreateCustomerPortalController(currentStore.StoreKey);
                var responseData = subscrController.TestCall(GetRequestGUID(Request));
                return BaseResponse<UserSessionInfo>.Success(responseData);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return BaseResponse<UserSessionInfo>.Error(message);
            }

        }

        /// <summary>
        /// CreateCreditCard create Credit Cards of Customer with provided details.
        /// </summary>
        /// <param name="eComContractAndPaymentMethodRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("CustomerPortal/CreateContractNewPaymentMethod")]
        public CreateContractNewPaymentMethodResponse CreateContractNewPaymentMethod([FromBody] EcomCreateContractNewPaymentMethodRequest eComContractAndPaymentMethodRequest)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            string validationErrorMessage = ValidateCreateContractNewPaymentMethodRequest(eComContractAndPaymentMethodRequest);

            if (!string.IsNullOrEmpty(validationErrorMessage))
            {
                return new CreateContractNewPaymentMethodResponse(false, validationErrorMessage, null, null,null);
            }

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "CreateContractNewPaymentMethod", DateTime.UtcNow);

            Guid ecomTransactionId = Guid.Empty;
            string methodName = string.Empty;

            try
            {
                ecomTransactionId = GetTransactionIdFromHeader();
                methodName = MethodBase.GetCurrentMethod().Name;
            }
            catch (Exception ex)
            {
                return new CreateContractNewPaymentMethodResponse(false, ex.Message, null, null,null);
            }

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.Select", DateTime.UtcNow);

            ServiceBusRequestLog log = ServiceBusRequestDAL.Select(ecomTransactionId, currentStore.StoreId, methodName);

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.Select", DateTime.UtcNow);

            if (log == null || log.Status == false)
            {

                ErpCreateContractNewPaymentMethod contractAndPaymentMethodRequest = new ErpCreateContractNewPaymentMethod();

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "Mapping", DateTime.UtcNow);

                contractAndPaymentMethodRequest = _mapper.Map<EcomCreateContractNewPaymentMethodRequest, ErpCreateContractNewPaymentMethod>(eComContractAndPaymentMethodRequest);

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "Mapping", DateTime.UtcNow);

                CreateContractNewPaymentMethodResponse contractAndPaymentMethodResponse;
                contractAndPaymentMethodResponse = new CreateContractNewPaymentMethodResponse(false, "", null, null,null);

                ErpCreateContractNewPaymentMethodResponse erpCreateContractNewPaymentMethodResponse = null;

                try
                {
                    var customerController = erpAdapterFactory.CreateCustomerPortalController(currentStore.StoreKey);

                    CreateContractNewPaymentMethodCreditCardResponse creditCard = null;

                    if (!contractAndPaymentMethodRequest.TenderLine.TenderTypeId.ToUpper().Equals("SEPA"))
                    {
                        creditCard = PopulateResposeObject(contractAndPaymentMethodRequest.TenderLine);
                    }

                    erpCreateContractNewPaymentMethodResponse = customerController.CreateNewContractPaymentMethod(contractAndPaymentMethodRequest, GetRequestGUID(Request));

                    if (erpCreateContractNewPaymentMethodResponse.Success)
                    {
                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.InsertOrUpdate", DateTime.UtcNow);

                        ServiceBusRequestDAL.InsertOrUpdate(ecomTransactionId, currentStore.StoreId, true, methodName);

                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.InsertOrUpdate", DateTime.UtcNow);

                        if (erpCreateContractNewPaymentMethodResponse.CreditCard != null)
                        {
                            creditCard.RecId = erpCreateContractNewPaymentMethodResponse.CreditCard.RecId;
                        }

                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCreateContractNewPaymentMethodResponse));
                        contractAndPaymentMethodResponse = new CreateContractNewPaymentMethodResponse(true, erpCreateContractNewPaymentMethodResponse.Message, creditCard, erpCreateContractNewPaymentMethodResponse.BankAccount,erpCreateContractNewPaymentMethodResponse.ErrorCode);
                    }
                    else if (!erpCreateContractNewPaymentMethodResponse.Success)
                    {
                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.InsertOrUpdate", DateTime.UtcNow);

                        ServiceBusRequestDAL.InsertOrUpdate(ecomTransactionId, currentStore.StoreId, false, methodName);

                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.InsertOrUpdate", DateTime.UtcNow);

                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from D365.");
                        contractAndPaymentMethodResponse = new CreateContractNewPaymentMethodResponse(false, erpCreateContractNewPaymentMethodResponse.Message, null, null,erpCreateContractNewPaymentMethodResponse.ErrorCode);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                    contractAndPaymentMethodResponse = new CreateContractNewPaymentMethodResponse(false, message, null, null,null);
                }
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                return contractAndPaymentMethodResponse;
            }
            else
            {
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                return new CreateContractNewPaymentMethodResponse(true,
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL400016, currentStore),
                    null,
                    null,null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateBillingAddressRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("CustomerPortal/UpdateBillingAddress")]
        public UpdateBillingAddressResponse UpdateBillingAddress(UpdateBillingAddressRequest updateBillingAddressRequest)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            UpdateBillingAddressResponse response;
            try
            {
                var portalController = new AXAdapter.Controllers.CustomerPortalController(currentStore.StoreKey);
                response = ValidateUpdateBillingAddressRequest(updateBillingAddressRequest);
                if (response != null)
                {
                    //Validation failed
                    return response;
                }

                Guid ecomTransactionId = Guid.Empty;
                string methodName = string.Empty;

                try
                {
                    ecomTransactionId = GetTransactionIdFromHeader();
                    methodName = MethodBase.GetCurrentMethod().Name;
                }
                catch (Exception ex)
                {
                    return new UpdateBillingAddressResponse(false, ex.Message);
                }

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.Select", DateTime.UtcNow);

                ServiceBusRequestLog log = ServiceBusRequestDAL.Select(ecomTransactionId, currentStore.StoreId, methodName);

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.Select", DateTime.UtcNow);

                if (log == null || log.Status == false)
                {
                    response = portalController.UpdateBillingAddress(updateBillingAddressRequest, GetRequestGUID(Request));

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.InsertOrUpdate", DateTime.UtcNow);

                    ServiceBusRequestDAL.InsertOrUpdate(ecomTransactionId, currentStore.StoreId, response.Status, methodName);

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.InsertOrUpdate", DateTime.UtcNow);

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                    return response;
                }
                else
                {
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                    return new UpdateBillingAddressResponse(true, CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL400016, currentStore));
                }
            }
            catch (Exception ex)
            {
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return response = new UpdateBillingAddressResponse(false, message);
            }
        }

        /// <summary>
        /// Cancel Contract/ContractLine by salesId or salesLineRecId  
        /// </summary>
        /// <param name="processContractTerminateRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("CustomerPortal/ProcessContractTerminate")]
        public ProcessContractTerminateResponse ProcessContractTerminate([FromBody] EcomProcessContractTerminateRequest processContractTerminateRequest)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            ProcessContractTerminateResponse processContractTerminateResponse;
            try
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
                processContractTerminateResponse = this.ValidateProcessContractTerminateRequest(processContractTerminateRequest);

                if (processContractTerminateResponse == null)
                {
                    Guid ecomTransactionId = Guid.Empty;
                    string methodName = string.Empty;

                    try
                    {
                        ecomTransactionId = GetTransactionIdFromHeader();
                        methodName = MethodBase.GetCurrentMethod().Name;
                    }
                    catch (Exception ex)
                    {
                        return new ProcessContractTerminateResponse(false, ex.Message);
                    }

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.Select", DateTime.UtcNow);

                    ServiceBusRequestLog log = ServiceBusRequestDAL.Select(ecomTransactionId, currentStore.StoreId, methodName);

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.Select", DateTime.UtcNow);

                    if (log == null || log.Status == false)
                    {
                        var erpUpdateSalesOrderController =
                                        erpAdapterFactory.CreateCustomerPortalController(currentStore.StoreKey);

                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "Mapping", DateTime.UtcNow);

                        var erpProcessContractTerminateRequest = _mapper.Map<ErpProcessContractTerminateRequest>(processContractTerminateRequest);

                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "Mapping", DateTime.UtcNow);


                        processContractTerminateResponse =
                            erpUpdateSalesOrderController.ProcessContractTerminate(erpProcessContractTerminateRequest,
                                GetRequestGUID(Request));

                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.InsertOrUpdate", DateTime.UtcNow);

                        ServiceBusRequestDAL.InsertOrUpdate(ecomTransactionId, currentStore.StoreId, processContractTerminateResponse.Status, methodName);

                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.InsertOrUpdate", DateTime.UtcNow);

                    }
                    else
                    {
                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                        return new ProcessContractTerminateResponse(true, CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL400016, currentStore));
                    }
                }
                else
                {
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                    //Validation failed
                    return processContractTerminateResponse;
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                processContractTerminateResponse = new ProcessContractTerminateResponse(false, message);
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            }
            return processContractTerminateResponse;
        }

        /// <summary>
        /// Update Subscription Contract
        /// </summary>
        /// <param name="eComUpdateSubscriptionContractRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("CustomerPortal/UpdateSubscriptionContract")]
        public UpdateSubscriptionContractResponse UpdateSubscriptionContract(EcomUpdateSubscriptionContractRequest eComUpdateSubscriptionContractRequest)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            UpdateSubscriptionContractResponse response = new UpdateSubscriptionContractResponse(false, "", null);

            string errorMessage = ValidateUpdateSubscriptionContractRequest(eComUpdateSubscriptionContractRequest);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                response = new UpdateSubscriptionContractResponse(false, errorMessage, null);
                return response;
            }

            Guid ecomTransactionId = Guid.Empty;
            string methodName = string.Empty;

            try
            {
                ecomTransactionId = GetTransactionIdFromHeader();
                methodName = MethodBase.GetCurrentMethod().Name;
            }
            catch (Exception ex)
            {
                return new UpdateSubscriptionContractResponse(false, ex.Message, null);
            }

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10001, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.Select", DateTime.UtcNow);

            ServiceBusRequestLog log = ServiceBusRequestDAL.Select(ecomTransactionId, currentStore.StoreId, methodName);

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.Select", DateTime.UtcNow);

            if (log == null || log.Status == false)
            {
                try
                {
                    ErpUpdateSubscriptionContractRequest erpUpdateSubscriptionContractRequest = _mapper.Map<ErpUpdateSubscriptionContractRequest>(eComUpdateSubscriptionContractRequest);

                    var portalController = erpAdapterFactory.CreateCustomerPortalController(currentStore.StoreKey);

                    ErpUpdateSubscriptionContract updateSubscriptionContract = PopulateUpdateSubscriptionContractRequestObject(erpUpdateSubscriptionContractRequest);

                    ErpUpdateSubscriptionContractResponse erpResponse = portalController.UpdateSubscriptionContract(updateSubscriptionContract, GetRequestGUID(Request));

                    response = _mapper.Map<UpdateSubscriptionContractResponse>(erpResponse);

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.InsertOrUpdate", DateTime.UtcNow);

                    ServiceBusRequestDAL.InsertOrUpdate(ecomTransactionId, currentStore.StoreId, response.Status, methodName);

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.InsertOrUpdate", DateTime.UtcNow);

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                    return response;
                }
                catch (Exception ex)
                {
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                    return response = new UpdateSubscriptionContractResponse(false, message, null);
                }
            }
            else
            {
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                return new UpdateSubscriptionContractResponse(true, CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL400016, currentStore), null);
            }
        }

        /// <summary>
        /// CalculateSubscriptionChange creates a cart, adds cartlines and applys coupon on cart with provided details to calculate contract totals.
        /// </summary>
        /// <param name="contractCalculationRequest"></param>
        /// <returns>ContractCalculationResponse</returns>

        [RequestResponseLog]
        [HttpPost]
        [Route("CustomerPortal/CalculateSubscriptionChange")]
        public ContractCalculationResponse CalculateSubscriptionChange([FromBody] ContractCalculationRequest contractCalculationRequest)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            ContractCalculationResponse cartResponse = new ContractCalculationResponse(false, "", null);
            try
            {
                cartResponse = this.ValidateContractCalculationRequest(contractCalculationRequest);
                if (cartResponse != null)
                {
                    return cartResponse;
                }
                else
                {
                    List<string> coupons = new List<string>();
                    if (contractCalculationRequest.CouponCodes != null)
                    {
                        foreach (CLCoupon coupon in contractCalculationRequest.CouponCodes)
                        {
                            coupons.Add(coupon.Code);
                        }
                    }

                    var customerPortalController = erpAdapterFactory.CreateCustomerPortalController(currentStore.StoreKey);

                    string subWeight = contractCalculationRequest.TMVSubscriptionWeight.Replace('-', '_');
                    CLSubscriptionOfferType tmvSubscriptionWeight = (CLSubscriptionOfferType)Enum.Parse(typeof(CLSubscriptionOfferType), subWeight);

                    cartResponse = customerPortalController.CalculateSubscriptionChange(contractCalculationRequest.UseOldContractDates, contractCalculationRequest.TMVContractStartDate, contractCalculationRequest.TMVContractEndDate, tmvSubscriptionWeight, contractCalculationRequest.RequestDate, contractCalculationRequest.AffiliationId, contractCalculationRequest.CartLines, contractCalculationRequest.DeliverySpecification, coupons, GetRequestGUID(Request));

                    if (cartResponse.Status)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(cartResponse.Cart));
                        cartResponse = new ContractCalculationResponse(cartResponse.Status, cartResponse.ErrorMessage, cartResponse.Cart);
                    }
                    else
                    {
                        cartResponse = new ContractCalculationResponse(cartResponse.Status, cartResponse.ErrorMessage, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new ContractCalculationResponse(false, message, null);
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                return cartResponse;
            }

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            return cartResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unblockContractRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("CustomerPortal/UnblockContract")]
        public UnblockContractResponse UnblockContract(UnblockContractRequest unblockContractRequest)
        {
            UnblockContractResponse response;
            try
            {
                var portalController = new AXAdapter.Controllers.CustomerPortalController(currentStore.StoreKey);

                if (!ModelState.IsValid)
                {
                    response = new UnblockContractResponse(false, GetModelErrors());
                    //Validation failed
                    return response;
                }

                Guid ecomTransactionId = Guid.Empty;
                string methodName = string.Empty;

                try
                {
                    ecomTransactionId = GetTransactionIdFromHeader();
                    methodName = MethodBase.GetCurrentMethod().Name;
                }
                catch (Exception ex)
                {
                    return new UnblockContractResponse(false, ex.Message);
                }

                ServiceBusRequestLog log = ServiceBusRequestDAL.Select(ecomTransactionId, currentStore.StoreId, methodName);

                if (log == null || log.Status == false)
                {
                    response = portalController.UnblockContract(unblockContractRequest, GetRequestGUID(Request));

                    ServiceBusRequestDAL.InsertOrUpdate(ecomTransactionId, currentStore.StoreId, response.Status, methodName);

                    return response;
                }
                else
                {
                    return new UnblockContractResponse(true, CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL400016, currentStore));
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return response = new UnblockContractResponse(false, message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assignCustomerPortalAccessRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("CustomerPortal/AssignCustomerPortalAccess")]
        public AssignCustomerPortalAccessResponse AssignCustomerPortalAccess(AssignCustomerPortalAccessRequest assignCustomerPortalAccessRequest)
        {
            AssignCustomerPortalAccessResponse response;
            try
            {
                var portalController = new AXAdapter.Controllers.CustomerPortalController(currentStore.StoreKey);

                if (assignCustomerPortalAccessRequest == null
                    || (String.IsNullOrEmpty(assignCustomerPortalAccessRequest.PACLicense) &&
                        String.IsNullOrEmpty(assignCustomerPortalAccessRequest.InvoiceId))
                    || (!String.IsNullOrEmpty(assignCustomerPortalAccessRequest.PACLicense) &&
                        !String.IsNullOrEmpty(assignCustomerPortalAccessRequest.InvoiceId)))
                {
                    //Validation failed
                    response = new AssignCustomerPortalAccessResponse(false,
                        CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL400018));
                    return response;
                }

                Guid ecomTransactionId = Guid.Empty;
                string methodName = string.Empty;

                try
                {
                    ecomTransactionId = GetTransactionIdFromHeader();
                    methodName = MethodBase.GetCurrentMethod().Name;
                }
                catch (Exception ex)
                {
                    return new AssignCustomerPortalAccessResponse(false, ex.Message);
                }

                ServiceBusRequestLog log = ServiceBusRequestDAL.Select(ecomTransactionId, currentStore.StoreId, methodName);

                if (log == null || log.Status == false)
                {
                    response = portalController.AssignCustomerPortalAccess(assignCustomerPortalAccessRequest, GetRequestGUID(Request));

                    ServiceBusRequestDAL.InsertOrUpdate(ecomTransactionId, currentStore.StoreId, response.Status, methodName);

                    return response;
                }
                else
                {
                    return new AssignCustomerPortalAccessResponse(true, CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL400016, currentStore));
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return response = new AssignCustomerPortalAccessResponse(false, message);
            }
        }

        /// <summary>
        /// Trigger Data Sync of Customer.
        /// </summary>
        /// <param name="request">Customer to trigger data sync.</param>
        /// <returns>TriggerDataSyncResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("CustomerPortal/TriggerDataSync")]
        public TriggerDataSyncResponse TriggerDataSync([FromBody] TriggerDataSyncRequest request)
        {
            var response = new TriggerDataSyncResponse(false, string.Empty, string.Empty, string.Empty);
            try
            {
                response = ValidateTriggerDataSyncRequest(request);
                if (response != null)
                {
                    return response;
                }

                string requestXML = string.Empty;

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TriggerDataSyncRequest));

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, request);
                    requestXML = textWriter.ToString();
                }

                var erpCustomerController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);
                var clResponse = erpCustomerController.TriggerDataSync(requestXML);

                if (clResponse.Status)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(clResponse));
                    response = new TriggerDataSyncResponse(true, clResponse.Code, clResponse.Message, clResponse.Email);
                }
                else if (!clResponse.Status)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, clResponse.Message);
                    response = new TriggerDataSyncResponse(false, clResponse.Code, clResponse.Message, clResponse.Email);
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                response = new TriggerDataSyncResponse(false, string.Empty, message, string.Empty);
                return response;
            }

            return response;
        }

        private TriggerDataSyncResponse ValidateTriggerDataSyncRequest(TriggerDataSyncRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new TriggerDataSyncResponse(false, string.Empty, message, string.Empty);
                }
                else if (string.IsNullOrWhiteSpace(request.EntityType.ToString()))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "EntityType");
                    return new TriggerDataSyncResponse(false, string.Empty, message, string.Empty);
                }
                else if (!Enum.IsDefined(typeof(TriggerDataSyncEntityTypes), request.EntityType.ToString()))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "EntityType");
                    return new TriggerDataSyncResponse(false, string.Empty, message, string.Empty);
                }
                else if (request.EntityType == TriggerDataSyncEntityTypes.None)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "EntityType");
                    return new TriggerDataSyncResponse(false, string.Empty, message, string.Empty);
                }
                else if (string.IsNullOrWhiteSpace(request.EntityId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "EntityId");
                    return new TriggerDataSyncResponse(false, string.Empty, message, string.Empty);
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new TriggerDataSyncResponse(false, string.Empty, message, string.Empty);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("CustomerPortal/ContractActivationLog")]
        public ContractActivationLogResponse ContractActivationLog(ContractActivationLogRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new ContractActivationLogResponse(false, GetModelErrors(), string.Empty);

                var portalController = new AXAdapter.Controllers.CustomerPortalController(currentStore.StoreKey);
                return portalController.ContractActivationLog(request, GetRequestGUID(Request));
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new ContractActivationLogResponse(false, message, string.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("CustomerPortal/PromiseToPay")]
        public PromiseToPayResponse PromiseToPay(PromiseToPayRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new PromiseToPayResponse(false, GetModelErrors());
                }

                var portalController = new AXAdapter.Controllers.CustomerPortalController(currentStore.StoreKey);
                return portalController.PromiseToPay(request, GetRequestGUID(Request));
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new PromiseToPayResponse(false, message);
            }
        }

        /// <summary>
        /// Cancel Contract/ContractLine by salesId or salesLineRecId  
        /// </summary>
        /// <param name="processContractReactivateRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("CustomerPortal/ProcessContractReactivate")]
        public ProcessContractReactivateResponse ProcessContractReactivate([FromBody] EcomProcessContractReactivateRequest processContractReactivateRequest)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            ProcessContractReactivateResponse processContractReactivateResponse;
            try
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
                processContractReactivateResponse = this.ValidateProcessContractReactivateRequest(processContractReactivateRequest);

                if (processContractReactivateResponse == null)
                {
                    Guid ecomTransactionId = Guid.Empty;
                    string methodName = string.Empty;

                    try
                    {
                        ecomTransactionId = GetTransactionIdFromHeader();
                        methodName = MethodBase.GetCurrentMethod().Name;
                    }
                    catch (Exception ex)
                    {
                        return new ProcessContractReactivateResponse(false, ex.Message);
                    }

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.Select", DateTime.UtcNow);

                    ServiceBusRequestLog log = ServiceBusRequestDAL.Select(ecomTransactionId, currentStore.StoreId, methodName);

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.Select", DateTime.UtcNow);

                    if (log == null || log.Status == false)
                    {
                        var erpUpdateSalesOrderController =
                                        erpAdapterFactory.CreateCustomerPortalController(currentStore.StoreKey);

                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "Mapping", DateTime.UtcNow);

                        var erpProcessContractReactivateRequest = _mapper.Map<ErpProcessContractReactivateRequest>(processContractReactivateRequest);

                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "Mapping", DateTime.UtcNow);


                        processContractReactivateResponse =
                            erpUpdateSalesOrderController.ProcessContractReactivate(erpProcessContractReactivateRequest,
                                GetRequestGUID(Request));

                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.InsertOrUpdate", DateTime.UtcNow);

                        ServiceBusRequestDAL.InsertOrUpdate(ecomTransactionId, currentStore.StoreId, processContractReactivateResponse.Status, methodName);

                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "ServiceBusRequestDAL.InsertOrUpdate", DateTime.UtcNow);

                    }
                    else
                    {
                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                        return new ProcessContractReactivateResponse(true, CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL400016, currentStore));
                    }
                }
                else
                {
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                    //Validation failed
                    return processContractReactivateResponse;
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                processContractReactivateResponse = new ProcessContractReactivateResponse(false, message);
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            }
            return processContractReactivateResponse;
        }

        #endregion

        #region Supporing methods

        private string GetModelErrors()
        {
            return String.Join(". ", ModelState.Values.SelectMany(a => a.Errors.Select(x => x.ErrorMessage)));
        }
        private UpdateBillingAddressResponse ValidateUpdateBillingAddressRequest(UpdateBillingAddressRequest updateBillingAddressRequest)
        {
            var error = new StringBuilder();

            if (updateBillingAddressRequest == null || updateBillingAddressRequest.BillingAddress == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                error.AppendLine(message);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(updateBillingAddressRequest.FirstName) && updateBillingAddressRequest.FirstName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "FirstName", "25");
                    error.AppendLine(message);
                }

                if (!string.IsNullOrWhiteSpace(updateBillingAddressRequest.LastName) && updateBillingAddressRequest.LastName.Length > 25)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "LastName", "25");
                    error.AppendLine(message);
                }

                if (string.IsNullOrWhiteSpace(updateBillingAddressRequest.CustomerEmail))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                        MethodBase.GetCurrentMethod().Name, "CustomerEmail"));
                }

                if (!string.IsNullOrWhiteSpace(updateBillingAddressRequest.CustomerEmail) && updateBillingAddressRequest.CustomerEmail.Length > 255)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "CustomerEmail", "255");
                    error.AppendLine(message);
                }

                if (string.IsNullOrWhiteSpace(updateBillingAddressRequest.BillingAddress.Street))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                        MethodBase.GetCurrentMethod().Name, "BillingAddress.Street"));
                }

                if (string.IsNullOrWhiteSpace(updateBillingAddressRequest.BillingAddress.City))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                        MethodBase.GetCurrentMethod().Name, "BillingAddress.City"));
                }

                if (string.IsNullOrWhiteSpace(updateBillingAddressRequest.BillingAddress.ZipCode))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                        MethodBase.GetCurrentMethod().Name, "BillingAddress.ZipCode"));
                }

                if (!string.IsNullOrWhiteSpace(updateBillingAddressRequest.BillingAddress.ZipCode) && updateBillingAddressRequest.BillingAddress.ZipCode.Length > 10)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "BillingAddress.ZipCode", "10");
                    error.AppendLine(message);
                }

                if (string.IsNullOrWhiteSpace(updateBillingAddressRequest.BillingAddress.ThreeLetterISORegionName))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                        MethodBase.GetCurrentMethod().Name, "BillingAddress.ThreeLetterISORegionName"));
                }

                if (!string.IsNullOrWhiteSpace(updateBillingAddressRequest.BillingAddress.ZipCode) && updateBillingAddressRequest.BillingAddress.ThreeLetterISORegionName.Length > 3)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "BillingAddress.ThreeLetterISORegionName", "3");
                    error.AppendLine(message);
                }

                var errorMessage = error.ToString();
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return new UpdateBillingAddressResponse(false, errorMessage);
                }
            }

            return null;
        }
        private ContractCalculationResponse ValidateContractCalculationRequest(ContractCalculationRequest contractCalculationRequest)
        {
            if (contractCalculationRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new ContractCalculationResponse(false, message, null);
            }
            else
            {
                if (string.IsNullOrEmpty(contractCalculationRequest.SalesOrderId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "SalesOrderId");
                    return new ContractCalculationResponse(false, message, null);
                }
                else if (contractCalculationRequest.TMVContractStartDate == DateTimeOffset.MinValue)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "TMVContractStartDate");
                    return new ContractCalculationResponse(false, message, null);
                }
                else if (contractCalculationRequest.TMVContractEndDate == DateTimeOffset.MinValue)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "TMVContractEndDate");
                    return new ContractCalculationResponse(false, message, null);
                }
                else if (string.IsNullOrEmpty(contractCalculationRequest.TMVSubscriptionWeight))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "TMVSubscriptionWeight");
                    return new ContractCalculationResponse(false, message, null);
                }
                else if (!string.IsNullOrEmpty(contractCalculationRequest.TMVSubscriptionWeight))
                {
                    string tmvSubscriptionWeight = contractCalculationRequest.TMVSubscriptionWeight.Replace('-', '_');

                    if (!Enum.IsDefined(typeof(CLSubscriptionOfferType), tmvSubscriptionWeight))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "TMVSubscriptionWeight");
                        return new ContractCalculationResponse(false, message, null);
                    }
                }
                else if (contractCalculationRequest.RequestDate == DateTimeOffset.MinValue)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "RequestDate");
                    return new ContractCalculationResponse(false, message, null);
                }
                else if (contractCalculationRequest.CartLines == null || contractCalculationRequest.CartLines.Count == 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CartLines");
                    return new ContractCalculationResponse(false, message, null);
                }

                if (contractCalculationRequest.CartLines != null)
                {
                    for (int index = 0; index < contractCalculationRequest.CartLines.Count; index++)
                    {
                        CLContractCartLine cartLine = contractCalculationRequest.CartLines[index];

                        if (string.IsNullOrEmpty(cartLine.Description))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CartLines[" + index.ToString() + "].Description");
                            return new ContractCalculationResponse(false, message, null);
                        }
                        else if (string.IsNullOrEmpty(cartLine.ItemId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CartLines[" + index.ToString() + "].ItemId");
                            return new ContractCalculationResponse(false, message, null);
                        }
                        else if (string.IsNullOrEmpty(cartLine.LineId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CartLines[" + index.ToString() + "].LineId");
                            return new ContractCalculationResponse(false, message, null);
                        }
                        else if (cartLine.Quantity < 1)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CartLines[" + index.ToString() + "].Quantity");
                            return new ContractCalculationResponse(false, message, null);
                        }
                        else if (string.IsNullOrEmpty(cartLine.UnitOfMeasureSymbol))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CartLines[" + index.ToString() + "].UnitOfMeasureSymbol");
                            return new ContractCalculationResponse(false, message, null);
                        }
                        else if (cartLine.TMVContractCalculateFrom == DateTimeOffset.MinValue)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CartLines[" + index.ToString() + "].TMVContractCalculateFrom");
                            return new ContractCalculationResponse(false, message, null);
                        }
                        else if (cartLine.TMVContractCalculateTo == DateTimeOffset.MinValue)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CartLines[" + index.ToString() + "].TMVContractCalculateTo");
                            return new ContractCalculationResponse(false, message, null);
                        }
                        else if (cartLine.CLSalesLineAction != CLContractOperation.Existing &&
                            cartLine.CLSalesLineAction != CLContractOperation.New &&
                            cartLine.CLSalesLineAction != CLContractOperation.QuantityUpgrade &&
                            cartLine.CLSalesLineAction != CLContractOperation.Switch
                            )
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CartLines[" + index.ToString() + "].CLSalesLineAction");
                            return new ContractCalculationResponse(false, message, null);
                        }
                        else if (string.IsNullOrEmpty(cartLine.CLParentLineNumber))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CartLines[" + index.ToString() + "].CLParentLineNumber");
                            return new ContractCalculationResponse(false, message, null);
                        }
                        else if (cartLine.CLSalesLineAction == CLContractOperation.Switch && string.IsNullOrEmpty(cartLine.CLSwitchFromLineId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CartLines[" + index.ToString() + "].CLSwitchFromLineId");
                            return new ContractCalculationResponse(false, message, null);
                        }
                    }
                }

                if (contractCalculationRequest.DeliverySpecification == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "DeliverySpecification");
                    return new ContractCalculationResponse(false, message, null);
                }
                else if (contractCalculationRequest.DeliverySpecification != null)
                {
                    if (contractCalculationRequest.DeliverySpecification.DeliveryAddress == null)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "DeliveryAddress");
                        return new ContractCalculationResponse(false, message, null);
                    }
                    else if (string.IsNullOrEmpty(contractCalculationRequest.DeliverySpecification.DeliveryAddress.TaxGroup))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "DeliveryAddress.TaxGroup");
                        return new ContractCalculationResponse(false, message, null);
                    }
                }
            }
            return null;
        }

        private string ValidateCreateContractNewPaymentMethodRequest(EcomCreateContractNewPaymentMethodRequest request)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            string errorMessage = "";
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    errorMessage = message;
                    return errorMessage;
                }

                if (request.TransactionDate == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TransactionDate");
                    errorMessage = message;
                    return errorMessage;
                }

                if (request.Customer == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.Customer");
                    errorMessage = message;
                    return errorMessage;
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(request.Customer.CustomerId))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.Customer.CustomerId");
                        errorMessage = message;
                        return errorMessage;
                    }
                    else if (String.IsNullOrWhiteSpace(request.Customer.Name))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.Customer.Name");
                        errorMessage = message;
                        return errorMessage;
                    }
                    else if (String.IsNullOrWhiteSpace(request.Customer.Email))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.Customer.Email");
                        errorMessage = message;
                        return errorMessage;
                    }
                }

                if (request.TenderLine == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine");
                    errorMessage = message;
                    return errorMessage;
                }
                else
                {
                    if (request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.PAYPAL_EXPRESS.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.TenderLine.PayerId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.PayerId");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.Authorization))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.Authorization");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.CardToken))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.CardToken");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.CardTypeId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.CardTypeId");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.ThreeDSecure))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.ThreeDSecure");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.CardOrAccount))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.CardOrAccount");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (request.TenderLine.ExpMonth == null || request.TenderLine.ExpMonth < 1 || request.TenderLine.ExpMonth > 12)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.ExpMonth");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (request.TenderLine.ExpYear == null || request.TenderLine.ExpYear < 0)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.ExpYear");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.PayerId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.PayerId");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.ParentTransactionId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.ParentTransactionId");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.Email))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.Email");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.Note))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.Note");
                            errorMessage = message;
                            return errorMessage;
                        }
                    }
                    else if (request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.SEPA.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.TenderLine.CardOrAccount))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.CardOrAccount");
                            errorMessage = message;
                            return errorMessage;
                        }

                        else if (string.IsNullOrWhiteSpace(request.TenderLine.SwiftCode))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.SwiftCode");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.IBAN))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.IBAN");
                            errorMessage = message;
                            return errorMessage;
                        }
                        // TODO have to check if Bank Name is mandatory or not?
                        //else if (string.IsNullOrWhiteSpace(request.TenderLine.BankName))
                        //{
                        //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.BankName");
                        //    errorMessage = message;
                        //    return errorMessage;
                        //}
                    }
                    else // Credit Card
                    {
                        if (string.IsNullOrWhiteSpace(request.TenderLine.MaskedCardNumber))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.CardNumber");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.Authorization))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.Authorization");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.CardToken))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.CardToken");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (request.TenderLine.ExpMonth == null || request.TenderLine.ExpMonth < 1 || request.TenderLine.ExpMonth > 12)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.ExpMonth");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (request.TenderLine.ExpYear == null || request.TenderLine.ExpYear < 0)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.ExpYear");
                            errorMessage = message;
                            return errorMessage;
                        }
                        else if (string.IsNullOrWhiteSpace(request.TenderLine.CardTypeId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.CardTypeId");
                            errorMessage = message;
                            return errorMessage;
                        }
                        //if (string.IsNullOrWhiteSpace(request.TenderLine.TransactionId))
                        //{
                        //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.Payment.TransactionId");
                        //    errorMessage = message;
                        //}

                        if (request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.ALLPAGO_CC.ToString()))
                        {
                            if (string.IsNullOrWhiteSpace(request.TenderLine.Email))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.Email");
                                errorMessage = message;
                                return errorMessage;
                            }
                            if (string.IsNullOrWhiteSpace(request.TenderLine.TransactionId))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.TransactionId");
                                errorMessage = message;
                                return errorMessage;
                            }
                            if (request.TenderLine.NumberOfInstallments == null || request.TenderLine.NumberOfInstallments < 1)
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.NumberOfInstallments");
                                errorMessage = message;
                                return errorMessage;
                            }
                            if (string.IsNullOrWhiteSpace(request.TenderLine.ApprovalCode))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.ApprovalCode");
                                errorMessage = message;
                                return errorMessage;
                            }
                            if (configurationHelper.GetSetting(CUSTOMER.Default_ThreeLetterISORegionName) == "BRA")
                            {
                                if (string.IsNullOrWhiteSpace(request.TenderLine.LocalTaxId))
                                {
                                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.LocalTaxId");
                                    errorMessage = message;
                                    return errorMessage;
                                }
                            }
                            if (string.IsNullOrWhiteSpace(request.TenderLine.IP))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.IP");
                                errorMessage = message;
                                return errorMessage;
                            }

                        }
                    }

                    if (string.IsNullOrWhiteSpace(request.TenderLine.CardOrAccount))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateContractNewPaymentMethod.TenderLine.CardOrAccount");
                        errorMessage = message;
                        return errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                errorMessage = message;
                return errorMessage;
            }

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            return errorMessage;
        }

        [Trace]
        private string ValidateUpdateSubscriptionContractRequest(EcomUpdateSubscriptionContractRequest request)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            string result = string.Empty;

            if (request == null)
            {
                result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                if (string.IsNullOrEmpty(request.RequestNumber) || string.IsNullOrWhiteSpace(request.RequestNumber))
                {
                    result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.RequestNumber");
                    return result;
                }
                else if (request.RequestDate == DateTime.MinValue)
                {
                    result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.RequestDate");
                    return result;
                }
                else if (string.IsNullOrEmpty(request.RequestNumber) || string.IsNullOrWhiteSpace(request.RequestNumber))
                {
                    result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.RequestNumber");
                    return result;
                }
                else if (string.IsNullOrEmpty(request.ContractAction) || string.IsNullOrWhiteSpace(request.ContractAction))
                {
                    result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.ContractAction");
                    return result;
                }
                else if (!(request.ContractAction.Equals(ErpTMVCrosssellType.ContractUpdate.ToString()) || request.ContractAction.Equals(ErpTMVCrosssellType.Switch.ToString()) || request.ContractAction.Equals(ErpTMVCrosssellType.QuantityUpgrade.ToString())))
                {
                    result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "request.ContractAction");
                    return result;
                }
                else if (string.IsNullOrEmpty(request.PrimaryPacLicense) || string.IsNullOrWhiteSpace(request.PrimaryPacLicense))
                {
                    result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.PrimaryPacLicense");
                    return result;
                }
                else if (request.ContractLines == null || request.ContractLines.Count == 0)
                {
                    result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.ContractLines");
                    return result;
                }
                else
                {
                    for (int index = 0; index < request.ContractLines.Count; index++)
                    {
                        if (request.ContractLines[index].LineNumber < 0)
                        {
                            result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.ContractLines[" + index.ToString() + "].LineNumber");
                            return result;
                        }
                        else if (request.ContractLines[index].TargetPrice < 0)
                        {
                            result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.ContractLines[" + index.ToString() + "].TargetPrice");
                            return result;
                        }
                        else if (request.ContractLines[index].TaxAmount < 0)
                        {
                            result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.ContractLines[" + index.ToString() + "].TaxAmount");
                            return result;
                        }
                        else if (string.IsNullOrEmpty(request.ContractLines[index].ProductId) || string.IsNullOrWhiteSpace(request.ContractLines[index].ProductId))
                        {
                            result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.ContractLines[" + index.ToString() + "].ProductId");
                            return result;
                        }
                        else if (request.ContractLines[index].Quantity < 0)
                        {
                            result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.ContractLines[" + index.ToString() + "].Quantity");
                            return result;
                        }
                        else if (string.IsNullOrEmpty(request.ContractLines[index].SalesLineAction) || string.IsNullOrWhiteSpace(request.ContractLines[index].SalesLineAction))
                        {
                            result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.ContractLines[" + index.ToString() + "].SalesLineAction");
                            return result;
                        }
                        else if (!(request.ContractLines[index].SalesLineAction.Equals(ErpTMVCrosssellType.New.ToString()) ||
                            request.ContractLines[index].SalesLineAction.Equals(ErpTMVCrosssellType.QuantityUpgrade.ToString()) ||
                            request.ContractLines[index].SalesLineAction.Equals(ErpTMVCrosssellType.Switch.ToString())))
                        {
                            result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "request.ContractAction");
                            return result;
                        }
                        else if (
                            (string.IsNullOrEmpty(request.ContractLines[index].OldLinePacLicense) ||
                                string.IsNullOrWhiteSpace(request.ContractLines[index].OldLinePacLicense)) &&
                            !request.ContractLines[index].SalesLineAction.Equals(ErpTMVCrosssellType.New.ToString()) &&
                            request.ContractLines[index].ParentLineNumber.Equals(0)
                            )
                        {
                            result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.ContractLines[" + index.ToString() + "].OldLinePacLicense");
                            return result;
                        }
                        else if (request.ContractLines[index].ParentLineNumber < 0)
                        {
                            result = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.ContractLines[" + index.ToString() + "].ParentLineNumber");
                            return result;
                        }
                    }
                }
            }

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            return result;
        }

        private CreateContractNewPaymentMethodCreditCardResponse PopulateResposeObject(ErpTenderLine tenderLine)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            CreateContractNewPaymentMethodCreditCardResponse creditCardResponse = new CreateContractNewPaymentMethodCreditCardResponse();

            creditCardResponse.RecId = tenderLine.RecId;
            creditCardResponse.TenderTypeId = tenderLine.TenderTypeId;
            creditCardResponse.Amount = tenderLine.Amount;
            creditCardResponse.Authorization = tenderLine.Authorization;
            creditCardResponse.CardToken = tenderLine.CardToken;
            creditCardResponse.CardTypeId = tenderLine.CardTypeId;
            creditCardResponse.MaskedCardNumber = tenderLine.MaskedCardNumber;
            creditCardResponse.ThreeDSecure = tenderLine.ThreeDSecure;
            creditCardResponse.CardOrAccount = tenderLine.CardOrAccount;
            creditCardResponse.ExpMonth = tenderLine.ExpMonth;
            creditCardResponse.ExpYear = tenderLine.ExpYear;
            creditCardResponse.ParentTransactionId = tenderLine.ParentTransactionId;
            creditCardResponse.PayerId = tenderLine.PayerId;
            creditCardResponse.email = tenderLine.Email;
            creditCardResponse.note = tenderLine.Note;

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            return creditCardResponse;
        }

        private ProcessContractTerminateResponse ValidateProcessContractTerminateRequest(EcomProcessContractTerminateRequest processContractTerminateRequest)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            ProcessContractTerminateResponse processContractTerminateResponse = null;
            StringBuilder error = new StringBuilder();

            if (processContractTerminateRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                error.AppendLine(message);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(processContractTerminateRequest.SalesOrderId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                        MethodBase.GetCurrentMethod().Name, "SalesOrderId");
                    error.AppendLine(message);
                }

                if (processContractTerminateRequest.IsLineOperation &&
                    (processContractTerminateRequest.SalesLineRecIds == null || processContractTerminateRequest.SalesLineRecIds.Count == 0))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                        MethodBase.GetCurrentMethod().Name, "SalesLineRecIds");
                    error.AppendLine(message);
                }

                //if (string.IsNullOrWhiteSpace(processContractTerminateRequest.ReasonId))
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                //        MethodBase.GetCurrentMethod().Name, "ReasonId");
                //    error.AppendLine(message);
                //}

                if (string.IsNullOrWhiteSpace(processContractTerminateRequest.ReasonCode))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                        MethodBase.GetCurrentMethod().Name, "ReasonCode");
                    error.AppendLine(message);
                }
            }

            var errorMessage = error.ToString();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                processContractTerminateResponse = new ProcessContractTerminateResponse(false, errorMessage);
            }

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            return processContractTerminateResponse;
        }

        private ProcessContractReactivateResponse ValidateProcessContractReactivateRequest(EcomProcessContractReactivateRequest processContractReactivateRequest)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            ProcessContractReactivateResponse processContractReactivateResponse = null;
            StringBuilder error = new StringBuilder();

            if (processContractReactivateRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                error.AppendLine(message);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(processContractReactivateRequest.SalesOrderId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                        MethodBase.GetCurrentMethod().Name, "SalesOrderId");
                    error.AppendLine(message);
                }

                if (processContractReactivateRequest.IsLineOperation &&
                    (processContractReactivateRequest.SalesLineRecIds == null || processContractReactivateRequest.SalesLineRecIds.Count == 0))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                        MethodBase.GetCurrentMethod().Name, "SalesLineRecIds");
                    error.AppendLine(message);
                }

            }

            var errorMessage = error.ToString();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                processContractReactivateResponse = new ProcessContractReactivateResponse(false, errorMessage);
            }

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            return processContractReactivateResponse;
        }
        [Trace]
        private ErpUpdateSubscriptionContract PopulateUpdateSubscriptionContractRequestObject(ErpUpdateSubscriptionContractRequest updateSubscriptionContractRequest)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            ErpUpdateSubscriptionContract updateSubscriptionContract = new ErpUpdateSubscriptionContract();

            updateSubscriptionContract.RequestNumber = updateSubscriptionContractRequest.RequestNumber;
            updateSubscriptionContract.RequestDate = updateSubscriptionContractRequest.RequestDate.ToString("yyyy-MM-dd");
            updateSubscriptionContract.ContractAction = updateSubscriptionContractRequest.ContractAction;
            updateSubscriptionContract.PrimaryPACLicense = updateSubscriptionContractRequest.PrimaryPacLicense;
            updateSubscriptionContract.UseOldContractDates = updateSubscriptionContractRequest.UseOldContractDates;
            updateSubscriptionContract.CustomerReference = updateSubscriptionContractRequest.CustomerReference;

            string commaSeperatedCouponCodes = "";
            updateSubscriptionContractRequest.CouponCodes.ForEach(k => commaSeperatedCouponCodes += k.Code.ToString() + ",");
            if (commaSeperatedCouponCodes.Length > 0)
            {
                commaSeperatedCouponCodes = commaSeperatedCouponCodes.Substring(0, commaSeperatedCouponCodes.Length - 1);
            }
            updateSubscriptionContract.CouponCodes = commaSeperatedCouponCodes;

            foreach (ErpContractLine contractLine in updateSubscriptionContractRequest.ContractLines)
            {
                ErpContractLine newContractLine = new ErpContractLine();

                newContractLine.LineNumber = contractLine.LineNumber;
                newContractLine.TargetPrice = contractLine.TargetPrice;
                newContractLine.TaxAmount = contractLine.TaxAmount;
                newContractLine.ProductId = contractLine.ProductId;
                newContractLine.ItemId = contractLine.ItemId;
                newContractLine.VariantId = contractLine.VariantId;
                newContractLine.Quantity = contractLine.Quantity;
                newContractLine.SalesLineAction = contractLine.SalesLineAction;
                newContractLine.OldLinePacLicense = contractLine.OldLinePacLicense;
                newContractLine.ParentLineNumber = contractLine.ParentLineNumber;

                if (updateSubscriptionContract.ContractLines == null)
                {
                    updateSubscriptionContract.ContractLines = new List<ErpContractLine>();
                }

                updateSubscriptionContract.ContractLines.Add(newContractLine);
            }

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            return updateSubscriptionContract;
        }

        #endregion

        #region Request Response classes

        /// <summary>
        /// Represents request of CalculateSubscriptionChange method API call
        /// </summary>
        public class ContractCalculationRequest : CLContractCart
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ContractCalculationRequest()
            {
                this.CouponCodes = new ObservableCollection<CLCoupon>();
            }
        }

        /// <summary>
        /// Represents response of Payment Method in CreateContractNewPaymentMethod API call
        /// </summary>
        public class CreateContractNewPaymentMethodCreditCardResponse
        {
            public long RecId { get; set; }
            public string TenderTypeId { get; set; }
            public decimal? Amount { get; set; }
            public string Authorization { get; set; }
            public string CardToken { get; set; }
            public string CardTypeId { get; set; }
            public string MaskedCardNumber { get; set; }
            public string ThreeDSecure { get; set; }
            public string CardOrAccount { get; set; }
            public int? ExpMonth { get; set; }
            public int? ExpYear { get; set; }
            public string ParentTransactionId { get; set; }
            public string PayerId { get; set; }
            public string email { get; set; }
            public string note { get; set; }
        }

        /// <summary>
        /// Represents response of CreateContractNewPaymentMethod API call
        /// </summary>
        public class CreateContractNewPaymentMethodResponse
        {

            /// <summary>
            /// Initializes a new instance of the CreateContractNewPaymentMethodResponse
            /// </summary>
            /// <param name="status"></param>
            /// <param name="message"></param>
            /// <param name="creditCard"></param>
            /// <param name="bankAccount"></param>
            public CreateContractNewPaymentMethodResponse(bool status, string message, CreateContractNewPaymentMethodCreditCardResponse creditCard, object bankAccount,string errorcode)
            {
                this.Status = status;
                this.Message = message;
                this.CreditCard = creditCard;
                this.BankAccount = bankAccount;
                this.ErrorCode = errorcode;
            }

            public CreateContractNewPaymentMethodResponse()
            {

            }

            /// <summary>
            /// Status of call
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// ErrorMessage from call
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Details of Payment Method
            /// </summary>
            public CreateContractNewPaymentMethodCreditCardResponse CreditCard { get; set; }

            /// <summary>
            /// Details of bank account
            /// </summary>
            public object BankAccount { get; set; }

            /// <summary>
            /// ErrorCode from call
            /// </summary>
            public string ErrorCode { get; set; }

        }

        /// <summary>
        /// TriggerDataSyncRequest
        /// </summary>
        public class TriggerDataSyncRequest
        {
            /// <summary>
            /// Entity type to sync
            /// </summary>
            public TriggerDataSyncEntityTypes EntityType { get; set; }

            /// <summary>
            /// Entity Id
            /// </summary>
            [Required]
            public string EntityId { get; set; }

            /// <summary>
            /// Initial Sync
            /// </summary>
            public bool InitialSync { get; set; }
        }

        /// <summary>
        /// TriggerDataSyncResponse
        /// </summary>
        public class TriggerDataSyncResponse
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="status"></param>
            /// <param name="code"></param>
            /// <param name="message"></param>
            public TriggerDataSyncResponse(bool status, string code, string message, string email)
            {
                Status = status;
                Code = code;
                Message = message;
                Email = email;
            }
            /// <summary>
            /// Status of trigger data sync request
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// Code against trigger data sync request
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Message against trigger data sync request
            /// </summary>
            public string Message { get; set; }
            
            /// <summary>
            /// Message against trigger data sync request
            /// </summary>
            public string Email { get; set; }
        }

        #endregion

    }

}
