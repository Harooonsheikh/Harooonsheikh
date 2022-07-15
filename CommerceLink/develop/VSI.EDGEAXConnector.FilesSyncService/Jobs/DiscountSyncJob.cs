using Quartz;
using System;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.FileSyncService
{

    /// <summary>
    /// DiscountSyncJob used to execute discount sync job.
    /// </summary>
    public class DiscountSyncJob : IJob
    {

        #region Public Methods

        /// <summary>
        /// execute discount sync job.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job DiscountJob = JobRepo.GetJob((int)SyncJobs.UploadDiscountSync);
                JobId = DiscountJob.JobID;

                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);

                var manager = new FilesSyncService.DiscountManager();

                manager.UploadDiscountsSync();

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
