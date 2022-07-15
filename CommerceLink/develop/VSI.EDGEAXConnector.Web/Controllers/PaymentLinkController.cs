using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.BoletoPayment;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web
{
    /// <summary>
    /// PaymentLinkController defines properties and methods for API controller for payment link.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class PaymentLinkController : ApiBaseController
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_erpAdapterFactory"></param>
        /// <param name="_eComAdapterFactory"></param>
        public PaymentLinkController()
        {
            ControllerName = "PaymentLinkController";
        }

        #region API Methods

        /// <summary>
        /// Get Customer By invoice number
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("PaymentLink/GetCustomerByInvoiceId")]
        public CustomerInfoResponse GetCustomerInfoByInvoiceId([FromBody] CustomerByInvoiceRequest request)
        {
            CustomerInfoResponse customerResponse = new CustomerInfoResponse(false, "", null);

            try
            {
                customerResponse = ValidateCustomerByInvoiceRequest(request);

                if (customerResponse != null)
                {
                    return customerResponse;
                }
                else
                {
                    ERPDataModels.Custom.CustomerByInvoiceRequest customerByInvoiceRequest = new ERPDataModels.Custom.CustomerByInvoiceRequest();
                    customerByInvoiceRequest.InvoiceId = request.InvoiceId;

                    var erpCustomerController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);
                    var clResponse = erpCustomerController.GetCustomerInfoByInvoiceId(customerByInvoiceRequest);

                    if (clResponse.Status)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(clResponse));
                        customerResponse = new CustomerInfoResponse(true, clResponse.Message, clResponse.CustomerInfo);
                    }
                    else if (!clResponse.Status)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from D365.");
                        customerResponse = new CustomerInfoResponse(false, clResponse.Message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                customerResponse = new CustomerInfoResponse(false, message, null);
                return customerResponse;
            }

            return customerResponse;
        }

        /// <summary>
        /// Get customer invoice details from D365 by invoice id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("PaymentLink/GetCustomerInvoiceDetails")]
        public CustomerInoviceDetailResponse GetCustomerInvoiceDetails([FromBody] CustomerInvoiceDetailRequest request)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "GetCustomerInvoiceDetails", DateTime.UtcNow);

            CustomerInoviceDetailResponse customerInvoiceResponse = new CustomerInoviceDetailResponse(false, null, null, "");

            try
            {
                customerInvoiceResponse = ValidateContractByInvoiceRequest(request);

                if (customerInvoiceResponse != null)
                {
                    return customerInvoiceResponse;
                }
                else
                {

                    ERPDataModels.Custom.CustomerInvoiceDetailRequest customerInvoiceRequest = new ERPDataModels.Custom.CustomerInvoiceDetailRequest();
                    customerInvoiceRequest.InvoiceId = request.InvoiceId;
                    customerInvoiceRequest.CustomerEmail = request.CustomerEmail;
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL600000, currentStore, GetRequestGUID(Request), "CreateCustomerController", DateTime.UtcNow);
                    var erpCustomerController = erpAdapterFactory.CreateCustomerController(currentStore.StoreKey);
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL600000, currentStore, GetRequestGUID(Request), "CreateCustomerController", DateTime.UtcNow);
                    var clResponse = erpCustomerController.GetCustomerInvoiceDetails(customerInvoiceRequest, GetRequestGUID(Request));

                    if (clResponse.Status)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(clResponse));
                        customerInvoiceResponse = new CustomerInoviceDetailResponse(true, clResponse.CustomerInfo, clResponse.CustomerInvoiceDetails, clResponse.Message);
                    }
                    else if (!clResponse.Status)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, clResponse.ErrorCode + ": " + clResponse.Message);
                        customerInvoiceResponse = new CustomerInoviceDetailResponse(false, null, null, clResponse.Message, clResponse.ErrorCode);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                customerInvoiceResponse = new CustomerInoviceDetailResponse(false, null, null, message);
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "GetCustomerInvoiceDetails", DateTime.UtcNow);

                return customerInvoiceResponse;
            }
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), "GetCustomerInvoiceDetails", DateTime.UtcNow);

            return customerInvoiceResponse;
        }

        /// <summary>
        /// Process payment for existing order
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("PaymentLink/AddPaymentLinkForInvoice")]
        public AddPaymentLinkForInvoiceResponse AddPaymentLinkForInvoice([FromBody] AddPaymentLinkForInvoiceRequest request)
        {

            AddPaymentLinkForInvoiceResponse processPaymentResponse = new AddPaymentLinkForInvoiceResponse(false, "", null);

            try
            {
                if (request.Payment != null && request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.BOLETO.ToString()))
                {
                    return AddPaymentLinkForInvoiceBoleto(request);
                }

                processPaymentResponse = ValidatePaymentLinkForInvoiceRequest(request);

                if (processPaymentResponse != null)
                {
                    return processPaymentResponse;
                }
                else
                {
                    ErpAddPaymentLinkForInvoiceRequest erpRequest = GetEcomRequestToErpRequest(request);
                    var erpContactSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
                    var erpResponse = erpContactSalesOrderController.AddPaymentLinkForInvoice(erpRequest, GetRequestGUID(Request));

                    if (erpResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));
                        processPaymentResponse = new AddPaymentLinkForInvoiceResponse(true, erpResponse.Message, erpResponse.ErrorCode);
                    }
                    else if (!erpResponse.Success)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from D365.");
                        processPaymentResponse = new AddPaymentLinkForInvoiceResponse(false, erpResponse.Message, erpResponse.ErrorCode);
                    }

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));

                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                processPaymentResponse = new AddPaymentLinkForInvoiceResponse(false, message, null);
                return processPaymentResponse;
            }

            return processPaymentResponse;
        }

        /// <summary>
        /// Process payment for existing order
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("PaymentLink/CreatePaymentJournal")]
        public CreatePaymentJournalResponse CreatePaymentJournal([FromBody] ErpCreatePaymentJournalRequest request)
        {
            var processPaymentResponse = new CreatePaymentJournalResponse(false, "");

            try
            {
                processPaymentResponse = ValidateCreatePaymentJournalRequest(request);

                if (processPaymentResponse != null)
                {
                    return processPaymentResponse;
                }

                var erpContactSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
                var erpResponse = erpContactSalesOrderController.CreatePaymentJournal(request, GetRequestGUID(Request));

                processPaymentResponse = new CreatePaymentJournalResponse(erpResponse.Success, erpResponse.Message);

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                processPaymentResponse = new CreatePaymentJournalResponse(false, message);
                return processPaymentResponse;
            }

            return processPaymentResponse;
        }

        /// <summary>
        /// Get Boleto Url by Inovoice Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("PaymentLink/GetBoletoUrl")]
        public GetBoletoUrlResponse GetBoletoUrl([FromBody] GetBoletoUrlRequest request)
        {
            GetBoletoUrlResponse processBoletoResponse;

            try
            {
                processBoletoResponse = ValidateBoletoUrlRequest(request);

                if (processBoletoResponse != null)
                {
                    return processBoletoResponse;
                }

                ErpGetBoletoUrlRequest erpRequest = new ErpGetBoletoUrlRequest();
                erpRequest.InvoiceId = request.InvoiceId;

                var erpContactSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
                var erpResponse = erpContactSalesOrderController.GetBoletoUrl(erpRequest, GetRequestGUID(Request));

                processBoletoResponse = new GetBoletoUrlResponse(erpResponse.Status, erpResponse.Message, erpResponse.TMVBoletoUrl);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                processBoletoResponse = new GetBoletoUrlResponse(false, null, message);
            }

            return processBoletoResponse;
        }

        #endregion

        #region "Private Methods"
        private CustomerInfoResponse ValidateCustomerByInvoiceRequest(CustomerByInvoiceRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new CustomerInfoResponse(false, message, null);
                }
                else if (String.IsNullOrWhiteSpace(request.InvoiceId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CustomerByInvoiceRequest.InvoiceId");
                    return new CustomerInfoResponse(false, message, null);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new CustomerInfoResponse(false, message, null);
            }

            return null;
        }
        private CustomerInoviceDetailResponse ValidateContractByInvoiceRequest(CustomerInvoiceDetailRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new CustomerInoviceDetailResponse(false, null, null, message);
                }
                else if (String.IsNullOrWhiteSpace(request.InvoiceId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CustomerInvoiceDetailRequest.InvoiceId");
                    return new CustomerInoviceDetailResponse(false, null, null, message);
                }
                else if (String.IsNullOrWhiteSpace(request.CustomerEmail))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CustomerInvoiceDetailRequest.CustomerEmail");
                    return new CustomerInoviceDetailResponse(false, null, null, message);
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new CustomerInoviceDetailResponse(false, null, null, message);
            }

            return null;
        }
        private AddPaymentLinkForInvoiceResponse ValidatePaymentLinkForInvoiceRequest(AddPaymentLinkForInvoiceRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new AddPaymentLinkForInvoiceResponse(false, message, null);
                }
                if (String.IsNullOrWhiteSpace(request.SalesId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.SalesId");
                    return new AddPaymentLinkForInvoiceResponse(false, message, null);
                }
                if (String.IsNullOrWhiteSpace(request.InvoiceId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.InvoiceId");
                    return new AddPaymentLinkForInvoiceResponse(false, message, null);
                }
                if (String.IsNullOrWhiteSpace(request.Currency))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Currency");
                    return new AddPaymentLinkForInvoiceResponse(false, message, null);
                }
                if (String.IsNullOrWhiteSpace(request.TransactionDate))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.TransactionDate");
                    return new AddPaymentLinkForInvoiceResponse(false, message, null);
                }
                if (request.Customer == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Customer");
                    return new AddPaymentLinkForInvoiceResponse(false, message, null);
                }
                else
                {

                    if (String.IsNullOrWhiteSpace(request.Customer.CustomerNo))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Customer.CustomerNo");
                        return new AddPaymentLinkForInvoiceResponse(false, message, null);
                    }
                    if (String.IsNullOrWhiteSpace(request.Customer.CustomerName))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Customer.CustomerName");
                        return new AddPaymentLinkForInvoiceResponse(false, message, null);
                    }
                    if (String.IsNullOrWhiteSpace(request.Customer.CustomerEmail))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Customer.CustomerEmail");
                        return new AddPaymentLinkForInvoiceResponse(false, message, null);
                    }
                    //if (request.Customer.BillingAddress == null)
                    //{
                    //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Customer.BillingAddress");
                    //    return new AddPaymentLinkForInvoiceResponse(false, message);
                    //}
                    //else
                    //{
                    //    if (string.IsNullOrWhiteSpace(request.Customer.BillingAddress.Name))
                    //    {
                    //        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Customer.BillingAddress.Name");
                    //        return new AddPaymentLinkForInvoiceResponse(false, message);
                    //    }
                    //    if (string.IsNullOrWhiteSpace(request.Customer.BillingAddress.Street))
                    //    {
                    //        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Customer.BillingAddress.Street");
                    //        return new AddPaymentLinkForInvoiceResponse(false, message);
                    //    }
                    //    if (string.IsNullOrWhiteSpace(request.Customer.BillingAddress.City))
                    //    {
                    //        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Customer.BillingAddress.City");
                    //        return new AddPaymentLinkForInvoiceResponse(false, message);
                    //    }
                    //    if (string.IsNullOrWhiteSpace(request.Customer.BillingAddress.ThreeLetterISORegionName))
                    //    {
                    //        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Customer.BillingAddress.ThreeLetterISORegionName");
                    //        return new AddPaymentLinkForInvoiceResponse(false, message);
                    //    }
                    //    if (string.IsNullOrWhiteSpace(request.Customer.BillingAddress.Phone))
                    //    {
                    //        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Customer.BillingAddress.Phone");
                    //        return new AddPaymentLinkForInvoiceResponse(false, message);
                    //    }
                    //}

                }
                if (request.Payment == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment");
                    return new AddPaymentLinkForInvoiceResponse(false, message, null);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(request.Payment.ProcessorId))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.ProcessorId");
                        return new AddPaymentLinkForInvoiceResponse(false, message, null);
                    }
                    else if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.PAYPAL_EXPRESS.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.PayerId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.PayerId");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.ParentTransactionId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.ParentTransactionId");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.Email))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.Email");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.Note))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.Note");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                    }
                    else if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.ADYEN_CC.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.BankIdentificationNumberStart))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.BankIdentificationNumberStart");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        //if (string.IsNullOrWhiteSpace(request.Payment.ApprovalCode))
                        //{
                        //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.ApprovalCode");
                        //    return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        //}
                        if (string.IsNullOrWhiteSpace(request.Payment.shopperReference))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.shopperReference");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        //if (string.IsNullOrWhiteSpace(request.Payment.IssuerCountry))
                        //{
                        //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.IssuerCountry");
                        //    return new AddPaymentLinkForInvoiceResponse(false, message);
                        //}
                    }
                    else if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.ALLPAGO_CC.ToString()))
                    {
                        if (configurationHelper.GetSetting(CUSTOMER.Default_ThreeLetterISORegionName) == "BRA")
                        {
                            if (string.IsNullOrWhiteSpace(request.Payment.LocalTaxId))
                            {
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.Payment.LocalTaxId");
                                return new AddPaymentLinkForInvoiceResponse(false, message, null);
                            }
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.IP))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.Payment.IP");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.Email))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.Payment.Email");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        if (request.Payment.NumberOfInstallments < 1)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.Payment.NumberOfInstallments");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.ApprovalCode))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "request.Payment.ApprovalCode");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                    }

                    if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.SEPA.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.IBAN))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.IBAN");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.SwiftCode))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.SwiftCode");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                    }
                    else if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.ADYEN_HPP.ToString()))
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.PspReference))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.PspReference");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }

                        if (string.IsNullOrWhiteSpace(request.Payment.CardType))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.CardType");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.CardNumber))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.CardNumber");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.Authorization))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.Authorization");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.CardToken))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.CardToken");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        if (request.Payment.expirationMonth == null || request.Payment.expirationMonth < 1 || request.Payment.expirationMonth > 12)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.expirationMonth");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        if (request.Payment.expirationYear == null || request.Payment.expirationYear < 0)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.expirationYear");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }

                        if (string.IsNullOrWhiteSpace(request.Payment.CardType))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.CardType");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                        if (string.IsNullOrWhiteSpace(request.Payment.TransactionId))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.TransactionId");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                    }

                    if (!request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.ADYEN_HPP.ToString())
                        )
                    {
                        if (string.IsNullOrWhiteSpace(request.Payment.CardHolder))
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.CardHolder");
                            return new AddPaymentLinkForInvoiceResponse(false, message, null);
                        }
                    }

                    if (request.Payment.Amount < 1)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.Amount");
                        return new AddPaymentLinkForInvoiceResponse(false, message, null);
                    }


                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new AddPaymentLinkForInvoiceResponse(false, message, null);
            }

            return null;
        }

        private CreatePaymentJournalResponse ValidateCreatePaymentJournalRequest(ErpCreatePaymentJournalRequest request)
        {
            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CreatePaymentJournalResponse(false, message);
            }
            else if (String.IsNullOrWhiteSpace(request.SalesId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreatePaymentJournal.SalesId");
                return new CreatePaymentJournalResponse(false, message);
            }
            else if (String.IsNullOrWhiteSpace(request.InvoiceId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreatePaymentJournal.InvoiceId");
                return new CreatePaymentJournalResponse(false, message);
            }
            else if (String.IsNullOrWhiteSpace(request.Currency))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreatePaymentJournal.Currency");
                return new CreatePaymentJournalResponse(false, message);
            }

            //Validate Transaction DateTime if sent in Request
            if (!String.IsNullOrWhiteSpace(request.TransactionDate))
            {
                var isValidDate = DateTime.TryParse(request.TransactionDate, out DateTime transDate);
                if (!isValidDate)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40009, currentStore, MethodBase.GetCurrentMethod().Name, "CreatePaymentJournal.TransactionDate");
                    return new CreatePaymentJournalResponse(false, message);
                }

            }

            if (request.Payment == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreatePaymentJournal.Payment");
                return new CreatePaymentJournalResponse(false, message);
            }
            else if (request.Payment.Amount <= 0)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreatePaymentJournal.Payment.Amount");
                return new CreatePaymentJournalResponse(false, message);
            }

            return null;
        }

        private AddPaymentLinkForInvoiceResponse AddPaymentLinkForInvoiceBoleto(AddPaymentLinkForInvoiceRequest request)
        {
            AddPaymentLinkForInvoiceResponse processPaymentResponse = new AddPaymentLinkForInvoiceResponse(false, "", null);

            processPaymentResponse = ValidateBoletoPaymentLinkForInvoice(request);

            if (processPaymentResponse != null)
                return processPaymentResponse;



            var erpRequest = _mapper.Map<ErpAddPaymentLinkForInvoiceBoletoRequest>(request);

            var erpContactSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
            var erpResponse = erpContactSalesOrderController.AddPaymentLinkForInvoiceBoleto(erpRequest, GetRequestGUID(Request));

            if (erpResponse.Success)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));
                processPaymentResponse = new AddPaymentLinkForInvoiceResponse(true, erpResponse.Message, erpResponse.ErrorCode);
            }
            else if (!erpResponse.Success)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from D365.");
                processPaymentResponse = new AddPaymentLinkForInvoiceResponse(false, erpResponse.Message, erpResponse.ErrorCode);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));

            return processPaymentResponse;
        }

        private AddPaymentLinkForInvoiceResponse ValidateBoletoPaymentLinkForInvoice(AddPaymentLinkForInvoiceRequest request)
        {

            if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.BOLETO.ToString()))
            {
                if (request.Payment.Boleto == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AddPaymentLinkForInvoiceRequest.Payment.Boleto");
                    return new AddPaymentLinkForInvoiceResponse(false, message, null);
                }

            }
            return null;
        }


        private ErpAddPaymentLinkForInvoiceRequest GetEcomRequestToErpRequest(AddPaymentLinkForInvoiceRequest request)
        {
            ErpAddPaymentLinkForInvoiceRequest erpRequest = new ErpAddPaymentLinkForInvoiceRequest();

            erpRequest.SalesId = request.SalesId;
            erpRequest.InvoiceId = request.InvoiceId;
            erpRequest.Currency = request.Currency;
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
            }

            return erpRequest;
        }

        private GetBoletoUrlResponse ValidateBoletoUrlRequest(GetBoletoUrlRequest request)
        {
            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new GetBoletoUrlResponse(false, message, "");
            }
            else if (String.IsNullOrWhiteSpace(request.InvoiceId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "InvoiceId");
                return new GetBoletoUrlResponse(false, message, "");
            }
            return null;
        }

        #endregion

        #region Request, Response classes

        public class CustomerByInvoiceRequest
        {
            /// <summary>
            /// Sales order invoice id to get customer
            /// </summary>
            [Required]
            public string InvoiceId { get; set; }
        }

        /// <summary>
        /// Customer invoices detail request
        /// </summary>
        public class CustomerInvoiceDetailRequest
        {
            /// <summary>
            /// Invoice Id
            /// </summary>
            [Required]
            public string InvoiceId { get; set; }

            /// <summary>
            /// customer email
            /// </summary>
            [Required]
            public string CustomerEmail { get; set; }
        }
        /// <summary>
        /// CustomerInoviceDetail Response
        /// </summary>
        public class CustomerInoviceDetailResponse
        {
            /// <summary>
            /// Initialize a new instance of the CustomerInvoiceDetailResponse 
            /// </summary>
            /// <param name="status">status of the CustomerInvoiceDetailResponse request</param>
            /// <param name="customerInfo">customerInfo</param>
            /// <param name="customerInvoice">Invoice of customer</param>
            /// <param name="message">message for CustomerInvoiceDetailResponse request</param>

            public CustomerInoviceDetailResponse(bool status, ErpCustomer customerInfo, ErpCustomerInvoiceDetail customerInvoice, string message, string errorCode = "")
            {
                this.Status = status;
                this.CustomerInfo = customerInfo;
                this.CustomerInvoiceDetails = customerInvoice;
                this.Message = message;
                this.ErrorCode = errorCode;
            }

            /// <summary>
            /// status of the CustomerInvoice request
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// Customer Info 
            /// </summary>
            public ErpCustomer CustomerInfo { get; set; }

            /// <summary>
            /// Customer Invoice 
            /// </summary>
            public ErpCustomerInvoiceDetail CustomerInvoiceDetails { get; set; }

            /// <summary>
            /// message of the CustomerInvoice request
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// message of the CustomerInvoice request
            /// </summary>
            public string ErrorCode { get; set; }

        }
        /// <summary>
        /// CustomerInfoResponse class
        /// </summary>
        public class CustomerInfoResponse
        {
            public CustomerInfoResponse(bool status, string message, object customerInfo)
            {
                this.Status = status;
                this.Message = message;
                this.CustomerInfo = customerInfo;
            }

            public bool Status { get; set; }
            public object CustomerInfo { get; set; }
            public string Message { get; set; }
        }

        /// <summary>
        /// ProcessPaymentOfExistingOrder Request
        /// </summary>
        public class AddPaymentLinkForInvoiceRequest
        {
            /// <summary>
            /// SalesorderId property
            /// </summary>
            [Required]
            public string SalesId { get; set; }
            /// <summary>
            /// InvoiceId property
            /// </summary>
            [Required]
            public string InvoiceId { get; set; }
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
            /// CustomerDetails
            /// </summary>
            public CustomerDetail Customer { get; set; }
            /// <summary>
            /// Payments details
            /// </summary>
            public PaymentDetail Payment { get; set; }
            /// <summary>
            /// ChannelReferenceId property
            /// </summary>            
            public string ChannelReferenceId { get; set; } = "";
        }
        /// <summary>
        /// PaymentDetails
        /// </summary>
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
            /// <summary>
            /// expirationMonth of PaymentDetails
            /// </summary>
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
            public String ThreeDSecure { get; set; }

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
            /// IP of PaymentDetails
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
        /// CustomerDetails
        /// </summary>
        public class CustomerDetail
        {
            /// <summary>
            /// ERP Customer Id
            /// </summary>
            public string CustomerNo { get; set; }
            /// <summary>
            /// Customer Name
            /// </summary>
            public string CustomerName { get; set; }
            /// <summary>
            /// Customer Email
            /// </summary>
            public string CustomerEmail { get; set; }
            /// <summary>
            /// BillingAddress of PaymentDetails
            /// </summary>
            public ErpAddress BillingAddress { get; set; }
        }

        /// <summary>
        /// ProcessPaymentOfExistingOrder Response
        /// </summary>
        public class AddPaymentLinkForInvoiceResponse
        {
            /// <summary>
            /// Initialize a new instance of the ProcessPaymentOfExistingOrderResponse 
            /// </summary>
            /// <param name="status">status of the ProcessPaymentOfExistingOrderRequest request</param>
            /// <param name="message">message for ProcessPaymentOfExistingOrderRequest request</param>
            public AddPaymentLinkForInvoiceResponse(bool status, string message, string errorcode)
            {
                this.status = status;
                this.message = message;
                this.ErrorCode = errorcode;
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
            /// ErrorCode of the ProcessPaymentOfExistingOrderRequest
            /// </summary>
            public string ErrorCode { get; set; }
        }

        /// <summary>
        /// PaymentLinkForInvoiceResponse
        /// </summary>
        public class CreatePaymentJournalResponse
        {
            /// <summary>
            /// Initialize a new instance of the ProcessPaymentOfExistingOrderResponse 
            /// </summary>
            /// <param name="status">status of the ProcessPaymentOfExistingOrderRequest request</param>
            /// <param name="message">message for ProcessPaymentOfExistingOrderRequest request</param>
            public CreatePaymentJournalResponse(bool status, string message)
            {
                this.status = status;
                this.message = message;
            }

            /// <summary>
            /// status of the ProcessPaymentOfExistingOrderRequest
            /// </summary>
            public bool status { get; set; }

            /// <summary>
            /// message of the ProcessPaymentOfExistingOrderRequest
            /// </summary>
            public string message { get; set; }
        }

        /// <summary>
        /// GetBoletoUrlRequest
        /// </summary>
        public class GetBoletoUrlRequest
        {
            /// <summary>
            /// InvoiceId of the GetBoletoUrlRequest
            /// </summary>
            [Required]
            public string InvoiceId { get; set; }
        }

        /// <summary>
        /// GetBoletoUrlResponse
        /// </summary>
        public class GetBoletoUrlResponse
        {
            /// <summary>
            /// Initialize a new instance of the BoletoResponse 
            /// </summary>
            /// <param name="status">status of the BoletoResponse response</param>
            /// <param name="tmvBoletoUrl">tmvBoletoUrl for BoletoResponse response</param>
            /// <param name="message">message for BoletoResponse response</param>
            public GetBoletoUrlResponse(bool status, string message, string tmvBoletoUrl)
            {
                Status = status;
                Message = message;
                TMVBoletoUrl = tmvBoletoUrl;
            }

            /// <summary>
            /// status of the BoletoResponse
            /// </summary>
            public bool Status { get; set; }
            /// <summary>
            /// TMVBoletoUrl of the BoletoResponse
            /// </summary>
            public string TMVBoletoUrl { get; set; }

            /// <summary>
            /// Message of the BoletoResponse
            /// </summary>
            public string Message { get; set; }
        }

        #endregion
    }
}
