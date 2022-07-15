using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Demandware.Inventory;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
{

    /// <summary>
    /// InventoryController class performs Inventory related activities.
    /// </summary>
    public class InventoryController : ProductBaseController, IInventoryController
    {


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
                CustomLogger customLogger = new CustomLogger();
                customLogger.LogException(exp);
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
                CustomLogger customLogger = new CustomLogger();
                customLogger.LogException(exp);
                throw;
            }
        }

        #endregion

        #region Private Methods

        //AF:Start
        private void ProcessInventoryData(ErpInventoryProducts inventoryProducts)
        {
            if(inventoryProducts.Products.Count>0)
            {

                foreach (var p in inventoryProducts.Products)
                {
                    p.SKU = configurationHelper.GetSetting(PRODUCT.SKU_Prefix)+p.ItemId;

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
                string fileName = FileHelper.GetProductInventoryCSVFileName();

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

            try
            {
                if (inventoryData != null)
                {
                    //string fileNameProduct = ConfigurationHelper.ProductDiscountTag + inventoryData.Description + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";

                    string fileNameInventory = configurationHelper.GetSetting(INVENTORY.Filename_Prefix) + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";
                    XmlTemplateHelper xmlHelper = new XmlTemplateHelper();
                    xmlHelper.GenerateXmlUsingTemplate(fileNameInventory, ConfigurationHelper.GetDirectory(configurationHelper.GetSetting(INVENTORY.Local_Output_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, inventoryData);
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
