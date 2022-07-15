using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.FileSyncService
{
    /// <summary>
    /// InventorySyncJob used to execute Inventory sync job.
    /// </summary>
    public class InventorySyncJob : IJob
    {
        #region Public Methods
        /// <summary>
        /// execute Inventory sync job.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job inventoryJob = JobRepo.GetJob((int)SyncJobs.UploadInventorySynch);
                JobId = inventoryJob.JobID;
                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);
                var inventoryManager = new FilesSyncService.InventoryManager();
                inventoryManager.UploadInventorySync();
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

