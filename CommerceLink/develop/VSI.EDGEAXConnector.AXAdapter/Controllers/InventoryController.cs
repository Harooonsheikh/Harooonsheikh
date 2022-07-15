//using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using System.Configuration;
using VSI.EDGEAXConnector.Common.Constants;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    /// <summary>
    /// InventoryController
    /// </summary>
    public class InventoryController : ProductController, IInventoryController
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public InventoryController(string storeKey) : base(storeKey)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// GetInventory gets Product Inventory. 
        /// </summary>
        /// <returns></returns>
        public List<ErpProduct> GetInventory()
        {
            CustomLogger.LogDebugInfo(string.Format("Enter in  GetInventory() "), currentStore.StoreId, currentStore.CreatedBy);
            List<ErpProduct> erpProducts = new List<ErpProduct>();
            try
            {
                bool defaultInventoryFlow = true;
                if (defaultInventoryFlow)
                {
                    // false will be pass because we need light products
                    var masterProducts = this.GetCatalogProducts(false, null, false);
                    var allProducts = this.ProcessProducts(masterProducts);

                    if (allProducts == null || allProducts.Count < 0)
                    {
                        CustomLogger.LogWarn("Found no Catalog/CRT Products while fetching inventory", currentStore.StoreId, currentStore.CreatedBy);
                    }
                    else
                    {
                        foreach (var productSet in allProducts)
                        {
                            if (ConfigurationManager.AppSettings["EcomAdapter"] == ApplicationConstant.ECOM_MAGENTO_ADAPTER_ASSEMBLY)
                            {
                                erpProducts.Add(productSet);
                            }
                            else
                            {
                                if (productSet.IsMasterProduct == false)
                                {
                                    base.PaddingZeros(productSet);
                                    erpProducts.Add(productSet);
                                }
                            }
                        }
                    }
                    if (erpProducts != null && erpProducts.Count > 0)
                    {
                        erpProducts = this.ProcessProductInventory(erpProducts);
                        CustomLogger.LogDebugInfo(string.Format("Exit from  ProcessProductInventory()"), currentStore.StoreId, currentStore.CreatedBy);
                    }
                    else
                    {
                        CustomLogger.LogWarn("No inventory found because there were no products", currentStore.StoreId, currentStore.CreatedBy);
                    }

                    //NS: As TeamViewer is not using fully qulified SKU so no need of SKUs generation
                    //if (ConfigurationManager.AppSettings["EcomAdapter"] == ApplicationConstant.ECOM_MAGENTO_ADAPTER_ASSEMBLY)
                    //{
                    //    this.GenerateSKUs(erpProducts);
                    //}
                }
                else
                {
                    erpProducts = ProcessProductInventoryRealTimeService();
                }
            }
            catch (Exception exp)
            {

                throw exp;
            }
            return erpProducts;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// ProcessProductInventory process product inventory from online store and related retail stores.
        /// </summary>
        /// <param name="erpProducts"></param>
        private List<ErpProduct> ProcessProductInventory(List<ErpProduct> erpProducts)
        {
            CustomLogger.LogDebugInfo(string.Format("Enter in  ProcessProductInventory()"), currentStore.StoreId, currentStore.CreatedBy);
            List<long> productIds = null;
            if (erpProducts != null && erpProducts.Count > 0)
                productIds = erpProducts.Select(prod => prod.RecordId).Distinct().ToList();
            bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();
            if (isFlatProductHierarchy)
            {
                erpProducts = erpProducts.Where(d => d.IsMasterProduct == false).ToList();
                CustomLogger.LogDebugInfo(string.Format("Fetched products with IsMasterProduct == false in case of Flat Product Hierarchy"), currentStore.StoreId, currentStore.CreatedBy);
            }
            this.ProcessProductChannelInventory(erpProducts, productIds, 1L); // dummy value, channel should be in CRT layer
            CustomLogger.LogDebugInfo(string.Format("Exit from  ProcessProductChannelInventory() "), currentStore.StoreId, currentStore.CreatedBy);
            productIds.Clear();
            return erpProducts;
        }


        /// <summary>
        /// ProcessProductChannelInventory process product inventory from provided channel.
        /// </summary>
        /// <param name="erpProducts"></param>
        /// <param name="productIds"></param>
        /// <param name="channelId"></param>
        /// <param name="settings"></param>
        private void ProcessProductChannelInventory(List<ErpProduct> erpProducts, List<long> productIds, long channelId)
        {
            var inventoryCRTManager = new InventoryCRTManager();
            erpProducts = inventoryCRTManager.GetProductAvailabilities(erpProducts, productIds, channelId, currentStore.StoreKey);
           
        }

        ///// <summary>
        ///// ProcessProductInventoryRealTimeService process product inventory using realtime service.
        ///// </summary>
        ///// <param name="erpProducts"></param>
        private List<ErpProduct> ProcessProductInventoryRealTimeService()
        {
            List<ErpProduct> csvInventories = new List<ErpProduct>();
            return csvInventories;
        }

        private void GenerateSKUs(List<ErpProduct> erpProducts)
        {
            CustomLogger.LogDebugInfo(string.Format("Enter in GenerateSKUs()"), currentStore.StoreId, currentStore.CreatedBy);
            foreach (ErpProduct prod in erpProducts)
            {
                base.PaddingZeros(prod);
                prod.SKU = prod.EcomProductId = prod.MasterProductNumber + "_" + prod.ColorId + "_" + prod.SizeId + "_" + prod.StyleId + "_" + prod.ItemId;

            }
            CustomLogger.LogDebugInfo(string.Format("Exit from GenerateSKUs()"), currentStore.StoreId, currentStore.CreatedBy);
        }

        #endregion
    }
}
