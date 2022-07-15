using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.ViewModel
{
    public class WorkFlowStatesVM
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Display { get; set; }

        public WorkFlowStatesVM()
        {
            this.Display = false;
        }

        public WorkFlowStatesVM(string name, string value, bool display)
        {
            this.Name = name;
            this.Value = value;
            this.Display = display;
        }
    }
}