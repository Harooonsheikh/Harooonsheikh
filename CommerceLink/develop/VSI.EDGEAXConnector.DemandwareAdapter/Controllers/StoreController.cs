using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Demandware.Store;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
{

    /// <summary>
    /// StoreController class performs store related activities.
    /// </summary>
    public class StoreController : BaseController, IStoreController
    {
        XmlSerializer xml = new XmlSerializer(typeof(stores));

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public StoreController()
            : base(false)
        {
        }

        public void CreateStores(List<EcomstoreEntity> stores)
        {
            throw new NotImplementedException();
        }

        #endregion
        public void PushStoresInfo(ErpStoreInfo erpStores)
        {
            this.CreateStoreInfoFile(erpStores);
        }


        private void CreateStoreInfoFile(ErpStoreInfo erpStores)
        {
            var stList = new List<complexTypeStore>();
            try
            {
                if (erpStores != null)
                {
                    foreach (ErpStore es in erpStores.stores)
                    {
                        var store = new complexTypeStore();
                        store.name = es.Name;
                        store.address1 = es.Address;
                        store.city = es.City;
                        store.countrycode = es.Country;
                        store.postalcode = es.ZipCode;
                        store.statecode = es.State;
                        store.email = es.Email;
                        store.phone = es.Phone;
                        store.latitude = Convert.ToDecimal(es.Latitude);
                        store.longitude = Convert.ToDecimal(es.Longitude);

                        stList.Add(store);
                    }
                    var output = new stores();
                    output.store = stList.ToArray();
                    string fileNameStores = ConfigurationHelper.GetDirectory(
                        configurationHelper.GetSetting(STORE.Local_Output_Path))
                        + "\\Stores" + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";

                    using (FileStream stream = new FileStream(fileNameStores, FileMode.Create))
                    {
                        xml.Serialize(stream, output);
                    }
                }
            }
            catch
            {
                throw;
            }


        }
    }
}