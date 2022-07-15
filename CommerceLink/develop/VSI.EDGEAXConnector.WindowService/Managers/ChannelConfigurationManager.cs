using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
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
    public class ChannelConfigurationManager : IJobManager
    {
        #region Properties
        private readonly IErpAdapterFactory _erpAdapterFactory;
        private readonly IEComAdapterFactory _eComAdapterFactory;
        public static readonly string IDENTIFIER = "ConfigurationSync";
        public static readonly string GROUP = "Synchronization";
        public StoreDto store = null;
        public EmailSender emailSender = null;

         #endregion

        #region Constructor
        /// <summary>
        /// ChannelConfigurationManager constructor initialize the class object.
        /// </summary>
        /// <param name="erpAdapterFactory"></param>
        /// <param name="eComAdapterFactory"></param>
        public ChannelConfigurationManager()
        {
            this._erpAdapterFactory = new ErpAdapterFactory();
            this._eComAdapterFactory = new EComAdapterFactory();
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Sync initialize the sync process to get channel configurations from AX and to push to Ecom.
        /// </summary>
        /// <returns></returns>

        public bool Sync()
        {
            try
            {
                CustomLogger.LogDebugInfo(string.Format("Enter in @@@@@@@@@@@@@ SyncConfigurations() @@@@@@@@@@@@@"), store.StoreId, store.CreatedBy);
                //Getting Configurations                
                var erpConfigurationController = _erpAdapterFactory.CreateChannelConfigurationController(store.StoreKey);
                ErpConfiguration erpConfiguration = new ErpConfiguration();
                erpConfiguration.Channel = erpConfigurationController.GetChannelInformation();
                erpConfiguration.ServiceProfile = erpConfigurationController.GetRetailServiceProfile().RetailServiceProfile;
                erpConfiguration.ChannelProfile = erpConfigurationController.GetRetailChannelProfile().RetailChannelProfile;

                if (erpConfiguration.Channel != null)
                {
                    if (erpConfiguration.ChannelProfile == null)
                    {
                        erpConfiguration.ChannelProfile = new ErpRetailChannelProfile();
                    }
                    if (erpConfiguration.ServiceProfile == null)
                    {
                        erpConfiguration.ServiceProfile = new ErpRetailServiceProfile();
                    }
                    using (var ecomConfigurationController = _eComAdapterFactory.CreateChannelConfigurationController(store.StoreKey))
                    {
                        ecomConfigurationController.PushConfiguration(erpConfiguration);
                        CustomLogger.LogDebugInfo(string.Format("Exit from PushConfiguration()"), store.StoreId, store.CreatedBy);
                    }
                }
                else
                {
                    CustomLogger.LogWarn(string.Format("No Configurations generated, Please check Logs"), store.StoreId, store.CreatedBy);
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                emailSender.NotifyThroughEmail(string.Empty, ex.ToString(), string.Empty, (int)Common.Enums.EmailTemplateId.SimpleNotification);
                throw;
            }
        }

        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Enums.SyncJobs.ConfigurationSync);
            return job;
        }

        public void UpdateJobStatus(JobSchedule jobstatus, Common.Enums.SynchJobStatus status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.UpdateJobStatus(jobstatus.JobId, (int)status, this.store.StoreId);
        }

        public void JobLog(JobSchedule jobLog, int status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.JobLog(jobLog.JobId, status);
        }
        public bool IsJobCompletedTodayInJobLog(JobSchedule jobSchedule, int jobStatus)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.IsJobCompletedTodayInJobLog(jobSchedule.JobId, jobStatus);
        }

        public string GetIdentifier()
        {
            return IDENTIFIER;
        }

        public string GetGroup()
        {
            return GROUP;
        }

        public void SetStore(StoreDto store)
        {
            this.store = store;
        }

        public JobSchedule GetSchedule()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.GetJobSchedule((int)Enums.SyncJobs.ConfigurationSync, this.store.StoreId);
        }

        public void InitializeParameter()
        {
            this.emailSender = new EmailSender(store.StoreKey);
        }

        #endregion
    }
}
