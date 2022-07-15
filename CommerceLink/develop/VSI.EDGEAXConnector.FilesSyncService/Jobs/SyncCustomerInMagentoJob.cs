using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.FileSyncService
{

    /// <summary>
    /// SyncCustomerInMagentoJob used to execute CustomerInMagento sync job.
    /// </summary>
    public class SyncCustomerInMagentoJob : IJob
    {

        #region Public Methods

        /// <summary>
        /// execute CustomerInMagento sync job.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job customerSyncInMagentoJob = JobRepo.GetJob((int)SyncJobs.UploadCustomerSyncInMagento);
                JobId = customerSyncInMagentoJob.JobID;

                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);

                var customerManager = new FilesSyncService.CustomerManager();

                customerManager.UploadUpdatedCustomersAndAddressesSync();

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






