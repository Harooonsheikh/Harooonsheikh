using EdgeAXCommerceLink.Commerce.RetailProxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using System.Linq;

namespace VSI.EDGEAXConnector.CRTAX7.Controllers
{
    public class PriceController : BaseController, IPriceController
    {
        public List<ErpProductPrice> GetActiveProductPrice(long channelId, long catalogId, List<long> productIds, DateTime date, string customerAccountNumber)
        {
            // Query Result Settings
            QueryResultSettings objQueryResultSettings = new QueryResultSettings();
            objQueryResultSettings.Paging = new PagingInfo();
            objQueryResultSettings.Paging.Skip = 0;
            objQueryResultSettings.Paging.Top = ConfigurationHelper.GetSetting(PRODUCT.Retail_Server_Paging).LongValue();

            List<ErpProductPrice> lstErpPrice = new List<ErpProductPrice>();

            PagedResult<ProductPrice> productPricePR;
            int productIndex = 0;

            // Get the Catalogs
            var pcManager = RPFactory.GetManager<IProductCatalogManager>();
            PagedResult<ProductCatalog> prCatalogs = pcManager.GetCatalogs(baseChannelId, true, objQueryResultSettings).Result;

            if (prCatalogs.Count() == 0)
            {
                throw new NullReferenceException("Exception in GetActiveProductPrice,Found 0 Catalog");
            }
            else
            {
                catalogId= prCatalogs.Select(c => c.RecordId).FirstOrDefault();
            }
            

            while (productIndex < productIds.Count)
            {
                int productCounter = 0;
                List<long> productsForPrices = new List<long>();

                // Update the upper bound for the loop
                int upperBound = 0;
                upperBound = (productIndex + (int)objQueryResultSettings.Paging.Top - 1) < (productIds.Count - 1) ? (productIndex + (int)objQueryResultSettings.Paging.Top - 1) : (productIds.Count - 1);

                // Add the products in list for paging
                for (productCounter = productIndex; productCounter <= upperBound; productCounter++)
                {
                    productsForPrices.Add(productIds[productCounter]);
                }

                // Update the counter for paging
                productIndex = productCounter;

                // Send AX Call
                productPricePR = AsyncGetActiveProductPrice(baseChannelId, catalogId, productsForPrices, objQueryResultSettings).Result;

                if(productPricePR!=null)
                {
                    foreach (var erpProdPrice in productPricePR.Results)
                    {
                        ErpProductPrice erpProductPrice = new ErpProductPrice();

                        erpProductPrice.AdjustedPrice = Convert.ToDecimal(erpProdPrice.AdjustedPrice);
                        erpProductPrice.BasePrice = Convert.ToDecimal(erpProdPrice.BasePrice);
                        erpProductPrice.TradeAgreementPrice = Convert.ToDecimal(erpProdPrice.TradeAgreementPrice);
                        erpProductPrice.ProductId = (long)erpProdPrice.ProductId;
                        erpProductPrice.ItemId = erpProdPrice.ItemId;

                        lstErpPrice.Add(erpProductPrice);
                    }

                }
                else
                {

                        throw new NullReferenceException("Found NULL Price, Exception CRT Price");
                }
                
               
            }

            return lstErpPrice;
        }

        private async Task<PagedResult<ProductPrice>> AsyncGetActiveProductPrice(long channelId, long catalogId, List<long> productIds, QueryResultSettings objQueryResultSettings)
        {
            try
            {
                // Create Product Manager Object
                var prManager = RPFactory.GetManager<IProductManager>();
                PagedResult<ProductPrice> productPrice;
                ProjectionDomain pd = new ProjectionDomain();
                pd.CatalogId = catalogId;
                pd.ChannelId = baseChannelId;



                return productPrice = await prManager.GetActivePrices(pd, productIds, DateTime.Now, string.Empty, null, objQueryResultSettings); 
            }
            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);
                throw exp;
            }
        }
    }
}
