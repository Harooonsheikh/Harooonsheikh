using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.FileSyncService
{

    /// <summary>
    /// ConfigurationSyncJob used to execute Configuration sync job.
    /// </summary>
    public class ConfigurationSyncJob : IJob
    {

        #region Public Methods

        /// <summary>
        /// execute Configuration sync job.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job configurationJob = JobRepo.GetJob((int)SyncJobs.UploadConfigurationSync);
                JobId = configurationJob.JobID;

                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);

                var configurationManager = new FilesSyncService.ChannelConfigurationManager();

                configurationManager.UploadConfigurationSync();

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

