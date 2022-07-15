using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.CRTD365.Controllers;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using Newtonsoft.Json;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    [TestClass]
    public class TestStoreController
    {
        string storeKey = "";

        [TestMethod]
        public void testSearchStores()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ErpToRSMappingConfiguration erpToRSMappingConfiguration = new ErpToRSMappingConfiguration();
            StoreController controller = new StoreController(storeKey);
            List<ErpStore> result = null;
            try
            {
                result = controller.SearchStores();
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            System.Console.WriteLine(result.ToString());
        }

        [TestMethod]
        public void testGetStoreAvailability()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            List<ErpInventoryInfo> lstInventoryInfo = new List<ErpInventoryInfo>();

            string itemId = "81121";
            string variantId = "VN-002109";

            ////string itemId = "81121";
            ////string variantId = "VN-002110";

            //string itemId = "81121";
            //string variantId = "VN-002115";

            try
            {
                StoreController storeController = new StoreController(storeKey);
                lstInventoryInfo = storeController.GetStoreAvailability(itemId, variantId);
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(lstInventoryInfo);
            System.Console.WriteLine("Inventory Lookup Info = " + JsonConvert.SerializeObject(lstInventoryInfo));
        }
    }
}
