using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using Microsoft.Dynamics.Commerce.RetailProxy;
using NewRelic.Api.Agent;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class DiscountController : BaseController, IDiscountController
    {
        StringBuilder discountTrace = new StringBuilder();

        public DiscountController(string storeKey) : base(storeKey)
        {

        }
        public ErpGetCustomIndependentProductPriceDiscountResponse GetIndependentProductPriceDiscount(List<long> productIds, string customerAccountNumber, List<ErpAffiliationLoyaltyTier> affiliations)
        {
            List<ErpProductPrice> lstErpProductPrice = new List<ErpProductPrice>();
            ErpGetCustomIndependentProductPriceDiscountResponse erpResponse = new ErpGetCustomIndependentProductPriceDiscountResponse(false, "", null);

            try
            {
                // Setup QueryResultSettings
                QueryResultSettings objQueryResultSettings = new QueryResultSettings();
                objQueryResultSettings.Paging = new PagingInfo();
                objQueryResultSettings.Paging.Skip = 0;
                objQueryResultSettings.Paging.Top = configurationHelper.GetSetting(PRODUCT.Retail_Server_Paging).LongValue();

                // Get the Catalogs
                PagedResult<ProductCatalog> prCatalogs = GetCatalogs(objQueryResultSettings);

                if (prCatalogs.Count() == 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40004, currentStore, MethodBase.GetCurrentMethod().Name + ", found 0 Catalog.");
                    erpResponse = new ErpGetCustomIndependentProductPriceDiscountResponse(false, message, null);
                }

                // Initialize and setup ProjectionDomain
                ProjectionDomain pdProjectionDomain = new ProjectionDomain();
                pdProjectionDomain.ChannelId = baseChannelId;
                pdProjectionDomain.CatalogId = prCatalogs.Select(c => c.RecordId).FirstOrDefault();

                //Getting Affiliations
                List<AffiliationLoyaltyTier> affiliationLoyaltyTiers = new List<AffiliationLoyaltyTier>();

                affiliationLoyaltyTiers = _mapper.Map<List<ErpAffiliationLoyaltyTier>, List<AffiliationLoyaltyTier>>(affiliations) ?? new List<AffiliationLoyaltyTier>();

                int productIndex = 0;
                while (productIndex < productIds.Count)
                {
                    int productCounter = 0;
                    List<long> productsForDiscounts = new List<long>();
                    // Update the upper bound for the loop
                    int upperBound = 0;
                    upperBound = (productIndex + (int)objQueryResultSettings.Paging.Top - 1) < (productIds.Count - 1) ? (productIndex + (int)objQueryResultSettings.Paging.Top - 1) : (productIds.Count - 1);
                    // Add the products in list for paging
                    for (productCounter = productIndex; productCounter <= upperBound; productCounter++)
                    {
                        productsForDiscounts.Add(productIds[productCounter]);
                    }
                    // Update the counter for paging
                    productIndex = productCounter;

                    EdgeAXCommerceLink.RetailProxy.Extensions.GetCustomIndependentProductPriceDiscountResponse response = new EdgeAXCommerceLink.RetailProxy.Extensions.GetCustomIndependentProductPriceDiscountResponse();
                    response = ECL_GetIndependentProductPriceDiscount(customerAccountNumber, pdProjectionDomain, affiliationLoyaltyTiers, productsForDiscounts);

                    if ((bool)response.Status)
                    {
                        List<ProductPrice> productPrices = JsonConvert.DeserializeObject<List<ProductPrice>>(response.Result);
                        foreach (ProductPrice pro in productPrices)
                        {
                            if (pro.ProductId != 0)
                            {
                                lstErpProductPrice.Add(_mapper.Map<ErpProductPrice>(pro));
                            }
                        }
                        erpResponse = new ErpGetCustomIndependentProductPriceDiscountResponse((bool)response.Status, response.Message, lstErpProductPrice);
                    }
                    else
                    {
                        erpResponse = new ErpGetCustomIndependentProductPriceDiscountResponse((bool)response.Status, response.Message, null);
                    }

                }
                return erpResponse;
            }
            catch (Exception exp)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(exp));
                erpResponse = new ErpGetCustomIndependentProductPriceDiscountResponse(false, message, null);
            }

            return erpResponse;
        }
        public List<ErpProductPrice> GetIndependentProductPriceDiscount(List<ErpProduct> products, long channelId, List<long> productIds, string customerAccountNumber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                List<ErpProductPrice> lstErpProductPrice = new List<ErpProductPrice>();
                PagedResult<ProductPrice> productPrices;

                // Setup QueryResultSettings
                QueryResultSettings objQueryResultSettings = new QueryResultSettings();
                objQueryResultSettings.Paging = new PagingInfo();
                objQueryResultSettings.Paging.Skip = 0;
                objQueryResultSettings.Paging.Top = configurationHelper.GetSetting(PRODUCT.Retail_Server_Paging).LongValue();

                // Get the Catalogs
                PagedResult<ProductCatalog> prCatalogs = GetCatalogs(objQueryResultSettings);

                if (prCatalogs.Count() == 0)
                {
                    string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40202);
                    CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, message);
                    throw new NullReferenceException(message);
                }

                // Initialize and setup ProjectionDomain
                ProjectionDomain pdProjectionDomain = new ProjectionDomain();
                pdProjectionDomain.ChannelId = baseChannelId;
                pdProjectionDomain.CatalogId = prCatalogs.Select(c => c.RecordId).FirstOrDefault();

                int productIndex = 0;
                while (productIndex < productIds.Count)
                {
                    int productCounter = 0;
                    List<long> productsForDiscounts = new List<long>();
                    // Update the upper bound for the loop
                    int upperBound = 0;
                    upperBound = (productIndex + (int)objQueryResultSettings.Paging.Top - 1) < (productIds.Count - 1) ? (productIndex + (int)objQueryResultSettings.Paging.Top - 1) : (productIds.Count - 1);
                    // Add the products in list for paging
                    for (productCounter = productIndex; productCounter <= upperBound; productCounter++)
                    {
                        productsForDiscounts.Add(productIds[productCounter]);
                    }
                    // Update the counter for paging
                    productIndex = productCounter;
                    //++RSCall
                    productPrices = ECL_GetCustomIndependentProductPriceDiscount(customerAccountNumber, objQueryResultSettings, pdProjectionDomain, productsForDiscounts);

                    if (productPrices == null || productPrices.Count() == 0)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40203));
                        throw new NullReferenceException(message);
                    }
                    else //if (productPrices.Count() > 0)
                    {
                        lstErpProductPrice.AddRange(this.ProcessDiscountItems(productPrices, productIds));
                    }
                    // CustomLogger.LogDebugInfo(discountTrace);
                }
                return lstErpProductPrice;
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
        }

        /// <summary>
        /// ProcessDiscountItems takes ProductPrice data and get and process applied discount prices.
        /// </summary>
        /// <param name="productPrices"></param>
        /// <param name="productIds"></param>
        /// <returns></returns>
        private List<ErpProductPrice> ProcessDiscountItems(IEnumerable<ProductPrice> productPrices, List<long> productIds)
        {
            List<ErpProductPrice> erpProductPrices = null;
            var retailDiscountItems = this.GetDiscountItems(productIds);

            if (retailDiscountItems != null && retailDiscountItems.Count() > 0)
            {
                erpProductPrices = new List<ErpProductPrice>();

                foreach (var price in productPrices)
                {
                    erpProductPrices.AddRange(this.ProcessDiscountItem(price, retailDiscountItems));
                }
            }
            else
            {
                erpProductPrices = _mapper.Map<List<ErpProductPrice>>(productPrices) ?? new List<ErpProductPrice>();
            }

            return erpProductPrices;
        }
        /// <summary>
        /// ProcessDiscountItem takes ProductPrice item data and discount items. It process and apply discount prices date range on price item.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="retailDiscountItems"></param>
        /// <returns></returns>
        private List<ErpProductPrice> ProcessDiscountItem(ProductPrice price, IEnumerable<ErpRetailDiscountItem> retailDiscountItems)
        {
            var erpPrice = _mapper.Map<ErpProductPrice>(price);

            List<ErpProductPrice> erpProductPrices = new List<ErpProductPrice>();

            var productDiscountItems = retailDiscountItems.Where(disc => disc.Product == price.ProductId).OrderBy(disc => disc.ValidFrom);

            if (productDiscountItems.Count() > 0)
            {
                //if (productDiscountItems.Count() == 1)
                //{
                //    var discountItem = productDiscountItems.First();
                //    erpPrice.ValidFrom = discountItem.ValidFrom.DateTime;
                //    erpPrice.ValidTo = discountItem.ValidTo.DateTime;
                //}
                //else
                //{
                //    erpPrice.ValidFrom = productDiscountItems.Max(disc => disc.ValidFrom).DateTime;
                //    erpPrice.ValidTo = productDiscountItems.Min(disc => disc.ValidTo).DateTime;
                //}

                foreach (var discountItem in productDiscountItems)
                {
                    ErpProductPrice erpProductPrice = new ErpProductPrice();

                    erpProductPrice = _mapper.Map<ErpProductPrice>(price);

                    erpProductPrice.ValidFrom = productDiscountItems.Max(disc => disc.ValidFrom).DateTime;
                    erpProductPrice.ValidTo = productDiscountItems.Min(disc => disc.ValidTo).DateTime;
                    //erpProductPrice.AffiliationId = (long)discountItem.AffiliationId;
                    //erpProductPrice.AffiliationName = discountItem.AffiliationName;
                    erpProductPrice.DiscountAmount = discountItem.DiscAmount;
                    erpProductPrice.DiscountType = discountItem.DiscountType;
                    erpProductPrice.PeriodicDiscountType = discountItem.PeriodicDiscountType;
                    erpProductPrice.OfferName = discountItem.Name;
                    erpProductPrice.OfferId = discountItem.OfferId;
                    erpProductPrice.DiscountPercentage = discountItem.DiscPct;
                    erpProductPrice.DiscountLineTypeValue = discountItem.DiscountType;
                    erpProductPrice.DiscountMethod = discountItem.DiscountMethod;
                    erpProductPrice.OfferPrice = discountItem.OfferPrice;


                    erpProductPrices.Add(erpProductPrice);
                }

            }

            var productDiscountItemsForOfferInfoMatch = retailDiscountItems.Where(disc => disc.ItemId == price.ItemId);

            if (productDiscountItemsForOfferInfoMatch.Count() > 0)
            {
                var discountItemForOfferInfoMatch = productDiscountItemsForOfferInfoMatch.First();
                erpPrice.OfferId = discountItemForOfferInfoMatch.OfferId;
                erpPrice.OfferName = discountItemForOfferInfoMatch.Name;
                erpPrice.PeriodicDiscountTypeValue = discountItemForOfferInfoMatch.PeriodicDiscountType;
                erpPrice.DiscountPercentage = discountItemForOfferInfoMatch.DiscPct;
                erpPrice.DiscountLineTypeValue = discountItemForOfferInfoMatch.DiscountType;
                erpPrice.DiscountMethod = discountItemForOfferInfoMatch.DiscountMethod;
            }

            return erpProductPrices;
        }
        /// <summary>
        /// GetProductDiscountItems get Retail Discount Items.
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public IEnumerable<ErpRetailDiscountItem> GetDiscountItems(List<long> productIds)
        {
            List<ErpRetailDiscountItem> erpDiscountItems = null;
            //var rsResponse = ECL_GetRetailDiscountItems(productIds);

            //if (rsResponse.Success)
            //{
            //    var discountItems = JsonConvert.DeserializeObject<List<EdgeAXCommerceLink.RetailProxy.Extensions.RetailDiscountItem>>(rsResponse.Result);
            //    erpDiscountItems =
            //        _mapper
            //            .Map<List<EdgeAXCommerceLink.RetailProxy.Extensions.RetailDiscountItem>,
            //                List<ErpRetailDiscountItem>>(discountItems) ?? new List<ErpRetailDiscountItem>();
            //}
            //else
            //{
            //    string message = CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30000, currentStore, MethodBase.GetCurrentMethod().Name, rsResponse.Message);
            //}

            return erpDiscountItems;
        }

        #region RetailServer API Calls
        [Trace]
        private PagedResult<ProductCatalog> GetCatalogs(QueryResultSettings objQueryResultSettings)
        {
            var pcManager = RPFactory.GetManager<IProductCatalogManager>();
            return Task.Run(async () => await pcManager.GetCatalogs(baseChannelId, true, objQueryResultSettings)).Result;
        }
        [Trace]
        private EdgeAXCommerceLink.RetailProxy.Extensions.GetCustomIndependentProductPriceDiscountResponse ECL_GetIndependentProductPriceDiscount(string customerAccountNumber, ProjectionDomain pdProjectionDomain, List<AffiliationLoyaltyTier> affiliationLoyaltyTiers, List<long> productsForDiscounts)
        {
            var managerProductManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager>();
            return Task.Run(async () => await managerProductManager.ECL_GetIndependentProductPriceDiscount(pdProjectionDomain, productsForDiscounts, customerAccountNumber, affiliationLoyaltyTiers)).Result;
        }
        [Trace]
        private PagedResult<ProductPrice> ECL_GetCustomIndependentProductPriceDiscount(string customerAccountNumber, QueryResultSettings objQueryResultSettings, ProjectionDomain pdProjectionDomain, List<long> productsForDiscounts)
        {
            var managerProductManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager>();
            return Task.Run(async () => await managerProductManager.ECL_GetCustomIndependentProductPriceDiscount(pdProjectionDomain, productsForDiscounts, customerAccountNumber, objQueryResultSettings)).Result;
        }
        //[Trace]
        //private EdgeAXCommerceLink.RetailProxy.Extensions.RetailDiscountResponse ECL_GetRetailDiscountItems(List<long> productIds)
        //{
        //    throw new NotImplementedException();
        //    //EdgeAXCommerceLink.RetailProxy.Extensions.IRetailDiscountItemManager retailDiscountItemManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IRetailDiscountItemManager>();
        //    //return Task.Run(async () => await retailDiscountItemManager.ECL_GetRetailDiscountItems(0, baseChannelId, baseCompany, ChannelCurrencyCode, DateTime.UtcNow, productIds)).Result;
        //}
        #endregion
    }
}
