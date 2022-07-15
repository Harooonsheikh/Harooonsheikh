using Microsoft.Dynamics.Commerce.RetailProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NewRelic.Api.Agent;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class InventoryController : BaseController, IInventoryController
    {
        public InventoryController(string storeKey) : base(storeKey)
        {

        }
        public List<ErpProduct> GetProductAvailabilities(List<ErpProduct> erpProducts, List<long> productIds, long channelId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            // Query Result Settings
            QueryResultSettings objQueryResultSettings = new QueryResultSettings();
            objQueryResultSettings.Paging = new PagingInfo();
            objQueryResultSettings.Paging.Skip = 0;
            objQueryResultSettings.Paging.Top = configurationHelper.GetSetting(PRODUCT.Retail_Server_Paging).LongValue();

            PagedResult<ProductAvailableQuantity> productInventoryPR = new PagedResult<ProductAvailableQuantity>();
            ProductAvailableQuantity productQuantity;
            List<ProductAvailableQuantity> productInventoryList = new List<ProductAvailableQuantity>();

            StringBuilder inventoryTrace = new StringBuilder();
            int productIndex = 0;
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
                PagedResult<ProductAvailableQuantity> productInventory;
                productInventory = AsyncGetProductAvailabilities(productsForPrices, baseChannelId, objQueryResultSettings).Result;
                if (productInventory.Count() > 0)
                {
                    foreach (var erpProdPrice in productInventory.Results)
                    {
                        productInventoryList.Add(erpProdPrice);
                    }
                }
                else
                {
                    if (productInventory.Count() == 0)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name,
                                CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40203));
                        throw new NullReferenceException(message);
                    }
                    if (productInventory == null)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name,
                                CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40203));
                        throw new NullReferenceException(message);
                    }
                }
            }
            productInventoryPR.Results = productInventoryList;

            if (productInventoryPR != null && productInventoryPR.Results != null && productInventoryPR.Results.Count() > 0)
            {
                foreach (ErpProduct prod in erpProducts)
                {
                    productQuantity = productInventoryPR.Results.FirstOrDefault(pq => pq.ProductId == prod.RecordId);
                    if (productQuantity != null)
                    {
                        if (productQuantity.AvailableQuantity < 0)
                        {
                            prod.AvailableQuantity = 0;
                        }
                        else
                        {
                            
                            string message = string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10214), prod.AvailableQuantity, prod.ItemId);
                            inventoryTrace.Append(message);
                            prod.AvailableQuantity += Convert.ToDecimal(productQuantity.AvailableQuantity);
                        }
                    }
                    // Task 2162: No need to add 24 hours to Allocation Time Stamp
                    // DateTime originalDate = DateTime.SpecifyKind(DateTime.UtcNow.AddHours(24), DateTimeKind.Utc);
                    String dateString = DateTime.UtcNow.ToString("o");
                    prod.AllocationTimeStamp = dateString;
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpProducts;
        }
        private async Task<PagedResult<ProductAvailableQuantity>> AsyncGetProductAvailabilities(List<long> productIds, long channelId, QueryResultSettings objQueryResultSettings)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            try
            {
                return ECL_GetProductAvailabilities(productIds, objQueryResultSettings);
            }
            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);
                throw exp;
            }
        }

        #region RetailServer API
        [Trace]
        private PagedResult<ProductAvailableQuantity> ECL_GetProductAvailabilities(List<long> productIds, QueryResultSettings objQueryResultSettings)
        {
            var prManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager>();
            return Task.Run(async () =>
                await prManager.ECL_GetProductAvailabilities(productIds, baseChannelId, objQueryResultSettings)).Result;
        } 
        #endregion
    }
}
