using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.CommerceLink.Demandware.Inventory;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;
using System.Linq;
using VSI.EDGEAXConnector.Common.Constants;

namespace VSI.CommerceLink.MagentoAdapter.Controllers
{

    /// <summary>
    /// InventoryController class performs Inventory related activities.
    /// </summary>
    public class InventoryController : ProductBaseController, IInventoryController
    {
        XmlTemplateHelper xmlHelper = null;
        FileHelper fileHelper = null;
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public InventoryController(string storeKey) : base(false, storeKey)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushProducts push products to Magento
        /// </summary>
        /// <param name="products"></param>
        public void PushAllProductInventory(List<EcomcatalogProductCreateEntity> products)
        {
            try
            {
                inventory inventoryData = this.ProcessProductInventoryData(products);
                this.CreateProductInventoryFile(inventoryData);
            }
            catch (Exception exp)
            {
                //TransactionLogging obj = new TransactionLogging();
                //obj.LogTransaction((int)SyncJobs.InventorySynch, "Inventory CSV generation Failed", DateTime.UtcNow, null);
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }
        }

        public void PushAllProductInventory(ErpInventoryProducts inventoryProducts)
        {
            try
            {
                //inventory inventoryData = this.ProcessProductInventoryData(products);
                //AF:Start
                // this.ProcessInventoryData(inventoryProducts);
                //AF:End
                this.CreateProductInventoryFile(inventoryProducts);
            }
            catch (Exception exp)
            {
                //TransactionLogging obj = new TransactionLogging();
                //obj.LogTransaction((int)SyncJobs.InventorySynch, "Inventory CSV generation Failed", DateTime.UtcNow, null);
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }
        }

        #endregion

        #region Private Methods

        //AF:Start
        private void ProcessInventoryData(ErpInventoryProducts inventoryProducts)
        {
            if (inventoryProducts.Products.Count > 0)
            {

                foreach (var p in inventoryProducts.Products)
                {
                    p.SKU = configurationHelper.GetSetting(PRODUCT.SKU_Prefix) + p.ItemId;

                }
            }



        }

        //AF:End


        /// <summary>
        /// This function creates product inventory CSV files.
        /// </summary>
        /// <param name="products"></param>
        private void CreateProductInventoryFile(inventory inventoryData)
        {
            if (inventoryData != null)
            {
                fileHelper = new FileHelper(currentStore.StoreKey);
                string fileName = fileHelper.GetProductInventoryCSVFileName();

                var serializer = new XmlSerializer(typeof(inventory));
                using (var stream = new StreamWriter(fileName))
                {
                    serializer.Serialize(stream, inventoryData);
                }

                //TransactionLogging obj = new TransactionLogging();
                //obj.LogTransaction(SyncJobs.InventorySynch, "Product Inventory Sync CSV generated Successfully", DateTime.UtcNow, null);
            }
        }

        private void CreateProductInventoryFile(ErpInventoryProducts inventoryData)
        {
            String strFileName = String.Empty;
            String strFileDirectory = String.Empty;
            ObjectToCsvConverter objectToCsvConverter = new ObjectToCsvConverter();
            try
            {
                if (inventoryData != null)
                {
                    if (configurationHelper.GetSetting(ECOM.Inventory_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_CSV)
                    {
                        #region CSV File Generation
                        strFileName = configurationHelper.GetSetting(INVENTORY.Filename_Prefix) + currentStore.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + "." + ApplicationConstant.FILE_TYPE_CSV.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(INVENTORY.Local_Output_Path));
                        if (!String.IsNullOrEmpty(strFileDirectory) && !strFileDirectory.EndsWith("\\"))
                        {
                            strFileDirectory = strFileDirectory + "\\";
                        }
                        string strTemplateObjectToCsvMappingXmlFileLocation = configurationHelper.GetSetting(INVENTORY.CSV_Map_Path);
                        processErpProduct(inventoryData.Products, null);
                        // Product Inventory
                        objectToCsvConverter.ConvertObjectToCsv(inventoryData.Products.ToArray(), strTemplateObjectToCsvMappingXmlFileLocation,
                           strFileDirectory, strFileName, true, null);
                        #endregion
                    }
                    else if (configurationHelper.GetSetting(ECOM.Inventory_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_XML)
                    {
                        #region XML File Generation
                        strFileName = configurationHelper.GetSetting(INVENTORY.Filename_Prefix) + currentStore.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + "." + ApplicationConstant.FILE_TYPE_XML.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(INVENTORY.Local_Output_Path));
                        xmlHelper = new XmlTemplateHelper(currentStore);
                        xmlHelper.GenerateXmlUsingTemplate(strFileName, strFileDirectory, XmlTemplateHelper.XmlSourceDirection.CREATE, inventoryData);
                        #endregion
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// FormatProductCSVDataExt format and arrange Products as per requirements for CSV Export.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="csvProducts"></param>
        /// <param name="csvRelatedProducts"></param>
        /// <param name="csvProductImages"></param>
        private inventory ProcessProductInventoryData(List<EcomcatalogProductCreateEntity> products)
        {
            inventory inventoryData = this.IntializeInventoryData(products);
            List<complexTypeInventoryRecord> inventoryRecords = new List<complexTypeInventoryRecord>();

            complexTypeInventoryRecord inventoryRecordItem;

            foreach (EcomcatalogProductCreateEntity prod in products)
            {
                if (prod.AvailableQuantity < 0)
                {
                    prod.AvailableQuantity = 0;
                }
                inventoryRecordItem = new complexTypeInventoryRecord();
                inventoryRecordItem.productid = prod.SKU;
                inventoryRecordItem.allocation = prod.AvailableQuantity;
                inventoryRecordItem.allocationSpecified = true;
                inventoryRecordItem.allocationtimestamp = DateTime.UtcNow;
                inventoryRecordItem.allocationtimestampSpecified = true;
                inventoryRecordItem.perpetual = false;
                inventoryRecordItem.perpetualSpecified = true;
                inventoryRecordItem.preorderbackorderhandling = simpleTypeInventoryRecordPreorderBackorderHandling.none;
                inventoryRecordItem.preorderbackorderhandlingSpecified = true;
                inventoryRecordItem.ats = prod.AvailableQuantity;
                inventoryRecordItem.atsSpecified = true;
                inventoryRecordItem.onorder = 0;
                inventoryRecordItem.onorderSpecified = true;
                inventoryRecordItem.turnover = 0;
                inventoryRecordItem.turnoverSpecified = true;

                inventoryRecords.Add(inventoryRecordItem);
            }

            inventoryData.inventorylist[0].records = inventoryRecords.ToArray();

            return inventoryData;
        }

        private inventory IntializeInventoryData(List<EcomcatalogProductCreateEntity> products)
        {
            inventory inventoryData = new inventory();

            inventoryData.inventorylist = new complexTypeInventoryList[1];
            inventoryData.inventorylist[0] = new complexTypeInventoryList();
            inventoryData.inventorylist[0].header = this.IntializeInventoryHeader();

            return inventoryData;
        }

        private complexTypeHeader IntializeInventoryHeader()
        {
            complexTypeHeader header = new complexTypeHeader();

            header.listid = "fabrikam_inventory_list";//ConfigurationHelper.InventoryListId;
            header.description = "Fabrikam Inventory List";
            header.usebundleinventoryonly = false;
            header.usebundleinventoryonlySpecified = true;

            return header;
        }

        /// <summary>
        /// Function to enrich erpProduct
        /// </summary>
        /// <param name="erpProductsCollection"></param>
        /// <param name="hirarchy"></param>
        private void processErpProduct(List<ErpProduct> erpProductsCollection, List<KeyValuePair<string, string>> hirarchy)
        {
            if (erpProductsCollection != null)
            {
                string strConfigurableVariations = String.Empty;

                for (int i = 0; i < erpProductsCollection.Count; i++)
                {
                    // Set ProductType in desisred format for Magento
                    erpProductsCollection[i].ProductType = erpProductsCollection[i].IsMasterProduct ? "simple" : "configurable";
                    // Set Visibility in desisred format for Magento
                    erpProductsCollection[i].Visibility = erpProductsCollection[i].IsMasterProduct ? "Catalog, Search" : "Not Visible Individually";
                    // Set SKU in desisred format for Magento
                    //erpProductsCollection[i].SKU = erpProductsCollection[i].SKU + "_" + erpProductsCollection[i].ColorId + "_" + erpProductsCollection[i].SizeId + "_" + erpProductsCollection[i].ItemId;
                    #region Set Additional Attributes in desired format for Magento
                    if (String.IsNullOrEmpty(erpProductsCollection[i].ColorId) && String.IsNullOrEmpty(erpProductsCollection[i].SizeId))
                    {
                        erpProductsCollection[i].AdditionalAttributes = "";
                    }
                    else if (String.IsNullOrEmpty(erpProductsCollection[i].ColorId))
                    {
                        erpProductsCollection[i].AdditionalAttributes = "size=" + erpProductsCollection[i].SizeId;
                    }
                    else if (String.IsNullOrEmpty(erpProductsCollection[i].SizeId))
                    {
                        erpProductsCollection[i].AdditionalAttributes = "color=" + erpProductsCollection[i].ColorId;
                    }
                    else
                    {
                        erpProductsCollection[i].AdditionalAttributes = "color=" + erpProductsCollection[i].ColorId + ",size=" + erpProductsCollection[i].SizeId;
                    }
                    #endregion

                    #region Set ConfigutableVariations in desired Magento format 
                    if (erpProductsCollection[i].IsMasterProduct)
                    {
                        erpProductsCollection[i].ConfigurableVariations = strConfigurableVariations;
                    }

                    strConfigurableVariations += "sku=" + erpProductsCollection[i].SKU + "," + erpProductsCollection[i].AdditionalAttributes;

                    if (i + 1 < erpProductsCollection.Count)
                    {
                        if (erpProductsCollection[i + 1] != null && !erpProductsCollection[i + 1].IsMasterProduct)
                        {
                            strConfigurableVariations += "|";
                        }
                    }
                    #endregion

                    #region Set CompleteHirarchy in desired format in Magento
                    if (hirarchy != null && erpProductsCollection[i].CategoryIds != null)
                    {
                        List<String> strHirarchyCollection = new List<String>();

                        for (int j = 0; j < erpProductsCollection[i].CategoryIds.Count; j++)
                        {
                            string strDesiredHirarchy = hirarchy.First(c => c.Key == Convert.ToString(erpProductsCollection[i].CategoryIds.ToArray()[j])).Value;

                            strHirarchyCollection.Add(strDesiredHirarchy);
                        }

                        erpProductsCollection[i].CompleteHirarchy = string.Join(",", strHirarchyCollection);
                    }
                    #endregion
                }
            }
        }

        #endregion


        #region Magento Code // Commented

        // Magento Controller Code Commented by KAR
        /*
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public InventoryController()
            : base(false)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushProducts push products to Magento
        /// </summary>
        /// <param name="products"></param>
        public void PushAllProductInventory(List<EcomcatalogProductCreateEntity> products)
        {
            try
            {
                List<ProductCSV> csvProducts = this.ProcessProductCSVDataExt(products);
                this.CreateProductInventoryCSVFile(csvProducts);
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging();
                obj.LogTransaction(SyncJobs.InventorySynch, "Inventory CSV generation Failed", DateTime.UtcNow, null);
                CustomLogger.LogException(exp);
                throw;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This function creates product inventory CSV files.
        /// </summary>
        /// <param name="products"></param>
        private void CreateProductInventoryCSVFile(List<ProductCSV> products)
        {
            if (products != null && products.Count > 0)
            {
                string fileName = FileHelper.GetProductInventoryCSVFileName();
                List<MapTemplate> fieldMaps = XMLHelper.LoadMap(ConfigurationHelper.ProductInventoryCSVMap);
                string csvData = CSVWriter.Write(products, true, fileName, fieldMaps);
                //bool upladFiles = true;

                ////TODO: Temp test code to disbale products upload
                //try
                //{
                //    upladFiles = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("UploadFileToFTP"));
                //}
                //catch
                //{
                //}

                //if (upladFiles)
                //{
                //    SFTPManager.UploadFile(fileName, ConfigurationHelper.ProductInventoryFTPPath);
                //    FileHelper.MoveFileToLocalFolder(fileName, "Processed", ConfigurationHelper.InventoryOutputPath);
                //}

                byte[] filebyte = System.Text.Encoding.UTF8.GetBytes(csvData);
                TransactionLogging obj = new TransactionLogging();
                obj.LogTransaction(SyncJobs.InventorySynch, "Product Inventory Sync CSV generated Successfully", DateTime.UtcNow, filebyte);
            }
        }


        /// <summary>
        /// FormatProductCSVDataExt format and arrange Products as per requirements for CSV Export.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="csvProducts"></param>
        /// <param name="csvRelatedProducts"></param>
        /// <param name="csvProductImages"></param>
        private List<ProductCSV> ProcessProductCSVDataExt(List<EcomcatalogProductCreateEntity> products)
        {
            List<ProductCSV> csvProducts = new List<ProductCSV>();
            List<ProductImageCSV> csvProductImages = new List<ProductImageCSV>();

            //base.ProcessProductCSVDataExt(products, csvProducts, csvProductImages);
            return csvProducts;
        }


        #endregion

        */

        #endregion
    }
}
