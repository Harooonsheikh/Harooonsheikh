using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web
{
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class InAppPurchaseController : ApiBaseController
    {
        /// <summary>
        /// In-App-Purchase Controller 
        /// </summary>
        public InAppPurchaseController()
        {
            ControllerName = "InAppPurchaseController";
        }

        #region API Methods

        /// <summary>
        /// Update AutoRenew flag of contract for all sale lines in D365
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("PartnerPortal/AutoRenewContract")]
        [Route("InAppPurchase/AutoRenewContract")]
        public AppAutoRenewContractResponse AutoRenewContract([FromBody] AppAutoRenewContractRequest request)
        {
            var autoRenewContractResponse = new AppAutoRenewContractResponse(false, "");

            try
            {
                autoRenewContractResponse = ValidateAutoRenewContractRequest(request);

                if (autoRenewContractResponse != null)
                {
                    return autoRenewContractResponse;
                }
                else
                {

                    var autoRenewContractRequest = new ERPDataModels.Custom.AppAutoRenewContractRequest()
                    {
                        ChannelReferenceId = request.ChannelReferenceId,
                        AutoRenew = request.AutoRenew,
                        SalesOrderId = request.SalesOrderId,
                        PONumber = request.PONumber,
                        IsUpdateRenewalPrice = request.IsUpdateRenewalPrice
                    };

                    var erpInAppPurchaseController = erpAdapterFactory.CreateInAppPurchaseController(currentStore.StoreKey);
                    var clResponse = erpInAppPurchaseController.AutoRenewContract(autoRenewContractRequest);

                    if (clResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(clResponse));
                        autoRenewContractResponse = new AppAutoRenewContractResponse(true, clResponse.Message);
                    }
                    else if (!clResponse.Success)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from AX.");
                        autoRenewContractResponse = new AppAutoRenewContractResponse(false, clResponse.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                autoRenewContractResponse = new AppAutoRenewContractResponse(false, message);
                return autoRenewContractResponse;
            }

            return autoRenewContractResponse;
        }

        /// <summary>
        /// Cancel Contract in D365
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("SalesOrder/CancelContract")]
        [Route("InAppPurchase/CancelContract")]
        public AppCancelContractResponse CancelContract([FromBody] AppCancelContractRequest request)
        {
            AppCancelContractResponse cancelContractResponse = new AppCancelContractResponse(false, "");

            try
            {
                cancelContractResponse = ValidateCancelContractRequest(request);

                if (cancelContractResponse != null)
                {
                    return cancelContractResponse;
                }
                else
                {
                    ERPDataModels.Custom.AppCancelContractRequest cancelContractRequest = new ERPDataModels.Custom.AppCancelContractRequest();
                    cancelContractRequest.ChannelReferenceId = request.ChannelReferenceId;


                    var erpInAppPurchaseController = erpAdapterFactory.CreateInAppPurchaseController(currentStore.StoreKey);
                    var clResponse = erpInAppPurchaseController.CancelContract(cancelContractRequest);

                    if (clResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(clResponse));
                        cancelContractResponse = new AppCancelContractResponse(true, clResponse.Message);
                    }
                    else if (!clResponse.Success)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from AX.");
                        cancelContractResponse = new AppCancelContractResponse(false, clResponse.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cancelContractResponse = new AppCancelContractResponse(false, message);
                return cancelContractResponse;
            }

            return cancelContractResponse;
        }

        /// <summary>
        /// Update AutoRenew flag of contract for all sale lines in D365
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("UpdateSalesOrder/ReactivateContract")]
        [Route("InAppPurchase/ReactivateContract")]
        public ReactivateContractResponse ReactivateContract([FromBody] ReactivateContractRequest request)
        {
            ReactivateContractResponse reactivateContractResponse = new ReactivateContractResponse(false, "");

            try
            {
                reactivateContractResponse = ValidateReactivateContractRequest(request);

                if (reactivateContractResponse != null)
                {
                    return reactivateContractResponse;
                }

                ERPDataModels.Custom.AppReactivateContractRequest reactivateContractRequest = new ERPDataModels.Custom.AppReactivateContractRequest();
                reactivateContractRequest.ChannelReferenceId = request.ChannelReferenceId;
                reactivateContractRequest.SubscriptionExpiryDate = request.SubscriptionExpiryDate;

                var erpInAppPurchaseController = erpAdapterFactory.CreateInAppPurchaseController(currentStore.StoreKey);
                var clResponse = erpInAppPurchaseController.ReactivateContract(reactivateContractRequest);

                if (clResponse.Success)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(clResponse));
                    reactivateContractResponse = new ReactivateContractResponse(true, clResponse.Message);
                }
                else if (!clResponse.Success)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from AX.");
                    reactivateContractResponse = new ReactivateContractResponse(false, clResponse.Message);
                }


            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                reactivateContractResponse = new ReactivateContractResponse(false, message);
                return reactivateContractResponse;
            }

            return reactivateContractResponse;
        }

        /// <summary>
        /// Update Transfer flag of contract for all sale lines in D365
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("UpdateSalesOrder/TransferContract")]
        [Route("InAppPurchase/TransferContract")]
        public TransferContractResponse TransferContract([FromBody] TransferContractRequest request)
        {
            TransferContractResponse transferContractResponse = new TransferContractResponse(false, "");

            try
            {
                transferContractResponse = ValidateTransferContractRequest(request);

                if (transferContractResponse != null)
                {
                    return transferContractResponse;
                }
                else
                {

                    ERPDataModels.Custom.AppTransferContractRequest transferContractRequest = new ERPDataModels.Custom.AppTransferContractRequest();
                    transferContractRequest.ChannelReferenceId = request.ChannelReferenceId;
                    transferContractRequest.CustomerEmail = request.CustomerEmail;

                    var erpInAppPurchaseController = erpAdapterFactory.CreateInAppPurchaseController(currentStore.StoreKey);
                    var clResponse = erpInAppPurchaseController.TransferContract(transferContractRequest);

                    if (clResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(clResponse));
                        transferContractResponse = new TransferContractResponse(true, clResponse.Message);
                    }
                    else if (!clResponse.Success)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from AX.");
                        transferContractResponse = new TransferContractResponse(false, clResponse.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                transferContractResponse = new TransferContractResponse(false, message);
                return transferContractResponse;
            }

            return transferContractResponse;
        }


        /// <summary>
        /// Update AutoRenew flag of contract for all sale lines in D365
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("UpdateSalesOrder/RebuyContract")]
        [Route("InAppPurchase/RebuyContract")]
        public RebuyContractResponse RebuyContract([FromBody] RebuyContractRequest request)
        {
            RebuyContractResponse RebuyContractResponse = new RebuyContractResponse(false, "","");

            try
            {
                RebuyContractResponse = ValidateRebuyContractRequest(request);

                if (RebuyContractResponse != null)
                {
                    return RebuyContractResponse;
                }

                ERPDataModels.Custom.AppRebuyContractRequest rebuyContractRequest = new ERPDataModels.Custom.AppRebuyContractRequest();
                rebuyContractRequest.CustomerRef = request.CustomerRef;
                

                var erpInAppPurchaseController = erpAdapterFactory.CreateInAppPurchaseController(currentStore.StoreKey);
                var clResponse = erpInAppPurchaseController.RebuyContract(rebuyContractRequest);

                if (clResponse.Success)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(clResponse));
                    RebuyContractResponse = new RebuyContractResponse(true, clResponse.Message, clResponse.SalesOrderId);
                }
                else if (!clResponse.Success)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from AX.");
                    RebuyContractResponse = new RebuyContractResponse(false, clResponse.Message, "");
                }


            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                RebuyContractResponse = new RebuyContractResponse(false, message, "");
                return RebuyContractResponse;
            }

            return RebuyContractResponse;
        }
        #endregion

        #region AutoRenew and Cancel Contract Request, Response classes

        /// <summary>
        /// AutoRenew Contract Request
        /// </summary>
        public class AppAutoRenewContractRequest
        {
            /// <summary>
            /// ChannelReferenceId
            /// </summary>
            [Required]
            public string ChannelReferenceId { get; set; }
            /// <summary>
            /// Contract AutoRenew flag
            /// </summary>
            public bool? AutoRenew { get; set; }
            /// <summary>
            /// SalesOrderId
            /// </summary>
            [Required]
            public string SalesOrderId { get; set; }

            /// <summary>
            /// PoNumber
            /// </summary>
            [Required]
            public string PONumber { get; set; }

            /// <summary>
            /// PoNumber
            /// </summary>
            [Required]
            public bool IsUpdateRenewalPrice { get; set; } = false;

        }

        /// <summary>
        /// AutoRenew Contract Response
        /// </summary>
        public class AppAutoRenewContractResponse
        {
            /// <summary>
            /// Initialize a new instance of the AutoRenewResponse 
            /// </summary>
            /// <param name="status">status of autorenew contract response</param>
            /// <param name="message">message of autorenew contract response</param>

            public AppAutoRenewContractResponse(bool status, string message)
            {
                this.status = status;
                this.message = message;
            }

            /// <summary>
            /// status of AutoRenew request
            /// </summary>
            public bool status { get; set; }
            /// <summary>
            /// message against AutoRenew request
            /// </summary>
            public string message { get; set; }
        }

        /// <summary>
        /// CancelContract Request
        /// </summary>
        public class AppCancelContractRequest
        {
            /// <summary>
            /// ChannelReferenceId
            /// </summary>
            [Required]
            public string ChannelReferenceId { get; set; }
        }

        /// <summary>
        /// CancelContract Response
        /// </summary>
        public class AppCancelContractResponse
        {
            /// <summary>
            /// Initialize a new instance of the AppCancelContractResponse 
            /// </summary>
            /// <param name="status">status of cancel contract response</param>
            /// <param name="message">message of cancel contract response</param>

            public AppCancelContractResponse(bool status, string message)
            {
                this.status = status;
                this.message = message;
            }

            /// <summary>
            /// status of cancel contract request
            /// </summary>
            public bool status { get; set; }
            /// <summary>
            /// message against cancel contract request
            /// </summary>
            public string message { get; set; }
        }

        /// <summary>
        /// Reactivate Contract Request
        /// </summary>
        public class ReactivateContractRequest
        {
            /// <summary>
            /// ChannelReferenceId
            /// </summary>
            [Required]
            public string ChannelReferenceId { get; set; }
            /// <summary>
            /// Contract Transfer flag
            /// </summary>
            [Required]
            public string SubscriptionExpiryDate { get; set; }
        }

        /// <summary>
        /// Reactivate Contract Response
        /// </summary>
        public class ReactivateContractResponse
        {
            /// <summary>
            /// Initialize a new instance of the ReactivateResponse 
            /// </summary>
            /// <param name="status">status of reactivate contract response</param>
            /// <param name="message">message of reactivate contract response</param>

            public ReactivateContractResponse(bool status, string message)
            {
                this.status = status;
                this.message = message;
            }

            /// <summary>
            /// status of Reactivate request
            /// </summary>
            public bool status { get; set; }
            /// <summary>
            /// message against Reactivate request
            /// </summary>
            public string message { get; set; }
        }

        /// <summary>
        /// Transfer Contract Request
        /// </summary>
        public class TransferContractRequest
        {
            /// <summary>
            /// ChannelReferenceId
            /// </summary>
            [Required]
            public string ChannelReferenceId { get; set; }
            /// <summary>
            /// Contract Transfer flag
            /// </summary>
            [Required]
            public string CustomerEmail { get; set; }
        }

        /// <summary>
        /// Transfer Contract Response
        /// </summary>
        public class TransferContractResponse
        {
            /// <summary>
            /// Initialize a new instance of the TransferResponse 
            /// </summary>
            /// <param name="status">status of transfer contract response</param>
            /// <param name="message">message of transfer contract response</param>

            public TransferContractResponse(bool status, string message)
            {
                this.status = status;
                this.message = message;
            }

            /// <summary>
            /// status of transfer request
            /// </summary>
            public bool status { get; set; }
            /// <summary>
            /// message against transfer request
            /// </summary>
            public string message { get; set; }
        }

        /// <summary>
        /// Reactivate Contract Request
        /// </summary>
        public class RebuyContractRequest
        {
            /// <summary>
            /// ChannelReferenceId
            /// </summary>
            [Required]
            public string CustomerRef { get; set; }
            
        }

        /// <summary>
        /// Reactivate Contract Response
        /// </summary>
        public class RebuyContractResponse
        {
            /// <summary>
            /// Initialize a new instance of the ReactivateResponse 
            /// </summary>
            /// <param name="status">status of reactivate contract response</param>
            /// <param name="message">message of reactivate contract response</param>

            public RebuyContractResponse(bool status, string message, string salesOrderId)
            {
                this.status = status;
                this.message = message;
                this.salesOrderId = salesOrderId;
            }

            /// <summary>
            /// status of Reactivate request
            /// </summary>
            public bool status { get; set; }
            /// <summary>
            /// message against Reactivate request
            /// </summary>
            public string message { get; set; }
            public string salesOrderId { get; set; }
        }
        #endregion

        #region Private Methods

        private AppAutoRenewContractResponse ValidateAutoRenewContractRequest(AppAutoRenewContractRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new AppAutoRenewContractResponse(false, message);
                }
                else if (String.IsNullOrWhiteSpace(request.ChannelReferenceId) && String.IsNullOrWhiteSpace(request.SalesOrderId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "One of AutoRenewContractRequest.ChannelReferenceId or AutoRenewContractRequest.SalesOrderId should be provided");
                    return new AppAutoRenewContractResponse(false, message);
                }
                else if (!String.IsNullOrWhiteSpace(request.ChannelReferenceId) && !String.IsNullOrWhiteSpace(request.SalesOrderId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Only one from AutoRenewContractRequest.ChannelReferenceId or AutoRenewContractRequest.SalesOrderId should be provided");
                    return new AppAutoRenewContractResponse(false, message);
                }
                else if (request.AutoRenew == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AutoRenewContractRequest.AutoRenew");
                    return new AppAutoRenewContractResponse(false, message);
                }
                else if (request.PONumber != null && request.PONumber.Length > 60)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40006, currentStore, "PONumber", "60");
                    return new AppAutoRenewContractResponse(false, message);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new AppAutoRenewContractResponse(false, message);
            }

            return null;
        }

        private AppCancelContractResponse ValidateCancelContractRequest(AppCancelContractRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new AppCancelContractResponse(false, message);
                }
                else if (String.IsNullOrWhiteSpace(request.ChannelReferenceId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "AppCancelContractRequest.ChannelReferenceId");
                    return new AppCancelContractResponse(false, message);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new AppCancelContractResponse(false, message);
            }

            return null;
        }


        private ReactivateContractResponse ValidateReactivateContractRequest(ReactivateContractRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new ReactivateContractResponse(false, message);
                }
                else if (String.IsNullOrWhiteSpace(request.ChannelReferenceId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ReactivateContractRequest.ChannelReferenceId");
                    return new ReactivateContractResponse(false, message);
                }
                else if (String.IsNullOrWhiteSpace(request.SubscriptionExpiryDate))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "ReactivateContractRequest.SED");
                    return new ReactivateContractResponse(false, message);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new ReactivateContractResponse(false, message);
            }

            return null;
        }

        private TransferContractResponse ValidateTransferContractRequest(TransferContractRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new TransferContractResponse(false, message);
                }
                else if (String.IsNullOrWhiteSpace(request.ChannelReferenceId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "TransferContractRequest.ChannelReferenceId");
                    return new TransferContractResponse(false, message);
                }
                else if (String.IsNullOrWhiteSpace(request.CustomerEmail))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "TransferContractRequest.CustomerEmail");
                    return new TransferContractResponse(false, message);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new TransferContractResponse(false, message);
            }

            return null;
        }

        private RebuyContractResponse ValidateRebuyContractRequest(RebuyContractRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new RebuyContractResponse(false, message,"");
                }
                else if (String.IsNullOrWhiteSpace(request.CustomerRef))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "RebuyContractRequest.ChannelReferenceId");
                    return new RebuyContractResponse(false, message,"");
                }
                
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new RebuyContractResponse(false, message,"");
            }

            return null;
        }
        #endregion
    }
}