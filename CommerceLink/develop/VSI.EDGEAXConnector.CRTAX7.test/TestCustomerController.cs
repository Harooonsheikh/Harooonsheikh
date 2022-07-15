using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.CRTAX7.Controllers;
using VSI.EDGEAXConnector.ERPDataModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

namespace VSI.EDGEAXConnector.CRTAX7.test
{
    [TestClass]
    public class TestCustomerController
    {
        [TestMethod]
        public void testCustomers()
        {
            // Disable the Certificate
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            // Initialize the mappings
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTAX7.ErpToRSMappingConfiguration();

            CustomerController objCustomerController = new CustomerController();
            ErpCustomer objERPCustomer = new ErpCustomer();
            ErpAddress objERPAddress = new ErpAddress();
            ErpCustomer objReturnERPCustomer = new ErpCustomer();

            objERPAddress.Name = "Shipping Location";
            objERPAddress.AddressType = ErpAddressType.Home;
            objERPAddress.AddressTypeValue = (int) ErpAddressType.Home;
            objERPAddress.FullAddress = "Street # 3, BROOMFIELD, CO, 80021";
            objERPAddress.Street = "Street # 3";
            objERPAddress.City = "BROOMFIELD";
            objERPAddress.ZipCode = "80021";
            objERPAddress.State = "CO";
            objERPAddress.Phone = "6173856890";
            objERPAddress.Email = "johnsmith@microsoft.com";
            objERPAddress.ThreeLetterISORegionName = "USA";
            objERPAddress.TwoLetterISORegionName = "US";

            // Fill some values ...
            objERPCustomer.EcomCustomerId = "00001006";
            // objERPCustomer.SLBirthMonth = "";
            objERPCustomer.AccountNumber = String.Empty; // Send empty for now
            objERPCustomer.Phone = "6173856890";
            objERPCustomer.Cellphone = "61738568901";
            objERPCustomer.Email = "johnsmith@microsoft.com";
            objERPCustomer.Name = "John Smith";
            objERPCustomer.FirstName = "John";
            objERPCustomer.MiddleName = "M";
            objERPCustomer.LastName = "Smith";
            objERPCustomer.Addresses = new List<ErpAddress>();
            //No need to send address with customer as we are doing in AX Adapter SalesOrderController.
            //objERPCustomer.Addresses.Add(objERPAddress);

            objERPCustomer.AccountNumber = new Guid().ToString();
            objERPCustomer.VatNumber = string.Empty;
            objERPCustomer.TaxGroup = "WA";
            objERPCustomer.CustomerGroup = "Default";
            objERPCustomer.CurrencyCode = "USD";
            objERPCustomer.Language = "en-us";
            objERPCustomer.CustomerType = ErpCustomerType.None;
            objERPCustomer.CustomerTypeValue = (int)ErpCustomerType.Person;
            
            long channelId = 5637146076;

            try
            {
                objReturnERPCustomer = objCustomerController.CreateCustomer(objERPCustomer, channelId);

                //Action<HttpResponseException> asserts = exception => Assert.That(exception.Response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                //await AssertEx.ThrowsAsync(() => userController.Get("foo"), asserts);
                //AssertEx.ThrowsAsync(() => objCustomerController.CreateCustomer(objERPCustomer, channelId), asserts);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(objReturnERPCustomer);
            System.Console.WriteLine(objReturnERPCustomer.ToString());
        }
    }

    public static class AssertEx
    {
        public static async Task ThrowsAsync<TException>(Func<Task> func) where TException : class
        {
            await ThrowsAsync<TException>(func, exception => { });
        }

        public static async Task ThrowsAsync<TException>(Func<Task> func, Action<TException> action) where TException : class
        {
            var exception = default(TException);
            var expected = typeof(TException);
            Type actual = null;
            try
            {
                await func();
            }
            catch (Exception e)
            {
                exception = e as TException;
                actual = e.GetType();
            }

            Assert.AreEqual(expected, actual);
            action(exception);
        }
    }
}
