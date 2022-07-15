using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using VSI.CommerceLink.EcomDataModel;
using VSI.CommerceLink.EcomDataModel.Enum;
using VSI.CommerceLink.EcomDataModel.Request;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <summary>
    /// CartController defines properties and methods for API controller for Cart.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class CartController : ApiBaseController
    {
        /// <summary>
        /// Cart Controller 
        /// </summary>
        public CartController()
        {
            ControllerName = "CartController";
        }

        #region API Methods

        /// <summary>
        /// ApplyCouponsWithNewCart creates a cart,adds cartlines and applys coupon on cart with provided details.
        /// </summary>
        /// <param name="createCartWithLinesAndCouponRequest"></param>
        /// <returns></returns>

        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/ApplyCouponsWithNewCart")]
        public CartResponse ApplyCouponsWithNewCart([FromBody] CreateCartWithLinesAndCouponRequest createCartWithLinesAndCouponRequest)
        {
            CartResponse cartResponse = new CartResponse(false, "", null);
            ErpCart erpCart = new ErpCart();
            ErpCalculationModes erpCalcualtionModes;
            List<ErpCartLine> erpCartLines;
            bool isLegacyDiscountCode = false;
            List<string> coupons = new List<string>();


            try
            {

                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

                //erpCalcualtionModes = _mapper.Map<EcomCalculationModes, ErpCalculationModes>(applyCartCouponRequest.CalculationModes);
                erpCartLines = _mapper.Map<List<EcomCartLine>, List<ErpCartLine>>(createCartWithLinesAndCouponRequest.CartLines);

                erpCart.Id = createCartWithLinesAndCouponRequest.CartId;
                erpCart.CartTypeValue = 1;

                cartResponse = this.ValidateCreateCartWithLinesAndCouponsRequest(createCartWithLinesAndCouponRequest);
                erpCalcualtionModes = _mapper.Map<EcomCalculationModes, ErpCalculationModes>(createCartWithLinesAndCouponRequest.CalculationModes);

                if (cartResponse != null)
                {
                    return cartResponse;
                }


                else
                {
                    if (integrationManager.GetKey(Entities.Cart, createCartWithLinesAndCouponRequest.CartId, "") == null)
                    {
                        var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);

                        ErpCartResponse erpCartResponse = erpCartController.CreateOrUpdateCart(erpCart, erpCalcualtionModes, false);
                        if (erpCartResponse.Success)
                        {
                            erpCalcualtionModes = _mapper.Map<EcomCalculationModes, ErpCalculationModes>(createCartWithLinesAndCouponRequest.CalculationModes);
                            erpCartResponse = erpCartController.AddCartLines(createCartWithLinesAndCouponRequest.CartId, erpCartLines, erpCalcualtionModes, (long)erpCartResponse.Cart.Version);

                            if (erpCartResponse.Success)
                            {
                                //erpCartResponse = erpCartController.UpdateDeliverySpecification(ecomCartMergeRequest.CartId, erpDeliverySpecification);

                                isLegacyDiscountCode = createCartWithLinesAndCouponRequest.isLegacyDiscountCode;
                                coupons = createCartWithLinesAndCouponRequest.CouponCodes;

                                ErpCartResponse erpCartCouponsResponse = erpCartController.AddCouponsToCart(createCartWithLinesAndCouponRequest.CartId, coupons, isLegacyDiscountCode, GetRequestGUID(Request));
                                if (erpCartCouponsResponse.Success)
                                {
                                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCartResponse.Cart));
                                    cartResponse = new CartResponse(erpCartCouponsResponse.Success, erpCartCouponsResponse.Message, erpCartCouponsResponse.Cart);
                                }
                                else
                                {
                                    cartResponse = new CartResponse(erpCartCouponsResponse.Success, erpCartCouponsResponse.Message, null);
                                }
                            }
                            else
                            {
                                cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                            }
                        }
                        else
                        {
                            cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                        }
                    }
                    else
                    {
                        string message = CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL40513, currentStore, createCartWithLinesAndCouponRequest.CartId);
                        cartResponse = new CartResponse(false, message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            return cartResponse;
        }

        /// <summary>
        /// GetCart gets cart with provided details.
        /// </summary>
        /// <param name="cartId">cart request to be fetch</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpGet]
        [Route("Cart/GetCart")]
        [Obsolete("GetCart is deprecated, please use GetCart with POST parameter instead.")]

        public CartResponse GetCart([FromUri] string cartId)
        {

            CartResponse cartResponse;

            // Throw error if customerAccount is null
            if (string.IsNullOrWhiteSpace(cartId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            // Extract the data from parameter
            string cart = cartId;

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " cartId: " + cart);

            cartResponse = new CartResponse(false, "", null);

            try
            {
                var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);

                ErpCartResponse erpCartResponse = erpCartController.GetCart(cart);

                if (erpCartResponse.Success)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCartResponse.Cart));
                    cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                }
                else
                {
                    cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
            }

            return cartResponse;
        }


        /// <summary>
        /// GetCart gets cart with provided details.
        /// </summary>
        /// <param name="cartRequest">cart request to be fetch</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [SanitizeInput]
        [HttpPost]
        [Route("Cart/GetCart")]
        public CartResponse GetCart([FromBody] GetCartRequest cartRequest)
        {
            CartResponse cartResponse;

            // Throw error if customerAccount is null
            if (string.IsNullOrWhiteSpace(cartRequest.CartId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            // Extract the data from parameter
            string cart = cartRequest.CartId;

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " cartId: " + cart);

            cartResponse = new CartResponse(false, "", null);

            try
            {
                var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);
                ErpCartResponse erpCartResponse = erpCartController.GetCart(cart);

                if (erpCartResponse.Success)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCartResponse.Cart));
                    cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                }
                else
                {
                    cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
            }

            return cartResponse;
        }

        /// <summary>
        /// CreateOrUpdateCart creates and updates cart with provided details.
        /// </summary>
        /// <param name="cartRequest">cart request to be create/update</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/CreateOrUpdateCart")]
        public CartResponse CreateOrUpdateCart([FromBody] EcomCartRequest cartRequest)
        {

            CartRequest erpCartRequest;
            CartResponse cartResponse = new CartResponse(false, "", null);
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

            try
            {
                erpCartRequest = _mapper.Map<EcomCartRequest, CartRequest>(cartRequest);

                if (erpCartRequest != null)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " cart: " + JsonConvert.SerializeObject(erpCartRequest));
                }

                cartResponse = this.ValidateCartRequest(erpCartRequest);

                if (cartResponse != null)
                {
                    return cartResponse;
                }
                else
                {
                    if (integrationManager.GetKey(Entities.Cart, erpCartRequest.Cart.Id, "") == null || erpCartRequest.IsUpdate)
                    {
                        var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);
                        ErpCartResponse erpCartResponse = erpCartController.CreateOrUpdateCart(erpCartRequest.Cart, erpCartRequest.CalculationModes, erpCartRequest.IsUpdate);
                        if (erpCartResponse.Success)
                        {
                            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCartResponse.Cart));
                            cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                        }
                        else
                        {
                            cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                        }
                    }
                    else
                    {
                        string message = CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL40513, currentStore, erpCartRequest.Cart.Id);
                        cartResponse = new CartResponse(false, message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            return cartResponse;
        }

        /// <summary>
        /// CreateMergedCart creates cart with provided details.
        /// </summary>
        /// <param name="createMergedCartRequest">cart request to be create/update</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/CreateMergedCart")]
        public CartResponse CreateMergedCart([FromBody] CreateMergedCartRequest createMergedCartRequest)
        {
            CartResponse cartResponse = new CartResponse(false, "", null);
            ErpCart erpCart = new ErpCart();
            ErpCalculationModes erpCalcualtionModes;
            List<ErpCartLine> erpCartLines;
            ErpDeliverySpecification erpDeliverySpecification;

            try
            {

                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

                cartResponse = this.ValidateCreateMergedCartRequest(createMergedCartRequest);


                erpCalcualtionModes = _mapper.Map<EcomCalculationModes, ErpCalculationModes>(createMergedCartRequest.CalculationModes);
                erpCartLines = _mapper.Map<List<EcomCartLine>, List<ErpCartLine>>(createMergedCartRequest.CartLines);
                erpDeliverySpecification = _mapper.Map<EcomDeliverySpecification, ErpDeliverySpecification>(createMergedCartRequest.DeliverySpecification);

                erpCart.Id = createMergedCartRequest.CartId;

                if (cartResponse != null)
                {
                    return cartResponse;
                }
                else
                {
                    if (integrationManager.GetKey(Entities.Cart, createMergedCartRequest.CartId, "") == null)
                    {
                        var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);

                        ErpCartResponse erpCartResponse = erpCartController.CreateMergedCart(createMergedCartRequest.CartId, createMergedCartRequest.AffiliationId, erpCalcualtionModes, erpCartLines, erpDeliverySpecification, createMergedCartRequest.CouponCodes, createMergedCartRequest.isLegacyDiscountCode, GetRequestGUID(Request));
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                    }
                    else
                    {
                        string message = String.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40513), createMergedCartRequest.CartId);
                        cartResponse = new CartResponse(false, message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            return cartResponse;
        }
        /// <summary>
        /// MergeAddUpdateCartLines creates and updates lines in cart with provided details.
        /// </summary>
        /// <param name="addCartLinesRequest">cart line request to be create/update lines in cart</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/MergeAddUpdateCartLines")]
        public CartResponse MergeAddUpdateCartLines([FromBody] EcomAddUpdateCartLinesRequest addUpdateCartLinesRequest)
        {
            return MergeAddUpdateRemoveCartlines(addUpdateCartLinesRequest, MethodBase.GetCurrentMethod().Name);
        }
        /// <summary>
        /// AddCartLines creates and updates lines in cart with provided details.
        /// </summary>
        /// <param name="addCartLinesRequest">cart line request to be create/update lines in cart</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/AddCartLines")]
        public CartResponse AddCartLines([FromBody] EcomCartLinesRequest addCartLinesRequest)
        {
            return AddCartLinesMethod(addCartLinesRequest, MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// VoidCartLines void lines in cart with provided details.
        /// </summary>
        /// <param name="voidCartLinesRequest">cart line request to be void lines in cart</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/VoidCartLines")]
        public CartResponse VoidCartLines([FromBody] CartLinesRequest voidCartLinesRequest)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (voidCartLinesRequest != null)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " CartLines: " + JsonConvert.SerializeObject(voidCartLinesRequest));
            }

            CartResponse cartResponse = new CartResponse(false, "", null);

            try
            {
                cartResponse = this.ValidateCartLine(voidCartLinesRequest, CartLineRequestType.Void);

                if (cartResponse != null)
                {
                    return cartResponse;
                }
                else
                {
                    var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);

                    ErpCartResponse erpCartResponse = erpCartController.VoidCartLines(voidCartLinesRequest.CartId, voidCartLinesRequest.CartLines, voidCartLinesRequest.CalculationModes);

                    if (erpCartResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCartResponse.Cart));
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                    }
                    else
                    {
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            return cartResponse;
        }

        /// <summary>
        /// RemoveCartLines removes lines in cart with provided details.
        /// </summary>
        /// <param name="removeCartLinesRequest">cart line request to be remove lines in cart</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/RemoveCartLines")]
        public CartResponse RemoveCartLines([FromBody] RemoveCartLinesRequest removeCartLinesRequest)
        {
            return RemoveCartLinesMethod(removeCartLinesRequest, MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// UpdateDeliverySpecification add delivery info in cart with provided details.
        /// </summary>
        /// <param name="deliverySpecificationRequest">deliverySpecificationRequest request to add delivery info in cart</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/UpdateDeliverySpecification")]
        public CartResponse UpdateDeliverySpecification([FromBody] DeliverySpecificationRequest deliverySpecificationRequest)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (deliverySpecificationRequest != null)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " Delivery Specification: " + JsonConvert.SerializeObject(deliverySpecificationRequest));
            }

            CartResponse cartResponse = new CartResponse(false, "", null);

            try
            {
                cartResponse = this.ValidateDeliverySpecificationRequest(deliverySpecificationRequest);

                if (cartResponse != null)
                {
                    return cartResponse;
                }
                else
                {
                    var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);

                    ErpCartResponse erpCartResponse = erpCartController.UpdateDeliverySpecification(deliverySpecificationRequest.CartId, deliverySpecificationRequest.DeliverySpecification);

                    if (erpCartResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCartResponse.Cart));
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                    }
                    else
                    {
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            return cartResponse;
        }

        /// <summary>
        /// UpdateCartLines update lines in cart with provided details.
        /// </summary>
        /// <param name="updateCartLinesRequest">cart line request to be void lines in cart</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/UpdateCartLines")]
        public CartResponse UpdateCartLines([FromBody] CartLinesRequest updateCartLinesRequest)
        {
            return UpdateCartMethod(updateCartLinesRequest, MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// AddCouponsToCart adds coupons in cart with provided details.
        /// </summary>
        /// <param name="addCouponsToCartRequest">request to be add coupons to cart</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/AddCouponsToCart")]
        public CartResponse AddCouponsToCart([FromBody] AddCouponsToCartRequest addCouponsToCartRequest)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (addCouponsToCartRequest != null)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " Coupons: " + JsonConvert.SerializeObject(addCouponsToCartRequest));
            }

            CartResponse cartResponse = new CartResponse(false, "", null);

            try
            {
                cartResponse = this.ValidateAddCouponsToCartRequest(addCouponsToCartRequest);

                if (cartResponse != null)
                {
                    return cartResponse;
                }
                else
                {
                    var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);

                    ErpCartResponse erpCartResponse = erpCartController.AddCouponsToCart(addCouponsToCartRequest.CartId, addCouponsToCartRequest.CouponCodes, addCouponsToCartRequest.isLegacyDiscountCode, GetRequestGUID(Request));

                    if (erpCartResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCartResponse.Cart));
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                    }
                    else
                    {
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            return cartResponse;
        }

        /// <summary>
        /// RemoveCouponsFromCart removes coupons in cart with provided details.
        /// </summary>
        /// <param name="removeCouponsFromCartRequest">request to be remove coupons from cart</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/RemoveCouponsFromCart")]
        public CartResponse RemoveCouponsFromCart([FromBody] RemoveCouponsFromCartRequest removeCouponsFromCartRequest)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (removeCouponsFromCartRequest != null)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " Coupons: " + JsonConvert.SerializeObject(removeCouponsFromCartRequest));
            }

            CartResponse cartResponse = new CartResponse(false, "", null);

            try
            {
                cartResponse = this.ValidateRemoveCouponsFromCartRequest(removeCouponsFromCartRequest);

                if (cartResponse != null)
                {
                    return cartResponse;
                }
                else
                {
                    var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);

                    ErpCartResponse erpCartResponse = erpCartController.RemoveCouponsFromCart(removeCouponsFromCartRequest.CartId, removeCouponsFromCartRequest.CouponCodes);

                    if (erpCartResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCartResponse.Cart));
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                    }
                    else
                    {
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            return cartResponse;
        }

        /// <summary>
        /// AddTenderLine adds payment in cart with provided details.
        /// </summary>
        /// <param name="addTenderLineRequest">request to be remove coupons from cart</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/AddTenderLine")]
        public CartResponse AddTenderLine([FromBody] AddTenderLineRequest addTenderLineRequest)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (addTenderLineRequest != null)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " TenderLine: " + JsonConvert.SerializeObject(addTenderLineRequest));
            }

            CartResponse cartResponse = new CartResponse(false, "", null);

            try
            {
                cartResponse = this.ValidateAddTenderLineRequest(addTenderLineRequest);

                if (cartResponse != null)
                {
                    return cartResponse;
                }
                else
                {
                    var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);

                    ErpCartResponse erpCartResponse = erpCartController.AddTenderLine(addTenderLineRequest.CartId, addTenderLineRequest.CartTenderLine, addTenderLineRequest.cartVersion);

                    if (erpCartResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCartResponse.Cart));
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                    }
                    else
                    {
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            return cartResponse;
        }

        /// <summary>
        /// AddPreprocessedTenderLine adds preprocessed payment in cart with provided details.
        /// </summary>
        /// <param name="addPreprocessedTenderLineRequest">request to be remove coupons from cart</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/AddPreprocessedTenderLine")]
        public CartResponse AddPreprocessedTenderLine([FromBody] AddPreprocessedTenderLineRequest addPreprocessedTenderLineRequest)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (addPreprocessedTenderLineRequest != null)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " PreprocessedTenderLine: " + JsonConvert.SerializeObject(addPreprocessedTenderLineRequest));
            }

            CartResponse cartResponse = new CartResponse(false, "", null);

            try
            {
                cartResponse = this.ValidateAddPreprocessedTenderLineRequest(addPreprocessedTenderLineRequest);

                if (cartResponse != null)
                {
                    return cartResponse;
                }
                else
                {
                    var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);

                    ErpCartResponse erpCartResponse = erpCartController.AddPreprocessedTenderLine(addPreprocessedTenderLineRequest.CartId, addPreprocessedTenderLineRequest.TenderLine, addPreprocessedTenderLineRequest.cartVersion);

                    if (erpCartResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCartResponse.Cart));
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                    }
                    else
                    {
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            return cartResponse;
        }
        /// <summary>
        /// MergeRemoveUpdateCartLines remove and update lines in cart with provided details.
        /// </summary>
        /// <param name="addCartLinesRequest">cart line request to be create/update lines in cart</param>
        /// <returns>CartResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Cart/MergeRemoveUpdateCartLines")]
        public CartResponse MergeRemoveUpdateCartLines([FromBody] EcomAddUpdateCartLinesRequest removeUpdateCartLinesRequest)
        {
            return MergeAddUpdateRemoveCartlines(removeUpdateCartLinesRequest, MethodBase.GetCurrentMethod().Name);
        }
        #endregion

        #region Private

        private CartResponse UpdateCartMethod(CartLinesRequest updateCartLinesRequest, string methodName)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            // Throw error if customerAccount is null
            if (updateCartLinesRequest != null)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, methodName, " CartLines: " + JsonConvert.SerializeObject(updateCartLinesRequest));
            }

            CartResponse cartResponse = new CartResponse(false, "", null);

            try
            {
                cartResponse = this.ValidateCartLine(updateCartLinesRequest, CartLineRequestType.Update);

                if (cartResponse != null)
                {
                    return cartResponse;
                }
                else
                {
                    var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);

                    ErpCartResponse erpCartResponse = erpCartController.UpdateCartLines(updateCartLinesRequest.CartId, updateCartLinesRequest.CartLines, updateCartLinesRequest.CalculationModes, GetRequestGUID(Request));

                    if (erpCartResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, methodName, JsonConvert.SerializeObject(erpCartResponse.Cart));
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                    }
                    else
                    {
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            return cartResponse;
        }

        private CartResponse RemoveCartLinesMethod(RemoveCartLinesRequest removeCartLinesRequest, string methodName)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (removeCartLinesRequest != null)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, methodName, " Line Ids: " + JsonConvert.SerializeObject(removeCartLinesRequest));
            }

            CartResponse cartResponse = new CartResponse(false, "", null);

            try
            {
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "ValidateRemoveCartLines", DateTime.UtcNow);

                cartResponse = this.ValidateRemoveCartLinesRequest(removeCartLinesRequest);

                if (cartResponse != null)
                {
                    return cartResponse;
                }
                else
                {
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "Create ERP AdapterController", DateTime.UtcNow);

                    var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), "Create ERP AdapterController Ends", DateTime.UtcNow);

                    ErpCartResponse erpCartResponse = erpCartController.RemoveCartLines(removeCartLinesRequest.CartId, removeCartLinesRequest.LineIds, removeCartLinesRequest.CalculationModes, GetRequestGUID(Request));

                    if (erpCartResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, methodName, JsonConvert.SerializeObject(erpCartResponse.Cart));
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                    }
                    else
                    {
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            return cartResponse;
        }

        private CartResponse MergeAddUpdateRemoveCartlines(EcomAddUpdateCartLinesRequest addUpdateCartLinesRequest, string methodName)
        {

            CartResponse cartResponse = new CartResponse(false, "", null);
            List<CartResponse> listCartResponse = new List<CartResponse>();
            CartLinesRequest erpAddCartLinesRequest;
            List<EcomCartLine> ecomCartLine = new List<EcomCartLine>();
            List<ErpCartLine> UpdateCartLine = new List<ErpCartLine>();
            if (addUpdateCartLinesRequest == null)
            {
                listCartResponse.Add(cartResponse);
                return listCartResponse.ElementAt(0);
            }

            try
            {
                erpAddCartLinesRequest = _mapper.Map<EcomAddUpdateCartLinesRequest, CartLinesRequest>(addUpdateCartLinesRequest);
                ecomCartLine = _mapper.Map<List<EcomAddUpdateCartLine>, List<EcomCartLine>>(addUpdateCartLinesRequest.CartLines);

                if (addUpdateCartLinesRequest.RemoveLineIds != null)
                {
                    RemoveCartLinesRequest removeCartLinesRequest = new RemoveCartLinesRequest();
                    removeCartLinesRequest.CalculationModes = erpAddCartLinesRequest.CalculationModes;
                    removeCartLinesRequest.CartId = erpAddCartLinesRequest.CartId;
                    removeCartLinesRequest.LineIds = addUpdateCartLinesRequest.RemoveLineIds;
                    cartResponse = RemoveCartLinesMethod(removeCartLinesRequest, methodName);

                    if (cartResponse.Status)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, methodName, JsonConvert.SerializeObject(cartResponse.Cart));
                        cartResponse = new CartResponse(cartResponse.Status, cartResponse.ErrorMessage, cartResponse.Cart);
                        listCartResponse.Add(cartResponse);
                    }
                    else
                    {
                        cartResponse = new CartResponse(cartResponse.Status, cartResponse.ErrorMessage, null);
                        listCartResponse.Add(cartResponse);
                        return listCartResponse.ElementAt(0);
                    }
                }
                //Loop For Add and Update CartLines
                List<EcomCartLine> addCartLine = new List<EcomCartLine>();
                for (var i = 0; i < addUpdateCartLinesRequest.CartLines.Count; i++)
                {
                    if (addUpdateCartLinesRequest.CartLines[i].IsUpdate == false)
                    {
                        addCartLine.Add(ecomCartLine[i]);
                    }
                    else if (addUpdateCartLinesRequest.CartLines[i].IsUpdate == true)
                    {
                        UpdateCartLine.Add(erpAddCartLinesRequest.CartLines[i]);
                    }
                }
                if (addCartLine != null && addCartLine.Count != 0)
                {
                    EcomCartLinesRequest ecomCartLinesRequest = new EcomCartLinesRequest();
                    ecomCartLinesRequest.CalculationModes = addUpdateCartLinesRequest.CalculationModes;
                    ecomCartLinesRequest.CartId = addUpdateCartLinesRequest.CartId;
                    ecomCartLinesRequest.cartVersion = addUpdateCartLinesRequest.cartVersion;
                    ecomCartLinesRequest.CartLines = addCartLine;
                    cartResponse = AddCartLinesMethod(ecomCartLinesRequest, methodName);

                    if (cartResponse.Status)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, methodName, JsonConvert.SerializeObject(cartResponse.Cart));
                        cartResponse = new CartResponse(cartResponse.Status, cartResponse.ErrorMessage, cartResponse.Cart);
                        listCartResponse.Add(cartResponse);
                    }
                    else
                    {
                        cartResponse = new CartResponse(cartResponse.Status, cartResponse.ErrorMessage, null);
                        listCartResponse.Add(cartResponse);
                        return listCartResponse.ElementAt(0);
                    }
                }
                if (UpdateCartLine != null && UpdateCartLine.Count != 0)
                {
                    CartLinesRequest cartLinesRequest = new CartLinesRequest();
                    cartLinesRequest.CalculationModes = erpAddCartLinesRequest.CalculationModes;
                    cartLinesRequest.CartId = erpAddCartLinesRequest.CartId;
                    cartLinesRequest.cartVersion = erpAddCartLinesRequest.cartVersion;
                    cartLinesRequest.CartLines = UpdateCartLine;
                    cartResponse = UpdateCartMethod(cartLinesRequest, methodName);

                    if (cartResponse.Status)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, methodName, JsonConvert.SerializeObject(cartResponse.Cart));
                        cartResponse = new CartResponse(cartResponse.Status, cartResponse.ErrorMessage, cartResponse.Cart);
                        listCartResponse.Add(cartResponse);
                    }
                    else
                    {
                        cartResponse = new CartResponse(cartResponse.Status, cartResponse.ErrorMessage, null);
                        listCartResponse.Add(cartResponse);
                    }
                }


            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                listCartResponse.Add(cartResponse);
                return listCartResponse.ElementAt(1);
            }

            return listCartResponse.ElementAt(listCartResponse.Count - 1);
        }

        private CartResponse AddCartLinesMethod(EcomCartLinesRequest addCartLinesRequest, string methodName)
        {

            CartResponse cartResponse = new CartResponse(false, "", null);
            CartLinesRequest erpAddCartLinesRequest;

            try
            {
                erpAddCartLinesRequest = _mapper.Map<EcomCartLinesRequest, CartLinesRequest>(addCartLinesRequest);

                if (erpAddCartLinesRequest != null)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " CartLines: " + JsonConvert.SerializeObject(erpAddCartLinesRequest));
                }

                cartResponse = this.ValidateCartLine(erpAddCartLinesRequest, CartLineRequestType.Add);

                if (cartResponse != null)
                {
                    return cartResponse;
                }
                else
                {
                    var erpCartController = erpAdapterFactory.CreateCartController(currentStore.StoreKey);

                    ErpCartResponse erpCartResponse = erpCartController.AddCartLines(erpAddCartLinesRequest.CartId, erpAddCartLinesRequest.CartLines, erpAddCartLinesRequest.CalculationModes, erpAddCartLinesRequest.cartVersion);

                    if (erpCartResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCartResponse.Cart));
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, erpCartResponse.Cart);
                    }
                    else
                    {
                        cartResponse = new CartResponse(erpCartResponse.Success, erpCartResponse.Message, null);
                    }
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cartResponse = new CartResponse(false, message, null);
                return cartResponse;
            }

            return cartResponse;
        }
        /// <summary>
        /// ValidateCartRequest validates Create/Update cart Object.
        /// </summary>
        /// <param name="cartRequest"></param>
        /// <returns></returns>
        private CartResponse ValidateCartRequest(CartRequest cartRequest)
        {
            if (cartRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CartResponse(false, message, null);
            }
            else
            {
                if (cartRequest.CalculationModes == ErpCalculationModes.None)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "cartRequest.CalculationModes");
                    return new CartResponse(false, message, null);
                }

                if (cartRequest.Cart == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "cartRequest.Cart");
                    return new CartResponse(false, message, null);
                }
                else if (string.IsNullOrWhiteSpace(cartRequest.Cart.Id))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "cartRequest.Cart.Id");
                    return new CartResponse(false, message, null);
                }
                //else if (cartRequest.Cart.CartType == ErpCartType.None)
                //{
                //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, MethodBase.GetCurrentMethod().Name, "cartRequest.Cart.CartType = None");
                //    return new CartResponse(false, message, null);
                //}
                else if (cartRequest.Cart.CartTypeValue == (int)ErpCartType.None)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "cartRequest.Cart.CartTypeValue = 0");
                    return new CartResponse(false, message, null);
                }
            }
            return null;
        }

        /// <summary>
        /// ValidateCartRequest validates Create/Update cart Object.
        /// </summary>
        /// <param name="createMergedCartRequest"></param>
        /// <returns></returns>
        private CartResponse ValidateCreateMergedCartRequest(CreateMergedCartRequest createMergedCartRequest)
        {
            if (createMergedCartRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CartResponse(false, message, null);
            }
            else
            {
                if (createMergedCartRequest.CalculationModes == EcomCalculationModes.None)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "createMergedCartRequest.CalculationModes");
                    return new CartResponse(false, message, null);
                }
                else if (string.IsNullOrWhiteSpace(createMergedCartRequest.CartId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "createMergedCartRequest.CartId");
                    return new CartResponse(false, message, null);
                }
                else if (createMergedCartRequest.CartLines == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "createMergedCartRequest.CartLines");
                    return new CartResponse(false, message, null);
                }
                else if (!string.IsNullOrWhiteSpace(createMergedCartRequest.CartId) && createMergedCartRequest.DeliverySpecification != null)
                {
                    ErpDeliverySpecification erpDeliverySpecification = _mapper.Map<EcomDeliverySpecification, ErpDeliverySpecification>(createMergedCartRequest.DeliverySpecification);

                    DeliverySpecificationRequest deliverySpecificationRequest = new DeliverySpecificationRequest();
                    deliverySpecificationRequest.CartId = createMergedCartRequest.CartId;
                    deliverySpecificationRequest.DeliverySpecification = erpDeliverySpecification;

                    return ValidateDeliverySpecificationRequest(deliverySpecificationRequest);
                }
            }
            return null;
        }

        private CartResponse ValidateCreateCartWithLinesAndCouponsRequest(CreateCartWithLinesAndCouponRequest createCartWithLinesAndCouponRequest)
        {
            if (createCartWithLinesAndCouponRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CartResponse(false, message, null);
            }
            else
            {
                if (createCartWithLinesAndCouponRequest.CalculationModes == EcomCalculationModes.None)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "CreateCartWithLinesAndCouponsRequest.CalculationModes");
                    return new CartResponse(false, message, null);
                }
                else if (string.IsNullOrWhiteSpace(createCartWithLinesAndCouponRequest.CartId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateCartWithLinesAndCouponsRequest.CartId");
                    return new CartResponse(false, message, null);
                }
                else if (createCartWithLinesAndCouponRequest.CartLines == null && createCartWithLinesAndCouponRequest.CartLines.Count <= 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateCartWithLinesAndCouponsRequest.CartLines");
                    return new CartResponse(false, message, null);
                }
                else if (createCartWithLinesAndCouponRequest.CouponCodes == null && createCartWithLinesAndCouponRequest.CouponCodes.Count <= 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CreateCartWithLinesAndCouponsRequest.CouponCodes");
                    return new CartResponse(false, message, null);
                }
            }

            return null;
        }

        /// <summary>
        /// ValidateRemoveCartLinesRequest validates remove cart line Object.
        /// </summary>
        /// <param name="removeCartLineRequest"></param>
        /// <returns></returns>
        private CartResponse ValidateRemoveCartLinesRequest(RemoveCartLinesRequest removeCartLineRequest)
        {
            if (removeCartLineRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CartResponse(false, message, null);
            }
            else
            {
                if (removeCartLineRequest.CalculationModes == ErpCalculationModes.None)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "removeCartLineRequest.CalculationModes");
                    return new CartResponse(false, message, null);
                }

                if (removeCartLineRequest.LineIds == null || removeCartLineRequest.LineIds.Count == 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "removeCartLineRequest.LineIds");
                    return new CartResponse(false, message, null);
                }
            }
            return null;
        }

        /// <summary>
        /// ValidateDeliverySpecificationRequest validates delivery info of cart Object.
        /// </summary>
        /// <param name="deliverySpecificationRequest"></param>
        /// <returns></returns>
        private CartResponse ValidateDeliverySpecificationRequest(DeliverySpecificationRequest deliverySpecificationRequest)
        {
            if (deliverySpecificationRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CartResponse(false, message, null);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(deliverySpecificationRequest.CartId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "deliverySpecificationRequest.CartId");
                    return new CartResponse(false, message, null);
                }

                if (deliverySpecificationRequest.DeliverySpecification == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "deliverySpecificationRequest.DeliverySpecification");
                    return new CartResponse(false, message, null);
                }
                else
                {
                    //if (string.IsNullOrWhiteSpace(deliverySpecificationRequest.DeliverySpecification.DeliveryModeId))
                    //{
                    //    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, MethodBase.GetCurrentMethod().Name, "deliverySpecificationRequest.DeliverySpecification.DeliveryModeId");
                    //    return new CartResponse(false, message, null);
                    //}

                    if (deliverySpecificationRequest.DeliverySpecification.DeliveryPreferenceType == ErpDeliveryPreferenceType.ShipToAddress &&
                        deliverySpecificationRequest.DeliverySpecification.DeliveryAddress == null)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "deliverySpecificationRequest.DeliverySpecification.DeliveryAddress");
                        return new CartResponse(false, message, null);
                    }

                    if (deliverySpecificationRequest.DeliverySpecification.DeliveryPreferenceType == ErpDeliveryPreferenceType.ShipToAddress &&
                        deliverySpecificationRequest.DeliverySpecification.DeliveryAddress != null &&
                        string.IsNullOrWhiteSpace(deliverySpecificationRequest.DeliverySpecification.DeliveryAddress.TaxGroup))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "deliverySpecificationRequest.DeliverySpecification.DeliveryAddress.TaxGroup");
                        return new CartResponse(false, message, null);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// RemoveCouponsFromCartRequest validates remove coupons from cart Object.
        /// </summary>
        /// <param name="removeCouponsFromCartRequest"></param>
        /// <returns></returns>
        private CartResponse ValidateRemoveCouponsFromCartRequest(RemoveCouponsFromCartRequest removeCouponsFromCartRequest)
        {
            if (removeCouponsFromCartRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CartResponse(false, message, null);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(removeCouponsFromCartRequest.CartId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "removeCouponsFromCartRequest.CartId");
                    return new CartResponse(false, message, null);
                }
                else if (removeCouponsFromCartRequest.CouponCodes == null || removeCouponsFromCartRequest.CouponCodes.Count == 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "removeCouponsFromCartRequest.CouponCodes");
                    return new CartResponse(false, message, null);
                }
            }
            return null;
        }

        /// <summary>
        /// ValidateAddCouponsToCartRequest validates add coupons to cart Object.
        /// </summary>
        /// <param name="addCouponsToCartRequest"></param>
        /// <returns></returns>
        private CartResponse ValidateAddCouponsToCartRequest(AddCouponsToCartRequest addCouponsToCartRequest)
        {
            if (addCouponsToCartRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CartResponse(false, message, null);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(addCouponsToCartRequest.CartId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "addCouponsToCartRequest.CartId");
                    return new CartResponse(false, message, null);
                }
                else if (addCouponsToCartRequest.CouponCodes == null || addCouponsToCartRequest.CouponCodes.Count == 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "addCouponsToCartRequest.CouponCodes");
                    return new CartResponse(false, message, null);
                }
            }
            return null;
        }

        /// <summary>
        /// ValidateCartLine validates cart line Object.
        /// </summary>
        /// <param name="cartLinesRequest"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        private CartResponse ValidateCartLine(CartLinesRequest cartLinesRequest, CartLineRequestType requestType)
        {
            if (cartLinesRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CartResponse(false, message, null);
            }
            else
            {
                if (cartLinesRequest.CalculationModes == ErpCalculationModes.None)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "CartLinesRequest.CalculationModes");
                    return new CartResponse(false, message, null);
                }

                if (string.IsNullOrWhiteSpace(cartLinesRequest.CartId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CartLinesRequest.CartId");
                    return new CartResponse(false, message, null);
                }

                if (cartLinesRequest.CartLines == null || cartLinesRequest.CartLines.Count == 0)
                {
                    if (requestType == CartLineRequestType.Add)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "CartLinesRequest.CartLines");
                        return new CartResponse(false, message, null);

                    }
                    else if (requestType == CartLineRequestType.Update)
                    {

                    }
                    else if (requestType == CartLineRequestType.Void)
                    {

                    }
                }
            }

            return null;
        }


        /// <summary>
        /// ValidateAddTenderLineRequest validates payment to add in cart Object.
        /// </summary>
        /// <param name="addTenderLineRequest"></param>
        /// <returns></returns>
        private CartResponse ValidateAddTenderLineRequest(AddTenderLineRequest addTenderLineRequest)
        {
            if (addTenderLineRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CartResponse(false, message, null);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(addTenderLineRequest.CartId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "addTenderLineRequest.CartId");
                    return new CartResponse(false, message, null);
                }
                else if (addTenderLineRequest.CartTenderLine == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "addTenderLineRequest.CartTenderLine");
                    return new CartResponse(false, message, null);
                }
            }
            return null;
        }

        /// <summary>
        /// ValidateAddPreprocessedTenderLineRequest validates preprocessed payment to add in cart Object.
        /// </summary>
        /// <param name="addPreprocessedTenderLineRequest"></param>
        /// <returns></returns>
        private CartResponse ValidateAddPreprocessedTenderLineRequest(AddPreprocessedTenderLineRequest addPreprocessedTenderLineRequest)
        {
            if (addPreprocessedTenderLineRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CartResponse(false, message, null);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(addPreprocessedTenderLineRequest.CartId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "addPreprocessedTenderLineRequest.CartId");
                    return new CartResponse(false, message, null);
                }
                else if (addPreprocessedTenderLineRequest.TenderLine == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "addPreprocessedTenderLineRequest.TenderLine");
                    return new CartResponse(false, message, null);
                }
            }
            return null;
        }

        #endregion

        #region Cart Request, Response classes

        /// <summary>
        /// Represents create/update cart request
        /// </summary>
        public class CartRequest
        {
            /// <summary>
            /// CalculationModes of cart
            /// </summary>
            public ErpCalculationModes CalculationModes { get; set; }

            /// <summary>
            /// IsUpdated of cart
            /// </summary>
            public bool IsUpdate { get; set; }

            /// <summary>
            /// Cart
            /// </summary>
            public ErpCart Cart { get; set; }

        }

        /// <summary>
        /// Represents void cart line request
        /// </summary>
        public class CartLinesRequest
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public CartLinesRequest()
            {
                this.CartLines = new List<ErpCartLine>();
            }

            /// <summary>
            /// cartId of cart
            /// </summary>
            [Required]
            public string CartId { get; set; }

            /// <summary>
            /// CalculationModes of cart
            /// </summary>
            public ErpCalculationModes CalculationModes { get; set; }

            /// <summary>
            /// Lines of cart
            /// </summary>
            public List<ErpCartLine> CartLines { get; set; }


            /// <summary>
            /// cartVersion of cart
            /// </summary>
            public long cartVersion { get; set; }

        }

        /// <summary>
        /// Represents remove cart line request
        /// </summary>
        public class RemoveCartLinesRequest
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public RemoveCartLinesRequest()
            {
                this.LineIds = new List<string>();
            }

            /// <summary>
            /// cartId of cart
            /// </summary>
            [Required]
            public string CartId { get; set; }

            /// <summary>
            /// CalculationModes of cart
            /// </summary>
            public ErpCalculationModes CalculationModes { get; set; }

            /// <summary>
            /// Lines of cart
            /// </summary>
            public List<string> LineIds { get; set; }

        }


        public class CreateCartWithLinesAndCouponRequest
        {
            /// <summary>
            /// Constructor
            /// </summary>
            //public CreateCartWithLinesAndCouponRequest()
            //{
            //    this.CouponCodes = new List<string>();
            //}

            [Required]
            public string CartId { get; set; }
            public EcomCalculationModes CalculationModes { get; set; }
            public List<EcomCartLine> CartLines { get; set; }

            public bool isLegacyDiscountCode { get; set; }

            /// <summary>
            /// Coupon codes of cart
            /// </summary>
            public List<string> CouponCodes { get; set; }

        }



        /// <summary>
        /// Represents AddCouponsToCart in cart request
        /// </summary>
        public class AddCouponsToCartRequest
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public AddCouponsToCartRequest()
            {
                this.CouponCodes = new List<string>();
            }

            /// <summary>
            /// cartId of cart
            /// </summary>
            [Required]
            public string CartId { get; set; }

            /// <summary>
            /// isLegacyDiscountCode of coupon code
            /// </summary>
            public bool isLegacyDiscountCode { get; set; }

            /// <summary>
            /// Coupon codes of cart
            /// </summary>
            public List<string> CouponCodes { get; set; }

        }

        /// <summary>
        /// Represents RemoveCouponsFromCart in cart request
        /// </summary>
        public class RemoveCouponsFromCartRequest
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public RemoveCouponsFromCartRequest()
            {
                this.CouponCodes = new List<string>();
            }

            /// <summary>
            /// cartId of cart
            /// </summary>
            [Required]
            public string CartId { get; set; }

            /// <summary>
            /// Coupon codes of cart
            /// </summary>
            public List<string> CouponCodes { get; set; }

        }

        /// <summary>
        /// Represents AddTenderLineRequest in cart request
        /// </summary>
        public class AddTenderLineRequest
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public AddTenderLineRequest()
            {
                this.CartTenderLine = new ErpCartTenderLine();
            }

            /// <summary>
            /// cartId of cart
            /// </summary>
            [Required]
            public string CartId { get; set; }

            /// <summary>
            /// CartTenderLine of cart
            /// </summary>
            public ErpCartTenderLine CartTenderLine { get; set; }

            /// <summary>
            /// cartVersion of cart
            /// </summary>
            public long cartVersion { get; set; }
        }

        /// <summary>
        /// Represents AddPreprocessedTenderLineRequest in cart request
        /// </summary>
        public class AddPreprocessedTenderLineRequest
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public AddPreprocessedTenderLineRequest()
            {
                this.TenderLine = new ErpTenderLine();
            }

            /// <summary>
            /// cartId of cart
            /// </summary>
            [Required]
            public string CartId { get; set; }

            /// <summary>
            /// CartTenderLine of cart
            /// </summary>
            public ErpTenderLine TenderLine { get; set; }

            /// <summary>
            /// cartVersion of cart
            /// </summary>
            public long cartVersion { get; set; }
        }

        /// <summary>
        /// Represents cart response
        /// </summary>
        public class CartResponse
        {

            /// <summary>
            /// Initializes a new instance of the CartResponse
            /// </summary>
            /// <param name="status">status</param>
            /// <param name="errorMessage">error Message</param>
            /// <param name="cart">cart</param>
            public CartResponse(bool status, string errorMessage, ErpCart cart)
            {
                this.Status = status;
                this.ErrorMessage = errorMessage;
                this.Cart = cart;
            }

            /// <summary>
            /// Status of cart
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// ErrorMessage of cart
            /// </summary>
            public string ErrorMessage { get; set; }

            /// <summary>
            /// Cart
            /// </summary>
            public ErpCart Cart { get; set; }

        }

        /// <summary>
        /// Represents to add delivery specifications to cart request
        /// </summary>
        public class DeliverySpecificationRequest
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public DeliverySpecificationRequest()
            {
                DeliverySpecification = new ErpDeliverySpecification();
            }

            /// <summary>
            /// cartId of cart
            /// </summary>

            [Required]
            public string CartId { get; set; }

            /// <summary>
            /// DeliverySpecification of cart
            /// </summary>
            public ErpDeliverySpecification DeliverySpecification { get; set; }

        }

        private enum CartLineRequestType
        {
            Add,
            Update,
            Void
        }
        /// <summary>
        /// Represents Get cart request
        /// </summary>
        public class GetCartRequest
        {
            /// <summary>
            /// CartId of cart
            /// </summary>
            [Required]
            public string CartId { get; set; }

        }

        #endregion
    }
}