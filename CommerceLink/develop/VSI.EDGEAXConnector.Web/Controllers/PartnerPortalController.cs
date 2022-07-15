using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <summary>
    /// PartnerPortalController defines properties and methods for API controller for Customer Portal
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class PartnerPortalController : ApiBaseController
    {
        /// <summary>
        /// Partner Portal Controller
        /// </summary>
        public PartnerPortalController()
        {
            ControllerName = "PartnerPortalController";
        }

        #region API Methods

        /// <summary>
        /// GetOrValidatePrice
        /// </summary>
        /// <param name="request"></param>
        /// <returns>PriceValidationResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("PartnerPortal/GetOrValidatePrice")]
        public PriceValidationResponse GetOrValidatePrice(PriceRequest request)
        {
            var validateResponse = ValidatePriceRequest(request);

            if (validateResponse != null)
                return validateResponse;

            var erpSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
            var response = erpSalesOrderController.GetOrValidatePriceInformation(request, true);

            if (response.Status)
            {
                List<PriceValidationLines> itemsInformation = new List<PriceValidationLines>();
                if (response.Result != null && response.Result.SaleLines != null && response.Result.SaleLines.Items != null)
                {
                    foreach (var salesLine in response.Result.SaleLines.Items)
                    {
                        var item = new PriceValidationLines()
                        {
                            LineNumber = salesLine.LineNumber,
                            ItemId = salesLine.ItemId + "_" + salesLine.VariantId,
                            BasePrice = salesLine.BasePrice,
                            NetAmount = salesLine.NetAmount,
                            UnitOfMeasureSymbol = salesLine.UnitOfMeasureSymbol,
                            TaxRatePercentage = salesLine.TaxRatePercent,
                            TaxAmount = salesLine.TaxAmount,
                            TotalAmount = salesLine.TotalAmount,
                            Message = salesLine.Message
                        };
                        itemsInformation.Add(item);
                    }
                }

                return new PriceValidationResponse(response.Status, response.Message, itemsInformation);
            }

            return new PriceValidationResponse(response.Status, response.Message, null);
        }

        /// <summary>
        /// GetQuotation gets quotation details.
        /// </summary>
        /// <param name="rejectCustomerQuotation">rejectCustomerQuotation  to Reject quotation </param>
        /// <returns>GetCustomerQuotationResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("PartnerPortal/RejectQuotation")]
        public ErpConfirmCustomerQuotationResponse RejectQuotation([FromBody] QuotationController.RejectCustomerQuotationRequest rejectCustomerQuotation)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                if (string.IsNullOrEmpty(rejectCustomerQuotation.QuotationId) || string.IsNullOrWhiteSpace(rejectCustomerQuotation.QuotationId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "quotationId");
                    return new ErpConfirmCustomerQuotationResponse(false, message, null);
                }

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "quotationId: " + rejectCustomerQuotation.QuotationId);

                var erpQuotationController = erpAdapterFactory.CreateQuotationController(currentStore.StoreKey);
                var erpResponse = erpQuotationController.RejectCustomerQuotation(rejectCustomerQuotation.QuotationId, rejectCustomerQuotation.ReasonId, currentStore.StoreKey);

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
                        return new ErpConfirmCustomerQuotationResponse(erpResponse.Success, message, rejectCustomerQuotation.QuotationId);
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
        /// Transfer contract.
        /// </summary>
        /// <param name="request">request  to Transfer contract. </param>
        /// <returns>TransferContractResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("PartnerPortal/TransferPartnerContract")]
        public TransferContractResponse TransferPartnerContract([FromBody] TransferPartnerContractRequest request)
        {
            try
            {
                var validateResponse = ValidateTransferPartnerContractRequest(request);

                if (validateResponse != null)
                    return validateResponse;

                var erpSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
                var erpResponse = erpSalesOrderController.TransferPartnerContract(request, GetRequestGUID(Request));

                return new TransferContractResponse(erpResponse.Success, erpResponse.Message, erpResponse.MessageCode, erpResponse.SalesOrderId);
            }
            catch (Exception ex)
            {
                var message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new TransferContractResponse(false, message, string.Empty, string.Empty);
            }
        }

        #endregion

        #region Helper function
        private PriceValidationResponse ValidatePriceRequest(PriceRequest request)
        {
            var error = new StringBuilder();

            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                error.AppendLine(message);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.CustomerAccount))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CustomerAccount"));
                }

                if (string.IsNullOrWhiteSpace(request.ResellerAccount))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ResellerAccount"));
                }

                if (string.IsNullOrWhiteSpace(request.IndirectCustomerAccount))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "IndirectCustomerAccount"));
                }

                if (string.IsNullOrWhiteSpace(request.Currency))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Currency"));
                }

                if (request.ContractLines == null)
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                        MethodBase.GetCurrentMethod().Name, "ContractLines"));
                }
                else
                {
                    foreach (var line in request.ContractLines)
                    {
                        if (string.IsNullOrWhiteSpace(line.ProductId))
                        {
                            error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ProductId"));
                        }

                        if (line.LineNumber <= 0)
                        {
                            error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "LineNumber"));
                        }

                        if (line.Quantity <= 0)
                        {
                            error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Quantity"));
                        }
                    }
                }
            }

            var errorMessage = error.ToString();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new PriceValidationResponse(false, errorMessage);
            }

            return null;
        }

        private TransferContractResponse ValidateTransferPartnerContractRequest(TransferPartnerContractRequest request)
        {
            var error = new StringBuilder();

            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                error.AppendLine(message);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.SalesOrderId))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "SalesOrderId"));
                }

                if (string.IsNullOrWhiteSpace(request.CustomerAccount))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CustomerAccount"));
                }

                ////36791 - Distributor can be empty in case of reseller
                //if (string.IsNullOrWhiteSpace(request.DistributorAccount))
                //{
                //    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "DistributorAccount"));
                //}
                ////36791 - Distributor can be empty in case of reseller

                if (string.IsNullOrWhiteSpace(request.ResellerAccount))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ResellerAccount"));
                }

                if (string.IsNullOrWhiteSpace(request.IndirectCustomerAccount))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "IndirectCustomerAccount"));
                }

            }

            var errorMessage = error.ToString();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new TransferContractResponse(false, errorMessage, string.Empty, string.Empty);
            }

            return null;
        }

        #endregion

        #region Request Response classes

        /// <summary>
        /// Price validation response
        /// </summary>
        public class PriceValidationResponse
        {

            /// <summary>
            /// Initializes a new instance of the PriceValidationResponse
            /// </summary>
            /// <param name="status">status of sales order trans</param>
            /// /// <param name="message">message of price validation</param>
            /// /// <param name="result">data of price validation</param>
            public PriceValidationResponse(bool status, string message, List<PriceValidationLines> result = null)
            {
                this.status = status;
                this.message = message;
                this.results = result;
            }

            /// <summary>
            /// status of price validation
            /// </summary>
            public bool status { get; set; }

            /// <summary>
            /// message of price validation
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// result of price validation
            /// </summary>
            public List<PriceValidationLines> results { get; set; }
        }

        /// <summary>
        /// Price validation lines
        /// </summary>
        public class PriceValidationLines
        {
            /// <summary>
            /// Line number of Price validation lines
            /// </summary>
            public string LineNumber { get; set; }

            /// <summary>
            /// Item Id of Price validation lines
            /// </summary>
            public string ItemId { get; set; }

            /// <summary>
            /// Base price of Price validation lines
            /// </summary>
            public decimal BasePrice { get; set; }

            /// <summary>
            /// Net amount of Price validation lines
            /// </summary>
            public decimal NetAmount { get; set; }

            /// <summary>
            /// Unit of measure of Price validation lines
            /// </summary>
            public string UnitOfMeasureSymbol { get; set; }

            /// <summary>
            /// Tax rate percentage of Price validation lines
            /// </summary>
            public decimal TaxRatePercentage { get; set; }

            /// <summary>
            /// Tax amount of Price validation lines
            /// </summary>
            public decimal TaxAmount { get; set; }

            /// <summary>
            /// Total amount of Price validation lines
            /// </summary>
            public decimal TotalAmount { get; set; }

            /// <summary>
            /// Message of Price validation lines
            /// </summary>
            public string Message { get; set; }
        }

        /// <summary>
        /// Transfer contract response
        /// </summary>
        public class TransferContractResponse
        {

            /// <summary>
            /// Initializes a new instance of the Transfer Contract Response
            /// </summary>
            /// <param name="status">status of transafer contract</param>
            /// /// <param name="message">message of transfer contract</param>
            /// /// <param name="code">message code of transfer contract</param>
            public TransferContractResponse(bool status, string message, string code, string salesOrderId)
            {
                this.status = status;
                this.message = message;
                this.code = code;
                this.salesOrderId = salesOrderId;
            }

            /// <summary>
            /// status of transfer contract
            /// </summary>
            public bool status { get; set; }

            /// <summary>
            /// message of transfer contract
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// code of transfer contract
            /// </summary>
            public string code { get; set; }

            /// <summary>
            /// sales Order Id
            /// </summary>
            public string salesOrderId { get; set; }
        }

        #endregion

    }

}
