using System;
using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpTransferOrderJournal
    {
        public ErpTransferOrderJournal()
        {

        }

        public string OrderId { get; set; }
        public string VoucherId { get; set; }
        public string UpdatedByWorker { get; set; }
        public string InventLocationIdFrom { get; set; }
        public string InventLocationIdTo { get; set; }
        public decimal? QuantityShipped { get; set; }
        public DateTimeOffset PostingDate { get; set; }
        public ObservableCollection<ErpTransferOrderJournalLine> JournalLines { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }

        //NS: D365 Update 8.1 Application change start
        public string DeliveryMode { get; set; }
        public string Comments { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}
