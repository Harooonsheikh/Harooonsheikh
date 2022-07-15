using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.CRTD365.Controllers;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using System.Linq;
using Newtonsoft.Json;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    [TestClass]
    public class TestProductController
    {
        string storeKey = "";

        [TestMethod]
        public void testProducts()
        {
            // Disable the Certificate
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ProductController objProductController = new ProductController(storeKey);

            InventoryController objInventoryController = new InventoryController(storeKey);

            PriceController objPriceController = new PriceController(storeKey);

            List<KeyValuePair<long, IEnumerable<ErpProduct>>> lstERPProducts = new List<KeyValuePair<long, IEnumerable<ErpProduct>>>();

            List<ErpProduct> searchedProducts = new List<ErpProduct>();

            List<ErpProductPrice> erpProductPrice = new List<ErpProductPrice>();

            List<long> productIds = new List<long>();



            try
            {
                lstERPProducts = objProductController.GetCatalogProducts(5637146076, true, null, true);



                lstERPProducts.ForEach(k => searchedProducts.AddRange(k.Value));

                if (searchedProducts != null && searchedProducts.Count > 0)
                    productIds = searchedProducts.Select(prod => prod.RecordId).Distinct().ToList();

                searchedProducts = objInventoryController.GetProductAvailabilities(searchedProducts, productIds, 5637146076);

                long catalogId = 5637146076;
                long channelId = 5637146076;

                erpProductPrice = objPriceController.GetActiveProductPrice(catalogId, channelId, productIds, System.DateTime.UtcNow, string.Empty);




            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
                //throw;
            }
            Assert.IsNotNull(lstERPProducts);
            Assert.IsTrue(lstERPProducts.Count > 0);
            System.Console.WriteLine(lstERPProducts.ToString());
        }


        [TestMethod]
        public void TestProductCustomFieldds()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();
            ProductController productController = new ProductController(storeKey);

            List<long> list = new List<long>();
            list.Add(22565422122);
            list.Add(22565422189);

            var res = productController.GetProductCustomFields(list);

            try
            {

            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(res != null);
            System.Console.WriteLine(JsonConvert.SerializeObject(res));
        }

        [TestMethod]
        public void TestOfferTypeGroups()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();
            ProductController productController = new ProductController(storeKey);



            try
            {

            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            // Assert.IsNotNull(res.Success);
            // System.Console.WriteLine(JsonConvert.SerializeObject(res.ProductCustomFields));
        }

        [TestMethod]
        public void TestRetailInventItemSalesSetup()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();
            ProductController productController = new ProductController("E550E995-1D34-4E65-9D09-FA4C15712ADA");

            List<long> productsList = new List<long>();
            productsList.Add(5637144661);
            productsList.Add(5637144663);
            productsList.Add(5637144664);
            productsList.Add(5637144666);
            productsList.Add(5637144670);

            try
            {
                var res = productController.GetRetailInventItemSalesSetup(productsList);

                Assert.IsNotNull(res != null);
                System.Console.WriteLine(JsonConvert.SerializeObject(res));
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }
        }
    }
}
