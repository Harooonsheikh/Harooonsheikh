using System;
using System.IO;
using System.Linq;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.WindowService.Managers;

namespace VSI.EDGEAXConnector.WindowService
{
    public class DataDeleteManager : IJobManager
    {
        public static readonly string IDENTIFIER = "DataDeleteSync";
        public static readonly string GROUP = "Synchronization";
        private readonly IEComAdapterFactory _eComAdapterFactory;
        private readonly IErpAdapterFactory _erpAdapterFactory;
        public StoreDto store = null;
        ConfigurationHelper configurationHelper;
        EmailSender emailSender = null;
        public FileHelper fileHelper = null;
        public DataDeleteManager()
        {
            //_erpAdapterFactory = erpAdapterFactory;
            _eComAdapterFactory = new EComAdapterFactory();
        }

        public bool Sync()
        {
            try
            {
                CustomLogger.LogDebugInfo("Sync Data Delete Started", store.StoreId, store.CreatedBy);
                using (var eComDataDeleteController = _eComAdapterFactory.CreateDataDeleteController(store.StoreKey))
                {
                    CustomLogger.LogDebugInfo("CreateDataDeleteController Instance Created", store.StoreId, store.CreatedBy);
                    eComDataDeleteController.DataDeleteSync();
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                //emailSender.NotifyThroughEmail("", ex.ToString(), "", (int)Common.Enums.EmailTemplateId.DataDelete);

                throw;
            }
        }

        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.DataDeleteSync);
            return job;
        }

        public void JobLog(Job jb, int status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.JobLog(jb.JobID, status);
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.DataDeleteSync, this.store.StoreId);
        }

        public void InitializeParameter()
        {
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
            fileHelper = new FileHelper(store.StoreKey);
        }
        
    }

}
