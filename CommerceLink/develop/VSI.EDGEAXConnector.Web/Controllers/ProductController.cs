using System;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <summary>
    /// Products controller - Realtime services related to products
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class ProductController : ApiBaseController
    {
        #region Properties

        #endregion

        #region Constructor

        /// <inheritdoc />
        public ProductController()
        {
            ControllerName = "ProductController";
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
        [Route("Product/ProductImageUrl")]
        [Authorize(Roles = "eCommerce")]
        public ProductImageUrlResponse ProductImageUrl([FromBody] ProductImageUrlRequest request)
        {
            ProductImageUrlResponse productImageUrlResponse;
            
            try
            {
                var productController = erpAdapterFactory.CreateProductController(currentStore.StoreKey);
                productImageUrlResponse = productController.GetProductImageUrl(request);
                
                return productImageUrlResponse;
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                productImageUrlResponse = new ProductImageUrlResponse(false, message);
                return productImageUrlResponse;
            }
        }

        #endregion

    }
}
