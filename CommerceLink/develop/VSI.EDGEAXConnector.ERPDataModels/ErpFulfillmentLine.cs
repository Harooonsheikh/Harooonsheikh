using System;
using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpFulfillmentLine
    {
        public ErpFulfillmentLine()
        {

        }

        public decimal Balance { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset RequestedReceiptDate { get; set; }
        public DateTimeOffset RequestedDeliveryDate { get; set; }
        public DateTimeOffset RequestedShipDate { get; set; }
        public string UnitOfMeasureSymbol { get; set; }
        public int FulfillmentStatusValue { get; set; }
        public ErpFulfillmentLineStatus FulfillmentStatus { get; set; }
        public string ItemName { get; set; }
        public string ProductVariant { get; set; }
        public long ProductId { get; set; }
        public string ItemId { get; set; }
        public ErpAddress ShippingAddress { get; set; }
        public decimal QuantityInvoiced { get; set; }
        public decimal QuantityPicked { get; set; }
        public decimal QuantityOrdered { get; set; }
        public int DeliveryTypeValue { get; set; }
        public ErpFulfillmentLineDeliveryType FulfillmentDeliveryType { get; set; }
        public int DocumentStatusValue { get; set; }
        public string DeliveryModeCode { get; set; }
        public string EmailAddress { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public string ChannelReferenceId { get; set; }
        public string ReceiptId { get; set; }
        public decimal SalesLineNumber { get; set; }
        public string SalesId { get; set; }
        public decimal QuantityPacked { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }

        //NS: D365 Update 8.1 Application change start
        public string SalesPersonStoreId { get; set; }
        public string SalesPersonId { get; set; }
        public string SalesPersonFirstName { get; set; }
        public string SalesPersonLastName { get; set; }
        public decimal StoreInventoryTotalQuantity { get; set; }
        public decimal StoreInventoryReservedQuantity { get; set; }
        public decimal StoreInventoryOrderedQuantity { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}