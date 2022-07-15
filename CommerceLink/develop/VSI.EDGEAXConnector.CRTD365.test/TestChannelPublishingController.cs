using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.ERPDataModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using VSI.EDGEAXConnector.CRTD365.Controllers;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    [TestClass]
    public class TestChannelPublishingController
    {
        string storeKey = "";

        [TestMethod]
        public void GetOnlineChannelPublishStatus()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            int channelPublishingStatus = -1;

            try
            {
                ChannelPublishingController channelPublishingController = new ChannelPublishingController(storeKey);
                channelPublishingStatus = channelPublishingController.GetOnlineChannelPublishStatus();
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsTrue(channelPublishingStatus > -1);
            System.Console.WriteLine( "Channel Publishing status is = " +    channelPublishingStatus.ToString() );
        }

      
        [TestMethod]
        public void testSetOnlineChannelPublishStatus()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            int channelPublishingStatus = 3;
            string statusMessage = "Marked Published";

            try
            {
                VSI.EDGEAXConnector.CRTD365.Controllers.ChannelPublishingController channelPublishingController = new VSI.EDGEAXConnector.CRTD365.Controllers.ChannelPublishingController(storeKey);
                channelPublishingController.SetOnlineChannelPublishingStatus(channelPublishingStatus, statusMessage);

            }
            catch (System.Exception e)
            {
                channelPublishingStatus = -1;
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(channelPublishingStatus);
            Assert.IsTrue(channelPublishingStatus > -1);
            System.Console.WriteLine("Result is = " + channelPublishingStatus.ToString());
        }

        [TestMethod]
        public void testGetChannelProductAttributes()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            List<ErpAttributeProduct> listErpAttributeProduct = new List<ErpAttributeProduct>();

            try
            {
                VSI.EDGEAXConnector.CRTD365.Controllers.ChannelPublishingController channelPublishingController = new VSI.EDGEAXConnector.CRTD365.Controllers.ChannelPublishingController(storeKey);
                listErpAttributeProduct = channelPublishingController.GetChannelProductAttributes();
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(listErpAttributeProduct);
            Assert.IsTrue(listErpAttributeProduct.Count > 0);
            System.Console.WriteLine("Total products for channel product attributes are = " + listErpAttributeProduct.ToString());
        }
    
        [TestMethod]
        public void GetCategoryAttributes()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            Tuple<IEnumerable<ErpCategory>, Dictionary<long, IEnumerable<ErpAttributeCategory>>> categoryAttributes = null;

            try
            {
                VSI.EDGEAXConnector.CRTD365.Controllers.ChannelPublishingController channelPublishingController = new VSI.EDGEAXConnector.CRTD365.Controllers.ChannelPublishingController(storeKey);
                categoryAttributes = channelPublishingController.GetCategoryAttributes();
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(categoryAttributes);
            Assert.IsNotNull(categoryAttributes.Item1);
            Assert.IsNotNull(categoryAttributes.Item2);
            //System.Console.WriteLine("Total products for channel product attributes are = " + listErpAttributeProduct.ToString());
        }


    }
}
