using System;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.CommerceLink.MagentoAdapter.Controllers
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
            CustomLogger.LogDebugInfo(string.Format("Enter in PushConfiguration()"), currentStore.StoreId, currentStore.CreatedBy);
            try
            {
                this.CreateFile(configuration);
                CustomLogger.LogDebugInfo(string.Format("Exit from CreateFile()"), currentStore.StoreId, currentStore.CreatedBy);
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(currentStore.StoreKey);
                obj.LogTransaction(SyncJobs.ConfigurationSync, "Configuration XML generation Failed", DateTime.UtcNow, null);
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
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
            CustomLogger.LogDebugInfo(string.Format("Enter in CreateFile()"), currentStore.StoreId, currentStore.CreatedBy);

            if (configuration != null && configuration.Channel != null)
            {
                string fileNameConfiguration = configurationHelper.GetSetting(CHANNELCONFIGURATION.Filename_Prefix) + currentStore.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + ".xml";
                XmlTemplateHelper xmlHelper = new XmlTemplateHelper(currentStore);
                xmlHelper.GenerateXmlUsingTemplate(fileNameConfiguration, this.configurationHelper.GetDirectory(configurationHelper.GetSetting(CHANNELCONFIGURATION.Local_Output_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, configuration);

                CustomLogger.LogDebugInfo(string.Format("File {0} has been completed in CreateFile()", fileNameConfiguration), currentStore.StoreId, currentStore.CreatedBy);
            }
        }
        #endregion
    }
}
