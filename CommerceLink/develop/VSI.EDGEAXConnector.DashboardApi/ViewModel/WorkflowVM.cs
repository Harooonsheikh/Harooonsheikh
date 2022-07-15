using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.ViewModel
{
    public class WorkflowVM
    {
        public int Id { get; set; }
        public string InstanceName { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public int EntityId { get; set; }
        public long JobId { get; set; }
        public int StoreId { get; set; }
        public DateTime Updated { get; set; }

        public string Status { get; set; }
    }
}