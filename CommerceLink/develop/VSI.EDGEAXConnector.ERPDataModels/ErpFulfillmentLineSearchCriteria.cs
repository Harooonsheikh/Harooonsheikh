using System;
using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpFulfillmentLineSearchCriteria
    {
        public ErpFulfillmentLineSearchCriteria()
        {

        }

        public string WarehouseId { get; set; }
        public string DeliveryModeCode { get; set; }
        public ObservableCollection<int> FulfillmentStatusValues { get; set; }
        public DateTimeOffset RequestedDeliveryEndDate { get; set; }
        public DateTimeOffset RequestedDeliveryStartDate { get; set; }
        public DateTimeOffset RequestedReceiptEndDate { get; set; }
        public DateTimeOffset RequestedReceiptStartDate { get; set; }
        public int DeliveryTypeValue { get; set; }
        public string ChannelReferenceId { get; set; }
        public string ReceiptId { get; set; }
        public DateTimeOffset OrderCreatedEndDate { get; set; }
        public DateTimeOffset OrderCreatedStartDate { get; set; }
        public string SalesId { get; set; }
        public string EmailAddress { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public string StoreId { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
}
