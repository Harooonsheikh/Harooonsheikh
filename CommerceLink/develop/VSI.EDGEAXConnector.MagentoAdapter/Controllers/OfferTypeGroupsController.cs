using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
{

    /// <summary>
    /// ChannelConfigurationController class performs configuration related activities.
    /// </summary>
    public class OfferTypeGroupsController : BaseController, IOfferTypeGroupController
    {

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OfferTypeGroupsController()
            : base(false)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushConfiguration push configurations to Magento
        /// </summary>
        /// <param name="configuration"></param>
        public void PushOfferTypeGoups(List<ERPOfferTypeGroup> offerTypeGroups)
        {
            CustomLogger customLogger = new CustomLogger();
            customLogger.LogDebugInfo(string.Format("Enter in PushOfferTypeGroups()"));
            try
            {
                ERPOfferTypeGroups eRPOfferTypeGroups = new ERPOfferTypeGroups();
                eRPOfferTypeGroups.erpOfferTypeGroup = offerTypeGroups;

                this.CreateFile(eRPOfferTypeGroups);
                customLogger.LogDebugInfo(string.Format("Exit from CreateFile()"));
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(StoreService.StoreLkey);
                obj.LogTransaction(SyncJobs.ConfigurationSync, "OfferType XML generation Failed", DateTime.UtcNow, null);
                customLogger.LogException(exp);
                throw;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// CreateFile create target file with provided data.
        /// </summary>
        /// <param name="configuration"></param>
        private void CreateFile(ERPOfferTypeGroups offerTypeGroups)
        {
            CustomLogger customLogger = new CustomLogger();
            customLogger.LogDebugInfo(string.Format("Enter in CreateFile()"));
            if (offerTypeGroups != null  )
            {
                string fileNameConfiguration = configurationHelper.GetSetting(OFFERTYPEGROUPS.Filename_Prefix) + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";
                XmlTemplateHelper xmlHelper = new XmlTemplateHelper();
                xmlHelper.GenerateXmlUsingTemplate(fileNameConfiguration, ConfigurationHelper.GetDirectory(configurationHelper.GetSetting(OFFERTYPEGROUPS.Local_Output_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, offerTypeGroups);

                customLogger.LogDebugInfo(string.Format("File {0} has been completed in CreateFile()", fileNameConfiguration));
            }
        }

        #endregion
    }
}
