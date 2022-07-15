using Autofac;
using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.WindowService
{
    class CustomerDeletedAddressesSyncJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job customeDeletedAddressesrJob = JobRepo.GetJob((int)SyncJobs.CustomerDeletedAddressesSync);
                JobId = customeDeletedAddressesrJob.JobID;

                //if (customeDeletedAddressesrJob.JobStatus != (int)SynchJobStatus.InProgress)
                //{
                    JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);
                    var manager = new FactoryManager();
                    var container = manager.Configure();
                    var customerManager = container.Resolve<CustomerManager>();
                    customerManager.SyncDeletedAddresses();

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
