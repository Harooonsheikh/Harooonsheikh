using Autofac;
using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.WindowService
{
    /// <summary>
    /// PriceSyncJob class is Price Sync Job.
    /// </summary>
    class PriceSyncJob : IJob
    {
        /// <summary>
        /// Execute method executes job task.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job PriceJob = JobRepo.GetJob((int)SyncJobs.PriceSync);
                JobId = PriceJob.JobID;
                //if (PriceJob.JobStatus != (int)SynchJobStatus.InProgress)
                //{
                    JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);
                    var manager = new FactoryManager();
                    var container = manager.Configure();
                    var PriceManager = container.Resolve<PriceManager>();

                    PriceManager.SyncPrices();
                    JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.Available);
                    JobRepo.JobLog(JobId, true);
                    
                //}
            }
            catch (Exception ex)
            {
                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.Available);
                CustomLogger.LogException(ex);
                JobRepo.JobLog(JobId, false);
                
            }
        }
    }

}
