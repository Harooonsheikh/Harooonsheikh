using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.FileSyncService
{

    /// <summary>
    /// SalesOrderSyncJob used to execute Sales Order sync job.
    /// </summary>
    public class SalesOrderSyncJob : IJob
    {

        #region Public Methods

        /// <summary>
        /// execute SalesOrder sync job.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job salesOrderJob = JobRepo.GetJob((int)SyncJobs.DownloadSalesOrderSync);
                JobId = salesOrderJob.JobID;
                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);
                var salesOrderManager = new FilesSyncService.SalesOrderManager();

                salesOrderManager.DownloadSalesOrderSync();

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



