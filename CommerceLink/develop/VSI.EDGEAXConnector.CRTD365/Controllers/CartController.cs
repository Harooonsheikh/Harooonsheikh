using Microsoft.Dynamics.Commerce.RetailProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using NewRelic.Api.Agent;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;
using System.Linq;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    /// <summary>
    /// CartController class provides features to get cart functionality.
    /// </summary>
    public class CartController : BaseController, ICartController
    {
        #region Public

        public CartController(string storeKey) : base(storeKey)
        {
        }
        public ErpCartResponse CreateMergedCart(string cartId, long affiliationId, ErpCalculationModes calculationModes, IEnumerable<ErpCartLine> cartLines, ErpDeliverySpecification deliverySpecification, IEnumerable<string> couponCodes, bool isLegacyDiscountCode, string requestId)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            bool isExternalSystemTimeLogged = false;
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);
            try
            {
                ErpCart erpCart = new ErpCart();
                CartResponse cartResponse = new CartResponse();
                List<CartLine> cLines = new List<CartLine>();
                var UpdateDeliverySpecification = _mapper.Map<ErpDeliverySpecification, DeliverySpecification>(deliverySpecification);

                foreach (ErpCartLine line in cartLines)
                {
                    CartLine cline = new CartLine();

                    cline.CatalogId = line.CatalogId;
                    cline.CommissionSalesGroup = line.CommissionSalesGroup;
                    cline.Description = line.Description;
                    cline.EntryMethodTypeValue = line.EntryMethodTypeValue;
                    cline.ItemId = line.ItemId;
                    cline.ProductId = line.ProductId;
                    cline.Quantity = line.Quantity;
                    cline.UnitOfMeasureSymbol = line.UnitOfMeasureSymbol;
                    cline.LineId = line.LineId;

                    if (line.Price != null && line.Price > 0)
                    {
                        cline.Price = line.Price;
                    }
                    cLines.Add(cline);
                }

                if (couponCodes == null)
                {
                    couponCodes = new List<string>();
                }
                timer = Stopwatch.StartNew();
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_CreateMergedCart", DateTime.UtcNow);
                cartResponse = ECL_CreateMergedCart(cartId, affiliationId, calculationModes, couponCodes, isLegacyDiscountCode, cLines, UpdateDeliverySpecification);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateMergedCart", GetElapsedTime());

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_CreateMergedCart", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;

                if ((bool)cartResponse.Status && couponCodes.Count() > 0)
                {
                    return AddCouponsToCart(cartId, couponCodes, isLegacyDiscountCode, requestId);
                }

                if ((bool)cartResponse.Status)
                {
                    Cart cartRes = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);
                    ErpCart erpCartWithItems = _mapper.Map<Cart, ErpCart>(cartRes);

                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCartWithItems);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateMergedCart", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_CreateMergedCart", DateTime.UtcNow);
                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateMergedCart", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_CreateMergedCart", DateTime.UtcNow);
                }

                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }

            return erpCartResponse;
        }
        //public ErpCartResponse CreateCartWithLinesAndCoupon(string cartId, ErpCalculationModes calculationModes, IEnumerable<ErpCartLine> cartLines, IEnumerable<string> couponCodes, bool isLegacyDiscountCode)
        //{
        //    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
        //    ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);
        //    try
        //    {
        //        ErpCart erpCart = new ErpCart();
        //        CartResponse cartResponse = new CartResponse();
        //        List<CartLine> cLines = new List<CartLine>();

        //        foreach (ErpCartLine line in cartLines)
        //        {
        //            CartLine cline = new CartLine();

        //            cline.CatalogId = line.CatalogId;
        //            cline.CommissionSalesGroup = line.CommissionSalesGroup;
        //            cline.Description = line.Description;
        //            cline.EntryMethodTypeValue = line.EntryMethodTypeValue;
        //            cline.ItemId = line.ItemId;
        //            cline.ProductId = line.ProductId;
        //            cline.Quantity = line.Quantity;
        //            cline.UnitOfMeasureSymbol = line.UnitOfMeasureSymbol;
        //            cline.LineId = line.LineId;

        //            if (line.Price != null && line.Price > 0)
        //            {
        //                cline.Price = line.Price;
        //            }
        //            cLines.Add(cline);
        //        }

        //        var cartManager = RPFactory.GetManager<ICartManager>();
        //        cartResponse = Task.Run(async () => await cartManager.ECL_CreateCartWithLinesAndCoupons(cartId, calculationModes.ToString(), cLines, couponCodes, isLegacyDiscountCode)).Result;

        //        if (cartResponse.Success)
        //        {
        //            Cart cartRes = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);
        //            ErpCart erpCartWithItems = _mapper.Map<Cart, ErpCart>(cartRes);

        //            erpCartResponse = new ErpCartResponse(cartResponse.Success, cartResponse.Message, erpCartWithItems);                    
        //        }
        //        else
        //        {
        //            erpCartResponse = new ErpCartResponse(cartResponse.Success, cartResponse.Message, null);
        //        }
        //    }
        //    catch (RetailProxyException rpe)
        //    {
        //        string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
        //        AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

        //        erpCartResponse = new ErpCartResponse(false, exp.Message, null);
        //    }
        //    catch (Exception exp)
        //    {
        //        CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
        //        erpCartResponse = new ErpCartResponse(false, exp.Message, null);
        //    }
        //    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        //    return erpCartResponse;
        //}
        public ErpCartResponse AddCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes, long cartVersion)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);
            try
            {
                CartResponse cartResponse = new CartResponse();
                var calcModes = _mapper.Map<ErpCalculationModes, Microsoft.Dynamics.Commerce.Runtime.DataModel.CalculationModes>(calculationModes);

                List<CartLine> cLines = new List<CartLine>();

                foreach (ErpCartLine line in cartLines)
                {
                    CartLine cline = new CartLine();

                    cline.CatalogId = line.CatalogId;
                    cline.CommissionSalesGroup = line.CommissionSalesGroup;
                    cline.Description = line.Description;
                    cline.EntryMethodTypeValue = line.EntryMethodTypeValue;
                    cline.ItemId = line.ItemId;
                    cline.ProductId = line.ProductId;
                    cline.Quantity = line.Quantity;
                    cline.UnitOfMeasureSymbol = line.UnitOfMeasureSymbol;
                    cline.LineId = line.LineId;

                    if (line.Price != null && line.Price > 0)
                    {
                        cline.Price = line.Price;
                    }
                    cLines.Add(cline);
                }
                cartResponse = ECL_AddCartLines(cartId, cartVersion, calcModes, cLines);

                if ((bool)cartResponse.Status)
                {
                    Cart cart = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);
                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpCartResponse;
        }
        public ErpCartResponse CreateOrUpdateCart(ErpCart cart, ErpCalculationModes calculationModes)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);
            try
            {
                CartResponse cartResponse = new CartResponse();
                var cartObject = _mapper.Map<ErpCart, Cart>(cart);
                var calcModes = _mapper.Map<ErpCalculationModes, Microsoft.Dynamics.Commerce.Runtime.DataModel.CalculationModes>(calculationModes);
                cartResponse = ECL_CreateOrUpdateCart(cartObject, calcModes);
                if ((bool)cartResponse.Status)
                {
                    Cart cartRes = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);
                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cartRes);
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpCartResponse;
        }
        public ErpCartResponse UpdateDeliverySpecification(string cartId, ErpDeliverySpecification deliverySpecification)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);
            try
            {
                CartResponse cartResponse = new CartResponse();

                var delSpec = _mapper.Map<ErpDeliverySpecification, DeliverySpecification>(deliverySpecification);

                cartResponse = ECL_UpdateDeliverySpecification(cartId, delSpec);

                if ((bool)cartResponse.Status)
                {
                    Cart cartRes = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);
                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cartRes);
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpCartResponse;
        }
        public ErpCartResponse GetCart(string cartId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);
            try
            {
                CartResponse cartResponse = new CartResponse();
                cartResponse = ECL_GetCart(cartId);

                if ((bool)cartResponse.Status)
                {
                    Cart cart = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);
                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartResponse;
        }
        public List<ErpCartResponse> GetCarts(List<string> cartIds)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpCartResponse> erpCartResponses = new List<ErpCartResponse>();

            ErpCartResponse cartResponse;

            if (cartIds != null && cartIds.Count > 0)
            {
                foreach (var cartId in cartIds)
                {
                    cartResponse = this.GetCart(cartId);

                    if (cartResponse != null && cartResponse.Success && !string.IsNullOrWhiteSpace(cartResponse.Cart.CustomerId))
                    {
                        erpCartResponses.Add(cartResponse);
                    }
                    else
                    {
                        IntegrationManager integrationManager = new IntegrationManager(configurationHelper.currentStore.StoreKey);
                        integrationManager.MarkAbandonedCartFailedCart(cartId);
                    }
                }
            }

            return erpCartResponses;
        }
        public ErpCartResponse VoidCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);
            try
            {
                CartResponse cartResponse = new CartResponse();

                var lines = _mapper.Map<IEnumerable<ErpCartLine>, IEnumerable<CartLine>>(cartLines);
                var calcModes = _mapper.Map<ErpCalculationModes, Microsoft.Dynamics.Commerce.Runtime.DataModel.CalculationModes>(calculationModes);
                cartResponse = ECL_VoidCartLines(cartId, lines, calcModes);
                if ((bool)cartResponse.Status)
                {
                    Cart cart = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);

                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);

                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartResponse;
        }
        public ErpCartResponse RemoveCartLines(string cartId, IEnumerable<string> lineIds, ErpCalculationModes calculationModes, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            var isExternalTimeLogged = false;
            try
            {
                CartResponse cartResponse = new CartResponse();

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, "AutoMap ErpCalculationModes", DateTime.UtcNow);
                var calcModes = _mapper.Map<ErpCalculationModes, Microsoft.Dynamics.Commerce.Runtime.DataModel.CalculationModes>(calculationModes);

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, "AutoMap ErpCalculationModes Ends", DateTime.UtcNow);

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_RemoveCartLines", DateTime.UtcNow);
                timer = Stopwatch.StartNew();
                cartResponse = ECL_RemoveCartLines(cartId, lineIds, calcModes);

                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "RemoveCartLines", GetElapsedTime());

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "ECL_RemoveCartLines", DateTime.UtcNow);

                isExternalTimeLogged = true;

                if ((bool)cartResponse.Status)
                {
                    Cart cart = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);

                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);

                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                if (!isExternalTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "RemoveCartLines", GetElapsedTime());

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_RemoveCartLines", DateTime.UtcNow);

                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                if (!isExternalTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "RemoveCartLines", GetElapsedTime());

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_RemoveCartLines", DateTime.UtcNow);

                }

                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartResponse;
        }
        public ErpCartResponse UpdateCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);
            try
            {
                CartResponse cartResponse = new CartResponse();
                var lines = _mapper.Map<IEnumerable<ErpCartLine>, IEnumerable<CartLine>>(cartLines);
                var calcModes = _mapper.Map<ErpCalculationModes, Microsoft.Dynamics.Commerce.Runtime.DataModel.CalculationModes>(calculationModes);

                cartResponse = ECL_UpdateCartLines(cartId, lines, calcModes);

                if ((bool)cartResponse.Status)
                {
                    Cart cart = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);
                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartResponse;
        }
        public ErpCartResponse AddCouponsToCart(string cartId, IEnumerable<string> couponCodes, bool isLegacyDiscountCode, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                CartResponse cartResponse = new CartResponse();

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "External ECL_AddCouponsToCart Starts", DateTime.UtcNow);
                timer = Stopwatch.StartNew();

                cartResponse = ECL_ValidateCouponsToCart(cartId, couponCodes, isLegacyDiscountCode);
                if (!(bool)cartResponse.Status)
                {
                    return new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }

                var cart = ECL_AddCouponsToCart(cartId, couponCodes, isLegacyDiscountCode);

                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "AddCouponsToCart", GetElapsedTime());

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "External ECL_AddCouponsToCart Ends", DateTime.UtcNow);

                if (cart != null)
                {
                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);
                    erpCartResponse = new ErpCartResponse(true, "VSIRSTV20000 | Success! A discount has been applied to your purchase.", erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse(false, "Empty cart", null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartResponse;
        }
        public ErpCartResponse RemoveCouponsFromCart(string cartId, IEnumerable<string> couponCodes)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                CartResponse cartResponse = new CartResponse();
                var cart = ECL_RemoveCouponsFromCart(cartId, couponCodes);

                if (cart != null)
                {
                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);
                    erpCartResponse = new ErpCartResponse(true, "Success", erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse(false, "Empty cart", null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartResponse;
        }
        public ErpCartResponse AddDiscountCodesToCart(string cartId, IEnumerable<string> dicountCodes)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);
            try
            {
                CartResponse cartResponse = new CartResponse();
                cartResponse = ECL_AddDiscountCodesToCart(cartId, dicountCodes);
                if ((bool)cartResponse.Status)
                {
                    Cart cart = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);

                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);

                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartResponse;
        }
        public ErpCartResponse RemoveDiscountCodesFromCart(string cartId, IEnumerable<string> dicountCodes)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                CartResponse cartResponse = new CartResponse();
                cartResponse = ECL_RemoveDiscountCodesFromCart(cartId, dicountCodes);

                if ((bool)cartResponse.Status)
                {
                    Cart cart = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);

                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);

                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartResponse;
        }
        public ErpCartResponse AddTenderLine(string cartId, ErpCartTenderLine cartTenderLine, long cartVersion)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                CartResponse cartResponse = new CartResponse();

                var tenderLine = _mapper.Map<ErpCartTenderLine, CartTenderLine>(cartTenderLine);
                cartResponse = ECL_AddTenderLine(cartId, cartVersion, tenderLine);

                if ((bool)cartResponse.Status)
                {
                    Cart cart = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);

                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);

                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartResponse;
        }
        public ErpCartResponse AddPreprocessedTenderLine(string cartId, ErpTenderLine preprocessedTenderLine, long cartVersion)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                CartResponse cartResponse = new CartResponse();

                var tenderLine = _mapper.Map<ErpTenderLine, TenderLine>(preprocessedTenderLine);

                cartResponse = ECL_AddPreprocessedTenderLine(cartId, cartVersion, tenderLine);

                if ((bool)cartResponse.Status)
                {
                    Cart cart = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);

                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);

                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartResponse;
        }
        public ErpCartCheckoutResponse Checkout(string cartId, string receiptEmail, string receiptNumberSequence, long cartVersion)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCartCheckoutResponse erpCartCheckoutResponse = new ErpCartCheckoutResponse(false, "", null);
            try
            {
                CartResponse cartResponse = new CartResponse();
                cartResponse = ECL_Checkout(cartId, receiptEmail, receiptNumberSequence, cartVersion);
                if ((bool)cartResponse.Status)
                {
                    SalesOrder salesOrder = JsonConvert.DeserializeObject<SalesOrder>(cartResponse.Result);

                    ErpSalesOrder erpSalesOrder = _mapper.Map<SalesOrder, ErpSalesOrder>(salesOrder);

                    erpCartCheckoutResponse = new ErpCartCheckoutResponse((bool)cartResponse.Status, cartResponse.Message, erpSalesOrder);
                }
                else
                {
                    erpCartCheckoutResponse = new ErpCartCheckoutResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartCheckoutResponse = new ErpCartCheckoutResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartCheckoutResponse = new ErpCartCheckoutResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartCheckoutResponse;
        }
        public ErpCartResponse RecalculateCustomerOrder(string cartId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                CartResponse cartResponse = new CartResponse();
                cartResponse = ECL_RecalculateCustomerOrder(cartId);

                if ((bool)cartResponse.Status)
                {
                    Cart cart = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);

                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);

                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartResponse;
        }
        public ErpCartResponse DeleteCarts(List<string> cartIds)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                CartResponse cartResponse = new CartResponse();
                cartResponse = ECL_DeleteCarts(cartIds);

                if ((bool)cartResponse.Status)
                {
                    Cart cart = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);

                    ErpCart erpCart = _mapper.Map<Cart, ErpCart>(cart);

                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, erpCart);
                }
                else
                {
                    erpCartResponse = new ErpCartResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCartResponse = new ErpCartResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCartResponse;
        }
        #endregion
        #region RetailServer API Calls
        [Trace]
        private CartResponse ECL_CreateMergedCart(string cartId, long affiliationId, ErpCalculationModes calculationModes, IEnumerable<string> couponCodes, bool isLegacyDiscountCode, List<CartLine> cLines, DeliverySpecification UpdateDeliverySpecification)
        {
            var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            return Task.Run(async () => await cartManager.ECL_CreateMergedCart(cartId, affiliationId, calculationModes.ToString(), cLines, UpdateDeliverySpecification)).Result;
        }
        [Trace]
        private CartResponse ECL_AddCartLines(string cartId, long cartVersion, Microsoft.Dynamics.Commerce.Runtime.DataModel.CalculationModes calcModes, List<CartLine> cLines)
        {
            var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            return Task.Run(async () => await cartManager.ECL_AddCartLines(cartId, cLines, calcModes.ToString(), cartVersion)).Result;
        }
        [Trace]
        private CartResponse ECL_CreateOrUpdateCart(Cart cartObject, Microsoft.Dynamics.Commerce.Runtime.DataModel.CalculationModes calcModes)
        {
            var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            return Task.Run(async () => await cartManager.ECL_CreateOrUpdateCart(cartObject, calcModes.ToString())).Result;
        }
        [Trace]
        private CartResponse ECL_UpdateDeliverySpecification(string cartId, DeliverySpecification delSpec)
        {
            var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            return Task.Run(async () => await cartManager.ECL_UpdateDeliverySpecification(cartId, delSpec)).Result;
        }
        [Trace]
        private CartResponse ECL_GetCart(string cartId)
        {
            var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            return Task.Run(async () => await cartManager.ECL_GetCart(cartId)).Result;
        }
        [Trace]
        private CartResponse ECL_VoidCartLines(string cartId, IEnumerable<CartLine> lines, Microsoft.Dynamics.Commerce.Runtime.DataModel.CalculationModes calcModes)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            //return Task.Run(async () => await cartManager.ECL_VoidCartLines(cartId, lines, calcModes.ToString())).Result;
        }
        [Trace]
        private CartResponse ECL_RemoveCartLines(string cartId, IEnumerable<string> lineIds, Microsoft.Dynamics.Commerce.Runtime.DataModel.CalculationModes calcModes)
        {
            var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            return Task.Run(async () => await cartManager.ECL_RemoveCartLines(cartId, lineIds, calcModes.ToString())).Result;
        }
        [Trace]
        private CartResponse ECL_UpdateCartLines(string cartId, IEnumerable<CartLine> lines, Microsoft.Dynamics.Commerce.Runtime.DataModel.CalculationModes calcModes)
        {
            var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            return Task.Run(async () => await cartManager.ECL_UpdateCartLines(cartId, lines, calcModes.ToString())).Result;
        }
        [Trace]
        private CartResponse ECL_ValidateCouponsToCart(string cartId, IEnumerable<string> couponCodes, bool isLegacyDiscountCode)
        {
            var clCartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            return Task.Run(async () => await clCartManager.ECL_ValidateCouponsToCart(cartId, couponCodes, isLegacyDiscountCode)).Result;
        }
        [Trace]
        private Cart ECL_AddCouponsToCart(string cartId, IEnumerable<string> couponCodes, bool isLegacyDiscountCode)
        {
            var cartManager = RPFactory.GetManager<Microsoft.Dynamics.Commerce.RetailProxy.ICartManager>();
            return Task.Run(async () => await cartManager.AddCoupons(cartId, couponCodes, isLegacyDiscountCode)).Result;
        }
        [Trace]
        private Cart ECL_RemoveCouponsFromCart(string cartId, IEnumerable<string> couponCodes)
        {
            var cartManager = RPFactory.GetManager<Microsoft.Dynamics.Commerce.RetailProxy.ICartManager>();
            return Task.Run(async () => await cartManager.RemoveCoupons(cartId, couponCodes)).Result;
        }
        [Trace]
        private CartResponse ECL_AddDiscountCodesToCart(string cartId, IEnumerable<string> dicountCodes)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            //return Task.Run(async () => await cartManager.ECL_AddDiscountCodesToCart(cartId, dicountCodes)).Result;
        }
        [Trace]
        private CartResponse ECL_RemoveDiscountCodesFromCart(string cartId, IEnumerable<string> dicountCodes)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            //return Task.Run(async () => await cartManager.ECL_RemoveDiscountCodesFromCart(cartId, dicountCodes)).Result;
        }
        [Trace]
        private CartResponse ECL_AddTenderLine(string cartId, long cartVersion, CartTenderLine tenderLine)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            //return Task.Run(async () => await cartManager.ECL_AddTenderLine(cartId, tenderLine, cartVersion)).Result;
        }
        [Trace]
        private CartResponse ECL_AddPreprocessedTenderLine(string cartId, long cartVersion, TenderLine tenderLine)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            //return Task.Run(async () => await cartManager.ECL_AddPreprocessedTenderLine(cartId, tenderLine, cartVersion)).Result;
        }
        [Trace]
        private CartResponse ECL_Checkout(string cartId, string receiptEmail, string receiptNumberSequence, long cartVersion)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            //return Task.Run(async () => await cartManager.ECL_Checkout(cartId, receiptEmail, receiptNumberSequence, cartVersion)).Result;
        }
        [Trace]
        private CartResponse ECL_RecalculateCustomerOrder(string cartId)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            //return Task.Run(async () => await cartManager.ECL_RecalculateCustomerOrder(cartId)).Result;
        }
        [Trace]
        private CartResponse ECL_DeleteCarts(List<string> cartIds)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            //return Task.Run(async () => await cartManager.ECL_DeleteCarts(cartIds)).Result;
        }
        #endregion
    }
}
