//using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class DiscountWithAffiliationController : ProductController, IDiscountWithAffiliationController
    {
        public DiscountWithAffiliationController(string storeKey) : base(storeKey)
        {

        }

        public ErpGetCustomIndependentProductPriceDiscountResponse GetIndependentProductPriceDiscount(List<long> productIds, string customerAccountNumber, List<ErpAffiliationLoyaltyTier> affiliations)
        {
            ErpGetCustomIndependentProductPriceDiscountResponse response = new ErpGetCustomIndependentProductPriceDiscountResponse(false, "", null);

            var crtDiscountManager = new DiscountWithAffiliationCRTManager();
            response = crtDiscountManager.GetIndependentProductPriceDiscount(productIds.Distinct().ToList(), customerAccountNumber, affiliations, currentStore.StoreKey);

            int priceRounding = -1;
            priceRounding = Convert.ToInt32(configurationHelper.GetSetting(APPLICATION.ERP_AX_PriceRounding));

            if (priceRounding > -1)
            {
                if (response.ProductPrices != null)
                {
                    foreach (var pPrice in response.ProductPrices)
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

        public List<ErpProductDiscountWithAffiliation> GetDiscountsWithAffiliation()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            List<ErpProductDiscountWithAffiliation> erpProductDiscountsList = new List<ErpProductDiscountWithAffiliation>();
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
                }
                foreach (var prod in erpProducts)
                {
                    productPrices = productDiscountedPrices.Where(pr => pr.ProductId == prod.RecordId).ToList();

                    foreach (ErpProductPrice productPrice in productPrices)
                    {
                        if (productPrice != null && ((productPrice.DiscountAmount > 0 || productPrice.OfferPrice > 0 || productPrice.DiscountPercentage > 0) && prod.IsMasterProduct == false))
                        {
                            ErpProductDiscountWithAffiliation erpProductDiscount = new ErpProductDiscountWithAffiliation();

                            string defaultCurrencyCode = configurationHelper.GetSetting(APPLICATION.Default_Currency_Code);

                            erpProductDiscount.CurrencyCode = string.IsNullOrWhiteSpace(productPrice.CurrencyCode) ? defaultCurrencyCode : productPrice.CurrencyCode;

                            erpProductDiscount.DiscPct = productPrice.DiscountPercentage;
                            erpProductDiscount.DiscAmount = Math.Round(productPrice.DiscountAmount, 2);
                            erpProductDiscount.DiscPrice = productPrice.OfferPrice;

                            if (productPrice.DiscountMethod == ErpRetailDiscountOfferLineDiscMethodBase.PercentOff)
                            {
                                //No need to calculate prices becuase its not channel specific module
                                //erpProductDiscount.OfferPrice = productPrice.AdjustedPrice - (productPrice.AdjustedPrice * (productPrice.DiscountPercentage / 100));
                                erpProductDiscount.OfferPrice = erpProductDiscount.DiscPct;
                            }
                            else if (productPrice.DiscountMethod == ErpRetailDiscountOfferLineDiscMethodBase.AmountOff)
                            {
                                //No need to calculate prices becuase its not channel specific module
                                //erpProductDiscount.OfferPrice = productPrice.AdjustedPrice - productPrice.DiscountAmount;
                                erpProductDiscount.OfferPrice = erpProductDiscount.DiscAmount;
                            }
                            else if (productPrice.DiscountMethod == ErpRetailDiscountOfferLineDiscMethodBase.Price)
                            {
                                //No need to calculate prices becuase its not channel specific module
                                //erpProductDiscount.OfferPrice = productPrice.OfferPrice;
                                erpProductDiscount.OfferPrice = erpProductDiscount.DiscPrice;
                            }

                            erpProductDiscount.ValidFrom = productPrice.ValidFrom != DateTime.MinValue ? productPrice.ValidFrom : DateTime.UtcNow.Date;
                            erpProductDiscount.ValidTo = productPrice.ValidTo != DateTime.MinValue ? productPrice.ValidTo : new DateTime(2154, 12, 31);
                            erpProductDiscount.Quantity = 1;
                            erpProductDiscount.SKU = prod.SKU;
                            erpProductDiscount.ItemId = productPrice.ItemId;
                            erpProductDiscount.ColorId = prod.ColorId;
                            erpProductDiscount.SizeId = prod.SizeId;
                            erpProductDiscount.StyleId = prod.StyleId;
                            erpProductDiscount.AffiliationId = (long)productPrice.AffiliationId;
                            erpProductDiscount.AffiliationName = productPrice.AffiliationName;
                            erpProductDiscount.PeriodicDiscountType = productPrice.PeriodicDiscountType;
                            erpProductDiscount.DiscountType = productPrice.DiscountType;
                            erpProductDiscount.LineType = productPrice.LineType;
                            erpProductDiscount.OfferId = productPrice.OfferId;
                            erpProductDiscount.OfferName = productPrice.OfferName;
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

        public List<ErpProductDiscountWithAffiliation> GetDiscountsWithAffiliationByProductIds(string itemId, string variant, string affiliationId, string currency)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            List<ErpProductDiscountWithAffiliation> erpProductDiscountsList = new List<ErpProductDiscountWithAffiliation>();

            try
            {
                var integrationManager = new IntegrationManager(currentStore.StoreKey);
                var product = integrationManager.GetIntegrationKey(Enums.Entities.Product, itemId + "_" + variant);

                if (product == null)
                {
                    throw new CommerceLinkError("Unable to find product with ComKey = " + itemId + "_" + variant);
                }

                var productId = long.Parse(product.ErpKey);

                var productDiscountedPrices = this.ProcessProductDiscountByProductIds(productId, affiliationId, currency).ToList();

                foreach (var productDiscountedPrice in productDiscountedPrices)
                {
                    if (productDiscountedPrice.DiscAmount > 0 || productDiscountedPrice.DiscPct > 0 || productDiscountedPrice.OfferPrice > 0)
                    {
                        var erpProductDiscountWithAffiliation = new ErpProductDiscountWithAffiliation()
                        {
                            AffiliationId = (long)productDiscountedPrice.AffiliationId,
                            AffiliationName = productDiscountedPrice.AffiliationName,
                            OfferId = productDiscountedPrice.OfferId,
                            OfferName = productDiscountedPrice.Name,
                            DiscAmount = Math.Round(productDiscountedPrice.DiscAmount, 2),
                            DiscPct = Math.Round(productDiscountedPrice.DiscPct, 2),
                            OfferPrice = Math.Round(productDiscountedPrice.OfferPrice, 2)
                        };

                        erpProductDiscountWithAffiliation.ValidFrom = productDiscountedPrice.ValidFrom.DateTime != DateTime.MinValue ? productDiscountedPrice.ValidFrom.DateTime : DateTime.UtcNow.Date;
                        erpProductDiscountWithAffiliation.ValidTo = productDiscountedPrice.ValidTo.DateTime != DateTime.MinValue ? productDiscountedPrice.ValidTo.DateTime : new DateTime(2154, 12, 31);
                        erpProductDiscountWithAffiliation.Quantity = 1;

                        erpProductDiscountsList.Add(erpProductDiscountWithAffiliation);
                    }

                }

            }
            catch (Exception exp)
            {
                throw;
            }

            // REMOVE THIS LINE // CustomLogger.LogDebugInfo("Exit from GetDiscount()");
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpProductDiscountsList;
        }

        private List<ErpProductPrice> ProcessProductDiscount(List<ErpProduct> erpProducts)
        {
            // REMOVE THIS LINE // CustomLogger.LogDebugInfo(string.Format("Entered in ProcessProductDiscount()"));
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpProductPrice> productPriceList = new List<ErpProductPrice>();
            try
            {
                List<long> productIds = null;
                if (erpProducts != null && erpProducts.Count > 0)
                    productIds = erpProducts.Select(prod => prod.RecordId).Distinct().ToList();

                var crtDiscountManager = new DiscountWithAffiliationCRTManager();
                productPriceList = crtDiscountManager.GetIndependentProductPriceDiscount(erpProducts, configurationHelper.GetSetting(APPLICATION.Channel_Id).LongValue(), productIds, string.Empty, currentStore.StoreKey);
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

        private List<ErpRetailDiscountWithAffiliationItem> ProcessProductDiscountByProductIds(long productId, string affiliationName, string currency)
        {
            var productPriceList = new List<ErpRetailDiscountWithAffiliationItem>();
            try
            {
                var crtDiscountManager = new DiscountWithAffiliationCRTManager();
                productPriceList = crtDiscountManager.GetDiscountWithAffiliationsByProductIds(productId, affiliationName, currency, configurationHelper.GetSetting(APPLICATION.Channel_Id).LongValue(), currentStore.StoreKey);
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
