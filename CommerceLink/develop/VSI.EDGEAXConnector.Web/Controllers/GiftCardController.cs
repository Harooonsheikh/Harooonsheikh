using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web
{

    /// <summary>
    /// GiftCardController defines properties and methods for API controller for Gift Card.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class GiftCardController : ApiBaseController
    {

        /// <summary>
        /// Gift Card Controller 
        /// </summary>
        public GiftCardController()
        {
            ControllerName = "GiftCardController";
        }

        #region API Methods


        /// <summary>
        /// IssueGiftCard issues card with provided details.
        /// </summary>
        /// <param name="giftCardRequest">Gift Card request to be created</param>
        /// <returns>GiftCardResponse</returns>
        [HttpPost]
        [Route("GiftCard/IssueGiftCard")]
        public GiftCardResponse IssueGiftCard([FromBody] GiftCardRequest giftCardRequest)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (giftCardRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new GiftCardResponse("false", message, 0);
            }
            else if (string.IsNullOrEmpty(giftCardRequest.giftCardId) || string.IsNullOrWhiteSpace(giftCardRequest.giftCardId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardRequest.giftCardId");
                return new GiftCardResponse("false", message, 0);
            }
            else if (string.IsNullOrEmpty(giftCardRequest.currencyCode) || string.IsNullOrWhiteSpace(giftCardRequest.currencyCode))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardRequest.currencyCode");
                return new GiftCardResponse("false", message, 0);
            }
            else if (string.IsNullOrEmpty(giftCardRequest.transactionId) || string.IsNullOrWhiteSpace(giftCardRequest.transactionId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardRequest.transactionId");
                return new GiftCardResponse("false", message, 0);
            }

            // Extract the data from parameter
            string requestedGiftCardId = giftCardRequest.giftCardId;
            decimal amount = giftCardRequest.amount;
            string depositCurrencyCode = giftCardRequest.currencyCode;
            string transactionId = giftCardRequest.transactionId;

            string terminalId = string.Empty;
            string staffId = string.Empty;
            string receiptId = string.Empty;

            ErpGiftCard giftCard = new ErpGiftCard();
            GiftCardResponse giftCardResponse = new GiftCardResponse("false", requestedGiftCardId, 0);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "requestedGiftCardId: {0}, amount: {1}, depositCurrencyCode: {2}, transactionId: {3}",
                requestedGiftCardId, amount, depositCurrencyCode, transactionId);

            try
            {
                try
                {
                    var erpGiftCardController = erpAdapterFactory.CreateGiftCardController(currentStore.StoreKey);

                    giftCard = erpGiftCardController.IssueGiftCard(requestedGiftCardId, amount, depositCurrencyCode, 0, terminalId, staffId, transactionId, receiptId);

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(giftCard));

                    if (giftCard.Id != null)
                    {
                        return new GiftCardResponse("true", giftCard.Id, giftCard.Balance);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name,
                        JsonConvert.SerializeObject(HttpStatusCode.InternalServerError));
                    return new GiftCardResponse("false", message, 0);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardResponse("false", message, 0);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new GiftCardResponse("false", message, 0);
            }

            return giftCardResponse;
        }

        /// <summary>
        /// GetGiftCardBalance returns balance and currency of provided card.
        /// </summary>
        /// <param name="giftCardId">Gift Card request to be get balance</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GiftCard/GetGiftCardBalance")]
        [Obsolete("GetGiftCardBalance is deprecated, please use GetGiftCardBalance with POST parameter instead.")]
        public GiftCardLargeResponse GetGiftCardBalance([FromUri] string giftCardId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrEmpty(giftCardId) || string.IsNullOrWhiteSpace(giftCardId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardId");
                return new GiftCardLargeResponse("false", null, 0, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardId: {0}", giftCardId);

            ErpGiftCard giftCard = new ErpGiftCard();
            GiftCardLargeResponse giftCardResponse = new GiftCardLargeResponse("false", giftCardId, 0, "");

            try
            {
                //TODO: Add parametters valiations if required
                try
                {
                    var erpGiftCardController = erpAdapterFactory.CreateGiftCardController(currentStore.StoreKey);
                    giftCard = erpGiftCardController.GetGiftCardBalance(giftCardId);
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(giftCard));

                    if (giftCard.Id != null)
                    {
                        giftCardResponse = new GiftCardLargeResponse("true", giftCard.Id, giftCard.Balance, giftCard.CardCurrencyCode);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                    
                    return new GiftCardLargeResponse("false", null, 0, null);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardLargeResponse("false", null, 0, null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardLargeResponse("false", null, 0, null);
            }

            return giftCardResponse;
        }


        /// <summary>
        /// GetGiftCardBalance returns balance and currency of provided card.
        /// </summary>
        /// <param name="GiftCardIdRequest">Gift Card request to be get balance</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GiftCard/GetGiftCardBalance")]
        [Authorize(Roles = "eCommerce")]
        public GiftCardLargeResponse GetGiftCardBalance([FromBody]  GiftCardIdRequest GiftCardId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrEmpty(GiftCardId.giftCardId) || string.IsNullOrWhiteSpace(GiftCardId.giftCardId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardId");
                return new GiftCardLargeResponse("false", null, 0, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardId: {0}", GiftCardId.giftCardId);

            ErpGiftCard giftCard = new ErpGiftCard();
            GiftCardLargeResponse giftCardResponse = new GiftCardLargeResponse("false", GiftCardId.giftCardId, 0, "");

            try
            {
                //TODO: Add parametters valiations if required
                try
                {
                    var erpGiftCardController = erpAdapterFactory.CreateGiftCardController(currentStore.StoreKey);
                    giftCard = erpGiftCardController.GetGiftCardBalance(GiftCardId.giftCardId);
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(giftCard));

                    if (giftCard.Id != null)
                    {
                        giftCardResponse = new GiftCardLargeResponse("true", giftCard.Id, giftCard.Balance, giftCard.CardCurrencyCode);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name,
                        JsonConvert.SerializeObject(HttpStatusCode.InternalServerError));
                    return new GiftCardLargeResponse("false", null, 0, null);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardLargeResponse("false", null, 0, null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardLargeResponse("false", null, 0, null);
            }

            return giftCardResponse;
        }

        /// <summary>
        /// LockGiftCard locks the gift card.
        /// </summary>
        /// <param name="giftCardId">Gift Card request to be Lock</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GiftCard/LockGiftCard")]
        [Obsolete("LockGiftCard is deprecated, please use LockGiftCard with POST parameter instead.")]
        public GiftCardResponse LockGiftCard([FromUri] string giftCardId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrEmpty(giftCardId) || string.IsNullOrWhiteSpace(giftCardId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new GiftCardResponse("false", message, 0);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardId: {0}", giftCardId);

            string terminalId = String.Empty;

            ErpGiftCard giftCard = new ErpGiftCard();
            GiftCardResponse giftCardResponse = giftCardResponse = new GiftCardResponse("false", giftCardId, 0);

            try
            {
                //TODO: Add parametters valiations if required
                try
                {
                    var erpGiftCardController = erpAdapterFactory.CreateGiftCardController(currentStore.StoreKey);

                    giftCard = erpGiftCardController.LockGiftCard(giftCardId, 0, terminalId);

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(giftCard));

                    if (giftCard.Id != null)
                    {
                        giftCardResponse = new GiftCardResponse("true", giftCard.Id, giftCard.Balance);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                    return new GiftCardResponse("false", message, 0);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardResponse("false", message, 0);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardResponse("false", message, 0);
            }

            return giftCardResponse;
        }



        /// <summary>
        /// LockGiftCard locks the gift card.
        /// </summary>
        /// <param name="GiftCardIdRequest">Gift Card request to be Lock</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GiftCard/LockGiftCard")]
        public GiftCardResponse LockGiftCard([FromBody] GiftCardIdRequest GiftCardId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrEmpty(GiftCardId.giftCardId) || string.IsNullOrWhiteSpace(GiftCardId.giftCardId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new GiftCardResponse("false", message, 0);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardId: {0}", GiftCardId.giftCardId);

            string terminalId = String.Empty;

            ErpGiftCard giftCard = new ErpGiftCard();
            GiftCardResponse giftCardResponse = giftCardResponse = new GiftCardResponse("false", GiftCardId.giftCardId, 0);

            try
            {
                //TODO: Add parametters valiations if required
                try
                {
                    var erpGiftCardController = erpAdapterFactory.CreateGiftCardController(currentStore.StoreKey);

                    giftCard = erpGiftCardController.LockGiftCard(GiftCardId.giftCardId, 0, terminalId);

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(giftCard));

                    if (giftCard.Id != null)
                    {
                        giftCardResponse = new GiftCardResponse("true", giftCard.Id, giftCard.Balance);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name,
                        JsonConvert.SerializeObject(HttpStatusCode.InternalServerError));
                    return new GiftCardResponse("false", message, 0);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardResponse("false", message, 0);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardResponse("false", message, 0);
            }

            return giftCardResponse;
        }

        /// <summary>
        /// UnlockGiftCard unlocks the gift card.
        /// </summary>
        /// <param name="giftCardId">Gift Card request to be Unlock</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GiftCard/UnlockGiftCard")]
        [Obsolete("UnlockGiftCard is deprecated, please use UnlockGiftCard with POST parameter instead.")]
        public GiftCardSmallResponse UnlockGiftCard([FromUri] string giftCardId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            if (string.IsNullOrEmpty(giftCardId) || string.IsNullOrWhiteSpace(giftCardId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardId");
                return new GiftCardSmallResponse("false", null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardId: {0}", giftCardId);
            GiftCardSmallResponse giftCardResponse = new GiftCardSmallResponse("false", giftCardId);

            try
            {
                //TODO: Add parametters valiations if required
                try
                {
                    var erpGiftCardController = erpAdapterFactory.CreateGiftCardController(currentStore.StoreKey);

                    bool result = erpGiftCardController.UnlockGiftCard(giftCardId);

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, result.ToString());

                    if (result)
                    {
                        giftCardResponse = new GiftCardSmallResponse("true", giftCardId);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                    return new GiftCardSmallResponse("false", null);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardSmallResponse("false", null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardSmallResponse("false", null);
            }

            return giftCardResponse;
        }

        /// <summary>
        /// UnlockGiftCard unlocks the gift card.
        /// </summary>
        /// <param name="giftCardId">Gift Card request to be Unlock</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GiftCard/UnlockGiftCard")]
        [Authorize(Roles = "eCommerce")]
        public GiftCardSmallResponse UnlockGiftCard([FromBody] GiftCardIdRequest GiftCardId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            if (string.IsNullOrEmpty(GiftCardId.giftCardId) || string.IsNullOrWhiteSpace(GiftCardId.giftCardId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardId");
                return new GiftCardSmallResponse("false", null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardId: {0}", GiftCardId.giftCardId);
            GiftCardSmallResponse giftCardResponse = new GiftCardSmallResponse("false", GiftCardId.giftCardId);

            try
            {
                //TODO: Add parametters valiations if required
                try
                {
                    var erpGiftCardController = erpAdapterFactory.CreateGiftCardController(currentStore.StoreKey);

                    bool result = erpGiftCardController.UnlockGiftCard(GiftCardId.giftCardId);

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, result.ToString());

                    if (result)
                    {
                        giftCardResponse = new GiftCardSmallResponse("true", GiftCardId.giftCardId);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                    return new GiftCardSmallResponse("false", null);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardSmallResponse("false", null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GiftCardSmallResponse("false", null);
            }

            return giftCardResponse;
        }

        /// <summary>
        /// PayGiftCard pays for sales order transaction returns call stauts with gift card.
        /// </summary>
        /// <param name="giftCardRequest">Gift Card request to be Pay Amount</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GiftCard/PayGiftCard")]
        public GiftCardSmallResponse PayGiftCard([FromBody] GiftCardRequest giftCardRequest)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (giftCardRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new GiftCardSmallResponse("false", null);
            }
            else if (string.IsNullOrEmpty(giftCardRequest.giftCardId) || string.IsNullOrWhiteSpace(giftCardRequest.giftCardId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardRequest.giftCardId");
                return new GiftCardSmallResponse("false", null);
            }
            else if (string.IsNullOrEmpty(giftCardRequest.currencyCode) || string.IsNullOrWhiteSpace(giftCardRequest.currencyCode))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardRequest.currencyCode");
                return new GiftCardSmallResponse("false", null);
            }
            else if (string.IsNullOrEmpty(giftCardRequest.transactionId) || string.IsNullOrWhiteSpace(giftCardRequest.transactionId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardRequest.transactionId");
                return new GiftCardSmallResponse("false", null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "giftCardRequest: {0}", JsonConvert.SerializeObject(giftCardRequest));

            // Extract the data from parameter
            string giftCardId = giftCardRequest.giftCardId;
            decimal paymentAmount = giftCardRequest.amount;
            string paymentCurrencyCode = giftCardRequest.currencyCode;
            string transactionId = giftCardRequest.transactionId;

            string terminalId = string.Empty;
            string staffId = string.Empty;
            string receiptId = string.Empty;

            GiftCardSmallResponse giftCardResponse = new GiftCardSmallResponse("false", giftCardId);

            try
            {
                //TODO: Add parametters valiations if required
                try
                {
                    var erpGiftCardController = erpAdapterFactory.CreateGiftCardController(currentStore.StoreKey);

                    bool result = erpGiftCardController.PayGiftCard(giftCardId, paymentAmount, paymentCurrencyCode, 0, terminalId, staffId, transactionId, receiptId);

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, result.ToString());

                    if (result)
                    {
                        giftCardResponse = new GiftCardSmallResponse("true", giftCardId);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                    return new GiftCardSmallResponse("false", null);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new GiftCardSmallResponse("false", null);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                return new GiftCardSmallResponse("false", null);
            }

            return giftCardResponse;
        }

        #endregion

        #region GiftCard Request, Response classes

        /// <summary>
        /// Represents gift card request.
        /// </summary>
        public class GiftCardRequest
        {
            /// <summary>
            /// giftCardId for gift card request
            /// </summary>
            [Required]
            public String giftCardId { get; set; }

            /// <summary>
            /// amount for gift card request
            /// </summary>
            public decimal amount { get; set; }

            /// <summary>
            /// currencyCode for gift card request
            /// </summary>
            [Required]
            public String currencyCode { get; set; }

            /// <summary>
            /// transactionId for gift card request
            /// </summary>
            [Required]
            public String transactionId { get; set; }
        }

        /// <summary>
        /// Represents gift card response
        /// </summary>
        public class GiftCardResponse
        {

            /// <summary>
            /// Initializes a new instance of the GiftCardResponse
            /// </summary>
            /// <param name="status">status of gift card</param>
            /// <param name="giftCardId">id of gift card</param>
            /// <param name="balance">balance of gift card</param>
            public GiftCardResponse(string status, string giftCardId, decimal balance)
            {
                this.status = status;
                this.giftCardId = giftCardId;
                this.balance = balance;
            }

            /// <summary>
            /// status of gift card
            /// </summary>
            public string status { get; set; }

            /// <summary>
            /// id of gift card
            /// </summary>
            public string giftCardId { get; set; }

            /// <summary>
            /// balance of gift card
            /// </summary>
            public decimal balance { get; set; }
        }

        /// <summary>
        /// Represents gift card small response
        /// </summary>
        public class GiftCardLargeResponse
        {

            /// <summary>
            /// Initializes a new instance of the GiftCardSmallResponse
            /// </summary>
            /// <param name="status">status of gift card</param>
            /// <param name="giftCardId">id of gift card</param>
            /// <param name="balance">balance of gift card</param>
            /// <param name="currency">currency of gift card</param>
            public GiftCardLargeResponse(string status, string giftCardId, decimal balance, string currency)
            {
                this.status = status;
                this.giftCardId = giftCardId;
                this.balance = balance;
                this.currency = currency;
            }

            /// <summary>
            /// status of gift card
            /// </summary>
            public string status { get; set; }

            /// <summary>
            /// id of gift card
            /// </summary>
            public string giftCardId { get; set; }

            /// <summary>
            /// balance of gift card
            /// </summary>
            public decimal balance { get; set; }

            /// <summary>
            /// currency of gift card
            /// </summary>
            public string currency { get; set; }
        }

        /// <summary>
        /// Represents gift card small response
        /// </summary>
        public class GiftCardSmallResponse
        {

            /// <summary>
            /// Initializes a new instance of the GiftCardSmallResponse
            /// </summary>
            /// <param name="status">status of gift card</param>
            /// <param name="giftCardId">id of gift card</param>
            public GiftCardSmallResponse(string status, string giftCardId)
            {
                this.status = status;
                this.giftCardId = giftCardId;
            }

            /// <summary>
            /// status of gift card
            /// </summary>
            public string status { get; set; }

            /// <summary>
            /// id of gift card
            /// </summary>
            [Required]
            public string giftCardId { get; set; }
        }

        public class GiftCardIdRequest
        {
            [Required]
            public string giftCardId { get; set; }
        }

        #endregion

    }

}
