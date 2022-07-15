using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.ViewModel
{
    public class StoreVM
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public KeyValuePair<int, string> EcomType { get; set; }
        public KeyValuePair<int, string> ERPType { get; set; }
        public Nullable<int> OrganizationId { get; set; }
        public bool IsActive { get; set; }
        public string StoreKey { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<int> DuplicateOf { get; set; }


        public List<AppSettingVM> applist { get; set; }
    }
}