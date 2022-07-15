using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web
{

    /// <summary>
    /// LoyaltyCardController defines properties and methods for API controller for Loyalty Card.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class LoyaltyCardController : ApiBaseController
    {
        /// <summary>
        /// Loyalty Card Controller in constructor
        /// </summary>
        public LoyaltyCardController()
        {
            ControllerName = "LoyaltyCardController";
        }

        #region API Methods
        //https://localhost:44300/api/LoyaltyCard/GetLoyaltyCardStatus?loyaltyCardNo=55004
        /// <summary>
        /// Get all the Details, Loyalty Groups, Rewards Points,Tiers
        /// </summary>
        /// <param name="loyaltyCardStatusRequest"></param>
        /// <returns>data</returns>
        [HttpPost]
        [SanitizeInput]
        [Route("Loyalty/GetLoyaltyCardStatus")]
        public IHttpActionResult GetLoyaltyCardStatus([FromBody] GetLoyaltyCardStatusRequest loyaltyCardStatusRequest)
        {
            try
            {
                ErpLoyaltyCard loyaltycard = loyaltyCardController.GetLoyaltyCardStatus(loyaltyCardStatusRequest.loyaltyCardNo);
                var data = new { loyaltycard.LoyaltyGroups, loyaltycard.RewardPoints, loyaltycard.CustomerAccount, loyaltycard.CardTenderType, loyaltycard.CardNumber };
                return Ok(data);

            }
            catch (ErpAdapterException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                var data = new { succeeded = false, errorMessage = message };
                return Ok(data);
            }
        }

        /// <summary>
        /// Issue Loyalty Card.
        /// </summary>
        /// <param name="issueLoyaltyCardRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [SanitizeInput]
        [Route("Loyalty/IssueLoyaltyCard")]
        public IHttpActionResult IssueLoyaltyCard([FromBody] IssueLoyaltyCardRequest issueLoyaltyCardRequest)
        {
            try
            {
                //CommerceRuntime runtime = CommerceRuntimeHelper.CommerceRuntime;
                //LoyaltyManager manager = LoyaltyManager.Create(runtime);
                //LoyaltyCard loyaltycard = manager.IssueLoyaltyCard(loyaltyCardNo, customerAcct);
                ErpLoyaltyCard loyaltycard = loyaltyCardController.IssueLoyaltyCard(issueLoyaltyCardRequest.loyaltyCardNo, issueLoyaltyCardRequest.customerAcct);
                var data = new { loyaltycard };
                // var data = new { succeeded = true, message = string.Format("Hello2 {0}!") };
                return Ok(data);
            }
            catch (ErpAdapterException ex)
            {
                var data = new { succeeded = false, errorMessage = ex.Message };
                return Ok(data);
            }
        }

        /// <summary>
        /// Get Customer Loyalty Cards.
        /// </summary>
        /// <param name="customerLoyaltyCardsRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [SanitizeInput]
        [Route("Loyalty/GetCustomerLoyaltyCards")]
        public IHttpActionResult GetCustomerLoyaltyCards([FromBody] GetCustomerLoyaltyCardsRequest customerLoyaltyCardsRequest)
        {
            try
            {
                List<ErpLoyaltyCard> LClist = loyaltyCardController.GetCustomerLoyaltyCards(customerLoyaltyCardsRequest.customerAcct);
                var data = new { LClist };
                return Ok(data);
            }
            catch (ErpAdapterException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                var data = new { succeeded = false, errorMessage = message };
                return Ok(data);
            }
            //catch (HeadquarterTransactionServiceException ex)
            //{
            //    var data = new { succeeded = false, errorMessage = ex.Message };
            //    return Ok(data);
            //}
        }

        /// <summary>
        /// Get All reward points, Redeemable, Non Redeemable, Total Transactions
        /// </summary>
        /// <param name="loyaltyCardStatusRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [SanitizeInput]
        [Route("Loyalty/GetLoyaltyCardRewardPointsStatus")]
        public IHttpActionResult GetLoyaltyCardRewardPointsStatus([FromBody] GetLoyaltyCardStatusRequest loyaltyCardStatusRequest)
        {
            try
            {
                //TransactionServiceClient tsClient = new TransactionServiceClient(CommerceRuntimeHelper.RequestContext);

                //Microsoft.Dynamics.Commerce.Runtime.DataModel.LoyaltyCard lc;
                ErpLoyaltyCard lc;

                //var lcData = tsClient.GetLoyaltyCardRewardPointsStatus(DateTime.UtcNow, loyaltyCardNo, false, false, true, true, false, "en-us");
                var lcData = loyaltyCardController.GetLoyaltyCardRewardPointsStatus(DateTime.UtcNow, loyaltyCardStatusRequest.loyaltyCardNo, false, false, true, true, false, "en-us");
                if (lcData != null && lcData.Count > 0)
                {
                    lc = lcData[0];
                    var data = new { succeeded = true, loyaltyCardNo = lc.CardNumber, RewardPoints = lc.RewardPoints };

                    return Ok(data);
                }
                else
                {
                    var data = new { succeeded = true, loyaltyCardNo = "no record" };
                    return Ok(data);
                }
            }
            catch (ErpAdapterException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                var data = new { succeeded = false, errorMessage = message };
                return Ok(data);
            }

        }

        //To be Done
        /// <summary>
        /// Save Loyalty Reward Points
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SanitizeInput]
        [Route("Loyalty/PostLoyaltyRewardPoints")]
        public IHttpActionResult PostLoyaltyRewardPoints()
        {
            try
            {
                //SalesTransaction st = new SalesTransaction();
                ErpSalesTransaction st = new ErpSalesTransaction();
                //TenderLine tl = new TenderLine();
                ErpTenderLine tl = new ErpTenderLine();

                //TransactionServiceClient tsClient = new TransactionServiceClient(CommerceRuntimeHelper.RequestContext);
                //loyaltyCardController.PostLoyaltyCardRewardPointTrans(st, LoyaltyRewardPointEntryType.Redeem, CommerceRuntimeHelper.ChannelConfiguration);
                loyaltyCardController.PostLoyaltyCardRewardPointTrans(st, ErpLoyaltyRewardPointEntryType.Redeem);

                var data = new { succeeded = true, message = "Loyalty Rewards Points has been Posted" };
                return Ok(data);

            }
            catch (ErpAdapterException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                var data = new { succeeded = false, errorMessage = message };
                return Ok(data);
            }
            //catch (HeadquarterTransactionServiceException ex)
            //{
            //    var data = new { succeeded = false, errorMessage = ex.Message };
            //    return Ok(data);
            //}
        }

        /// <summary>
        /// Get transactions on a loyalty card.
        /// </summary>
        /// <param name="loyaltyCardNo"></param>
        /// <param name="top"></param>
        /// <param name="skip"></param>
        /// <returns>IHTTPActionResult</returns>
        // https://localhost:44300/api/LoyaltyCard/GetLoyaltyCardTransactions?loyaltyCardNo=55004
        [HttpPost]
        [SanitizeInput]
        [Route("Loyalty/GetLoyaltyCardTransactions")]
        public IHttpActionResult GetLoyaltyCardTransactions([FromBody] GetLoyaltyCardTransactionRequest loyaltyCardTransactionRequest)
        {
            try
            {

                string LoyalyReward = "Loyalty rewards";
                List<ErpLoyaltyCardTransaction> loyaltycardlist = loyaltyCardController.GetLoyaltyCardTransactions(loyaltyCardTransactionRequest.loyaltyCardNo, LoyalyReward, loyaltyCardTransactionRequest.top, loyaltyCardTransactionRequest.skip, false);
                var data = new { loyaltycardlist };
                return Ok(data);
            }
            catch (ErpAdapterException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));

                var data = new { succeeded = false, errorMessage = message };
                return Ok(data);
            }
        }
        #endregion

        #region LoyaltyCard Request, Response classes
        /// <summary>
        /// GetLoyaltyCardStatusRequest Request Class 
        /// </summary>
        public class GetLoyaltyCardStatusRequest
        {
            [Required]
            public string loyaltyCardNo { get; set; }

        }

        /// <summary>
        /// IssueLoyaltyCardRequest Request Class 
        /// </summary>
        public class IssueLoyaltyCardRequest
        { 
            [Required]
            public string loyaltyCardNo { get; set; }
            [Required]
            public string customerAcct { get; set; }
        }
        /// <summary>
        /// GetCustomerLoyaltyCardsRequest Request Class 
        /// </summary>
        public class GetCustomerLoyaltyCardsRequest
        {
            [Required]
            public string customerAcct { get; set; }
        }

        public class GetLoyaltyCardTransactionRequest
        {
            [Required]
            public string loyaltyCardNo { get; set; }
            public long top { get; set; }
            public long skip { get; set; }

        }
        #endregion
    }
}

