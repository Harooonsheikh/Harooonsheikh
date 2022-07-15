using Autofac;
using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.WindowService.Jobs
{
    public class InventorySyncJob : IJob 
    {
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job inventoryJob = JobRepo.GetJob((int)SyncJobs.InventorySynch);
                JobId = inventoryJob.JobID;
                //if (inventoryJob.JobStatus != (int)SynchJobStatus.InProgress) //TODO
                //{
                    JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);

                    var manager = new FactoryManager();
                    var container = manager.Configure();
                    var inventoryManager = container.Resolve<InventoryManager>();
                    inventoryManager.SyncInventory();
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

