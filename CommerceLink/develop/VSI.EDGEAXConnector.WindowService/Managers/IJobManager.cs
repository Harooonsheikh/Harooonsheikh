using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;

namespace VSI.EDGEAXConnector.WindowService.Managers
{
    interface IJobManager
    {
        bool Sync();
        Job GetJob();
        void UpdateJobStatus(JobSchedule jobstatus, SynchJobStatus status);
        void JobLog(JobSchedule jobLog, int status);
        string GetIdentifier();
        string GetGroup();
        void SetStore(StoreDto store);
        JobSchedule GetSchedule();
        void InitializeParameter();
        bool IsJobCompletedTodayInJobLog(JobSchedule job, int status);

    }
}
