using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.CRTD365.Controllers;
using Newtonsoft.Json;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    [TestClass]
    public class TestDQSController
    {
        string storeKey = "";

        [TestMethod]
        public void TestGetDQS()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            DQSController dqsController = new DQSController(storeKey);

            ErpDQSResponse erpDQSResponse = new ErpDQSResponse(false, string.Empty, null);

            try
            {
                const string username = "admin";
                const string password = "admin";
                const string workflowName = "RBWF_SanctionListCheck_Web";
                string jsonInput = "{\"address1_city\": \"Hamburg\",\"address1_country\": \"DE\",\"address1_line1\": \"Schottweg 5\",\"address1_postalcode\": \"22087\",\"entitytypename\": \"account\",\"id\": \"cf68e0b8-69d3-e711-8100-0050568e49af\",\"name\": \"Rede de Osama bin Laden\",\"orb_sanctionlistchk\": \"1\",\"orb_sanctionlistinformation\": \"\",\"orb_sanctionliststatuscode\": \"\",\"orb_checksum\": \"\",\"positions\": [{\"entitytypename\": \"\",\"id\": \"\",\"modifiedby\": \"\",\"modifiedbyname\": \"\",\"modifiedon\": \"\",\"orb_checksum\": \"\",\"orb_manualstatuscode\": \"\",\"orb_manualstatusreason\": \"\",\"orb_resultdetails\": \"\",\"orb_resultreason\": \"\",\"orb_sanctionlistid\": \"\",\"orb_sanctionlistresultcode\": \"\",\"orb_sanctionlistre sultid\": \"\"}]}";
                string endPoint = "https://dataquality01.teamviewer.com/DQServer/DQServer.asmx";

                erpDQSResponse = dqsController.GetDQS(username, password, workflowName, jsonInput, endPoint);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpDQSResponse);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpDQSResponse));
        }
    }
}
