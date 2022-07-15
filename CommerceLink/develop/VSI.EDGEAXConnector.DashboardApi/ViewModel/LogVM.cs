using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.ViewModel
{
    public class LogVM
    {
        public int LogId { get; set; }
        public int StoreId { get; set; }
        public string Level { get; set; }
        public string StackTrace { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}