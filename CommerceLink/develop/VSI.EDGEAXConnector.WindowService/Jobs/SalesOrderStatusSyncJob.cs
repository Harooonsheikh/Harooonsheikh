using Autofac;
using Quartz;
using System;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.WindowService.Jobs
{
    public class SalesOrderStatusSyncJob : IJob 
    {
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job salesOrderJob = JobRepo.GetJob((int)SyncJobs.SalesOrderStatusSync);
                JobId = salesOrderJob.JobID;
                if (salesOrderJob.JobID != (int)SynchJobStatus.InProgress)//TODO
                {
                    JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);

                    var manager = new FactoryManager();
                    var container = manager.Configure();
                    var salesOrderManager = container.Resolve<SalesOrderManager>();
                    salesOrderManager.SyncOrderStatus();
                    JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.Available);
                    JobRepo.JobLog(JobId, true);
                    
                }
                else
                {
                    int resetMinutes = 0;
                    ConfigurationHelper configurationHelper = ConfigurationHelper.GetInstance;
                    if (int.TryParse(configurationHelper.GetSetting(APPLICATION.Reset_Time_InMinutes), out resetMinutes) && resetMinutes > 0)
                    {
                        DateTime lastCall = JobRepo.GetLastJobCallTimeStamp(JobId);
                        var mins = (DateTime.UtcNow - lastCall).TotalMinutes;
                        if (mins > resetMinutes)
                        {
                            JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.Available);
                            JobRepo.JobLog(JobId, false);
                        }
                    }

                }
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


    
  