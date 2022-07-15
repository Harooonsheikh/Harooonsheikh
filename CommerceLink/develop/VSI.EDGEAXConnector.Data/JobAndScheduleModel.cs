using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data
{
   public class JobAndScheduleModel
    {
        public long JobID { get; set; }
        public string JobName { get; set; }
        public Nullable<bool> Enabled { get; set; }
        public Nullable<bool> JobTypeId { get; set; }
        public long JobSchduleId { get; set; }
        public long JobInterval { get; set; }
        public bool IsRepeatable { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public bool IsActive { get; set; }
        public int storeId { get; set; }
    }
}
