//using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    /// <summary>
    /// CartController class performs cart related activities.
    /// </summary>
    public class CartController : BaseController, ICartController
    {
        #region Constructor
        public CartController(string storeKey) : base(storeKey)
        {
        }

        #endregion

        #region Public
        public ErpCartResponse CreateMergedCart(string cartId, long affiliationId, ErpCalculationModes calculationModes, IEnumerable<ErpCartLine> cartLines, ErpDeliverySpecification deliverySpecification, IEnumerable<string> couponCodes, bool isLegacyDiscountCode, string requestId)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ProcessCartLine(cartLines);

            if (deliverySpecification != null && string.IsNullOrWhiteSpace(deliverySpecification.DeliveryModeId))
            {
                deliverySpecification.DeliveryModeId = configurationHelper.GetSetting(SALESORDER.AX_Default_Delivery_Mode);
            }

            var cartManager = new CartCRTManager();
            ErpCartResponse cartResponse = cartManager.CreateMergedCart(cartId, affiliationId, calculationModes, cartLines, deliverySpecification, couponCodes, isLegacyDiscountCode, currentStore.StoreKey, requestId);

            if (cartResponse.Cart != null)
            {
                ReCalculateCart(cartResponse.Cart);
            }

            return cartResponse;
        }
        
        //public ErpCartResponse CreateCartWithItemsAndCoupon(string cartId, ErpCalculationModes calculationModes, IEnumerable<ErpCartLine> cartLines, IEnumerable<string> couponCodes, bool isLegacyDiscountCode)
        //{
        //    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
        //    ProcessCartLine(cartLines);
        //    var cartManager = new CartCRTManager();               
        //    ErpCartResponse cartResponse = cartManager.CreateCartWithItemsAndCoupons(cartId, calculationModes,cartLines,couponCodes, isLegacyDiscountCode, currentStore.StoreKey);

        //    if (cartResponse.Cart != null)
        //    {
        //        ReCalculateCart(cartResponse.Cart);
        //    }

        //    return cartResponse;
        //}

        public ErpCartResponse AddCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes, long cartVersion)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ProcessCartLine(cartLines);
            var cartManager = new CartCRTManager();
            ErpCartResponse cartResponse = cartManager.AddCartLines(cartId, cartLines, calculationModes, cartVersion, currentStore.StoreKey);

            if (cartResponse.Cart != null)
            {
                ReCalculateCart(cartResponse.Cart);
            }

            return cartResponse;
        }
        

        public ErpCartResponse CreateOrUpdateCart(ErpCart cart, ErpCalculationModes calculationModes, bool isUpdate)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ProcessCartLine(cart.CartLines);
            var cartManager = new CartCRTManager();

            ErpCartResponse cartResponse = cartManager.CreateOrUpdateCart(cart, calculationModes, currentStore.StoreKey);
            if(cartResponse.Success && !isUpdate)

            {
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                integrationManager.CreateIntegrationKey(Entities.Cart, cart.Id, "");
            }


            if (cartResponse.Cart != null)
            {
                ReCalculateCart(cartResponse.Cart);
            }

            return cartResponse;
        }

        public ErpCartResponse GetCart(string cartId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var cartManager = new CartCRTManager();
            ErpCartResponse cartResponse = cartManager.GetCart(cartId,currentStore.StoreKey);

            if (cartResponse.Cart != null)
            {
                ReCalculateCart(cartResponse.Cart);
            }

            return cartResponse;
        }
        public ErpCartResponse UpdateCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes,string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var cartManager = new CartCRTManager();
            ProcessCartLine(cartLines);
            long cartVersion = 0;
            var cartResponse = cartManager.RemoveCartLines(cartId, cartLines.Select(c => c.LineId).ToList(), calculationModes, currentStore.StoreKey,requestId);
            
            if (cartResponse != null && cartResponse.Success)
            {
                cartVersion = (long)cartResponse.Cart.Version;
                cartResponse = cartManager.AddCartLines(cartId, cartLines, calculationModes, cartVersion, currentStore.StoreKey);
            }

            if (cartResponse.Cart != null)
            {
                ReCalculateCart(cartResponse.Cart);
            }

            return cartResponse;
        }

        public ErpCartResponse VoidCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ProcessCartLine(cartLines);
            var cartManager = new CartCRTManager();
            ErpCartResponse cartResponse = cartManager.VoidCartLines(cartId, cartLines, calculationModes, currentStore.StoreKey);

            if (cartResponse.Cart != null)
            {
                ReCalculateCart(cartResponse.Cart);
            }

            return cartResponse;
        }

        public ErpCartResponse RemoveCartLines(string cartId, IEnumerable<string> lineIds, ErpCalculationModes calculationModes,string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var cartManager = new CartCRTManager();

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, "Create CRT CartManager Ends", DateTime.UtcNow);

            ErpCartResponse cartResponse = cartManager.RemoveCartLines(cartId, lineIds, calculationModes, currentStore.StoreKey, requestId);

            if (cartResponse.Cart != null)
            {
                ReCalculateCart(cartResponse.Cart);
            }

            return cartResponse;
        }

        public ErpCartResponse AddCouponsToCart(string cartId, IEnumerable<string> couponCodes, bool isLegacyDiscountCode,string requestId)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var cartManager = new CartCRTManager();
            ErpCartResponse cartResponse = cartManager.AddCouponsToCart(cartId, couponCodes, isLegacyDiscountCode, currentStore.StoreKey, requestId);

            if (cartResponse.Cart != null)
            {
                ReCalculateCart(cartResponse.Cart);
            }

            return cartResponse;
        }

        public ErpCartResponse RemoveCouponsFromCart(string cartId, IEnumerable<string> couponCodes)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var cartManager = new CartCRTManager();
            ErpCartResponse cartResponse = cartManager.RemoveCouponsFromCart(cartId, couponCodes, currentStore.StoreKey);

            if (cartResponse.Cart != null)
            {
                ReCalculateCart(cartResponse.Cart);
            }

            return cartResponse;
        }
        public ErpCartResponse AddTenderLine(string cartId, ErpCartTenderLine cartTenderLine, long cartVersion)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var cartManager = new CartCRTManager();
            ErpCartResponse cartResponse = cartManager.AddTenderLine(cartId, cartTenderLine, cartVersion, currentStore.StoreKey);

            if (cartResponse.Cart != null)
            {
                ReCalculateCart(cartResponse.Cart);
            }

            return cartResponse;
        }

        public ErpCartResponse AddPreprocessedTenderLine(string cartId, ErpTenderLine preprocessedTenderLine, long cartVersion)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var cartManager = new CartCRTManager();
            ErpCartResponse cartResponse = cartManager.AddPreprocessedTenderLine(cartId, preprocessedTenderLine, cartVersion, currentStore.StoreKey);

            if (cartResponse.Cart != null)
            {
                ReCalculateCart(cartResponse.Cart);
            }

            return cartResponse;
        }

        public ErpCartResponse UpdateDeliverySpecification(string cartId, ErpDeliverySpecification deliverySpecification)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            if (string.IsNullOrWhiteSpace(deliverySpecification.DeliveryModeId))
            {
                deliverySpecification.DeliveryModeId = configurationHelper.GetSetting(SALESORDER.AX_Default_Delivery_Mode);
            }
            var cartManager = new CartCRTManager();
            ErpCartResponse cartResponse = cartManager.UpdateDeliverySpecification(cartId, deliverySpecification, currentStore.StoreKey);

            if (cartResponse.Cart != null)
            {
                ReCalculateCart(cartResponse.Cart);
            }

            return cartResponse;
        }

        /// <summary>
        /// GetAbandonedCarts get list of abandoned Carts.
        /// </summary>
        /// <returns></returns>
        public List<ErpCart> GetAbandonedCarts()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpCart> carts = new List<ErpCart>(); IntegrationManager integrationManager = new IntegrationManager(configurationHelper.currentStore.StoreKey);
            var abandonedCartKeys = integrationManager.GetAbandonedCartKeys(configurationHelper.GetSetting(APPLICATION.AbandonedCartDays).IntValue());

            var abandonedCartIds = abandonedCartKeys.Select(k => k.ErpKey).ToList();
            var cartManager = new CartCRTManager();

            var responses = cartManager.GetCarts(abandonedCartIds, currentStore.StoreKey);

            List<ErpCart> erpCarts;

            if (responses != null)
            {
                erpCarts = responses.Select(r => r.Cart).ToList();
            }
            else
            {
                erpCarts = new List<ErpCart>();
            }

            return erpCarts;
        }
        #endregion

        #region Private

        //NS: Cart ReCalculation to avoid penny issue
        private void ReCalculateCart(ErpCart cart)
        {
            if (cart.CartLines != null && cart.CartLines.Count() > 0)
            {
                foreach (ErpCartLine cartLine in cart.CartLines)
                {
                    //Rounding each discount on 2 decimal places
                    foreach(ErpDiscountLine discountLine in cartLine.DiscountLines)
                    {
                        int discountPriceRounding = -1;
                        discountPriceRounding = Convert.ToInt32(configurationHelper.GetSetting(APPLICATION.ERP_AX_DiscountPriceRounding));

                        if (discountPriceRounding > -1)
                        {
                            discountLine.EffectiveAmount = Math.Round(discountLine.EffectiveAmount, discountPriceRounding);
                        }
                        else
                        {
                            discountLine.EffectiveAmount = Math.Round(discountLine.EffectiveAmount, 2);
                        }
                    }
                    //Update total discount amount of cart line
                    cartLine.DiscountAmount = cartLine.DiscountLines.Sum(d => d.EffectiveAmount);

                    int salesOrderPriceRounding = -1;
                    salesOrderPriceRounding = Convert.ToInt32(configurationHelper.GetSetting(APPLICATION.ERP_AX_SalesOrderPriceRounding));
                    
                    if(salesOrderPriceRounding > -1)
                    {
                        cartLine.Price = Math.Round(Convert.ToDecimal(cartLine.Price), salesOrderPriceRounding);
                    }

                    //Update NetAmountWithoutTax
                    cartLine.ExtendedPrice = cartLine.NetAmountWithoutTax = (cartLine.Price * cartLine.Quantity) - cartLine.DiscountAmount;

                    //Re-calculate tax
                    if (cartLine.TaxRatePercent > 0)
                    {
                        cartLine.TaxAmount = cartLine.NetAmountWithoutTax * cartLine.TaxRatePercent / 100;
                    }

                    if (salesOrderPriceRounding > -1)
                    {
                        cartLine.TaxAmount = Math.Round(Convert.ToDecimal(cartLine.TaxAmount), salesOrderPriceRounding);
                    }

                    //Update line totals
                    cartLine.TotalAmount = cartLine.NetAmountWithoutTax + cartLine.TaxAmount;


                    //Ecom require virtual(ComKey) in ItemId for Cart API response
                    IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                    var key = integrationManager.GetComKey(Entities.Product, cartLine.ProductId.ToString());
                    if (key != null)
                    {
                        cartLine.ItemId = key.ComKey;
                    }
                }

                //Update cart discounts
                cart.DiscountAmount = cart.CartLines.Sum(d => d.DiscountAmount);
                //Update cart Taxs
                cart.TaxAmount = cart.CartLines.Sum(c => c.TaxAmount);
                //Update cart Totals
                cart.SubtotalAmount = cart.CartLines.Sum(st => st.ExtendedPrice);
                cart.SubtotalAmountWithoutTax = cart.CartLines.Sum(st => st.NetAmountWithoutTax);
                cart.AmountDue = cart.TotalAmount = cart.CartLines.Sum(cl => cl.TotalAmount);
            }
        }

        private void ProcessCartLine(IEnumerable<ErpCartLine> cartLines)
        {
            if (cartLines != null && cartLines.Count() > 0)
            {
                foreach (ErpCartLine cartLine in cartLines)
                {
                    GetCartLineItemIdProductIdFromIntegrationDB(cartLine);

                    if (string.IsNullOrWhiteSpace(cartLine.UnitOfMeasureSymbol))
                    {
                        cartLine.UnitOfMeasureSymbol = configurationHelper.GetSetting(SALESORDER.AX_Default_UnitofMeasure);
                    }

                    //NS: TMV re-calculate tax to avoid penny issue in AX totals
                    if (cartLine.TaxRatePercent > 0)
                    {
                        cartLine.TaxAmount = cartLine.NetAmountWithoutTax * cartLine.TaxRatePercent / 100;
                    }
                }
            }
        }

        private void GetCartLineItemIdProductIdFromIntegrationDB(ErpCartLine line)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name, "CartLine: " + JsonConvert.SerializeObject(line));
            bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            var key = integrationManager.GetErpKey(Entities.Product, line.ItemId);
            if (key == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40504, currentStore);
                throw new CommerceLinkError(message);
            }
            string itemId = string.Empty;
            string variantId = string.Empty;
            if (!isFlatProductHierarchy) //Standard Product Hierarchy
            {
                try
                {
                    var temp = key.Description.Split(':');

                    if (temp != null && temp.Any())
                    {
                        itemId = temp[0];
                        if (temp.Length > 1)
                        {
                            variantId = temp[1];
                        }
                    }
                }
                catch (Exception)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40504, currentStore);
                    throw new Exception(message);
                }

                line.ProductId = Convert.ToInt64(key.ErpKey);
                line.ItemId = itemId;
            }
            else
            {
                line.ProductId = Convert.ToInt64(key.ErpKey);
                line.ItemId = key.Description;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }


        #endregion
    }
}
