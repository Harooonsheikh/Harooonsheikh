using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpNotificationDetail
    {
        public ErpNotificationDetail()
        { }

        public string ActionProperty { get; set; }
        public long? ItemCount { get; set; }
        public string DisplayText { get; set; }
        public bool? IsNew { get; set; }
        public string LastUpdatedDateTimeStr { get; set; }
        public bool? IsSuccess { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }

        //NS: D365 Update 8.1 Application change start
        public bool IsLiveContentOnly { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}
