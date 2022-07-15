using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.Infrastructure
{
    public class StoreAction
    {
        public StoreAction()
        {
            this.ActionParams = new HashSet<ActionParam>();
        }
        public int ActionId { get; set; }
        public int StoreId { get; set; }
        public string ActionName { get; set; }
        public string ActionRoute { get; set; }
        public string RequestType { get; set; }
        public virtual ICollection<ActionParam> ActionParams { get; set; }
        public virtual ApplicationStore Store { get; set; }


    }
}