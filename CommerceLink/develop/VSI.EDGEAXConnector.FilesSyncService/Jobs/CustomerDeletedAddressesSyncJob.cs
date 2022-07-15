using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.FilesSyncService
{

    /// <summary>
    /// CustomerDeletedAddressesSyncJob used to execute deleted address sync job.
    /// </summary>
    class CustomerDeletedAddressesSyncJob : IJob
    {

        #region Public Methods

        /// <summary>
        /// execute deleted address sync job.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job customeDeletedAddressesrJob = JobRepo.GetJob((int)SyncJobs.DownloadCustomerDeletedAddressesSync);
                JobId = customeDeletedAddressesrJob.JobID;

                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);

                var customerManager = new CustomerManager();

                customerManager.DownloadDeletedAddressesSync();

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
