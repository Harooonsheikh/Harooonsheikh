using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.WindowService.Managers
{
    /// <summary>
    /// ChannelConfigurationManager
    /// </summary>
    public class OfferTypeGroupManager
    {
        #region Properties
        private readonly IErpAdapterFactory _erpAdapterFactory;
        private readonly IEComAdapterFactory _eComAdapterFactory;
        public EmailSender emailSender = null;
        #endregion

        #region Constructor
        /// <summary>
        /// ChannelConfigurationManager constructor initialize the class object.
        /// </summary>
        /// <param name="erpAdapterFactory"></param>
        /// <param name="eComAdapterFactory"></param>
        public OfferTypeGroupManager(IErpAdapterFactory erpAdapterFactory, IEComAdapterFactory eComAdapterFactory)
        {
            this._erpAdapterFactory = erpAdapterFactory;
            this._eComAdapterFactory = eComAdapterFactory;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// SyncOfferTypeGroups the sync process to OfferType group from AX and to push to Ecom.
        /// </summary>
        /// <returns></returns>
        public bool SyncOfferTypeGroups()
        {
            try
            {

                CustomLogger.LogDebugInfo(string.Format("Enter in @@@@@@@@@@@@@ SyncOfferTypeGroups() @@@@@@@@@@@@@"), 1, "System");
                //Getting Configurations                
                var erpOfferTypeGroupController = _erpAdapterFactory.CreateOfferTypeGroupController("TODO");
                List<ERPOfferTypeGroup> erpOfferTypeGroups = new List<ERPOfferTypeGroup>();
                ERPOfferTypeGroupsResponse eRPOfferTypeGroupsResponse = erpOfferTypeGroupController.GetERPOfferTypeGroups();
                if (eRPOfferTypeGroupsResponse.Success)
                {
                    erpOfferTypeGroups = eRPOfferTypeGroupsResponse.OfferTypeGroups;
                }
                if (erpOfferTypeGroups != null)
                {
                    using (var ecomOfferTypeGroupController = _eComAdapterFactory.CreateIOfferTypeGroupController("TODO"))
                    {
                        ecomOfferTypeGroupController.PushOfferTypeGoups(erpOfferTypeGroups);
                        CustomLogger.LogDebugInfo(string.Format("Exit from SyncOfferTypeGroups()"), 1, "System");
                    }
                }
                else
                {
                    CustomLogger.LogWarn(string.Format("No Groups received, Please check Logs"),1, "System");
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, 1, "System");
                emailSender.NotifyThroughEmail(string.Empty, ex.ToString(), string.Empty, (int)EmailTemplateId.SimpleNotification);
                return false;
            }
        }
        #endregion

    }
}
