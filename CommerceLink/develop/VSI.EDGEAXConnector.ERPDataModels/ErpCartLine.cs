using System;
using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCartLine
	{
		public ErpCartLine()
		{
		}

        public ObservableCollection<ErpAttributeValueBase> AttributeValues { get; set; }
        public string Barcode { get; set; }
        public long? CatalogId { get; set; }
        public ObservableCollection<ErpChargeLine> ChargeLines { get; set; }
        public string Comment { get; set; }
        public string CommissionSalesGroup { get; set; }
        public string DeliveryMode { get; set; }
        public decimal? DeliveryModeChargeAmount { get; set; }
        public string Description { get; set; }
        public decimal? DiscountAmount { get; set; }
        public ObservableCollection<ErpDiscountLine> DiscountLines { get; set; }
        public string ElectronicDeliveryEmail { get; set; }
        public string ElectronicDeliveryEmailContent { get; set; }
        public ErpBarcodeEntryMethodType EntryMethodType { get; set; }
        public int? EntryMethodTypeValue { get; set; }
        public decimal? ExtendedPrice { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
        public string FulfillmentStoreId { get; set; }
        public string InventoryDimensionId { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string InvoiceId { get; set; }
        public bool? IsCustomerAccountDeposit { get; set; }
        public bool? IsGiftCardLine { get; set; }
        public bool? IsInvoiceLine { get; set; }
        public bool? IsPriceKeyedIn { get; set; }
        public bool? IsPriceOverridden { get; set; }
        public bool? IsVoided { get; set; }
        public string ItemId { get; set; }
        public string ItemTaxGroupId { get; set; }
        public decimal? LineDiscount { get; set; }
        public string LineId { get; set; }
        public decimal? LineManualDiscountAmount { get; set; }
        public decimal? LineManualDiscountPercentage { get; set; }
        public decimal? LineNumber { get; set; }
        public decimal? LinePercentageDiscount { get; set; }
        public string LinkedParentLineId { get; set; }
        public decimal? NetAmountWithoutTax { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? Price { get; set; }
        public long? ProductId { get; set; }
        public ObservableCollection<string> PromotionLines { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityInvoiced { get; set; }
        public decimal? QuantityOrdered { get; set; }
        public ObservableCollection<ErpReasonCodeLine> ReasonCodeLines { get; set; }
        public DateTimeOffset? RequestedDeliveryDate { get; set; }
        public string ReturnInventTransId { get; set; }
        public decimal? ReturnLineNumber { get; set; }
        public string ReturnTransactionId { get; set; }
        public ErpSalesStatus SalesStatus { get; set; }
        public int? SalesStatusValue { get; set; }
        public string SerialNumber { get; set; }
        public ErpAddress ShippingAddress { get; set; }
        public string StaffId { get; set; }
        public decimal? TaxAmount { get; set; }
        public string TaxOverrideCode { get; set; }
        public decimal? TaxRatePercent { get; set; }
        public decimal? TotalAmount { get; set; }
        public string TrackingId { get; set; }
        public string UnitOfMeasureSymbol { get; set; }
        public string WarehouseId { get; set; }

        //NS: D365 Update 12 Platform change start

        public string GiftCardId { get; set; }
        public decimal QuantityCanceled { get; set; }
        public decimal GiftCardBalance { get; set; }
        public int LineVersion { get; set; }
        public ObservableCollection<string> RelatedDiscountedLineIds { get; set; }
        public decimal QuantityShipped { get; set; }
        public decimal QuantityRemained { get; set; }
        public ErpThirdPartyGiftCardInfo ThirdPartyGiftCardInfo { get; set; }
        public decimal BarcodeEmbeddedPrice { get; set; }
        public bool PriceInBarcode { get; set; }

        //NS: D365 Update 8.1 Application change start
        public decimal QuantityReturnable { get; set; }
        public int InvoiceTypeValue { get; set; }

        //NS: D365 Update 8.1 Application change end

        //HK: D365 Update 10.0 Application change start
        public bool IsTaxOverideCodeTaxExempt { get; set; }
        public string InvoicedSalesId { get; set; }
        //HK: D365 Update 10.0 Application change end
    }
}
