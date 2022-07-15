using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Demandware.Inventory;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
//using VSI.EDGEAXConnector.GenericCSVParser;
using VSI.EDGEAXConnector.Logging;
//using VSI.EDGEAXConnector.SFTPlib;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
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
                inventory inventoryData = this.ProcessProductInventoryData(products);
                this.CreateProductInventoryFile(inventoryData);
            }
            catch (Exception exp)
            {
                //TransactionLogging obj = new TransactionLogging();
                //obj.LogTransaction((int)SyncJobs.InventorySynch, "Inventory CSV generation Failed", DateTime.UtcNow, null);
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

        public void PushAllProductInventory(ErpInventoryProducts inventoryProducts)
        {
            throw new NotImplementedException();
        }

    }
}
