using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    /// <summary>
    /// CartCRTManager class implements the CRT Manager to manage crt ax controller.
    /// </summary>
    public class CartCRTManager
    {
        #region Properties
        private readonly ICRTFactory _crtFactory;
        #endregion Properties

        #region Constructor      
        public CartCRTManager()
        {
            _crtFactory = new CRTFactory();
        }
        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// AddTenderLine resolves CRT Controller to call it's AddTenderLine method.
        /// </summary>
        /// <returns></returns>
        public ErpCartResponse AddTenderLine(string cartId, ErpCartTenderLine cartTenderLine, long cartVersion, string storeKey)
        {
            var cartController = _crtFactory.CreateCartController(storeKey);
            return cartController.AddTenderLine(cartId, cartTenderLine, cartVersion);
        }

        /// <summary>
        /// AddPreprocessedTenderLine resolves CRT Controller to call it's AddPreprocessedTenderLine method.
        /// </summary>
        /// <returns></returns>
        public ErpCartResponse AddPreprocessedTenderLine(string cartId, ErpTenderLine preprocessedTenderLine, long cartVersion, string storeKey)
        {
            var cartController = _crtFactory.CreateCartController(storeKey);
            return cartController.AddPreprocessedTenderLine(cartId, preprocessedTenderLine, cartVersion);
        }

        /// <summary>
        /// UpdateCartLines resolves CRT Controller to call it's UpdateCartLines method.
        /// </summary>
        /// <returns></returns>
        public ErpCartResponse UpdateCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes, string storeKey)
        {
            var cartController = _crtFactory.CreateCartController(storeKey);

            return cartController.UpdateCartLines(cartId, cartLines, calculationModes);
        }

        /// <summary>
        /// AddCouponsToCart resolves CRT Controller to call it's AddCouponsToCart method.
        /// </summary>
        /// <returns></returns>
        public ErpCartResponse AddCouponsToCart(string cartId, IEnumerable<string> couponsCodes, bool isLegacyDiscountCode, string storeKey,string requestId)
        {
            var cartController = _crtFactory.CreateCartController(storeKey);

            return cartController.AddCouponsToCart(cartId, couponsCodes, isLegacyDiscountCode, requestId);
        }

        /// <summary>
        /// RemoveCouponsFromCart resolves CRT Controller to call it's RemoveCouponsFromCart method.
        /// </summary>
        /// <returns></returns>
        public ErpCartResponse RemoveCouponsFromCart(string cartId, IEnumerable<string> couponsCodes, string storeKey)
        {
            var cartController = _crtFactory.CreateCartController(storeKey);

            return cartController.RemoveCouponsFromCart(cartId, couponsCodes);
        }

        /// <summary>
        /// RemoveCartLines resolves CRT Controller to call it's RemoveCartLines method.
        /// </summary>
        /// <returns></returns>
        public ErpCartResponse RemoveCartLines(string cartId, IEnumerable<string> lineIds, ErpCalculationModes calculationModes, string storeKey,string requestId)
        {

            var cartController = _crtFactory.CreateCartController(storeKey);

            return cartController.RemoveCartLines(cartId, lineIds, calculationModes, requestId);
        }

        /// <summary>
        /// VoidCartLines resolves CRT Controller to call it's VoidCartLines method.
        /// </summary>
        /// <returns></returns>
        public ErpCartResponse VoidCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes, string storeKey)
        {
            var cartController = _crtFactory.CreateCartController(storeKey);

            return cartController.VoidCartLines(cartId, cartLines, calculationModes);
        }

        /// <summary>
        /// UpdateDeliverySpecification resolves CRT Controller to call it's UpdateDeliverySpecification method.
        /// </summary>
        /// <returns></returns>
        public ErpCartResponse UpdateDeliverySpecification(string cartId, ErpDeliverySpecification deliverySpecification, string storeKey)
        {
            var cartController = _crtFactory.CreateCartController(storeKey);

            return cartController.UpdateDeliverySpecification(cartId, deliverySpecification);
        }

        /// <summary>
        /// AddCartLines resolves CRT Controller to call it's AddCartLines method.
        /// </summary>
        /// <returns></returns>
        public ErpCartResponse AddCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes, long cartVersion, string storeKey)
        {
            var cartController = _crtFactory.CreateCartController(storeKey);

            return cartController.AddCartLines(cartId, cartLines, calculationModes, cartVersion);
        }

        /// <summary>
        /// CreateOrUpdateCart resolves CRT Controller to call it's CreateOrUpdateCart method.
        /// </summary>
        /// <returns></returns>
        public ErpCartResponse CreateOrUpdateCart(ErpCart cart, ErpCalculationModes calculationModes, string storeKey)
        {
            var cartController = _crtFactory.CreateCartController(storeKey);

            return cartController.CreateOrUpdateCart(cart, calculationModes);
        }



        //public ErpCartResponse CreateCartWithItemsAndCoupons(string cartId, ErpCalculationModes calculationModes, IEnumerable<ErpCartLine> cartLines, IEnumerable<string> couponCodes, bool isLegacyDiscountCode,string storeKey)
        //{
        //    var cartController = _crtFactory.CreateCartController(storeKey);

        //    return cartController.CreateCartWithLinesAndCoupon(cartId, calculationModes, cartLines, couponCodes, isLegacyDiscountCode);
        //}

        public ErpCartResponse CreateMergedCart(string cartId, long affiliationId, ErpCalculationModes calculationModes, IEnumerable<ErpCartLine> cartLines, ErpDeliverySpecification deliverySpecification, IEnumerable<string> couponCodes, bool isLegacyDiscountCode, string storeKey, string requestId)
        {
            var cartController = _crtFactory.CreateCartController(storeKey);

            return cartController.CreateMergedCart(cartId, affiliationId, calculationModes, cartLines, deliverySpecification, couponCodes, isLegacyDiscountCode, requestId);
        }


        /// <summary>
        /// GetCart resolves CRT Controller to call it's GetCart method.
        /// </summary>
        /// <returns></returns>
        public ErpCartResponse GetCart(string cartId, string storeKey)
        {
            var cartController = _crtFactory.CreateCartController(storeKey);

            return cartController.GetCart(cartId);
        }

        /// <summary>
        /// GetCarts resolves CRT Controller to call it's GetCarts method.
        /// </summary>
        /// <param name="cartIds"></param>
        /// <returns></returns>
        public List<ErpCartResponse> GetCarts(List<string> cartIds, string storeKey)
        {
            var cartController = _crtFactory.CreateCartController(storeKey);

            return cartController.GetCarts(cartIds);
        }
        #endregion
    }
}
