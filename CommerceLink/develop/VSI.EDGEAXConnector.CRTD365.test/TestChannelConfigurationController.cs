using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using VSI.EDGEAXConnector.CRTD365.Controllers;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    /// <summary>
    /// TestChannelConfigurationController class is a Test class for ChannelConfigurationController.
    /// </summary>
    [TestClass]
    public class TestChannelConfigurationController
    {
        string storeKey = "";
        /// <summary>
        /// TestGetRetailServiceProfile executes Test cases for GetRetailServiceProfile.
        /// </summary>
        [TestMethod]
        public void TestGetRetailServiceProfile()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();
            ChannelConfigurationController configurationController = new ChannelConfigurationController(storeKey);

            ErpRetailServiceProfile erpServiceProfile = null;

            try
            {
                //erpServiceProfile = configurationController.GetRetailServiceProfile();
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpServiceProfile);
            Assert.IsFalse(erpServiceProfile.ServiceProfileId>0);
            Assert.IsNotNull(erpServiceProfile.ServiceProfileProperties);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpServiceProfile));
        }

        /// <summary>
        /// TestGetChannelInformation executes Test cases for GetChannelInformation.
        /// </summary>
        [TestMethod]
        public void TestGetChannelInformation()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();
            ChannelConfigurationController configurationController = new ChannelConfigurationController(storeKey);

            ErpChannel erpChannel = null;

            try
            {
                erpChannel = configurationController.GetChannelInformation();
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpChannel);
            Assert.IsFalse(erpChannel.RecordId > 0);
            Assert.IsNotNull(erpChannel.OperatingUnitNumber);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpChannel));
        }
    }
}
