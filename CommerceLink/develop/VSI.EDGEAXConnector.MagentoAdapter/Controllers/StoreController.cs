using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
{
    /// <summary>
    /// StoreController class performs store related activities.
    /// </summary>
    public class StoreController : BaseController, IStoreController
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public StoreController(string storeKey) : base(false, storeKey)
        {
        }
        #endregion

        #region Public Methods
        

        public void CreateStores(List<EcomstoreEntity> stores)
        {
            byte[] filebyte = new byte[] { };
            StringBuilder sb = new StringBuilder();

            try
            {
                string fileName = Path.Combine(configurationHelper.GetSetting(STORE.Local_Output_Path), "Store Inventory" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".csv");
                int length = stores.Count;
                
                using (var w = new StreamWriter(fileName))
                {
                    w.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                         "StoreId", "Name", "Status", "Address", "City", "State",
                          "Country", "zipcode", "Latitute", "Longitute", "Fax", "phone", "Email", "link", "zoom", "ImageName", "tagstore"));

                    foreach (var St in stores)
                    {
                        w.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                           St.store_id, St.name, St.is_active, St.Address, St.City, St.State,
                            St.Country, St.zipcode, St.Latitute, St.Longitute, St.Fax, St.phone, St.Email, St.link, St.zoom, St.ImageName, St.tagstore));
                        w.Flush();
                    }
                }

                filebyte = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                TransactionLogging obj = new TransactionLogging(currentStore.StoreKey);
                obj.LogTransaction(SyncJobs.StoreSync, "Store Sync CSV generated Successfully", DateTime.UtcNow, filebyte);
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(currentStore.StoreKey);
                obj.LogTransaction(SyncJobs.StoreSync, "Store Sync CSV generation Failed", DateTime.UtcNow, filebyte);
                CustomLogger customLogger = new CustomLogger();
                customLogger.LogException(exp);
            }
        }
        public void PushStoresInfo(ErpStoreInfo erpStores)
        {
            this.CreateStoreInfoFile(erpStores);
        }
        private void CreateStoreInfoFile(ErpStoreInfo erpStores)
        {
            try
            {
                if (erpStores != null)
                {
                    //string fileNameProduct = ConfigurationHelper.ProductDiscountTag + inventoryData.Description + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";

                    string fileNameStores = "Stores" + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";
                    XmlTemplateHelper xmlHelper = new XmlTemplateHelper();
                    xmlHelper.GenerateXmlUsingTemplate(fileNameStores, ConfigurationHelper.GetDirectory(configurationHelper.GetSetting(STORE.Local_Output_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, erpStores);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
