using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.CRTD365.Controllers;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    [TestClass]
    public class TestShippingController
    {
        string storeKey = "";

        [TestMethod]
        public void TestShippingCharges()
        {

            // Disable the Certificate
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ErpToRSMappingConfiguration erpToRSMappingConfiguration = new ErpToRSMappingConfiguration();

            // Shipping Charges Controller
            ShippingController shippingChargesController = new ShippingController(storeKey);

            string shippingMethod = "Standard";

            List<ErpCommerceProperty> productIdWithQuantity = new List<ErpCommerceProperty>();
            productIdWithQuantity.Add(new ErpCommerceProperty("5637154334", new ErpCommercePropertyValue { DecimalValue = 1 }));

            string zipCode = "80021";
            string countryCode = "US";
            string address1 = "";
            string address2 = "";

            try
            {
                shippingChargesController.GetShippingCharges(shippingMethod, productIdWithQuantity, zipCode, countryCode, address1, address2);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }
            Assert.IsNotNull("");

            System.Console.WriteLine("");

        }

        [TestMethod]
        public void TestShippingTrackingId()
        {

            // Disable the Certificate
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ErpToRSMappingConfiguration erpToRSMappingConfiguration = new ErpToRSMappingConfiguration();

            // Shipping Charges Controller
            ShippingController shippingController = new ShippingController(storeKey);

            string salesOrderId = "000636690";

            try
            {
                shippingController.GetShippingTrackingId(salesOrderId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }
            Assert.IsNotNull("");

            System.Console.WriteLine("");

        }
    }
}
