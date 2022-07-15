using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.WindowService.Jobs
{
    public class StoreSyncJob : IJob 
    {
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job storeJob = JobRepo.GetJob((int)SyncJobs.StoreSync);
                JobId = storeJob.JobID;
                //if (storeJob.JobStatus != (int)SynchJobStatus.InProgress)
                //{
                    JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);
                  
                    // Synch logic will be placed here.
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
    }
}


    
  
   