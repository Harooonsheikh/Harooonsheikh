using AutoMapper;
using EdgeAXCommerceLink.Commerce.RetailProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.CRTAX7.Controllers
{
    public class DiscountController : BaseController, IDiscountController
    {
        StringBuilder discountTrace = new StringBuilder();

        public ErpGetCustomIndependentProductPriceDiscountResponse GetIndependentProductPriceDiscount(List<long> productIds)
        {
            throw new NotImplementedException();
        }

        public List<ErpProductPrice> GetIndependentProductPriceDiscount(List<ErpProduct> products, long channelId, List<long> productIds, string customerAccountNumber)
        {
            try
            {
                List<ErpProductPrice> lstErpProductPrice = new List<ErpProductPrice>();
                PagedResult<ProductPrice> productPrices;

                // Setup QueryResultSettings
                QueryResultSettings objQueryResultSettings = new QueryResultSettings();
                objQueryResultSettings.Paging = new PagingInfo();
                objQueryResultSettings.Paging.Skip = 0;
                objQueryResultSettings.Paging.Top = ConfigurationHelper.GetSetting(PRODUCT.Retail_Server_Paging).LongValue();

                // Get the Catalogs
                var pcManager = RPFactory.GetManager<IProductCatalogManager>();
                PagedResult<ProductCatalog> prCatalogs = pcManager.GetCatalogs(baseChannelId, true, objQueryResultSettings).Result;

                if(prCatalogs.Count()==0)
                {
                    throw new NullReferenceException("Exception in GetIndependentProductPriceDiscount,Found 0 Catalog");
                }

                // Initialize and setup ProjectionDomain
                ProjectionDomain pdProjectionDomain = new ProjectionDomain();
                pdProjectionDomain.ChannelId = baseChannelId;
                pdProjectionDomain.CatalogId = prCatalogs.Select(c => c.RecordId).FirstOrDefault();


                // Setup the Manager
                var managerProductManager = RPFactory.GetManager<IProductManager>();

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

                    // AX Call
                    productPrices = managerProductManager.GetCustomIndependentProductPriceDiscount(pdProjectionDomain, productsForDiscounts, customerAccountNumber, objQueryResultSettings).Result;

                    if (productPrices.Count() > 0)
                    {
                        foreach (ProductPrice pp in productPrices)
                        {                         
                            discountTrace.Append(string.Format("Get the Discount Amount: {0} for Product :{1}", pp.DiscountAmount, pp.ItemId));

                            lstErpProductPrice.Add(Mapper.Map<ErpProductPrice>(pp));
                        }
                    }

                    else
                    {

                        if (productPrices.Count() == 0)
                        {


                            throw new NullReferenceException("Found 0 Discounts, Exception CRT Discounts");

                        }

                        if (productPrices == null)
                        {


                            throw new NullReferenceException("Found NULL Discounts, Exception CRT Discounts");

                        }

                    }
                    
                    CustomLogger.LogDebugInfo(discountTrace);


                }

                return lstErpProductPrice;
            }
            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);
                throw exp;
            }
        }
    }
}
