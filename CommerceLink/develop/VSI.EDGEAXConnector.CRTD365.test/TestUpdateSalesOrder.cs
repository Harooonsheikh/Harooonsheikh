using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.CRTD365.Controllers;
using Newtonsoft.Json;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    [TestClass]
    public class TestUpdateSalesOrder
    {
        string storeKey = "";

        [TestMethod]
        public void TestCancelContract()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

            UpdateSalesOrderController updateSalesOrderController = new UpdateSalesOrderController(storeKey);

            ErpCancelContractResponse erpCancelContractResponse = new ErpCancelContractResponse(false, "", null);

            string salesOrderId = "000054025";
            string salesLineRecId = ""; //Optional to cancel a specific contract line

            try
            {
                erpCancelContractResponse = updateSalesOrderController.CancelContract(salesOrderId, salesLineRecId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCancelContractResponse.Success);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCancelContractResponse));
        }

        [TestMethod]
        public void TestTerminateContract()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

            UpdateSalesOrderController updateSalesOrderController = new UpdateSalesOrderController(storeKey);

            ErpTerminateContractResponse erpTerminateContractResponse = new ErpTerminateContractResponse(false, "", null);

            string salesOrderId = "000054025";
            string salesLineRecId = ""; //Optional to terminate a specific contract line

            try
            {
                erpTerminateContractResponse = updateSalesOrderController.TerminateContract(salesOrderId, salesLineRecId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpTerminateContractResponse.Success);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpTerminateContractResponse));
        }

        [TestMethod]
        public void TestSwitchContract()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

            UpdateSalesOrderController updateSalesOrderController = new UpdateSalesOrderController(storeKey);

            ErpUpdateContractResponse erpSwitchContractResponse = new ErpUpdateContractResponse(false, "", null);

            ErpTMVCrosssellType action = ErpTMVCrosssellType.Switch;
            string salesOrderId = "000054356";
            string salesLineRecId = "5637272826";

            ErpAdditionalSalesLine newSalesLine = new ErpAdditionalSalesLine();
            newSalesLine.ItemId = "TVP0001";
            newSalesLine.InventDimensionId = "000000122";
            newSalesLine.Quantity = 1;

            ErpTenderLine payment = new ErpTenderLine();
            payment.Amount = (decimal)284.6;

            ErpCustomerOrderInfo customerOrderInfo = new ErpCustomerOrderInfo();

            try
            {
                erpSwitchContractResponse = updateSalesOrderController.UpdateContract(action, salesOrderId, salesLineRecId, newSalesLine, customerOrderInfo);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpSwitchContractResponse.Success);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpSwitchContractResponse));
        }
    }
}
