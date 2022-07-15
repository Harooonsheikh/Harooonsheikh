using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.Data.ViewModels
{
    public class JobVM
    {
        public long JobID { get; set; }
        public string JobName { get; set; }
        public long JobInterval { get; set; }
        public bool IsRepeatable { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> JobStatus { get; set; }
        public Nullable<bool> JobTypeId { get; set; }
        public int StoreId { get; set; }
    }
}