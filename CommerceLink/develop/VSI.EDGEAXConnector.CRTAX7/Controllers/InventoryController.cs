using EdgeAXCommerceLink.Commerce.RetailProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.CRTAX7.Controllers
{
    public class InventoryController : BaseController, IInventoryController
    {
        public List<ErpProduct> GetProductAvailabilities(List<ErpProduct> erpProducts, List<long> productIds, long channelId)
        {

            // Query Result Settings
            QueryResultSettings objQueryResultSettings = new QueryResultSettings();
            objQueryResultSettings.Paging = new PagingInfo();
            objQueryResultSettings.Paging.Skip = 0;
            objQueryResultSettings.Paging.Top = ConfigurationHelper.GetSetting(PRODUCT.Retail_Server_Paging).LongValue();

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


                        throw new NullReferenceException("Found 0 Inventory, Exception CRT Inventory");

                    }

                    if (productInventory == null)
                    {


                        throw new NullReferenceException("Found NULL Inventory, Exception CRT Inventory");

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

                            inventoryTrace.Append(string.Format("New Available Physical Inventory :{0} for Product : {1}", prod.AvailableQuantity, prod.ItemId));

                            prod.AvailableQuantity += Convert.ToDecimal(productQuantity.AvailableQuantity);
                        }

                    }

                    DateTime originalDate = DateTime.SpecifyKind(DateTime.Now.AddHours(24), DateTimeKind.Utc);
                    String dateString = originalDate.ToString("o");
                    prod.AllocationTimeStamp = dateString;
                }
            }

            CustomLogger.LogDebugInfo(inventoryTrace.ToString());

            return erpProducts;
        }


        private async Task<PagedResult<ProductAvailableQuantity>> AsyncGetProductAvailabilities(List<long> productIds, long channelId, QueryResultSettings objQueryResultSettings)
        {

            try

            {
                // Create Product Manager Object
                var prManager = RPFactory.GetManager<IProductManager>();
                PagedResult<ProductAvailableQuantity> productInventory;

                return productInventory = await prManager.GetProductAvailabilities(productIds, baseChannelId, objQueryResultSettings);
            }

            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);
                throw exp;
            }
        }
    }
}
