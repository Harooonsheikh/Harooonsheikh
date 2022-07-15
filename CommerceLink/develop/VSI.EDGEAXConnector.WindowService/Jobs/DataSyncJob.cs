using Quartz;
using Quartz.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.WindowService.Managers;

namespace VSI.EDGEAXConnector.WindowService.Jobs
{
    class DataSyncJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            CustomLogger.LogSyncTrace(1, 0, "", "DataSyncJob.Execute", $"Entered Method");
            JobSchedule jobSchedule = null;
            IJobManager manager = null;

            try
            {
                manager = (IJobManager)context.JobDetail.JobDataMap["manager"];
                manager.InitializeParameter();
                CustomLogger.LogSyncTrace(manager.GetSchedule().StoreId, manager.GetSchedule().StoreId,
                    manager.GetSchedule().JobSchduleId.ToString(), "DataSyncJob.Execute", $"Initialize Parameter Success");
                jobSchedule = manager.GetSchedule();
                CustomLogger.LogSyncTrace(manager.GetSchedule().StoreId, manager.GetSchedule().StoreId,
                    manager.GetSchedule().JobSchduleId.ToString(), "DataSyncJob.Execute", $"GetSchedule Success");

                if (jobSchedule.JobStatus != (int)SynchJobStatus.InProgress)
                {
                    CustomLogger.LogSyncTrace(manager.GetSchedule().StoreId, manager.GetSchedule().StoreId,
                        manager.GetSchedule().JobSchduleId.ToString(), "DataSyncJob.Execute", $"Job Status Not in Progress");

                    manager.JobLog(jobSchedule, (int)JobLogStatuses.Started);

                    CustomLogger.LogSyncTrace(manager.GetSchedule().StoreId, manager.GetSchedule().StoreId,
                        manager.GetSchedule().JobSchduleId.ToString(), "DataSyncJob.Execute", $"Job Status: In Progress");

                    manager.UpdateJobStatus(jobSchedule, SynchJobStatus.InProgress);

                    CustomLogger.LogSyncTrace(manager.GetSchedule().StoreId, manager.GetSchedule().StoreId,
                        manager.GetSchedule().JobSchduleId.ToString(), "DataSyncJob.Execute", $"Sync Started");

                    manager.Sync();

                    manager.UpdateJobStatus(jobSchedule, SynchJobStatus.Available);
                    manager.JobLog(jobSchedule, (int)JobLogStatuses.Completed);

                    CustomLogger.LogSyncTrace(manager.GetSchedule().StoreId, manager.GetSchedule().StoreId,
                        manager.GetSchedule().JobSchduleId.ToString(), "DataSyncJob.Execute", $"Exection Completed");
                }
                else if (!(manager.IsJobCompletedTodayInJobLog(jobSchedule, (int)JobLogStatuses.Completed)))
                {
                    CustomLogger.LogSyncTrace(manager.GetSchedule().StoreId, manager.GetSchedule().StoreId,
                        manager.GetSchedule().JobSchduleId.ToString(), "DataSyncJob.Execute", $"Job Not Completed Today");

                    manager.UpdateJobStatus(jobSchedule, SynchJobStatus.InProgress);

                    CustomLogger.LogSyncTrace(manager.GetSchedule().StoreId, manager.GetSchedule().StoreId,
                        manager.GetSchedule().JobSchduleId.ToString(), "DataSyncJob.Execute", $"Sync Started");

                    manager.Sync();

                    CustomLogger.LogSyncTrace(manager.GetSchedule().StoreId, manager.GetSchedule().StoreId,
                        manager.GetSchedule().JobSchduleId.ToString(), "DataSyncJob.Execute", $"Sync Completed");

                    manager.UpdateJobStatus(jobSchedule, SynchJobStatus.Available);
                    manager.JobLog(jobSchedule, (int)JobLogStatuses.Completed);

                    CustomLogger.LogSyncTrace(manager.GetSchedule().StoreId, manager.GetSchedule().StoreId,
                        manager.GetSchedule().JobSchduleId.ToString(), "DataSyncJob.Execute", $"Exection Completed");

                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogSyncTrace(manager.GetSchedule().StoreId, manager.GetSchedule().StoreId,
                                        manager.GetSchedule().JobSchduleId.ToString(), "DataSyncJob.Execute", $"Sync Error");
                manager.UpdateJobStatus(jobSchedule, SynchJobStatus.Available);
                CustomLogger.LogException(ex, 1, "Global");
                manager.JobLog(jobSchedule, (int)JobLogStatuses.Error);
            }
        }
    }

    class MisfireLogger : TriggerListenerSupport
    {
        public override string Name => getName();
        private string getName()
        {
            return "Misfire Logging";
        }

        public override void TriggerMisfired(ITrigger trigger)
        {
            CustomLogger.LogSyncTrace(9999, 9999, trigger.Key.Name, "Trigger Missfired", $" Missfire instruction:{trigger.MisfireInstruction} ");
        }

    }

}
