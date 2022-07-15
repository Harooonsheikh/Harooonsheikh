using Quartz;
using System;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.FileSyncService
{

    /// <summary>
    /// CategorySyncJob used to execute category sync job.
    /// </summary>
    public class CategorySyncJob : IJob
    {

        #region Public Methods

        /// <summary>
        /// execute category sync job.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job categoryJob = JobRepo.GetJob((int)SyncJobs.CategorySync);
                JobId = categoryJob.JobID;
                //if (categoryJob.JobStatus != (int)SynchJobStatus.InProgress) //TODOUsman
                //{
                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);

                // Synch code will be placed here.
                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.Available);
                JobRepo.JobLog(JobId, true);
                // }
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
