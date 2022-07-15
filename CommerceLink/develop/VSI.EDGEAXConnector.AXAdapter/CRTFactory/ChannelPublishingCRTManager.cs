using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class ChannelPublishingCRTManager 
    {
        private readonly ICRTFactory _crtFactory;

        //CustomLogger customLogger = null;

        public ChannelPublishingCRTManager()
        {
            _crtFactory = new CRTFactory();

        }

        public int GetOnlineChannelPublishStatus(string storeKey)
        {
           // CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000,, MethodBase.GetCurrentMethod().Name); //TODO

            int onlineChannelPublishStatus;

            // REMOVE THIS LINE // CustomLogger.LogTraceInfo("Inside VSI.EDGEAXConnector.AXAdapter.CRTFactory.ChannelPublishingCRTManager.GetOnlineChannelPublishStatus() going to call _crtFactory.CreateChannelPublishingController()");
            // REMOVE THIS LINE // CustomLogger.LogTraceInfo("_crtFactory.ToString() = " + _crtFactory.ToString());
           // CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10005, customLogger, MethodBase.GetCurrentMethod().Name, "_crtFactory,ToString()", _crtFactory, ToString());
            var channelPublishingController = _crtFactory.CreateChannelPublishingController(storeKey);
            // REMOVE THIS LINE // CustomLogger.LogTraceInfo("Inside VSI.EDGEAXConnector.AXAdapter.CRTFactory.ChannelPublishingCRTManager.GetOnlineChannelPublishStatus() going to call channelCRTManager.GetOnlineChannelPublishStatus()");
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10006, customLogger, MethodBase.GetCurrentMethod(), "channelPublishingController.GetOnlineChannelPublishStatus()");
            onlineChannelPublishStatus = channelPublishingController.GetOnlineChannelPublishStatus();

           // CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, customLogger, MethodBase.GetCurrentMethod().Name, onlineChannelPublishStatus.ToString());

           // CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, customLogger, MethodBase.GetCurrentMethod().Name);

            return onlineChannelPublishStatus;
        }

        public void SetOnlineChannelPublishStatus(int publishingStatus, string statusMessage, string storeKey)
        {
            var channelPublishingController = _crtFactory.CreateChannelPublishingController(storeKey);
            channelPublishingController.SetOnlineChannelPublishingStatus(publishingStatus, statusMessage);
        }

        public List<ErpAttributeProduct> GetChannelProductAttributes(string storeKey)
        {
            var channelPublishingController = _crtFactory.CreateChannelPublishingController(storeKey);
            return channelPublishingController.GetChannelProductAttributes();
        }

        public Tuple<IEnumerable<ErpCategory>, Dictionary<long, IEnumerable<ErpAttributeCategory>>> GetCategoryAttributes(string storeKey)
        {
            var channelPublishingController = _crtFactory.CreateChannelPublishingController(storeKey);
            return channelPublishingController.GetCategoryAttributes();
        }
    }
}
