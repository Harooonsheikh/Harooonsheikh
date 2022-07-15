using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.FileSyncService
{

    /// <summary>
    /// CustomerSyncJob used to execute customer sync job.
    /// </summary>
    class CustomerSyncJob : IJob
    {

        #region Public Methods
        /// <summary>
        /// execute customer sync job.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job customerJob = JobRepo.GetJob((int)SyncJobs.DownloadCustomerSync);
                JobId = customerJob.JobID;

                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);

                var customerManager = new FilesSyncService.CustomerManager();

                customerManager.DownloadCustomerSync();

                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.Available);
                JobRepo.JobLog(JobId, true);
            }
            catch (Exception ex)
            {
                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.Available);
                CustomLogger.LogException(ex);
                JobRepo.JobLog(JobId, false);
            }
        }
        #endregion
    }
}
