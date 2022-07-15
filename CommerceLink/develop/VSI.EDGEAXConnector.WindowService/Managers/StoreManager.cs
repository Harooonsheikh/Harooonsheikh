using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.WindowService.Managers;

namespace VSI.EDGEAXConnector.WindowService
{
    public class StoreManager : IJobManager
    {
        public static readonly string IDENTIFIER = "StoreSync";
        public static readonly string GROUP = "Synchronization";
        private readonly IEComAdapterFactory _eComAdapterFactory;
        private readonly IErpAdapterFactory _erpAdapterFactory;
        public StoreDto store = null;
        EmailSender emailSender = null;

        public StoreManager()
        {
            _erpAdapterFactory = new ErpAdapterFactory();
            _eComAdapterFactory = new EComAdapterFactory();
        }
        public string GetIdentifier()
        {
            return IDENTIFIER;
        }
        public string GetGroup()
        {
            return GROUP;
        }
        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.StoreSync);
            return job;
        }
        public bool Sync()
        {
            try
            {
                var erpStoreController = _erpAdapterFactory.CreateStoreController(store.StoreKey);
                // Code For Vitamin World, with New Mapping engine
                List<ErpStore> erpStoreslst = erpStoreController.GetStores();
                ErpStoreInfo erpStoreInfo = new ErpStoreInfo();
                erpStoreInfo.stores = erpStoreslst;
                using (var ecomStoreController = _eComAdapterFactory.CreateStoreController(store.StoreKey))
                {
                    ecomStoreController.PushStoresInfo(erpStoreInfo);
                }
                return true;

            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                emailSender.NotifyThroughEmail("", ex.ToString(), "", (int)Common.Enums.EmailTemplateId.Store);
                throw;
            }

        }
        public void SetStore(StoreDto store)
        {
     
            this.store = store;
        }
        public void UpdateJobStatus(JobSchedule jobSchedule, Common.Enums.SynchJobStatus status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.UpdateJobStatus(jobSchedule.JobId, (int)status, this.store.StoreId);
        }
        public void JobLog(JobSchedule jobSchedule, int status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.JobLog(jobSchedule.JobId, status);
        }
        public bool IsJobCompletedTodayInJobLog(JobSchedule jobSchedule, int jobStatus)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.IsJobCompletedTodayInJobLog(jobSchedule.JobId, jobStatus);
        }
        public JobSchedule GetSchedule()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.StoreSync, this.store.StoreId);
        }
        public void InitializeParameter()
        {
            emailSender = new EmailSender(store.StoreKey);
        }
    }
}
