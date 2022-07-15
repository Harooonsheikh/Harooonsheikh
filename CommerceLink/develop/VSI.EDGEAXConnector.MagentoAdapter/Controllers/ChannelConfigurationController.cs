using System;
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
    public class ChannelConfigurationController : BaseController, IChannelConfigurationController
    {

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelConfigurationController(string storeKey) : base(false, storeKey)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushConfiguration push configurations to Magento
        /// </summary>
        /// <param name="configuration"></param>
        public void PushConfiguration(ErpConfiguration configuration)
        {
            CustomLogger customLogger = new CustomLogger();
            customLogger.LogDebugInfo(string.Format("Enter in PushConfiguration()"));
            try
            {
                this.CreateFile(configuration);
                customLogger.LogDebugInfo(string.Format("Exit from CreateFile()"));
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(currentStore.StoreKey);
                obj.LogTransaction(SyncJobs.ConfigurationSync, "Configuration XML generation Failed", DateTime.UtcNow, null);
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
        private void CreateFile(ErpConfiguration configuration)
        {
            CustomLogger customLogger = new CustomLogger();
            customLogger.LogDebugInfo(string.Format("Enter in CreateFile()"));

            if (configuration != null && configuration.ServiceProfile != null)
            {
                string fileNameConfiguration = configurationHelper.GetSetting(CHANNELCONFIGURATION.Filename_Prefix) + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";
                XmlTemplateHelper xmlHelper = new XmlTemplateHelper();
                xmlHelper.GenerateXmlUsingTemplate(fileNameConfiguration, ConfigurationHelper.GetDirectory(configurationHelper.GetSetting(CHANNELCONFIGURATION.Local_Output_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, configuration);
                customLogger.LogDebugInfo(string.Format("File {0} has been completed in CreateFile()", fileNameConfiguration));
            }
        }
        #endregion
    }
}
