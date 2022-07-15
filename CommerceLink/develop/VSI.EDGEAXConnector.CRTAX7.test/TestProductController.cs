using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.CRTAX7.Controllers;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using System.Linq;

namespace VSI.EDGEAXConnector.CRTAX7.test
{
    [TestClass]
    public class TestProductController
    {
        [TestMethod]
        public void testProducts()
        {
            ProductController objProductController = new ProductController();

            InventoryController objInventoryController = new InventoryController();

            PriceController objPriceController = new PriceController();

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

                erpProductPrice = objPriceController.GetActiveProductPrice(catalogId, channelId, productIds, System.DateTime.Now, string.Empty);




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
    }
}
