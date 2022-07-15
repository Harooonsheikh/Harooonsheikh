using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.CRTD365.Controllers;
using Newtonsoft.Json;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    [TestClass]
    public class TestDiscountController
    {
        string storeKey = "";

        [TestMethod]
        public void testDiscount()
        {

            List<ErpProduct> lstErpProducts = new List<ErpProduct>();
            List<long> lstProductIds = new List<long>();
            long lngChannelID = 5637146076;
            string strCustomerAccountNumber = string.Empty;

            // Disable the Certificate
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            // Initialize the mappings
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

            // Create and Initialize the Discount Controller
            DiscountController controllerDiscountController = new Controllers.DiscountController(storeKey);

            // Setup the Manager
            ProductController controllerProductController = new Controllers.ProductController(storeKey);

            // Get the Products ...
            List<KeyValuePair<long, IEnumerable<ErpProduct>>> lstkvpErpProducts = controllerProductController.GetCatalogProducts(lngChannelID, false, null, true);
            IEnumerable<ErpProduct> objErpProducts = lstkvpErpProducts[0].Value;

            // Populate lstErpProducts and lstProductIds
            foreach (ErpProduct erpProduct in objErpProducts)
            {
                lstErpProducts.Add(erpProduct);
                lstProductIds.Add(erpProduct.RecordId);
            }

            try
            {
                controllerDiscountController.GetIndependentProductPriceDiscount(lstErpProducts, lngChannelID, lstProductIds, strCustomerAccountNumber);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(null);
            //System.Console.WriteLine(null);

        }


        [TestMethod]
        public void testGetDiscountItems()
        {

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();
            DiscountController controller = new DiscountController(storeKey);

            List<long> list = new List<long>();
            list.Add(68719490053);
            list.Add(68719489961);
            list.Add(68719490054);
            list.Add(68719489962);
            list.Add(68719490055);
            list.Add(68719489963);
            list.Add(68719490056);
            list.Add(68719489964);
            list.Add(68719490058);
            list.Add(68719490059);
            list.Add(68719489966);
            list.Add(22565422189);

            var res = controller.GetDiscountItems(list);

            Assert.IsNotNull(res != null);
            System.Console.WriteLine(JsonConvert.SerializeObject(res));

        }
    }
}
