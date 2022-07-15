using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using VSI.CommerceLink.EcomDataModel;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.BoletoPayment;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <summary>
    /// QuotationController defines properties and methods for API controller for Quotation.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class QuotationController : ApiBaseController
    {
        /// <summary>
        /// Quotation Controller
        /// </summary>
        /// <param name="_erpAdapterFactory"></param>
        public QuotationController()
        {
            ControllerName = "QuotationController";
        }


        #region API Methods
        /// <summary>
        /// CreateQuotation creates quotation.
        /// </summary>
        /// <param name="createQuotationRequest">Quotation request to create quotation</param>
        /// <returns>CreateCustomerQuotationResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Quotation/CreateQuotation")]
        public CreateCustomerQuotationResponse CreateQuotation([FromBody] CreateCustomerQuotationRequest createQuotationRequest)
        {
            CreateCustomerQuotationResponse response = new CreateCustomerQuotationResponse(false, "", "");
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            try
            {
                const string tmvParentAttribute = "TMVPARENT";
                if (createQuotationRequest == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    response = new CreateCustomerQuotationResponse(false, message, "");
                    return response;
                }
                else if (createQuotationRequest.customerQuotation == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL401106, currentStore, MethodBase.GetCurrentMethod().Name);
                    response = new CreateCustomerQuotationResponse(false, message, "");
                    return response;
                }
                else if (createQuotationRequest.customerQuotation.Items == null || createQuotationRequest.customerQuotation.Items.Count <= 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL401107, currentStore, MethodBase.GetCurrentMethod().Name);
                    response = new CreateCustomerQuotationResponse(false, message, "");
                    return response;
                }
                else
                {
                    if (createQuotationRequest.customerQuotation == null)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customerQuotation");
                        response = new CreateCustomerQuotationResponse(false, message, "");
                        return response;
                    }
                    else if (string.IsNullOrEmpty(createQuotationRequest.IsEcomCustomerId.ToString()) || string.IsNullOrWhiteSpace(createQuotationRequest.IsEcomCustomerId.ToString()))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "IsEcomCustomerId");
                        response = new CreateCustomerQuotationResponse(false, message, "");
                        return response;
                    }

                    foreach (var item in createQuotationRequest.customerQuotation.Items)
                    {
                        if (item.LineNumber < 1)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Quotation.Items.LineNumber");
                            response = new CreateCustomerQuotationResponse(false, message, "");
                            return response;
                        }

                        var custAttribute = item.CustomAttributes.Find(c => c.Key.ToUpper().Equals(tmvParentAttribute));
                        bool isTMVParentValueProvided = false;
                        if (!string.IsNullOrWhiteSpace(custAttribute.Value))
                        {
                            isTMVParentValueProvided = Convert.ToInt32(custAttribute.Value) < 0 ? false : true;
                        }

                        if (!isTMVParentValueProvided)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Quotation.Items.CustomAttributes.TMVPARENT");
                            response = new CreateCustomerQuotationResponse(false, message, "");
                            return response;
                        }
                    }
                }

                ErpCustomerOrderInfo customerQuotation = _mapper.Map<ErpCustomerOrderInfo>(createQuotationRequest.customerQuotation);
                bool isEcomCustomerId = createQuotationRequest.IsEcomCustomerId;

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "customerQuotation: " + JsonConvert.SerializeObject(customerQuotation) + " and isEcomCustomerId: " + isEcomCustomerId.ToString());

                if (isEcomCustomerId)
                {
                    //Extract customer AX account number from Integration database
                    var integrationKey = integrationManager.GetErpKey(Enums.Entities.Customer, customerQuotation.CustomerAccount);
                    if (integrationKey != null)
                    {
                        customerQuotation.CustomerAccount = integrationKey.Description;
                        customerQuotation.CustomerRecordId = integrationKey.ErpKey;
                    }
                    else
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL41101, currentStore, customerQuotation.CustomerAccount);
                        response = new CreateCustomerQuotationResponse(false, message, "");
                        return response;
                    }
                }
                //++ Setting Extenstion Properties 
                SetCustomAttributes(customerQuotation);
                CreateCustomerQuotationResponse requestValidation = TMV_ValidateQuotationRequest(customerQuotation);
                if (!requestValidation.Success)
                {
                    return requestValidation;
                }

                SetQuotationDefaultProperties(customerQuotation);

                var erpQuotationController = erpAdapterFactory.CreateQuotationController(currentStore.StoreKey);
                var erpResponse = erpQuotationController.CreateCustomerQuotation(customerQuotation, currentStore.StoreKey, GetRequestGUID(Request));

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));

                if (erpResponse.Success)
                {
                    if (!string.IsNullOrEmpty(erpResponse.QuotationId))
                    {
                        response = new CreateCustomerQuotationResponse(erpResponse.Success, erpResponse.Message, erpResponse.QuotationId);
                    }
                    //++ Creating Integration Key
                    integrationManager.CreateIntegrationKey(Enums.Entities.Quotation, erpResponse.QuotationId, createQuotationRequest.customerQuotation.ChannelReferenceId, createQuotationRequest.customerQuotation.TransactionId);
                }
                else
                {
                    CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40004, currentStore, MethodBase.GetCurrentMethod().Name);
                    CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, erpResponse.Message);
                    response = new CreateCustomerQuotationResponse(erpResponse.Success, erpResponse.Message, erpResponse.QuotationId);
                    return response;
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                response = new CreateCustomerQuotationResponse(false, message, null);
                return response;
            }
            return response;
        }

        /// <summary>
        /// GetQuotation gets quotation details.
        /// </summary>
        /// <param name="getCustomerQuotationRequest">Quotation Id to get quotation info</param>
        /// <returns>GetCustomerQuotationResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Quotation/GetQuotation")]
        public GetCustomerQuotationResponse GetQuotation([FromBody] GetCustomerQuotationRequest request)
        {
            GetCustomerQuotationResponse customerQuotationResponse = new GetCustomerQuotationResponse(false, "", null, null, null);

            try
            {
                customerQuotationResponse = ValidateGetQuotationRequest(request);

                if (customerQuotationResponse != null)
                {
                    return customerQuotationResponse;
                }
                else
                {

                    var erpQuotationController = erpAdapterFactory.CreateQuotationController(currentStore.StoreKey);
                    var erpResponse = erpQuotationController.GetCustomerQuotation(request.CustAccount, request.Status, request.OfferType, request.QuotationId, currentStore.StoreKey, GetRequestGUID(Request));

                    if (erpResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));
                        customerQuotationResponse = new GetCustomerQuotationResponse(erpResponse.Success, erpResponse.Message, erpResponse.Customer, erpResponse.ContactPerson, erpResponse.Quotations);
                    }
                    else if (!erpResponse.Success)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from D365.");
                        customerQuotationResponse = new GetCustomerQuotationResponse(erpResponse.Success, erpResponse.Message, erpResponse.Customer, erpResponse.ContactPerson, erpResponse.Quotations);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                customerQuotationResponse = new GetCustomerQuotationResponse(false, message, null, null, null);
                return customerQuotationResponse;
            }

            return customerQuotationResponse;
        }


        /// <summary>
        /// GetQuotation gets quotation details.
        /// </summary>
        /// <param name="quotationId">Quotation Id to get quotation info</param>
        /// <returns>GetCustomerQuotationResponse</returns>
        [RequestResponseLog]
        [HttpGet]
        [Route("Quotation/ConfirmCustomerQuotation")]
        [Obsolete("ConfirmCustomerQuotation is deprecated, please use ConfirmCustomerQuotation with POST parameter instead.")]
        public ErpConfirmCustomerQuotationResponse ConfirmCustomerQuotation([FromUri] string quotationId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                if (string.IsNullOrEmpty(quotationId) || string.IsNullOrWhiteSpace(quotationId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "quotationId");
                    return new ErpConfirmCustomerQuotationResponse(false, message, null);
                }

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "quotationId: " + quotationId);
                var erpQuotationController = erpAdapterFactory.CreateQuotationController(currentStore.StoreKey);
                var erpResponse = erpQuotationController.ConfirmCustomerQuotation(quotationId, currentStore.StoreKey, GetRequestGUID(Request));

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));
                if (erpResponse.Success)
                {
                    if (erpResponse.QuotationId != null)
                    {
                        return new ErpConfirmCustomerQuotationResponse(erpResponse.Success, erpResponse.Message, erpResponse.QuotationId);
                    }
                    else
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40004, currentStore, MethodBase.GetCurrentMethod().Name);
                        return new ErpConfirmCustomerQuotationResponse(erpResponse.Success, message, quotationId);
                    }
                }
                else
                {
                    CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40004, currentStore, MethodBase.GetCurrentMethod().Name);
                    CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, erpResponse.Message);
                    return new ErpConfirmCustomerQuotationResponse(erpResponse.Success, erpResponse.Message, erpResponse.QuotationId);
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new ErpConfirmCustomerQuotationResponse(false, message, null);
            }
        }


        /// <summary>
        /// GetQuotation gets quotation details.
        /// </summary>
        /// <param name="quotationtRequest">Quotation Id to get quotation info</param>
        /// <returns>GetCustomerQuotationResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Quotation/ConfirmCustomerQuotation")]
        public ErpConfirmCustomerQuotationResponse ConfirmCustomerQuotation([FromBody] GetQuotationtRequest quotationtRequest)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                if (string.IsNullOrEmpty(quotationtRequest.quotationId) || string.IsNullOrWhiteSpace(quotationtRequest.quotationId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "quotationId");
                    return new ErpConfirmCustomerQuotationResponse(false, message, null);
                }

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "quotationId: " + quotationtRequest.quotationId);

                var erpQuotationController = erpAdapterFactory.CreateQuotationController(currentStore.StoreKey);
                var erpResponse = erpQuotationController.ConfirmCustomerQuotation(quotationtRequest.quotationId, currentStore.StoreKey, GetRequestGUID(Request));

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));

                if (erpResponse.Success)
                {
                    if (erpResponse.QuotationId != null)
                    {
                        return new ErpConfirmCustomerQuotationResponse(erpResponse.Success, erpResponse.Message, erpResponse.QuotationId);
                    }
                    else
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40004, currentStore, MethodBase.GetCurrentMethod().Name);
                        return new ErpConfirmCustomerQuotationResponse(erpResponse.Success, message, quotationtRequest.quotationId);
                    }
                }
                else
                {
                    CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40004, currentStore, MethodBase.GetCurrentMethod().Name);
                    CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, erpResponse.Message);
                    return new ErpConfirmCustomerQuotationResponse(erpResponse.Success, erpResponse.Message, erpResponse.QuotationId);
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), GetRequestGUID(Request));
                throw new Exception(message);
            }
        }

        [RequestResponseLog]
        [HttpPost]
        [Route("Quotation/ConfirmQuotation")]

        public ConfirmQuotationResponse ConfirmQuotation([FromBody] ConfirmQuotationRequest request)
        {
            ConfirmQuotationResponse confirmQuotationResponse;

            try
            {
                if (request.Payment != null && request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.BOLETO.ToString()))
                {
                    return ConfirmQuotationBoleto(request);
                }

                confirmQuotationResponse = ValidateConfirmQuotationRequest(request);

                if (confirmQuotationResponse != null)
                {
                    return confirmQuotationResponse;
                }

                ErpConfirmQuotationRequest erpRequest = GetEcomRequestToErpRequest(request);
                var erpContactQuotationController = erpAdapterFactory.CreateQuotationController(currentStore.StoreKey);
                var erpResponse = erpContactQuotationController.ConfirmQuotation(erpRequest, GetRequestGUID(Request));

                confirmQuotationResponse = new ConfirmQuotationResponse(erpResponse.Success, erpResponse.Message, erpResponse.ErrorCode);

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                confirmQuotationResponse = new ConfirmQuotationResponse(false, message, "Other");
                return confirmQuotationResponse;
            }

            return confirmQuotationResponse;
        }


        [RequestResponseLog]
        [HttpPost]
        [Route("Quotation/QuoteOpportunityUpdate")]
        public QuoteOpportunityUpdateResponse QuoteOpportunityUpdate([FromBody] QuoteOpportunityUpdateRequest request)
        {
            QuoteOpportunityUpdateResponse quoteOpportunityUpdateResponse = new QuoteOpportunityUpdateResponse(false, "");
            try
            {
                quoteOpportunityUpdateResponse = ValidateQuoteOpportunityUpdateRequest(request);

                if (quoteOpportunityUpdateResponse != null)
                {
                    return quoteOpportunityUpdateResponse;
                }
                else
                {
                    ErpQuoteOpportunityUpdateRequest erpRequest = GetEcomRequestToErpRequest(request);
                    var erpContactQuotationController = erpAdapterFactory.CreateQuotationController(currentStore.StoreKey);
                    var erpResponse = erpContactQuotationController.QuoteOpportunityUpdate(erpRequest);
                    if (erpResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));
                        quoteOpportunityUpdateResponse = new QuoteOpportunityUpdateResponse(true, erpResponse.Message);
                    }
                    else
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, erpResponse.Message);
                        quoteOpportunityUpdateResponse = new QuoteOpportunityUpdateResponse(false, erpResponse.Message);
                    }
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                quoteOpportunityUpdateResponse = new QuoteOpportunityUpdateResponse(false, message);
                return quoteOpportunityUpdateResponse;
            }
            return quoteOpportunityUpdateResponse;
        }
        #endregion

        #region "Private Methods"

        private void SetCustomAttributes(ErpCustomerOrderInfo customerQuotation)
        {
            //++ converting Custom Attribute to CommerceProperty 
            if (customerQuotation.CustomAttributes != null)
            {
                customerQuotation.ExtensionProperties = new ObservableCollection<ErpCommerceProperty>();
                foreach (var item in customerQuotation.CustomAttributes)
                {
                    customerQuotation.ExtensionProperties.Add(new ErpCommerceProperty { Key = item.Key, Value = new ErpCommercePropertyValue { StringValue = item.Value } });
                }

                // Check if isPartnerQuote property is added or not!
                ErpCommerceProperty isPartnerQuoteProperty = customerQuotation.ExtensionProperties.FirstOrDefault(p => p.Key.ToUpper().Equals("ISPARTNERQUOTE"));
                if (isPartnerQuoteProperty == null)
                {
                    customerQuotation.ExtensionProperties.Add(new ErpCommerceProperty { Key = "IsPartnerQuote", Value = new ErpCommercePropertyValue { StringValue = "false" } });
                }
            }

            //++ Setting up 
            if (customerQuotation.Items != null)
            {
                foreach (var item in customerQuotation.Items)
                {
                    if (item.CustomAttributes != null)
                    {
                        item.ExtensionProperties = new ObservableCollection<ErpCommerceProperty>();
                        foreach (var inneritem in item.CustomAttributes)
                        {
                            item.ExtensionProperties.Add(new ErpCommerceProperty { Key = inneritem.Key, Value = new ErpCommercePropertyValue { StringValue = inneritem.Value } });
                        }
                    }
                }
            }
        }

        private void SetQuotationDefaultProperties(ErpCustomerOrderInfo customerQuotation)
        {
            // D365 Update 40 Platform Changes Start
            customerQuotation.ContractVersion = "0";
            // D365 Update 40 Platform Changes End
        }

        private CreateCustomerQuotationResponse TMV_ValidateQuotationRequest(ErpCustomerOrderInfo quotationRequest)
        {
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            if (string.IsNullOrEmpty(quotationRequest.ChannelReferenceId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL401101, currentStore, MethodBase.GetCurrentMethod().Name, "ChannelReferenceId missing");
                return new CreateCustomerQuotationResponse(false, message, null);
            }
            else
            {
                var integrationKey = integrationManager.GetErpKey(Enums.Entities.Quotation, quotationRequest.ChannelReferenceId);

                if (integrationKey != null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL401102, currentStore, quotationRequest.ChannelReferenceId);
                    return new CreateCustomerQuotationResponse(false, message, "");
                }
            }

            if (string.IsNullOrEmpty(quotationRequest.TransactionId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "TransactionId");
                return new CreateCustomerQuotationResponse(false, message, null);
            }
            else
            {
                var integrationKey = integrationManager.GetKeyByDescription(Enums.Entities.Quotation, quotationRequest.TransactionId);

                if (integrationKey != null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL401102, currentStore, quotationRequest.TransactionId);
                    return new CreateCustomerQuotationResponse(false, message, "");
                }
            }
            var contactPersonId = quotationRequest.ExtensionProperties.FirstOrDefault(x => x.Key == "ContactPersonId");// quotationRequest.customerQuotation.ExtensionProperties.FirstOrDefault(pr => pr != null && pr.Key.Equals("ContactPersonId", StringComparison.CurrentCultureIgnoreCase));
            if (contactPersonId == null || contactPersonId.Value.StringValue == "")
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL401103, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CreateCustomerQuotationResponse(false, message, quotationRequest.Id);
            }
            if (quotationRequest.Items != null && quotationRequest.Items.Count > 0)
            {
                foreach (var item in quotationRequest.Items)
                {
                    if (item.ItemId == null || string.IsNullOrEmpty(item.ItemId))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL401105, currentStore, MethodBase.GetCurrentMethod().Name);
                        return new CreateCustomerQuotationResponse(false, "", message);
                    }

                    if (quotationRequest.ExtensionProperties != null)
                    {
                        var tmvContractValidFrom = item.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractValidFrom");
                        if (tmvContractValidFrom == null || tmvContractValidFrom.Value.StringValue == "")
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL401104, currentStore, MethodBase.GetCurrentMethod().Name);
                            return new CreateCustomerQuotationResponse(false, "", message);
                        }
                    }
                }
            }
            return new CreateCustomerQuotationResponse(true, "", "");
        }

        #endregion

        #region Request, Response classes

        /// <summary>
        /// Represents quotation request.
        /// </summary>
        public class CreateCustomerQuotationRequest
        {
            /// <summary>
            /// customerQuotation
            /// </summary>
            public EcomCustomerOrderInfo customerQuotation { get; set; }

            /// <summary>
            /// IsEcomCustomerId
            /// </summary>
            public bool IsEcomCustomerId { get; set; }

        }


        /// <summary>
        /// Represents quotation request.
        /// </summary>
        public class GetCustomerQuotationRequest
        {

            //string custAccount, string company, string status, string offerType, string quotationId
            /// <summary>
            /// custAccount
            /// </summary>
            [Required]
            public string CustAccount { get; set; }
            /// <summary>
            /// status
            /// </summary>
            [Required]
            public string Status { get; set; }
            /// <summary>
            /// offerType
            /// </summary>
            [Required]
            public string OfferType { get; set; }
            /// <summary>
            /// quotationId
            /// </summary>
            [Required]
            public string QuotationId { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="status"></param>
            /// <param name="custAccount"></param>
            /// <param name="quotationId"></param>
            /// <param name="offerType"></param>
            public GetCustomerQuotationRequest(string custAccount, string status, string offerType, string quotationId)
            {
                this.QuotationId = quotationId;
                this.CustAccount = custAccount;
                this.OfferType = offerType;
                this.Status = status;
            }

        }


        /// <summary>
        /// Represents Reject quotation request.
        /// </summary>
        public class RejectCustomerQuotationRequest
        {
            /// <summary>
            /// ReasonId
            /// </summary>
            [Required]
            public string ReasonId { get; set; }
            /// <summary>
            /// quotationId
            /// </summary>
            [Required]
            public string QuotationId { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="quotationId"></param>
            /// <param name="reasonId"></param>
            public RejectCustomerQuotationRequest(string reasonId, string quotationId)
            {
                this.QuotationId = quotationId;
                this.ReasonId = reasonId;
            }
        }

        /// <summary>
        /// Response
        /// </summary>
        public class CreateCustomerQuotationResponse
        {/// <summary>
         ///Message
         /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// ProductPrices
            /// </summary>
            public string QuotationId { get; set; }
            /// <summary>
            /// Success
            /// </summary>
            public bool Success { get; set; }
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="success"></param>
            /// <param name="message"></param>
            /// <param name="quotationId"></param>
            public CreateCustomerQuotationResponse(bool success, string message, string quotationId)
            {
                this.Success = success;
                this.Message = message;
                this.QuotationId = quotationId;
            }
        }

        /// <summary>
        /// Response
        /// </summary>
        public class GetCustomerQuotationResponse
        {
            /// <summary>
            /// Success
            /// </summary>
            public bool Success { get; set; }
            public ErpCustomer Customer { get; set; }
            public ErpContactPerson ContactPerson { get; set; }
            public List<ErpCustomerOrderInfo> Quotations { get; set; }
            /// <summary>
            ///Message
            /// </summary>
            public string Message { get; set; }

            /// <param name="success"></param>
            /// <param name="message"></param>
            /// <param name="quotation"></param>
            public GetCustomerQuotationResponse(bool success, string message, ErpCustomer customer, ErpContactPerson contactPerson, List<ErpCustomerOrderInfo> quotations)
            {
                Success = success;
                Message = message;
                Customer = customer;
                ContactPerson = contactPerson;
                Quotations = quotations ?? new List<ErpCustomerOrderInfo>();
            }
        }
        /// <summary>
        /// Confirm Quotation Request
        /// </summary>
        public class ConfirmQuotationRequest
        {
            /// <summary>
            /// QuotationId property
            /// </summary>
            [Required]
            public string QuotationId { get; set; }
            /// <summary>
            /// Currency property
            /// </summary>
            [Required]
            public string Currency { get; set; }
            /// <summary>
            /// TransactionDate property
            /// </summary>
            [Required]
            public string TransactionDate { get; set; }
            /// <summary>
            /// TMVSalesOrigin property
            /// </summary>
            [Required]
            public string TMVSalesOrigin { get; set; }
            /// <summary>
            /// TMVFraudReviewStatus property
            /// </summary>
            [Required]
            public string TMVFraudReviewStatus { get; set; }
            /// <summary>
            /// TMVKountScore property
            /// </summary>
            [Required]
            public string TMVKountScore { get; set; }
            /// <summary>
            /// CustomerDetails
            /// </summary>
            public CustomerDetail Customer { get; set; }
            /// <summary>
            /// Payments details
            /// </summary>
            public PaymentDetail Payment { get; set; }
            /// <summary>
            /// ChannelReferenceId
            /// </summary>
            public string ChannelReferenceId { get; set; } = "";

        }
        public class PaymentDetail
        {
            /// <summary>
            /// CardType of PaymentDetails
            /// </summary>
            public string CardType { get; set; }
            /// <summary>
            /// CardNumber of PaymentDetails
            /// </summary>
            public string CardNumber { get; set; }
            /// <summary>
            /// CardHolder of PaymentDetails
            /// </summary>
            public string CardHolder { get; set; }
            /// <summary>
            /// expirationMonth of PaymentDetails
            /// </summary>
            /// <summary>
            /// Authorization of PaymentDetails
            /// </summary>
            public string Authorization { get; set; }
            /// <summary>
            /// CardToken of PaymentDetails
            /// </summary>
            public string CardToken { get; set; }
            public int? expirationMonth { get; set; }
            /// <summary>
            /// expirationYear of PaymentDetails
            /// </summary>
            public int? expirationYear { get; set; }
            /// <summary>
            /// PayerId of PaymentDetails
            /// </summary>
            public string PayerId { get; set; }
            /// <summary>
            /// ParentTransactionId of PaymentDetails
            /// </summary>
            public string ParentTransactionId { get; set; }
            /// <summary>
            /// Email of PaymentDetails
            /// </summary>
            public string Email { get; set; }
            /// <summary>
            /// Note of PaymentDetails
            /// </summary>
            public string Note { get; set; }
            /// <summary>
            /// Amount of PaymentDetails
            /// </summary>
            public decimal Amount { get; set; }
            /// <summary>
            /// ProcessorId of PaymentDetails
            /// </summary>
            public string ProcessorId { get; set; }
            /// <summary>
            /// TransactionId of PaymentDetails
            /// </summary>
            public string TransactionId { get; set; }
            /// <summary>
            /// TransactionId of PaymentDetails
            /// </summary>
            public int NumberOfInstallments { get; set; }

            /// <summary>
            /// BankIdentificationNumberStart of PaymentDetails
            /// </summary>
            public string BankIdentificationNumberStart { get; set; }
            /// <summary>
            /// ApprovalCode of PaymentDetails
            /// </summary>
            public string ApprovalCode { get; set; }
            /// <summary>
            /// shopperReference of PaymentDetails
            /// </summary>
            public string shopperReference { get; set; }
            /// <summary>
            /// IssuerCountry of PaymentDetails
            /// </summary>
            public string IssuerCountry { get; set; }

            /// <summary>
            /// ThreeDSecure of PaymentDetails
            /// </summary>
            public string ThreeDSecure { get; set; }

            /// <summary>
            /// IBAN of PaymentDetails
            /// </summary>
            public string IBAN { get; set; }

            /// <summary>
            /// SwiftCode of PaymentDetails
            /// </summary>
            public string SwiftCode { get; set; }

            /// <summary>
            /// BankName of PaymentDetails
            /// </summary>
            public string BankName { get; set; }

            /// <summary>
            /// Boleto of PaymentDetails
            /// </summary>
            public Boleto Boleto { get; set; }

            /// <summary>
            /// LocalTaxId of PaymentDetails
            /// </summary>
            public string LocalTaxId { get; set; }

            /// <summary>
            /// LocalTaxId of PaymentDetails
            /// </summary>
            public string IP { get; set; }

            /// <summary>
            /// Alipay object of PaymentDetails
            /// </summary>
            public ErpAlipay Alipay { get; set; }

            /// <summary>
            /// PspReference of PaymentDetails
            /// </summary>
            public string PspReference { get; set; }
        }
        /// <summary>
        /// Response
        /// </summary>
        public class ConfirmQuotationResponse
        {
            /// <summary>
            /// Initialize a new instance of the ProcessPaymentOfExistingOrderResponse 
            /// </summary>
            /// <param name="status">status of the ConfirmQuotation response</param>
            /// <param name="message">message for ConfirmQuotation response</param>
            /// <param name="errorCode">error code for ConfirmQuotation response</param>
            public ConfirmQuotationResponse(bool status, string message, string errorCode)
            {
                this.status = status;
                this.message = message;
                this.errorCode = errorCode;
            }

            /// <summary>
            /// status of the ProcessPaymentOfExistingOrderRequest
            /// </summary>
            public bool status { get; set; }

            /// <summary>
            /// message of the ProcessPaymentOfExistingOrderRequest
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// message of the ProcessPaymentOfExistingOrderRequest
            /// </summary>
            public string errorCode { get; set; }
        }
        public class GetQuotationtRequest
        {
            /// <summary>
            /// quotationId of Quotation
            /// </summary>
            [Required]
            public string quotationId { get; set; }

        }
        #region QuoteOpportunityUpdate

        /// <summary>
        /// Request
        /// </summary>
        public class QuoteOpportunityUpdateRequest
        {
            /// <summary>
            /// QuoteId
            /// </summary>
            [Required]
            public string QuoteId { get; set; }
            /// <summary>
            /// OpportunityId
            /// </summary>
            [Required]
            public string OpportunityId { get; set; }
            /// <summary>
            /// OpportunityGuid
            /// </summary>
            [Required]
            public string OpportunityGuid { get; set; }
        }
        /// <summary>
        /// Response
        /// </summary>
        public class QuoteOpportunityUpdateResponse
        {
            public QuoteOpportunityUpdateResponse(bool success, string message)
            {
                this.Success = success;
                this.Message = message;
            }
            /// <summary>
            /// Success
            /// </summary>
            public bool Success { get; set; }
            /// <summary>
            /// Message
            /// </summary>
            public string Message { get; set; }
        }

        #endregion
        #endregion

        #region Private Methods
        private GetCustomerQuotationResponse ValidateGetQuotationRequest(GetCustomerQuotationRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new GetCustomerQuotationResponse(false, message, null, null, null);
                }
                else if (String.IsNullOrWhiteSpace(request.CustAccount) && String.IsNullOrWhiteSpace(request.QuotationId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL400010, currentStore, "GetCustomerQuotationRequest.CustAccount", "GetCustomerQuotationRequest.QuotationId");
                    return new GetCustomerQuotationResponse(false, message, null, null, null);
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new GetCustomerQuotationResponse(false, message, null, null, null);
            }

            return null;
        }

        private ConfirmQuotationResponse ValidateConfirmQuotationRequest(ConfirmQuotationRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                }
                if (String.IsNullOrWhiteSpace(request.QuotationId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.QuotationId");
                    return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                }
                if (String.IsNullOrWhiteSpace(request.Currency))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Currency");
                    return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                }
                if (String.IsNullOrWhiteSpace(request.TransactionDate))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.TransactionDate");
                    return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                }
                if (String.IsNullOrWhiteSpace(request.TMVSalesOrigin))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.TMVSalesOrigin");
                    return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                }
                if (request.Customer == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Customer");
                    return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                }
                else
                {

                    if (String.IsNullOrWhiteSpace(request.Customer.CustomerNo))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Customer.CustomerNo");
                        return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                    }
                    if (String.IsNullOrWhiteSpace(request.Customer.CustomerName))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Customer.CustomerName");
                        return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                    }
                    if (String.IsNullOrWhiteSpace(request.Customer.CustomerEmail))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Customer.CustomerEmail");
                        return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                    }

                }
                if (request.Payment == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment");
                    return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(request.Payment.ProcessorId))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.ProcessorId");
                        return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                    }
                    else if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.PAYPAL_EXPRESS.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.PayerId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.PayerId");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.ParentTransactionId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.ParentTransactionId");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.Email))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.Email");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.Note))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.Note");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                    }
                    else if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.ALLPAGO_CC.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.Email))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.Email");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                        if (request.Payment.NumberOfInstallments < 1)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.NumberOfInstallments");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.ApprovalCode))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.ApprovalCode");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                        if (configurationHelper.GetSetting(CUSTOMER.Default_ThreeLetterISORegionName) == "BRA")
                        {
                            if (string.IsNullOrWhiteSpace(request.Payment.LocalTaxId))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.LocalTaxId");
                                return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                            }
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.IP))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.IP");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }

                    }
                    else if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.ADYEN_CC.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.BankIdentificationNumberStart))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.BankIdentificationNumberStart");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                        //if (string.IsNullOrWhiteSpace(request.Payment.ApprovalCode))
                        //{
                        //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.ApprovalCode");
                        //    return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        //}
                        if (string.IsNullOrWhiteSpace(request.Payment.shopperReference))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.shopperReference");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                    }
                    else if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.BASIC_CREDIT.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.ThreeDSecure))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.ThreeDSecure");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                    }
                    else if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.SEPA.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.IBAN))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.IBAN");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.SwiftCode))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.SwiftCode");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                    }
                    else if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.ADYEN_HPP.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.PspReference))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.PspReference");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }

                        if (string.IsNullOrWhiteSpace(request.Payment.CardType))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.CardType");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                    }

                    if (!request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.PURCHASEORDER.ToString()) &&
                        !request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.ADYEN_HPP.ToString()) 
                        )
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.CardHolder))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.CardHolder");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                        if (!(request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.SEPA.ToString())))
                        {
                            if (string.IsNullOrWhiteSpace(request.Payment.CardNumber))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.CardNumber");
                                return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                            }

                            if (string.IsNullOrWhiteSpace(request.Payment.Authorization))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.Authorization");
                                return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                            }
                            if (string.IsNullOrWhiteSpace(request.Payment.CardToken))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.CardToken");
                                return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                            }
                            if (request.Payment.expirationMonth == null || request.Payment.expirationMonth < 1 || request.Payment.expirationMonth > 12)
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.expirationMonth");
                                return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                            }
                            if (request.Payment.expirationYear == null || request.Payment.expirationYear < 0)
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.expirationYear");
                                return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                            }
                            if (string.IsNullOrWhiteSpace(request.Payment.CardType))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.CardType");
                                return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                            }
                            if (string.IsNullOrWhiteSpace(request.Payment.TransactionId))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.TransactionId");
                                return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                            }
                        }
                    }
                    
                    if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.PURCHASEORDER.ToString()))
                    {
                        if (request.Payment.Amount < 0)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.Amount");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                    }
                    else
                    {
                        if (request.Payment.Amount < 1)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.Payment.Amount");
                            return new ConfirmQuotationResponse(false, message, "ValidationFailed");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new ConfirmQuotationResponse(false, message, "Other");
            }

            return null;
        }

        private ErpConfirmQuotationRequest GetEcomRequestToErpRequest(ConfirmQuotationRequest request)
        {
            ErpConfirmQuotationRequest erpRequest = new ErpConfirmQuotationRequest();

            erpRequest.QuotationId = request.QuotationId;
            erpRequest.Currency = request.Currency;
            erpRequest.TMVSalesOrigin = request.TMVSalesOrigin;
            erpRequest.TMVFraudReviewStatus = request.TMVFraudReviewStatus;
            erpRequest.TMVKountScore = request.TMVKountScore;
            erpRequest.TransactionDate = string.IsNullOrWhiteSpace(request.TransactionDate) == true ? erpRequest.TransactionDate : DateTimeOffset.Parse(request.TransactionDate);
            erpRequest.TransactionDate = new DateTimeOffset(erpRequest.TransactionDate.Year, erpRequest.TransactionDate.Month, erpRequest.TransactionDate.Day, erpRequest.TransactionDate.Hour, erpRequest.TransactionDate.Minute, erpRequest.TransactionDate.Second, TimeSpan.Zero);
            erpRequest.ChannelReferenceId = request.ChannelReferenceId;
            if (request.Customer != null)
            {
                erpRequest.Customer.CustomerNo = request.Customer.CustomerNo;
                erpRequest.Customer.CustomerName = request.Customer.CustomerName;
                erpRequest.Customer.CustomerEmail = request.Customer.CustomerEmail;

                if (request.Customer.BillingAddress != null)
                {
                    erpRequest.Customer.BillingAddress.Name = request.Customer.BillingAddress.Name;
                    erpRequest.Customer.BillingAddress.Street = request.Customer.BillingAddress.Street;
                    erpRequest.Customer.BillingAddress.City = request.Customer.BillingAddress.City;
                    erpRequest.Customer.BillingAddress.ZipCode = request.Customer.BillingAddress.ZipCode;
                    if (request.Customer.BillingAddress.State != null)
                    {
                        erpRequest.Customer.BillingAddress.State = request.Customer.BillingAddress.State;
                    }
                    erpRequest.Customer.BillingAddress.ThreeLetterISORegionName = request.Customer.BillingAddress.ThreeLetterISORegionName;
                    erpRequest.Customer.BillingAddress.Phone = request.Customer.BillingAddress.Phone;
                }

                erpRequest.SalesOrder.CustomerId = request.Customer.CustomerNo;
                erpRequest.SalesOrder.CurrencyCode = request.Currency;
                erpRequest.SalesOrder.BillingAddress = erpRequest.Customer.BillingAddress;
            }

            if (request.Payment != null)
            {
                erpRequest.TenderLine.CardTypeId = request.Payment.CardType;
                erpRequest.TenderLine.MaskedCardNumber = request.Payment.CardNumber;
                erpRequest.TenderLine.CardOrAccount = request.Payment.CardHolder;
                erpRequest.TenderLine.Authorization = request.Payment.Authorization;
                erpRequest.TenderLine.CardToken = request.Payment.CardToken;
                erpRequest.TenderLine.ExpMonth = request.Payment.expirationMonth;
                erpRequest.TenderLine.ExpYear = request.Payment.expirationYear;
                erpRequest.TenderLine.PayerId = request.Payment.PayerId;
                erpRequest.TenderLine.ParentTransactionId = request.Payment.ParentTransactionId;
                erpRequest.TenderLine.Email = request.Payment.Email;
                erpRequest.TenderLine.Note = request.Payment.Note;
                erpRequest.TenderLine.TenderTypeId = request.Payment.ProcessorId;
                erpRequest.TenderLine.Amount = request.Payment.Amount;
                erpRequest.TenderLine.CustomAttributes = new List<KeyValuePair<string, string>>();
                erpRequest.TenderLine.CustomAttributes.Add(new KeyValuePair<string, string>("transaction-id", request.Payment.TransactionId));
                erpRequest.TransactionId = request.Payment.TransactionId;
                erpRequest.TenderLine.LineNumber = 1;
                erpRequest.TenderLine.NumberOfInstallments = request.Payment.NumberOfInstallments;
                erpRequest.TenderLine.BankIdentificationNumberStart = request.Payment.BankIdentificationNumberStart;
                erpRequest.TenderLine.ApprovalCode = request.Payment.ApprovalCode;
                erpRequest.TenderLine.shopperReference = request.Payment.shopperReference;
                erpRequest.TenderLine.IssuerCountry = request.Payment.IssuerCountry;
                erpRequest.TenderLine.ThreeDSecure = request.Payment.ThreeDSecure;
                erpRequest.TenderLine.IBAN = request.Payment.IBAN;
                erpRequest.TenderLine.SwiftCode = request.Payment.SwiftCode;
                erpRequest.TenderLine.BankName = request.Payment.BankName;
                erpRequest.TenderLine.LocalTaxId = request.Payment.LocalTaxId;
                erpRequest.TenderLine.IP = request.Payment.IP;
                erpRequest.TenderLine.TransactionId = request.Payment.TransactionId;

                erpRequest.TenderLine.PspReference = request.Payment.PspReference;

                erpRequest.TenderLine.Alipay = new ErpAlipay()
                {
                    BuyerId = request.Payment.Alipay?.BuyerId ?? string.Empty,
                    BuyerEmail = request.Payment.Alipay?.BuyerEmail ?? string.Empty,
                    OutTradeNo = request.Payment.Alipay?.OutTradeNo ?? string.Empty,
                    TradeNo = request.Payment.Alipay?.TradeNo ?? string.Empty
                };

                if (request.Payment.ProcessorId == PaymentCon.ADYEN_HPP.ToString())
                {
                    erpRequest.TenderLine.MaskedCardNumber = erpRequest.TenderLine.Alipay.BuyerEmail;
                }
            }

            return erpRequest;
        }

        private ConfirmQuotationResponse ConfirmQuotationBoleto(ConfirmQuotationRequest request)
        {
            var confirmQuotationResponse = ValidateConfirmQuotationRequestBoleto(request);
            if (confirmQuotationResponse != null)
            {
                return confirmQuotationResponse;
            }

            var erpRequest = _mapper.Map<ErpConfirmQuotationRequest>(request);

            erpRequest.TenderLine.TenderTypeId = request.Payment.ProcessorId;
            erpRequest.TenderLine.Amount = request.Payment.Amount;
            erpRequest.TenderLine.Boleto = request.Payment.Boleto;

            var erpContactQuotationController = erpAdapterFactory.CreateQuotationController(currentStore.StoreKey);
            var erpResponse = erpContactQuotationController.ConfirmQuotation(erpRequest, GetRequestGUID(Request));

            confirmQuotationResponse = new ConfirmQuotationResponse(erpResponse.Success, erpResponse.Message, erpResponse.ErrorCode);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));
            return confirmQuotationResponse;
        }

        private ConfirmQuotationResponse ValidateConfirmQuotationRequestBoleto(ConfirmQuotationRequest request)
        {
            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new ConfirmQuotationResponse(false, message, "ValidationFailed");
            }
            else if (String.IsNullOrWhiteSpace(request.QuotationId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.QuotationId");
                return new ConfirmQuotationResponse(false, message, "ValidationFailed");
            }
            else if (String.IsNullOrWhiteSpace(request.TMVSalesOrigin))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ConfirmQuotationRequest.TMVSalesOrigin");
                return new ConfirmQuotationResponse(false, message, "ValidationFailed");
            }

            return null;
        }

        private QuoteOpportunityUpdateResponse ValidateQuoteOpportunityUpdateRequest(QuoteOpportunityUpdateRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new QuoteOpportunityUpdateResponse(false, message);
                }
                if (String.IsNullOrWhiteSpace(request.QuoteId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "QuoteOpportunityUpdateRequest.QuotationId");
                    return new QuoteOpportunityUpdateResponse(false, message);
                }
                if (String.IsNullOrWhiteSpace(request.OpportunityId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "QuoteOpportunityUpdateRequest.OpportunityId");
                    return new QuoteOpportunityUpdateResponse(false, message);
                }
                if (String.IsNullOrWhiteSpace(request.OpportunityGuid))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "QuoteOpportunityUpdateRequest.OpportunityGuid");
                    return new QuoteOpportunityUpdateResponse(false, message);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new QuoteOpportunityUpdateResponse(false, message);
            }

            return null;
        }
        private ErpQuoteOpportunityUpdateRequest GetEcomRequestToErpRequest(QuoteOpportunityUpdateRequest request)
        {
            ErpQuoteOpportunityUpdateRequest erpRequest = new ErpQuoteOpportunityUpdateRequest();
            erpRequest.QuoteId = request.QuoteId;
            erpRequest.OpportunityId = request.OpportunityId;
            erpRequest.OpportunityGuid = request.OpportunityGuid;
            return erpRequest;
        }
        #endregion
    }
}