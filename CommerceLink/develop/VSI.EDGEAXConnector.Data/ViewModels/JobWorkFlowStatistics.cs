using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.ViewModels
{
    public class JobWorkFlowStatistics
    {
        public long UniqueWorkFlowsCount { get; set; }
        public long InProcessWorkFlowsCount { get; set; }
        public long FailedWorkFlowsCount { get; set; }
        public long CompletedWorkFlowsCount { get; set; }
    }
}
