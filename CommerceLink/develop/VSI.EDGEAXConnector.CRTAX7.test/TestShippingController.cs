using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.CRTAX7.Controllers;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using System.Globalization;

namespace VSI.EDGEAXConnector.CRTAX7.test
{
    [TestClass]
    public class TestShippingController
    {
        private string CountryThreeLetterISOCode(string CountryTwoLetterISOCode)
        {
            string twoLetterISORegionName = CountryTwoLetterISOCode;
            string threeLetterISORegionName = "";

            var countryCodesMapping = new Dictionary<string, RegionInfo>();
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var culture in cultures)
            {
                var region = new RegionInfo(culture.LCID);
                if (region.TwoLetterISORegionName.ToString().Equals(twoLetterISORegionName))
                {
                    threeLetterISORegionName = region.ThreeLetterISORegionName;
                    break;
                }
            }

            return threeLetterISORegionName;
        }

        [TestMethod]
        public void TestShippingCharges()
        {

            // Disable the Certificate
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ErpToRSMappingConfiguration erpToRSMappingConfiguration = new ErpToRSMappingConfiguration();

            // Shipping Charges Controller
            ShippingController shippingChargesController = new ShippingController();

            string shippingMethod = "Internatio";

            List<ErpCommerceProperty> productIdWithQuantity = new List<ErpCommerceProperty>();
            productIdWithQuantity.Add(new ErpCommerceProperty("10924", new ErpCommercePropertyValue { DecimalValue = 18 }));
            productIdWithQuantity.Add(new ErpCommerceProperty("10001", new ErpCommercePropertyValue { DecimalValue = 36 }));
            productIdWithQuantity.Add(new ErpCommerceProperty("10007", new ErpCommercePropertyValue { DecimalValue = 54 }));

            string zipCode = "M4B 1B4";
            string countryCode = CountryThreeLetterISOCode("AU");
            string address1 = "Melbourne";
            string address2 = "";

            IEnumerable<ErpCommerceProperty> returnData = new List<ErpCommerceProperty>();

            try
            {
                returnData = shippingChargesController.GetShippingCharges(shippingMethod, productIdWithQuantity, zipCode, countryCode, address1, address2);
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
            ShippingController shippingController = new ShippingController();

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
