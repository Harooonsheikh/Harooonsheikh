using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web
{
    /// <summary>
    ///  WishListController defines properties and methods for API controller for wishlist.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class WishListController : ApiBaseController
    {
        /// <summary>
        /// WishList Controller in constructor
        /// </summary>
        public WishListController()
        {
            ControllerName = "WishListController";
        }

        #region API Methods

        /// <summary>
        /// Create Wish List
        /// </summary>
        /// <param name="erpCommerceList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("WishList/CreateWishList")]
        public WishListResponse CreateWishList([FromBody] ErpCommerceList erpCommerceList)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (erpCommerceList == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001,currentStore, MethodBase.GetCurrentMethod().Name);
                return new WishListResponse("false", message, null);
            }
            else if (string.IsNullOrEmpty(erpCommerceList.CustomerId) || string.IsNullOrWhiteSpace(erpCommerceList.CustomerId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "erpCommerceList.CustomerId");
                return new WishListResponse("false", message, null);
            }
            else if (string.IsNullOrEmpty(erpCommerceList.Name) || string.IsNullOrWhiteSpace(erpCommerceList.Name))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "erpCommerceList.Name");
                return new WishListResponse("false", message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "erpCommerceList: {0}", JsonConvert.SerializeObject(erpCommerceList));

            try
            { 
                try
                {
                    var erpWishListController = erpAdapterFactory.CreateWishListController(currentStore.StoreKey);
                    ErpCommerceList wishList = erpWishListController.CreateWishList(erpCommerceList);
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(wishList));
                    if (wishList != null)
                    {
                        return new WishListResponse("true", null, wishList.Id.ToString());
                    }
                    else
                    {
                        return new WishListResponse("false", "Wish List not craeted", null);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                    return new WishListResponse("false", message, null);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new WishListResponse("false", message, null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new WishListResponse("false", message, null);
            }
        }

        /// <summary>
        /// Get Wish Lists
        /// </summary>
        /// <param name="wishListId"></param>
        /// <param name="customerId"></param>
        /// <param name="favoriteFilter"></param>
        /// <param name="publicFilter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("WishList/GetWishLists")]
        [Obsolete("GetWishLists is deprecated, please use GetWishLists with POST parameter instead.")]
        public WishListSearchResponse GetWishLists([FromUri] long wishListId, string customerId, bool favoriteFilter, bool publicFilter)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrEmpty(customerId) || string.IsNullOrWhiteSpace(customerId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customerId");
                return new WishListSearchResponse("false", message, null);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "wishListId: {0}, customerId: {1}, favoriteFilter: {2}, publicFilter: {3}", 
                wishListId.ToString(), customerId, favoriteFilter.ToString(), publicFilter.ToString());
            try
            { 
                try
                {
                    var erpWishListController = erpAdapterFactory.CreateWishListController(currentStore.StoreKey);
                    List<ErpCommerceList> wishLists = erpWishListController.GetWishList(wishListId, customerId, favoriteFilter, publicFilter);
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(wishLists));

                    if (wishLists == null || wishLists.Count == 0)
                    {
                        return new WishListSearchResponse("false", "No Wish Lists found", null);
                    }
                    else
                    {
                        return new WishListSearchResponse("true", null, wishLists);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                    return new WishListSearchResponse("false", message, null);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new WishListSearchResponse("false", message, null);
            }
        }


        /// <summary>
        /// Get Wish Lists
        /// </summary>
        /// <param name="wishListRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [SanitizeInput]
        [Route("WishList/GetWishLists")]
        public WishListSearchResponse GetWishLists([FromBody] GetWishListRequest wishListRequest)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrEmpty(wishListRequest.customerId) || string.IsNullOrWhiteSpace(wishListRequest.customerId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customerId");
                return new WishListSearchResponse("false", message, null);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "wishListId: {0}, customerId: {1}, favoriteFilter: {2}, publicFilter: {3}",
                wishListRequest.wishListId.ToString(), wishListRequest.customerId, wishListRequest.favoriteFilter.ToString(), wishListRequest.publicFilter.ToString());
            try
            {
                try
                {
                    var erpWishListController = erpAdapterFactory.CreateWishListController(currentStore.StoreKey);
                    List<ErpCommerceList> wishLists = erpWishListController.GetWishList(wishListRequest.wishListId, wishListRequest.customerId, wishListRequest.favoriteFilter, wishListRequest.publicFilter);
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(wishLists));

                    if (wishLists == null || wishLists.Count == 0)
                    {
                        return new WishListSearchResponse("false", "No Wish Lists found", null);
                    }
                    else
                    {
                        return new WishListSearchResponse("true", null, wishLists);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                    return new WishListSearchResponse("false", message, null);

                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new WishListSearchResponse("false", message, null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new WishListSearchResponse("false", message, null);
            }
        }

        /// <summary>
        /// Delete Wish List
        /// </summary>
        /// <param name="wishListId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("WishList/DeleteWishList")]
        public WishListResponse DeleteWishList([FromUri] long wishListId, string customerId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrEmpty(customerId) || string.IsNullOrWhiteSpace(customerId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "customerId");
                return new WishListResponse("false", message, null);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "wishListId: {0}, customerId: {1}", wishListId.ToString(), customerId);
            try
            { 
                try
                {
                    var erpWishListController = erpAdapterFactory.CreateWishListController(currentStore.StoreKey);
                    bool deleteResult = erpWishListController.DeleteWishList(wishListId, customerId);
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, deleteResult.ToString());
                    return new WishListResponse("true", null, null);
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                    return new WishListResponse("false", message, null);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new WishListResponse("false", message, null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new WishListResponse("false", message, null);
            }
        }

        /// <summary>
        /// Create Wish List Line
        /// </summary>
        /// <param name="erpCommerceListLine"></param>
        /// <param name="filterAccountNumber"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("WishList/CreateWishListLine")]
        public WishListResponse CreateWishListLine([FromBody] ErpCommerceListLine erpCommerceListLine, [FromUri] string filterAccountNumber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (erpCommerceListLine == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new WishListResponse("false", message, null);
            }
            else if (string.IsNullOrEmpty(filterAccountNumber) || string.IsNullOrWhiteSpace(filterAccountNumber))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "filterAccountNumber");
                return new WishListResponse("false", message, null);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "erpCommerceListLine: {0}, filterAccountNumber: {1}",
                JsonConvert.SerializeObject(erpCommerceListLine), filterAccountNumber);

            try
            { 
                try
                {
                    var erpWishListController = erpAdapterFactory.CreateWishListController(currentStore.StoreKey);
                    ErpCommerceList wishList = erpWishListController.CreateWishListLine(erpCommerceListLine, filterAccountNumber);
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(wishList));
                    if (wishList != null)
                    {
                        return new WishListResponse("true", null, erpCommerceListLine.CommerceListId.ToString());
                    }
                    else
                    {
                        return new WishListResponse("false", "Wish List Line not created", null);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                    return new WishListResponse("false", message, null);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new WishListResponse("false", message, null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new WishListResponse("false", message, null);
            }
        }

        /// <summary>
        /// Update Wish List Line
        /// </summary>
        /// <param name="erpCommerceListLine"></param>
        /// <param name="filterAccountNumber"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("WishList/UpdateWishListLine")]
        public WishListResponse UpdateWishListLine([FromBody] ErpCommerceListLine erpCommerceListLine, [FromUri] string filterAccountNumber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            if (erpCommerceListLine == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new WishListResponse("false", message, null);
            }
            else if (string.IsNullOrEmpty(filterAccountNumber) || string.IsNullOrWhiteSpace(filterAccountNumber))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "filterAccountNumber");
                return new WishListResponse("false", message, null);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "erpCommerceListLine: {0}, filterAccountNumber: {1}",
                JsonConvert.SerializeObject(erpCommerceListLine), filterAccountNumber);
            try
            { 
                try
                {
                    var erpWishListController = erpAdapterFactory.CreateWishListController(currentStore.StoreKey);
                    ErpCommerceList wishList = erpWishListController.UpdateWishListLine(erpCommerceListLine, filterAccountNumber);
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(wishList));
                    if (wishList != null)
                    {
                        return new WishListResponse("true", null, erpCommerceListLine.CommerceListId.ToString());
                    }
                    else
                    {
                        return new WishListResponse("false", "Wish List Line not undated", null);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                    return new WishListResponse("false", message, null);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new WishListResponse("false", message, null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new WishListResponse("false", message, null);
            }
        }

        /// <summary>
        /// Delete Wish List Line
        /// </summary>
        /// <param name="wishListLineId"></param>
        /// <param name="wishListId"></param>
        /// <param name="filterAccountNumber"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("WishList/DeleteWishListLine")]
        public WishListResponse DeleteWishListLine([FromUri] long wishListLineId, long wishListId, string filterAccountNumber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrEmpty(filterAccountNumber) || string.IsNullOrWhiteSpace(filterAccountNumber))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new WishListResponse("false", message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "wishListLineId: {0}, wishListId: {1}, filterAccountNumber: {2}",
                wishListLineId, wishListId, filterAccountNumber);
            try
            { 
                try
                {
                    var erpWishListController = erpAdapterFactory.CreateWishListController(currentStore.StoreKey);
                    ErpCommerceList wishList = erpWishListController.DeleteWishListLine(wishListLineId, wishListId, filterAccountNumber);
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(wishList));
                    if (wishList != null)
                    {
                        return new WishListResponse("true", null, wishList.Id);
                    }
                    else
                    {
                        return new WishListResponse("false", "Wish List Line not deleted", wishListId.ToString());
                    }
                }
                catch (Exception exp)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                    return new WishListResponse("false", message, null);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new WishListResponse("false", message, null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new WishListResponse("false", message, null);
            }
        }

        #endregion

        #region " Response Classes "

        /// <summary>
        /// Wish List Response
        /// </summary>
        public class WishListResponse
        {
            /// <summary>
            /// Initializes a new instance of Wish List Response
            /// </summary>
            /// <param name="status"></param>
            /// <param name="message"></param>
            /// <param name="wishListId"></param>
            public WishListResponse(string status, string message, string wishListId)
            {
                this.Status = status;
                this.WishListId = wishListId;
                this.Message = message;
            }

            /// <summary>
            /// Status of Response
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// Message of Response
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Wish List Id
            /// </summary>
            public string WishListId { get; set; }

        }

        /// <summary>
        /// Initializes a new instance of Wish List Search Response
        /// </summary>
        public class WishListSearchResponse
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="status"></param>
            /// <param name="message"></param>
            /// <param name="erpWishLists"></param>
            public WishListSearchResponse(string status, string message, List<ErpCommerceList> erpWishLists)
            {
                this.Status = status;
                this.Message = message;
                this.WishLists = erpWishLists;
            }

            /// <summary>
            /// Status of Response
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// Message of Response
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Wish Lists
            /// </summary>
            public List<ErpCommerceList> WishLists { get; set; }
        }

        public class GetWishListRequest
        {
            /// <summary>
            /// Status of Response
            /// </summary>
            public long wishListId { get; set; }

            /// <summary>
            /// Message of Response
            /// </summary>
            [Required]
            public string customerId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool favoriteFilter { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool publicFilter { get; set; }
        }
        #endregion

    }
}