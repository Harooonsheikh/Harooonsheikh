using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.ViewModel
{
    public class WorkFlowTransitionVM
    {
        public long WorkFlowTransitionID { get; set; }
        public int InstanceID { get; set; }
        public int StateID { get; set; }
        public DateTime Created { get; set; }
    }
}