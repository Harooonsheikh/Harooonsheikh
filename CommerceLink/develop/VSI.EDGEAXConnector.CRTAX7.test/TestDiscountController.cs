using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.CRTAX7.Controllers;

namespace VSI.EDGEAXConnector.CRTAX7.test
{
    [TestClass]
    public class TestDiscountController
    {
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
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTAX7.ErpToRSMappingConfiguration();

            // Create and Initialize the Discount Controller
            DiscountController controllerDiscountController = new Controllers.DiscountController();

            // Setup the Manager
            ProductController controllerProductController = new Controllers.ProductController();

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
    }
}
