using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.FileSyncService
{
    public class SalesOrderStatusSyncJob : IJob
    {
        #region Public Methods

        /// <summary>
        /// execute SalesOrderStatusSync job.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job SalesOrderStatusJob = JobRepo.GetJob((int)SyncJobs.UploadSalesOrderStatusSync);
                JobId = SalesOrderStatusJob.JobID;
                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);
                var manager = new FilesSyncService.SalesOrderStatusManager();
                manager.UploadSalesOrderStatusSync();
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
