using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using System.Linq;
using VSI.EDGEAXConnector.Common;
using System.Reflection;
using VSI.EDGEAXConnector.Enums.Enums;
using Microsoft.Dynamics.Commerce.RetailProxy;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class PriceController : BaseController, IPriceController
    {
        public PriceController(string storeKey) : base(storeKey)
        {

        }
        public List<ErpProductPrice> GetActiveProductPrice(long channelId, long catalogId, List<long> productIds, DateTime date, string customerAccountNumber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            // Query Result Settings
            QueryResultSettings objQueryResultSettings = new QueryResultSettings();
            objQueryResultSettings.Paging = new PagingInfo();
            objQueryResultSettings.Paging.Skip = 0;
            objQueryResultSettings.Paging.Top = configurationHelper.GetSetting(PRODUCT.Retail_Server_Paging).LongValue();

            List<ErpProductPrice> lstErpPrice = new List<ErpProductPrice>();
            PagedResult<ProductPrice> productPricePR;
            int productIndex = 0;
            
            // Get the Catalogs
            var prCatalogs = GetCatalogs(objQueryResultSettings);

            if (prCatalogs.Count() == 0)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name,
                    CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40208));
                throw new NullReferenceException(message);
            }
            else
            {
                catalogId = prCatalogs.Select(c => c.RecordId).FirstOrDefault();
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
                //++RSCall
                productPricePR = AsyncGetActiveProductPrice(baseChannelId, catalogId, productsForPrices, objQueryResultSettings).Result;

                if (productPricePR != null)
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
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name,
                        CommerceLinkLoggerMessages.VSICL40205);
                    throw new NullReferenceException(message);
                }
            }
            return lstErpPrice;
        }
        private async Task<PagedResult<ProductPrice>> AsyncGetActiveProductPrice(long channelId, long catalogId, List<long> productIds, QueryResultSettings objQueryResultSettings)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            try
            {
                PagedResult<ProductPrice> productPrice;
                ProjectionDomain pd = new ProjectionDomain();
                pd.CatalogId = catalogId;
                pd.ChannelId = baseChannelId;
                //++RSCall
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
                return ECL_GetActivePrices(productIds, objQueryResultSettings, pd);
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
        }
        #region RetailServer API
        private PagedResult<ProductPrice> ECL_GetActivePrices(List<long> productIds, QueryResultSettings objQueryResultSettings, ProjectionDomain pd)
        {
            var prManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager>();
            return Task.Run(async () => await prManager.ECL_GetActivePrices(pd, productIds, DateTime.UtcNow, string.Empty, null,
                objQueryResultSettings)).Result;
        }
        private PagedResult<ProductCatalog> GetCatalogs(QueryResultSettings objQueryResultSettings)
        {
            var pcManager = RPFactory.GetManager<IProductCatalogManager>();
            return Task.Run(async () => await pcManager.GetCatalogs(baseChannelId, true, objQueryResultSettings)).Result;
        } 
        #endregion
    }
}
