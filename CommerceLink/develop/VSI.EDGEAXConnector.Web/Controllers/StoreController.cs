using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web
{

    /// <summary>
    /// StoreController defines properties and methods for API controller for Store.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class StoreController : ApiBaseController
    {
        /// <summary>
        /// Store Controller
        /// </summary>
        public StoreController()
        {
            ControllerName = "StoreController";
        }

        #region API Methods

        /// <summary>
        /// Inventory Lookup - returns inventory for selected item ID and variant ID from different stores
        /// </summary>
        /// <param name="itemId">itemId for inventory lookup</param>
        /// <param name="variantId">itemId for inventory lookup</param>
        /// <returns>GetStoreAvailabilityResponse</returns>
        [RequestResponseLog]
        [HttpGet]
        [Route("Store/GetStoreAvailability")]
        [Obsolete("GetStoreAvailability is deprecated, please use GetStoreAvailability with POST parameter instead.")]
        public GetStoreAvailabilityResponse GetStoreAvailability([FromUri] string itemId, [FromUri] string variantId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            // Throw error if getStoreAvailabilityRequest is null
            if (string.IsNullOrEmpty(itemId) || string.IsNullOrWhiteSpace(itemId))
            {
                //throw new System.ArgumentException("Invalid parameter value for 'itemId'.", "itemId", new HttpResponseException(HttpStatusCode.BadRequest));
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "itemId");
                return new GetStoreAvailabilityResponse(message, null);
            }
            if (string.IsNullOrEmpty(variantId) || string.IsNullOrWhiteSpace(variantId))
            {
                //throw new System.ArgumentException("Invalid parameter value for 'variantId'.", "variantId", new HttpResponseException(HttpStatusCode.BadRequest));
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "variantId");
                return new GetStoreAvailabilityResponse(message, null);
            }

            List<ErpInventoryInfo> inventoryData = new List<ErpInventoryInfo>();
            GetStoreAvailabilityResponse inventoryLookupData;
            inventoryLookupData = new GetStoreAvailabilityResponse("false", null);

            try
            {
                //TODO: Add parametters valiations if required
                try
                {
                    var erpStoreController = erpAdapterFactory.CreateStoreController(currentStore.StoreKey);

                    inventoryData = erpStoreController.GetStoreAvailability(itemId, variantId);


                    if (inventoryData != null)
                    {
                        inventoryLookupData = new GetStoreAvailabilityResponse("true", inventoryData);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), GetRequestGUID(Request));
                    throw new Exception(message); ;
                }
            }
            catch (ArgumentException exp)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(exp));
                return new GetStoreAvailabilityResponse(message, null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), GetRequestGUID(Request));
                throw new Exception(message);
            }

            return inventoryLookupData;
        }

        /// <summary>
        /// Inventory Lookup - returns inventory for selected item ID and variant ID from different stores
        /// </summary>
        /// <param name="storeAvailabilityRequest"></param>
        /// <returns>GetStoreAvailabilityResponse</returns>
        [RequestResponseLog]
        [SanitizeInput]
        [HttpPost]
        [Route("Store/GetStoreAvailability")]
        public GetStoreAvailabilityResponse GetStoreAvailability([FromBody] GetStoreAvailabilityRequest storeAvailabilityRequest)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            // Throw error if getStoreAvailabilityRequest is null
            if (string.IsNullOrEmpty(storeAvailabilityRequest.itemId) || string.IsNullOrWhiteSpace(storeAvailabilityRequest.itemId))
            {
                //throw new System.ArgumentException("Invalid parameter value for 'itemId'.", "itemId", new HttpResponseException(HttpStatusCode.BadRequest));
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "itemId");
                return new GetStoreAvailabilityResponse(message, null);
            }
            if (string.IsNullOrEmpty(storeAvailabilityRequest.variantId) || string.IsNullOrWhiteSpace(storeAvailabilityRequest.variantId))
            {
                //throw new System.ArgumentException("Invalid parameter value for 'variantId'.", "variantId", new HttpResponseException(HttpStatusCode.BadRequest));
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "variantId");
                return new GetStoreAvailabilityResponse(message, null);
            }

            List<ErpInventoryInfo> inventoryData = new List<ErpInventoryInfo>();
            GetStoreAvailabilityResponse inventoryLookupData;
            inventoryLookupData = new GetStoreAvailabilityResponse("false", null);

            try
            {
                //TODO: Add parametters valiations if required
                try
                {
                    var erpStoreController = erpAdapterFactory.CreateStoreController(currentStore.StoreKey);

                    inventoryData = erpStoreController.GetStoreAvailability(storeAvailabilityRequest.itemId, storeAvailabilityRequest.variantId);


                    if (inventoryData != null)
                    {
                        inventoryLookupData = new GetStoreAvailabilityResponse("true", inventoryData);
                    }
                }
                catch (Exception ex)
                {
                    //ex.Data.Add("store:", HttpStatusCode.InternalServerError);
                    //ex.Data.Add("message", "Exception occured at Dynamics Retail Server.");
                    //throw ex;
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(HttpStatusCode.InternalServerError));
                    return new GetStoreAvailabilityResponse(message, null);
                }
            }
            catch (ArgumentException ex)
            {
                //CustomLogger.LogException(exp, ControllerName);
                //return inventoryLookupData;
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GetStoreAvailabilityResponse(message, null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GetStoreAvailabilityResponse(message, null);
            }

            return inventoryLookupData;
        }

        /// <summary>
        /// GetDiscountThreshold - Returns Discount Threshold
        /// </summary>
        /// <returns>Get</returns>
        [RequestResponseLog]
        [HttpGet]
        [Route("Store/GetDiscountThreshold")]
        public ErpChannelCustomDiscountThresholdResponse GetDiscountThreshold()
        {
            ErpChannelCustomDiscountThresholdResponse response = new ErpChannelCustomDiscountThresholdResponse(false, string.Empty, string.Empty);
            try
            {
                var erpStoreController = erpAdapterFactory.CreateStoreController(currentStore.StoreKey);
                response = erpStoreController.GetDiscountThreshold();
                if (response.Success)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(response));
                    response = new ErpChannelCustomDiscountThresholdResponse(response.Success, response.Message, response.Result);
                }
                else if (!response.Success)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, "Result not found from AX.");
                    response = new ErpChannelCustomDiscountThresholdResponse(response.Success, response.Message, response.Result);
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                response = new ErpChannelCustomDiscountThresholdResponse(false, message, string.Empty);
                return response;

            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return response;
        }

        #endregion

        #region GetStoreAvailability Request, Response classes

        /// <summary>
        /// Represents Inventory Lookup response
        /// </summary>
        public class GetStoreAvailabilityResponse
        {

            /// <summary>
            /// Initializes a new instance of the GetStoreAvailabilityResponse
            /// </summary>
            public GetStoreAvailabilityResponse()
            {
                this.status = status;
                this.inventoryLookupData = new List<ErpInventoryInfo>();
            }

            /// <summary>
            /// Initializes a new instance of the GetStoreAvailabilityResponse
            /// </summary>
            /// <param name="status">status of Inventory Lookup</param>
            /// <param name="inventoryLookupData">data of Inventory Lookup</param>
            public GetStoreAvailabilityResponse(string status, List<ErpInventoryInfo> inventoryLookupData)
            {
                this.status = status;
                this.inventoryLookupData = inventoryLookupData;
            }

            /// <summary>
            /// status of Inventory Lookup
            /// </summary>
            public string status { get; set; }

            /// <summary>
            /// Data of Inventory Lookup
            /// </summary>
            public List<ErpInventoryInfo> inventoryLookupData { get; set; }            
        }

        public class GetStoreAvailabilityRequest
        {
            /// <summary>
            /// variantId of Inventory Lookup
            /// </summary>
            [Required]
            public string variantId { get; set; }

            /// <summary>
            /// itemId of Inventory Lookup
            /// </summary>
            [Required]
            public string itemId { get; set; }
        }


        #endregion
    }
}



