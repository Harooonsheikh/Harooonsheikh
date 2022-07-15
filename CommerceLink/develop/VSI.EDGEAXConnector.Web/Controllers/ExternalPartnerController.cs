using System.Reflection;
using System.Text;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <summary>
    /// ExternalPartnerController defines properties and methods for API controller for external partner
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class ExternalPartnerController : ApiBaseController
    {
        /// <summary>
        /// External Partner Controller
        /// </summary>
        public ExternalPartnerController()
        {
            ControllerName = "ExternalPartnerController";
        }

        #region API Methods

        /// <summary>
        /// ContractRenewal
        /// </summary>
        /// <param name="request"></param>
        /// <returns>ContractRenewalResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Partner/ContractRenewal")]
        public ContractRenewalResponse ContractRenewal(ContractRenewalRequest request)
        {
            var validateResponse = ValidateContractRenewalRequest(request);

            if (validateResponse != null)
                return validateResponse;

            var erpSalesOrderController = erpAdapterFactory.CreateSalesOrderController(currentStore.StoreKey);
            var response = erpSalesOrderController.ContractRenewal(request, GetRequestGUID(Request));

            return new ContractRenewalResponse(response.Success, response.Message, response.SalesOrderId);
        }

        #endregion

        #region Helper function
        private ContractRenewalResponse ValidateContractRenewalRequest(ContractRenewalRequest request)
        {
            var error = new StringBuilder();

            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                error.AppendLine(message);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.RequestNumber))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "RequestNumber"));
                }

                if (string.IsNullOrWhiteSpace(request.PACLicense))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "PACLicense"));
                }

                if (string.IsNullOrWhiteSpace(request.Distributor))
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Distributor"));
                }

                if (request.SalesLines == null)
                {
                    error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore,
                        MethodBase.GetCurrentMethod().Name, "SalesLines"));
                }
                else
                {
                    foreach (var line in request.SalesLines)
                    {
                        if (string.IsNullOrWhiteSpace(line.SKU))
                        {
                            error.AppendLine(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "SKU"));
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
                return new ContractRenewalResponse(false, errorMessage, string.Empty);
            }

            return null;
        }

        #endregion

        #region Request Response classes

        /// <summary>
        /// Contract renewal response
        /// </summary>
        public class ContractRenewalResponse
        {

            /// <summary>
            /// Initializes a new instance of the ContractRenewalResponse
            /// </summary>
            /// <param name="status">status of contract renewal</param>
            /// /// <param name="message">message of contract renewal</param>
            /// /// <param name="salesOrderId">sales order id of contract renewal</param>
            public ContractRenewalResponse(bool status, string message, string salesOrderId)
            {
                this.status = status;
                this.message = message;
                this.salesOrderId = salesOrderId;
            }

            /// <summary>
            /// status of contract renewal
            /// </summary>
            public bool status { get; set; }

            /// <summary>
            /// message of contract renewal
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// sales order id of contract renewal
            /// </summary>
            public string salesOrderId { get; set; }
        }

        #endregion

    }

}
