using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.ViewModel
{
    public class ActionParamVM
    {
        public int ParamId { get; set; }
        public int ActionId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}