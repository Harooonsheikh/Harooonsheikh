using Autofac;
using Quartz;
using System;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.WindowService.Jobs
{
    public class SyncCustomerInMagentoJob : IJob 
    {
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job customerSyncInMagentoJob = JobRepo.GetJob((int)SyncJobs.CustomerSyncInMagento);
                JobId = customerSyncInMagentoJob.JobID;
                if (customerSyncInMagentoJob.JobID != (int)SynchJobStatus.InProgress) //TODO
                {
                    JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);
                    var manager = new FactoryManager();
                    var container = manager.Configure();
                    var customerManager = container.Resolve<CustomerManager>();
                    customerManager.SynchUpdatedCustomersAndAddresses();
                    JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.Available);
                    JobRepo.JobLog(JobId, true);
                    
                }
                else
                {
                    ConfigurationHelper configurationHelper = ConfigurationHelper.GetInstance;
                    int resetMinutes = 0;
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


    
  
   
     
 