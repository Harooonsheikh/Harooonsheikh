using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.Configuration;

namespace VSI.EdgeAX.Connector.Common.test
{
    [TestClass]
    public class TestConfiguration
    {
        [TestMethod]
        public void TestLoadFromXml()
        {
            // this should load the default config first as it is static
            //ConfigurationHelper configuration = ConfigurationHelper.GetInstance;
            //Assert.AreEqual("123456789", configuration.GetSetting(ECOM.Root_Category_Id));
            //configuration.loadFromXml(typeof(TestConfiguration).Assembly, "test.config.xml");
            //Assert.AreEqual("9977553311", configuration.GetSetting(ECOM.Root_Category_Id));

        }
    }
}
