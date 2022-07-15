using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.ViewModel
{
    public class StoreActionVM
    {
        public StoreActionVM()
        {
            this.ActionParams = new List<ActionParamVM>();
        }

        public int ActionId { get; set; }
        public int StoreId { get; set; }
        public string ActionName { get; set; }
        public string ActionRoute { get; set; }
        public string RequestType { get; set; }
        public List<ActionParamVM> ActionParams { get; set; }
    }
}