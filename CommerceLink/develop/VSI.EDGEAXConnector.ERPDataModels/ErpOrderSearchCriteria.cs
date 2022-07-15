using System;
using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 9 Platform new class
    public class ErpOrderSearchCriteria
    {
        public ErpOrderSearchCriteria()
        { }

        public string CustomerAccountNumber { get; set; }
        public string CustomerName { get; set; }
        public string EmailAddress { get; set; }
        public string SalesId { get; set; }
        public string ReceiptId { get; set; }
        public ObservableCollection<int> OrderStatusValues { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public string ChannelReferenceId { get; set; }
        public string StoreId { get; set; }
        public int? OrderType { get; set; }

        //NS: D365 Update 12 Platform change start
        public ObservableCollection<ErpSearchFilter> CustomFilters { get; set; }
        //NS: D365 Update 12 Platform change end
    }
}
