using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.CRTAX7.Controllers;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRTAX7.test
{
    [TestClass]
    public class TestStoreController
    {
        [TestMethod]
        public void testSearchStores()
        {
            StoreController controller = new StoreController();
            List<ErpStore> result = null;
            try
            {
                result = controller.SearchStores();
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            System.Console.WriteLine(result.ToString());
        }

    }
}
