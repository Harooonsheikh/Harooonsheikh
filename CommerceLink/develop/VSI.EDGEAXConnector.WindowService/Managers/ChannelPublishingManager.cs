using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.WindowService.Managers
{
    public class ChannelPublishingManager: IJobManager
    {
        public static readonly string IDENTIFIER = "ChannelPublishingSync";
        public static readonly string GROUP = "Synchronization";
        private readonly IErpAdapterFactory _erpAdapterFactory;
        public StoreDto store = null;
       // public EmailSender emailSender = null;
       public ChannelPublishingManager()
        {
            _erpAdapterFactory = new ErpAdapterFactory();
        }
        public void ProcessChannelPublishing()
        {
            try
            {
                StringBuilder traceInfo = new StringBuilder();
                traceInfo.Append(string.Format("ProcesChannel Publishing Manager=>sChannelPublishing() Started at [{0}]", DateTime.UtcNow) + Environment.NewLine);
                var erpChannelPublishingController = _erpAdapterFactory.CreateChannelPublishingController(store.StoreKey);
                erpChannelPublishingController.ProcessChannelPublishing();
                traceInfo.Append(string.Format("Channel Publishing Manager=>ProcessChannelPublishing() Completed at [{0}]", DateTime.UtcNow) + Environment.NewLine);
                CustomLogger.LogTraceInfo(traceInfo.ToString(), store.StoreId, store.CreatedBy);
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("ProcessChannelPublishing() Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
            }
        }
        public bool Sync()
        {
            try
            {
                StringBuilder traceInfo = new StringBuilder();
                traceInfo.Append(string.Format("ProcesChannel Publishing Manager=>sChannelPublishing() Started at [{0}]", DateTime.UtcNow) + Environment.NewLine);
                var erpChannelPublishingController = _erpAdapterFactory.CreateChannelPublishingController(store.StoreKey);
                erpChannelPublishingController.ProcessChannelPublishing();
                traceInfo.Append(string.Format("Channel Publishing Manager=>ProcessChannelPublishing() Completed at [{0}]", DateTime.UtcNow) + Environment.NewLine);
                CustomLogger.LogTraceInfo(traceInfo.ToString(), store.StoreId, store.CreatedBy);
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("ProcessChannelPublishing() Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
        }
        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.ChannelPublishingSync);
            return job;
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.ChannelPublishingSync, this.store.StoreId);
        }
        public void InitializeParameter()
        {
        }
    }
}
