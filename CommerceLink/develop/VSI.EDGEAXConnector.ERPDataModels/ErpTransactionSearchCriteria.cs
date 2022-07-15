using System;
using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 9 Platform new class
    public class ErpTransactionSearchCriteria
    {
        public ErpTransactionSearchCriteria()
        {

        }

        public string SearchIdentifiers { get; set; }
        public string ReceiptEmailAddress { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public string StaffId { get; set; }
        public string SerialNumber { get; set; }
        public string Barcode { get; set; }
        public string ItemId { get; set; }
        public string TerminalId { get; set; }
        public string StoreId { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string ChannelReferenceId { get; set; }
        public string ReceiptId { get; set; }
        public string SalesId { get; set; }
        public ObservableCollection<string> TransactionIds { get; set; }
        public int? SearchLocationTypeValue { get; set; }
        //NS: D365 Update 12 Platform change start
        public ObservableCollection<ErpSearchFilter> CustomFilters { get; set; }
        //NS: D365 Update 12 Platform change end
    }
}
