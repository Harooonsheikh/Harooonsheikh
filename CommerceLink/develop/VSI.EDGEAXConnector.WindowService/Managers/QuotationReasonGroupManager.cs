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
    public class QuotationReasonGroupManager :IJobManager
    {
        #region Properties
        private readonly IErpAdapterFactory _erpAdapterFactory;
        private readonly IEComAdapterFactory _eComAdapterFactory;

        public static readonly string IDENTIFIER = "QuotationReasonGroupSync";
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
        public QuotationReasonGroupManager()
        {
            this._erpAdapterFactory = new ErpAdapterFactory();
            this._eComAdapterFactory =  new EComAdapterFactory();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// SyncOfferTypeGroups the sync process to OfferType group from AX and to push to Ecom.
        /// </summary>
        /// <returns></returns>

        public bool Sync()
        {
            try
            {
                CustomLogger.LogDebugInfo(string.Format("Enter in @@@@@@@@@@@@@ SyncQuotationReasonGroup() @@@@@@@@@@@@@"), store.StoreId, store.CreatedBy);

                //Getting Configurations                
                var erpQuotationReasonController = _erpAdapterFactory.CreateQuotationController(store.StoreKey);
                List<ERPQuotationReasonGroup> erpQuotationReasonGroups = new List<ERPQuotationReasonGroup>();
                ERPQuotationReasonGroupsResponse eRPQuotationReasonGroups = erpQuotationReasonController.GetQuotationReasonGroups(store.StoreKey);
                if (eRPQuotationReasonGroups.Success)
                {
                    erpQuotationReasonGroups = eRPQuotationReasonGroups.QuotationReasonGroup;

                }
                if (erpQuotationReasonGroups != null)
                {
                    using (var ecomQuotationReasonGroupController = _eComAdapterFactory.CreateIQuotationReasonController(store.StoreKey))
                    {
                        ecomQuotationReasonGroupController.PushQuotationReasonGoups(erpQuotationReasonGroups);
                        CustomLogger.LogDebugInfo(string.Format("Exit from SyncQuotationReasonGroup()"), store.StoreId, store.CreatedBy);
                    }
                }
                else
                {
                    CustomLogger.LogWarn(string.Format("No Groups received, Please check Logs"), store.StoreId, store.CreatedBy);
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                emailSender.NotifyThroughEmail(string.Empty, ex.ToString(), string.Empty, (int)Common.Enums.EmailTemplateId.QuotationReasonGroup);
                throw;
            }
        }

        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Enums.SyncJobs.QuotationReasonGroupSync);
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
            return jobRepo.GetJobSchedule((int)Enums.SyncJobs.QuotationReasonGroupSync, this.store.StoreId);
        }

        public void InitializeParameter()
        {
            EmailSender emailSender = new EmailSender(store.StoreKey);
        }
        #endregion

    }
}
