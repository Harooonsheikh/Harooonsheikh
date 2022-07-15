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
    public class QuotationReasonGroupsController : BaseController, IQuotationReasonGroupController
    {

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public QuotationReasonGroupsController(string storeKey) : base(false, storeKey)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushConfiguration push configurations to Magento
        /// </summary>
        /// <param name="configuration"></param>
        public void PushQuotationReasonGoups(List<ERPQuotationReasonGroup> quotationReasonGroups)
        {
            CustomLogger customLogger = new CustomLogger();
            customLogger.LogDebugInfo(string.Format("Enter in PushQuotationReasonGroups()"));
            try
            {
                ERPQuotationReasonGroups eRPQuotationReasonGroups = new ERPQuotationReasonGroups();
                eRPQuotationReasonGroups.erpQuotationReasonGroup = quotationReasonGroups;

                this.CreateFile(eRPQuotationReasonGroups);
                customLogger.LogDebugInfo(string.Format("Exit from CreateFile()"));
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(currentStore.StoreKey);
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
        private void CreateFile(ERPQuotationReasonGroups quotationReasonGroup)
        {
            CustomLogger customLogger = new CustomLogger();
            customLogger.LogDebugInfo(string.Format("Enter in CreateFile()"));

            if (quotationReasonGroup != null  )
            {
                string fileNameConfiguration = configurationHelper.GetSetting(QUOTATIONREASONGROUP.Filename_Prefix) + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";
                XmlTemplateHelper xmlHelper = new XmlTemplateHelper();
                xmlHelper.GenerateXmlUsingTemplate(fileNameConfiguration, ConfigurationHelper.GetDirectory(configurationHelper.GetSetting(QUOTATIONREASONGROUP.Local_Output_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, quotationReasonGroup);

                customLogger.LogDebugInfo(string.Format("File {0} has been completed in CreateFile()", fileNameConfiguration));
            }
        }

        #endregion
    }
}
