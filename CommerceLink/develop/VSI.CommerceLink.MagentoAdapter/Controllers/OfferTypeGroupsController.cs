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

namespace VSI.CommerceLink.MagentoAdapter.Controllers
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
        public OfferTypeGroupsController(string storeKey) : base(false, storeKey)
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
            CustomLogger.LogDebugInfo(string.Format("Enter in PushOfferTypeGroups()"), currentStore.StoreId, currentStore.CreatedBy);
            try
            {
                ERPOfferTypeGroups eRPOfferTypeGroups = new ERPOfferTypeGroups();
                eRPOfferTypeGroups.erpOfferTypeGroup = offerTypeGroups;

                this.CreateFile(eRPOfferTypeGroups);
                CustomLogger.LogDebugInfo(string.Format("Exit from CreateFile()"), currentStore.StoreId, currentStore.CreatedBy);
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(currentStore.StoreKey);
                obj.LogTransaction(SyncJobs.ConfigurationSync, "OfferType XML generation Failed", DateTime.UtcNow, null);
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
        private void CreateFile(ERPOfferTypeGroups offerTypeGroups)
        {
            CustomLogger.LogDebugInfo(string.Format("Enter in CreateFile()"), currentStore.StoreId, currentStore.CreatedBy);

            if (offerTypeGroups != null  )
            {
                string fileNameConfiguration = configurationHelper.GetSetting(OFFERTYPEGROUPS.Filename_Prefix) + currentStore.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + ".xml";
                XmlTemplateHelper xmlHelper = new XmlTemplateHelper(currentStore);
                xmlHelper.GenerateXmlUsingTemplate(fileNameConfiguration, this.configurationHelper.GetDirectory(configurationHelper.GetSetting(OFFERTYPEGROUPS.Local_Output_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, offerTypeGroups);

                CustomLogger.LogDebugInfo(string.Format("File {0} has been completed in CreateFile()", fileNameConfiguration), currentStore.StoreId, currentStore.CreatedBy);
            }
        }

        #endregion
    }
}
