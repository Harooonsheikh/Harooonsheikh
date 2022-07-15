//using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class DiscountController : ProductController, IDiscountController
    {
        public DiscountController(string storeKey) : base(storeKey)
        {

        }
   
        public ErpGetCustomIndependentProductPriceDiscountResponse GetIndependentProductPriceDiscount(List<long> productIds, string customerAccountNumber, List<ErpAffiliationLoyaltyTier> affiliations)
        {
            ErpGetCustomIndependentProductPriceDiscountResponse response = new ErpGetCustomIndependentProductPriceDiscountResponse(false, "", null);

            var crtDiscountManager = new DiscountCRTManager();
            response = crtDiscountManager.GetIndependentProductPriceDiscount(productIds.Distinct().ToList(), customerAccountNumber, affiliations, currentStore.StoreKey);

            int priceRounding = -1;
            priceRounding = Convert.ToInt32(configurationHelper.GetSetting(APPLICATION.ERP_AX_PriceRounding));
            
            if (priceRounding > -1)
            {
                if(response.ProductPrices != null)
                {
                    foreach(var pPrice in response.ProductPrices)
                    {
                        pPrice.BasePrice = Math.Round(pPrice.BasePrice, priceRounding);
                        pPrice.TradeAgreementPrice = Math.Round(pPrice.TradeAgreementPrice, priceRounding);
                        pPrice.AdjustedPrice = Math.Round(pPrice.AdjustedPrice, priceRounding);
                        pPrice.CustomerContextualPrice = Math.Round(pPrice.CustomerContextualPrice, priceRounding);
                        pPrice.DiscountAmount = Math.Round(pPrice.DiscountAmount, priceRounding);
                    }
                }
            }

            return response;
        }

        public List<ErpProductDiscount> GetDiscounts()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            List<ErpProductDiscount> erpProductDiscountsList = new List<ErpProductDiscount>();
            try
            {
                // VW Specific Code
                List<ErpProductPrice> productDiscountedPrices = new List<ErpProductPrice>();
                bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();

                List<ErpProductPrice> productPrices;
                List<ErpProduct> erpProducts = new List<ErpProduct>();

                // REMOVE THIS LINE // CustomLogger.LogDebugInfo(string.Format("GetDiscounts() - Enter in GetCatalogProducts()"));
                var masterProducts = this.GetCatalogProducts(false, null, false);
                erpProducts = this.ProcessProducts(masterProducts);
                if (erpProducts != null && erpProducts.Count > 0)
                {
                    productDiscountedPrices = this.ProcessProductDiscount(erpProducts);
                    productDiscountedPrices = productDiscountedPrices.Where(x => x.OfferId != null).ToList();
                }
                foreach (var prod in erpProducts)
                {
                    productPrices = productDiscountedPrices.Where(pr => pr.ProductId == prod.RecordId).ToList();

                    foreach (ErpProductPrice productPrice in productPrices)
                    {
                        if (productPrice != null && ((productPrice.DiscountAmount > 0 || productPrice.OfferPrice > 0 || productPrice.DiscountPercentage > 0) && prod.IsMasterProduct == false))
                        {

                            ErpProductDiscount erpProductDiscount = new ErpProductDiscount();

                            string defaultCurrencyCode = configurationHelper.GetSetting(APPLICATION.Default_Currency_Code);

                            erpProductDiscount.CurrencyCode = string.IsNullOrWhiteSpace(productPrice.CurrencyCode) ? defaultCurrencyCode : productPrice.CurrencyCode;

                            if (productPrice.DiscountMethod == ErpRetailDiscountOfferLineDiscMethodBase.PercentOff)
                            {
                                erpProductDiscount.OfferPrice = productPrice.AdjustedPrice - (productPrice.AdjustedPrice * (productPrice.DiscountPercentage / 100));
                            }
                            else if (productPrice.DiscountMethod == ErpRetailDiscountOfferLineDiscMethodBase.AmountOff)
                            {
                                erpProductDiscount.OfferPrice = productPrice.AdjustedPrice - productPrice.DiscountAmount;
                            }
                            else if (productPrice.DiscountMethod == ErpRetailDiscountOfferLineDiscMethodBase.Price)
                            {
                                erpProductDiscount.OfferPrice = erpProductDiscount.DiscPrice = productPrice.OfferPrice;
                            }

                            erpProductDiscount.ValidFrom = productPrice.ValidFrom != DateTime.MinValue ? productPrice.ValidFrom : DateTime.UtcNow.Date;
                            erpProductDiscount.ValidTo = productPrice.ValidTo != DateTime.MinValue ? productPrice.ValidTo : new DateTime(2154, 12, 31);
                            erpProductDiscount.DiscAmount = Math.Round(productPrice.DiscountAmount, 2);
                            erpProductDiscount.Quantity = 1;
                            erpProductDiscount.SKU = prod.SKU;
                            erpProductDiscount.ItemId = productPrice.ItemId;
                            erpProductDiscount.ColorId = prod.ColorId;
                            erpProductDiscount.SizeId = prod.SizeId;
                            erpProductDiscount.StyleId = prod.StyleId;

                            erpProductDiscount.OfferId = productPrice.OfferId;
                            erpProductDiscount.DiscountName = productPrice.OfferName;
                            erpProductDiscount.DiscPct = productPrice.DiscountPercentage;
                            erpProductDiscount.ManualDiscountTypeValue = productPrice.ManualDiscountTypeValue;
                            erpProductDiscount.PeriodicDiscountTypeValue = productPrice.PeriodicDiscountTypeValue;
                            erpProductDiscount.CustomerDiscountTypeValue = productPrice.CustomerDiscountTypeValue;
                            erpProductDiscount.DiscountLineTypeValue = productPrice.DiscountLineTypeValue;
                            erpProductDiscount.DiscountMethod = productPrice.DiscountMethod;

                            //NS: TMV Customization for Ecom
                            erpProductDiscount.TMV_ProductType = prod.IsMasterProduct ?
                                configurationHelper.GetSetting(PRODUCT.MasterProductTypeEcomName) :
                                configurationHelper.GetSetting(PRODUCT.VariantProductTypeEcomName);

                            erpProductDiscountsList.Add(erpProductDiscount);
                        } 
                    }
                }
            }
            catch (Exception exp)
            {
                throw;
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpProductDiscountsList;
        }

        private List<ErpProductPrice> ProcessProductDiscount(List<ErpProduct> erpProducts)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpProductPrice> productPriceList = new List<ErpProductPrice>();
            try
            {
                List<long> productIds = null;
                if (erpProducts != null && erpProducts.Count > 0)
                    productIds = erpProducts.Select(prod => prod.RecordId).Distinct().ToList();

                var crtDiscountManager = new DiscountCRTManager();
                productPriceList = crtDiscountManager.GetIndependentProductPriceDiscount(erpProducts, configurationHelper.GetSetting(APPLICATION.Channel_Id).LongValue(), productIds, string.Empty,currentStore.StoreKey);
            }

            catch (Exception ex)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                throw ex;
            }

            // REMOVER THIS LINE // CustomLogger.LogDebugInfo(string.Format("Exit from ProcessProductDiscount()"));
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return productPriceList;
        }
        
    }
}



