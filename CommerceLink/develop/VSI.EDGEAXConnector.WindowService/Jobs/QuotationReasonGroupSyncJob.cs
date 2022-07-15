using Autofac;
using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.WindowService.Managers;

namespace VSI.EDGEAXConnector.WindowService.Jobs
{
    public class QuotationReasonGroupSyncJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job quotationReasonGroupJob = JobRepo.GetJob((int)SyncJobs.QuotationReasonGroupSync);
                JobId = quotationReasonGroupJob.JobID;
                //if (quotationReasonGroupJob.JobStatus != (int)SynchJobStatus.InProgress) //TODOUsman
                //{
                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);

                    var manager = new FactoryManager();
                    var container = manager.Configure();
                    var quotationReasonGroupManager = container.Resolve<QuotationReasonGroupManager>();

                    quotationReasonGroupManager.SyncQuotationReasonGroup();

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

