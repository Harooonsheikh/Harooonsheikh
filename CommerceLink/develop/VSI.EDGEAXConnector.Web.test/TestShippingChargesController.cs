using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace VSI.EDGEAXConnector.Web.test

{
    [TestClass]
    public class TestShippingChargesController
    {
        private IContainer _container;

        public TestShippingChargesController(IComponentContext _context)
        {
            this._container = (IContainer)_context;
        }

        #region GetShippingCharges Test Scenarios

        #region Valid Parameters

        [TestMethod]
        public void ValidUSPostalCode_For_GetShippingCharges()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("10924", 10);
            order.Add("10001", 20);
            order.Add("10007", 30);
            shippingMethod = "Internatio";
            postalCode = "M4B 1B4";
            countryCode = "AU";
            address1 = "Sydney";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }
/*
        [TestMethod]
        public void ValidUSPostalCode_For_GetShippingCharges2()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "6032";
            countryCode = "US";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void ValidInternationalPostalCode_For_GetShippingCharges()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "abcde";
            countryCode = "PK";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void ValidInternationalPostalCode_For_GetShippingCharges2()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "";
            countryCode = "PK";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void ValidInternationalPostalCode_For_GetShippingCharges3()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "abcd12345";
            countryCode = "PK";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }


        #endregion Valid Parameters

        #region Invalid Parameters

        [TestMethod]
        public void InvalidShippingMethod_For_GetShippingCharges()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2; //, message;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "";
            postalCode = "xyz66032";
            countryCode = "PK";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidUSPostalCode_For_GetShippingCharges()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "xyz66032";
            countryCode = "US";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidUSPostalCode_For_GetShippingCharges2()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "yz66032";
            countryCode = "US";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidUSPostalCode_For_GetShippingCharges3()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "z66032";
            countryCode = "US";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidUSPostalCode_For_GetShippingCharges4()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "660";
            countryCode = "US";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidUSPostalCode_For_GetShippingCharges5()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "66";
            countryCode = "US";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidUSPostalCode_For_GetShippingCharges6()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "6";
            countryCode = "US";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidUSPostalCode_For_GetShippingCharges7()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "";
            countryCode = "US";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidUSPostalCode_For_GetShippingCharges8()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "abc";
            countryCode = "US";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidUSPostalCode_For_GetShippingCharges9()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "abcd";
            countryCode = "US";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidUSPostalCode_For_GetShippingCharges10()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "abcde";
            countryCode = "US";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidUSPostalCode_For_GetShippingCharges11()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "";
            countryCode = "";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidCountryCode_For_GetShippingCharges()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "";
            countryCode = "PKK";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidCountryCode_For_GetShippingCharges2()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "";
            countryCode = "P";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidCountryCode_For_GetShippingCharges3()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "";
            countryCode = "P1";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidCountryCode_For_GetShippingCharges4()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "";
            countryCode = "99";
            address1 = "2 H Faisal Town";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidAddress1ForInternationalShipping_For_GetShippingCharges4()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "";
            countryCode = "PK";
            address1 = "";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidQuantity_For_GetShippingCharges()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order = new Dictionary<string, decimal>();
            order.Add("prod1", 0);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "66032";
            countryCode = "US";
            address1 = "";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidQuantity_For_GetShippingCharges2()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order = new Dictionary<string, decimal>();
            order.Add("prod1", 10);
            order.Add("prod2", 0);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "66032";
            countryCode = "US";
            address1 = "";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidQuantity_For_GetShippingCharges3()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order = new Dictionary<string, decimal>();
            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 0);
            shippingMethod = "Courier";
            postalCode = "66032";
            countryCode = "US";
            address1 = "";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidProductID_For_GetShippingCharges2()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order = new Dictionary<string, decimal>();
            order.Add("", 10);
            order.Add("prod2", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "66032";
            countryCode = "US";
            address1 = "";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidProductID_For_GetShippingCharges3()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order = new Dictionary<string, decimal>();
            order.Add("prod1", 10);
            order.Add("", 20);
            order.Add("prod3", 30);
            shippingMethod = "Courier";
            postalCode = "66032";
            countryCode = "US";
            address1 = "";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }

        [TestMethod]
        public void InvalidProductID_For_GetShippingCharges4()
        {
            Dictionary<string, decimal> order = new Dictionary<string, decimal>();
            string shippingMethod, postalCode, countryCode, address1, address2;

            order = new Dictionary<string, decimal>();
            order.Add("prod1", 10);
            order.Add("prod2", 20);
            order.Add("", 30);
            shippingMethod = "Courier";
            postalCode = "66032";
            countryCode = "US";
            address1 = "";
            address2 = "";
            string legalCompany = "VWO";
            CallShippingCharges(shippingMethod, postalCode, countryCode, address1, address2, legalCompany, order);
        }
        */
        #endregion Invalid Parameters

        [TestMethod]
        private void CallShippingCharges(string shippingMethod, string postalCode, string countryCode, string address1, string address2,
            string legalCompany, Dictionary<string, decimal> order)
        {
            EstimateSmallResponse result;
            ShippingController controller = _container.Resolve<ShippingController>();
            try
            {
                //VSI.EDGEAXConnector.Configuration.ConfigurationHelper configHelper;
                //configHelper = new Configuration.ConfigurationHelper();

                EstimateRequest estimateRequest = new EstimateRequest();
                estimateRequest.shippingMethod = shippingMethod; // "USPS";
                estimateRequest.address1 = address1; // "";
                estimateRequest.address2 = address2; // "";
                estimateRequest.postalCode = postalCode; // "10010";
                estimateRequest.countryCode = countryCode; // "US";
                
                estimateRequest.order = new List<EstimateRequestOrderLine>();

                foreach(var a in order)
                {
                    estimateRequest.order.Add(
                        new EstimateRequestOrderLine(a.Key, a.Value));
                }

                estimateRequest.order = new List<EstimateRequestOrderLine>();

                EstimateRequestOrderLine estimateRequestOrderLine = new EstimateRequestOrderLine("123456", 12);

                estimateRequest.order.Add(estimateRequestOrderLine);

                result = controller.EstimateShippingCharges(estimateRequest, true);

                Assert.IsNotNull(result, "Successful execution.", shippingMethod, order, postalCode, countryCode, address1, address2);
            }
            catch (ArgumentException exp)
            {
                HttpResponseException response = (HttpResponseException)exp.InnerException;
                Assert.AreEqual(HttpStatusCode.BadRequest, response.Response.StatusCode, exp.Message, shippingMethod, order, postalCode, countryCode, address1, address2);
            }
            catch (Exception exp)
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, ((HttpResponseException)exp.InnerException).Response.StatusCode,
                    string.Format("Status Code: {0}, Message: {1}", ((HttpResponseException)exp.InnerException).Response.StatusCode, exp.Message),
                    shippingMethod, order, postalCode, countryCode, address1, address2);
            }
        }

        #endregion GetShippingCharges Test Scenarios

    }
}
