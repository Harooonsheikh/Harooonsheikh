using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.CRTD365.Controllers;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    [TestClass]
    public class TestContactPersonController
    {
        string storeKey = "";

        [TestMethod]
        public void TestGetContactPerson()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();
            ContactPersonController contactPersonController = new ContactPersonController(storeKey);

            ERPContactPersonResponse erpContactPersonResponse = new ERPContactPersonResponse(false, "", null);
            string customerAccount = "005302";
            try
            {
                erpContactPersonResponse = contactPersonController.GetContactPerson(customerAccount);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpContactPersonResponse.ContactPerson.ContactPersonId);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpContactPersonResponse));
        }

        [TestMethod]
        public void TestCreateContactPerson()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();
            ContactPersonController contactPersonController = new ContactPersonController(storeKey);

            ErpContactPerson erpContactPerson = new ErpContactPerson();
            // initialize contact person and values here

            erpContactPerson.ContactForParty = 52565463104;
            erpContactPerson.CustAccount = "004013";
            erpContactPerson.InActive = false;
            erpContactPerson.Title = "new Title CL6";
            erpContactPerson.FirstName = "AliABC CL6";
            erpContactPerson.MiddleName = "A";
            erpContactPerson.LastName = "ZuberiABCD CL6";
            erpContactPerson.Phone = "0123456789";
            erpContactPerson.Email = "ns1@abc.co";
            erpContactPerson.Language = "en-us";

            ERPContactPersonResponse erpContactPersonResponse = new ERPContactPersonResponse(false, "", null);
            try
            {
                erpContactPersonResponse = contactPersonController.UpdateContactPerson(erpContactPerson);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpContactPersonResponse.ContactPerson.ContactPersonId);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpContactPersonResponse));
        }

        [TestMethod]
        public void TestUpdateContactPerson()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();
            ContactPersonController contactPersonController = new ContactPersonController(storeKey);

            ErpContactPerson erpContactPerson = new ErpContactPerson();
            // initialize contact person updated values here

            erpContactPerson.DirPartyRecordId = 52565463104;
            erpContactPerson.ContactPersonId = "000006";
            erpContactPerson.ContactForParty = 52565463104;
            erpContactPerson.CustAccount = "004013";
            erpContactPerson.InActive = false;
            erpContactPerson.EmailRecordId = 52565441500;
            erpContactPerson.PhoneRecordId = 52565441502;

            // fields to be modified
            erpContactPerson.Title = "new Title CL6";
            erpContactPerson.FirstName = "AliABC CL6";
            erpContactPerson.MiddleName = "A";
            erpContactPerson.LastName = "ZuberiABCD CL6";
            erpContactPerson.Phone = "0123456789";
            erpContactPerson.Email = "ns1@abc.co";
            erpContactPerson.Language = "en-us";

            ERPContactPersonResponse erpContactPersonResponse = new ERPContactPersonResponse(false, "", null);
            try
            {
                erpContactPersonResponse = contactPersonController.UpdateContactPerson(erpContactPerson);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpContactPersonResponse.ContactPerson.ContactPersonId);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpContactPersonResponse));
        }
    }
}
