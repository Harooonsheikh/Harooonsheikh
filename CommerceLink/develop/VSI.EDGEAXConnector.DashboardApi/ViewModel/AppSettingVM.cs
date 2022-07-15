using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.ViewModel
{
    public class AppSettingVM
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public int SortOrder { get; set; }
        public int IsActive { get; set; }
        public int StoreId { get; set; }
        public Nullable<int> FieldTypeId { get; set; }
        public bool IsUserForDuplicateStore { get; set; }
        public string FieldType { get; set; }
    }
}