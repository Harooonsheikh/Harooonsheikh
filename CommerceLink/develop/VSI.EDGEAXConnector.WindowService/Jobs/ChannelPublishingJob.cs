using Autofac;
using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.WindowService.Managers;

namespace VSI.EDGEAXConnector.WindowService
{
    class ChannelPublishingJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job channelPublishingJob = JobRepo.GetJob((int)SyncJobs.ChannelPublishingSync);
                JobId = channelPublishingJob.JobID;
                //if (channelPublishingJob. != (int)SynchJobStatus.InProgress)
                //{
                    JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);

                    var manager = new FactoryManager();
                    var container = manager.Configure();
                    var channelPublishingManager = container.Resolve<ChannelPublishingManager>();
                    channelPublishingManager.ProcessChannelPublishing();
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
