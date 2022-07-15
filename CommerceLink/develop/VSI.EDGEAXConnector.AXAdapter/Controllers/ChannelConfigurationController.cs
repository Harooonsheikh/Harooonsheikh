//using Autofac;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    /// <summary>
    /// ChannelConfigurationController class provides features to get Channel Level Configurations from CRT.
    /// </summary>
    public class ChannelConfigurationController : BaseController, IChannelConfigurationController
    {

        public ChannelConfigurationController(string storeKey) : base(storeKey)
        {

        }
        #region Public Methods
        /// <summary>
        /// GetRetailServiceProfile gets Retail Service Profile data associated with channel.
        /// </summary>
        /// <returns></returns>
        public ErpRetailServiceProfileResponse GetRetailServiceProfile()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpRetailServiceProfileResponse response = new ErpRetailServiceProfileResponse(false, "", null); 

            var configurationManager = new ChannelConfigurationCRTManager();
            response = configurationManager.GetRetailServiceProfile(currentStore.StoreKey);
            
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return response;
        }

        /// <summary>
        /// GetRetailChannelProfile gets Retail Channel Profile data associated with channel.
        /// </summary>
        /// <returns></returns>
        public ErpRetailChannelProfileResponse GetRetailChannelProfile()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpRetailChannelProfileResponse response = new ErpRetailChannelProfileResponse(false, "", null);

            var configurationManager = new ChannelConfigurationCRTManager();
            response = configurationManager.GetRetailChannelProfile(currentStore.StoreKey);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return response;
        }

        /// <summary>
        /// GetChannelInformation get channel information of currently configured store.
        /// </summary>
        /// <returns></returns>
        public ErpChannel GetChannelInformation()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var configurationManager = new ChannelConfigurationCRTManager();
            var channelInformation = configurationManager.GetChannelInformation(currentStore.StoreKey);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return channelInformation;
        }
        #endregion
    }
}
