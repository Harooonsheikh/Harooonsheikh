using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <summary>
    /// Products controller - Realtime services related to products
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class DiscountController : ApiBaseController
    {
        #region Constructor

        /// <inheritdoc />
        public DiscountController()
        {
            ControllerName = "DiscountController";
        }

        #endregion

        #region API Methods

        /// <summary>
        /// GetProductImageURL Gets image URLs of the product.
        /// </summary>
        /// <param name="request">Product IDs</param>
        /// <returns>bool</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Discount/GetDiscountWithAffiliation")]
        public GetDiscountWithAffiliationResponse GetDiscountWithAffiliation([FromBody] GetDiscountWithAffiliationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new GetDiscountWithAffiliationResponse(false, GetModelErrors(), null);
            }

            try
            {
                var controller = erpAdapterFactory.CreateDiscountWithAffiliationController(currentStore.StoreKey);
                var result = controller.GetDiscountsWithAffiliationByProductIds(request.ItemId, request.Variant, request.AffiliationName, request.Currency);
                var discountResult = _mapper.Map<List<ProductDiscountWithAffiliation>>(result);

                return new GetDiscountWithAffiliationResponse(true, "Success", discountResult);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new GetDiscountWithAffiliationResponse(false, message, null);
            }
        }

        #endregion

        #region Request, Response classes

        /// <summary>
        /// CustomerRequest class is used as output parmeter for Customer calls.
        /// </summary>
        public class GetDiscountWithAffiliationRequest
        {
            /// <summary>
            /// ItemId
            /// </summary>
            [Required]
            public string ItemId { get; set; }

            /// <summary>
            /// Variant
            /// </summary>
            [Required]
            public string Variant { get; set; }

            /// <summary>
            /// AffiliationId
            /// </summary>
            [Required]
            public string AffiliationName { get; set; }

            /// <summary>
            /// Currency
            /// </summary>
            [Required]
            public string Currency { get; set; }
        }

        /// <summary>
        /// Get discount with affiliation response class.
        /// </summary>
        public class GetDiscountWithAffiliationResponse
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="status"></param>
            /// <param name="message"></param>
            /// <param name="result"></param>
            public GetDiscountWithAffiliationResponse(bool status, string message, object result)
            {
                this.Status = status;
                this.Message = message;
                this.Result = result;
            }

            /// <summary>
            /// Status of the Get discount with affiliation request
            /// </summary>
            public bool Status { get; set; }
            
            /// <summary>
            /// Message of the Get discount with affiliation request
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Result of the Get discount with affiliation request
            /// </summary>
            public object Result { get; set; }
        }

        /// <summary>
        /// ProductDiscountWithAffiliation
        /// </summary>
        public class ProductDiscountWithAffiliation
        {
            /// <summary>
            /// AffiliationId
            /// </summary>
            public string AffiliationId { get; set; }

            /// <summary>
            /// AffiliationName
            /// </summary>
            public string AffiliationName { get; set; }
            /// <summary>
            /// OfferId
            /// </summary>
            public string OfferId { get; set; }
            /// <summary>
            /// OfferName
            /// </summary>
            public string OfferName { get; set; }
            /// <summary>
            /// ValidFrom
            /// </summary>
            public string ValidFrom { get; set; }
            /// <summary>
            /// ValidTo
            /// </summary>
            public string ValidTo { get; set; }
            /// <summary>
            /// DiscAmount
            /// </summary>
            public string DiscAmount { get; set; }
            /// <summary>
            /// DiscPct
            /// </summary>
            public string DiscPct { get; set; }

            /// <summary>
            /// OfferPrice
            /// </summary>
            public string OfferPrice { get; set; }
        }

        #endregion

    }
}
