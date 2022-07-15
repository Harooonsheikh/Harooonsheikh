using Microsoft.Dynamics.Commerce.RetailProxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpCustomerOrderInfo
    {
        public ErpCustomerOrderInfo()
        {
            Affiliations = new ObservableCollection<ErpAffiliationInfo>();
            Charges = new ObservableCollection<ErpChargeInfo>();
            ExtensionProperties = new ObservableCollection<ErpCommerceProperty>();
            Items = new ObservableCollection<ErpItemInfo>();
            Payments = new ObservableCollection<ErpPaymentInfo>();
            ReasonCodeLines = new ObservableCollection<ErpReasonCodeLineInfo>();
            Taxes = new ObservableCollection<ErpTaxInfo>();

        }

        public string AddressRecordId { get; set; }
        public ObservableCollection<ErpAffiliationInfo> Affiliations { get; set; }
        public int AllocationPriority { get; set; }
        public bool AutoPickOrder { get; set; }
        public string ChannelRecordId { get; set; }
        public string ChannelReferenceId { get; set; }
        public ObservableCollection<ErpChargeInfo> Charges { get; set; }
        public string Comment { get; set; }
        public string CommissionSalesGroup { get; set; }
        public int ContinuityLineEval { get; set; }
        public string CreationDateString { get; set; }
        public string CreditCardToken { get; set; }
        public string CurrencyCode { get; set; }
        public string CustomerAccount { get; set; }
        public string CustomerRecordId { get; set; }
        public string DataAreaId { get; set; }
        public string DeliveryMode { get; set; }
        public int DocumentStatus { get; set; }
        public string Email { get; set; }
        public string ExpiryDateString { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }

        public bool HasLoyaltyPayment { get; set; }
        public string Id { get; set; }
        public bool IsCatalogUpSellShown { get; set; }
        public bool IsContinuityChild { get; set; }
        public bool IsContinuityOrder { get; set; }
        public bool IsFTCExempt { get; set; }
        public bool IsInstallmentBillingPrompt { get; set; }
        public bool IsInstallmentOrderSubmitted { get; set; }
        public bool IsPriceOverride { get; set; }
        public string IsTaxIncludedInPrice { get; set; }
        public ObservableCollection<ErpItemInfo> Items { get; set; }
        public int LocalHourOfDay { get; set; }
        public string LoyaltyCardId { get; set; }
        public string OrderType { get; set; }
        public DateTimeOffset OriginalTransactionTime { get; set; }
        public int OutOfBalanceReleaseType { get; set; }
        public int PaymentOutOfBalanceType { get; set; }
        public ObservableCollection<ErpPaymentInfo> Payments { get; set; }
        public ErpPreauthorization Preauthorization { get; set; }
        public decimal PrepaymentAmountApplied { get; set; }
        public bool PrepaymentAmountOverridden { get; set; }
        public decimal PreviouslyInvoicedAmount { get; set; }
        public string QuotationId { get; set; }
        public string RequestedDeliveryDateString { get; set; }
        public string ReturnReasonCodeId { get; set; }
        public decimal RoundingDifference { get; set; }
        public string SalespersonName { get; set; }
        public string SalespersonStaffId { get; set; }
        public string InventSiteId { get; set; }
        public string SourceId { get; set; }
        public int Status { get; set; }
        public string StoreId { get; set; }
        public ObservableCollection<ErpTaxInfo> Taxes { get; set; }
        public string TerminalId { get; set; }
        public decimal TotalManualDiscountAmount { get; set; }
        public decimal TotalManualDiscountPercentage { get; set; }
        public string TransactionId { get; set; }
        public string WarehouseId { get; set; }

        //NS: D365 Update 12 Platform change start
        public string TaxOverrideCode { get; set; }
        public ObservableCollection<ErpReasonCodeLineInfo> ReasonCodeLines { get; set; }
        //NS: D365 Update 12 Platform change end

        public string SubtotalAmount { get; set; }

        public string TotalLineDiscount { get; set; }

        public string TotalAmount { get; set; }

        public string TotalSalesTax { get; set; }

        // D365 Update 40 Platform Changes Start
        public string ContractVersion { get; set; }
        // D365 Update 40 Platform Changes End
    }

    public class ErpReasonCodeLineInfo
    {
        public ErpReasonCodeLineInfo()
        {

        }

        public decimal Amount { get; set; }
        public string Information { get; set; }
        public decimal InformationAmount { get; set; }
        public int InputTypeValue { get; set; }
        public string ItemTender { get; set; }
        public decimal LineNumber { get; set; }
        public int LineTypeValue { get; set; }
        public string ReasonCodeId { get; set; }
        public string StatementCode { get; set; }
        public string SubReasonCodeId { get; set; }
        public string SourceCode { get; set; }
        public string SourceCode2 { get; set; }
        public string SourceCode3 { get; set; }
    }
    public class ErpAffiliationInfo
    {
        public ErpAffiliationInfo()
        {

        }

        public long AffiliationRecordId { get; set; }
        public string AffiliationType { get; set; }
        public long LoyaltyTierRecordId { get; set; }
    }

    public class ErpChargeInfo
    {
        public ErpChargeInfo()
        {

        }
        public decimal Amount { get; set; }
        public string Code { get; set; }
        public string Method { get; set; }
        public string SalesTaxGroup { get; set; }
        public string TaxGroup { get; set; }
        //HK: D365 Update 10.0 Application change start
        public int MarkupAutoTableRecId { get; set; }
        public int ShouldApplyEveryInvoice { get; set; }
        //HK: D365 Update 10.0 Application change end
    }

    public partial class ErpItemInfo
    {
        public ErpItemInfo()
        {
            Charges = new ObservableCollection<ErpChargeInfo>();
            Discounts = new ObservableCollection<ErpDiscountInfo>();
            Taxes = new ObservableCollection<ErpTaxInfo>();
            ReasonCodeLines = new ObservableCollection<ErpReasonCodeLineInfo>();
        }

        public string AddressRecordId { get; set; }
        public string BatchId { get; set; }
        public long Catalog { get; set; }
        public ObservableCollection<ErpChargeInfo> Charges { get; set; }
        public string ColorId { get; set; }
        public string ColorName { get; set; }
        public string Comment { get; set; }
        public string CommissionSalesGroup { get; set; }
        public string ConfigId { get; set; }
        public string ConfigName { get; set; }
        public string ContinuityScheduleId { get; set; }
        public long CustInvoiceTransId { get; set; }
        public string DeliveryMode { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPercent { get; set; }
        public ObservableCollection<ErpDiscountInfo> Discounts { get; set; }
        public string ExchangeInventTransId { get; set; }
        public string FulfillmentStoreId { get; set; }
        public bool Giftcard { get; set; }
        public string GiftcardDeliveryEmail { get; set; }
        public string GiftcardDeliveryMessage { get; set; }
        public string GiftcardNumber { get; set; }
        public string InventDimensionId { get; set; }
        public string InventTransId { get; set; }
        public string InvoiceId { get; set; }
        public bool IsInstallmentEligible { get; set; }
        public string ItemId { get; set; }
        public string ItemTaxGroup { get; set; }
        public decimal LineDscAmount { get; set; }
        public decimal LineManualDiscountAmount { get; set; }
        public decimal LineManualDiscountPercentage { get; set; }
        public decimal LineNumber { get; set; }
        public int LineType { get; set; }
        public string ListingId { get; set; }
        public decimal NetAmount { get; set; }
        public string ParentInventTransId { get; set; }
        public decimal PeriodicDiscount { get; set; }
        public decimal PeriodicPercentageDiscount { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityPicked { get; set; }
        public long RecId { get; set; }
        public string RequestedDeliveryDateString { get; set; }
        public string ReturnInventTransId { get; set; }
        public decimal SalesMarkup { get; set; }
        public string SalesTaxGroup { get; set; }
        public string SerialId { get; set; }
        public string SiteId { get; set; }
        public string SizeId { get; set; }
        public string SizeName { get; set; }
        public string SourceId { get; set; }
        public int Status { get; set; }
        public string StyleId { get; set; }
        public string StyleName { get; set; }
        public ObservableCollection<ErpTaxInfo> Taxes { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPctDiscount { get; set; }
        public string Unit { get; set; }
        public int UpSellOrigin { get; set; }
        public string UpSellOriginOfferId { get; set; }
        public string VariantId { get; set; }
        public string WarehouseId { get; set; }

        //NS: D365 Update 12 Platform change start
        public decimal QuantityRemained { get; set; }
        public decimal QuantityShipped { get; set; }
        public decimal QuantityPickedWarehouse { get; set; }
        public decimal QuantityPacked { get; set; }
        public decimal QuantityCanceled { get; set; }
        public decimal OriginalPrice { get; set; }
        public bool IsPriceOverridden { get; set; }
        public string LineVersion { get; set; }
        public ObservableCollection<ErpReasonCodeLineInfo> ReasonCodeLines { get; set; }
        //NS: D365 Update 12 Platform change end

        public string TotalPrice { get; set; }
        public string TMVTargetAmount { get; set; }

        // D365 Update 40 Platform Changes Start
        public ErpInventoryQuantities InventoryQuantities { get; set; }
        // D365 Update 40 Platform Changes End
    }

    public class ErpPaymentInfo
    {
        public ErpPaymentInfo()
        {

        }

        public decimal Amount { get; set; }
        public string CardType { get; set; }
        public string CreditCardAuthorization { get; set; }
        public string CreditCardToken { get; set; }
        public string Currency { get; set; }
        public string DateString { get; set; }
        public bool PaymentCaptured { get; set; }
        public string PaymentType { get; set; }
        public bool Prepayment { get; set; }
        //NS: D365 Update 12 Platform change start
        public ObservableCollection<ErpReasonCodeLineInfo> ReasonCodeLines { get; set; }
        //NS: D365 Update 12 Platform change end
        //HK: D365 Update 10.0 Application change start
        public decimal LineNum { get; set; }
        //HK: D365 Update 10.0 Application change end
    }
    public class ErpPreauthorization
    {
        public ErpPreauthorization()
        {

        }

        public string PaymentPropertiesBlob { get; set; }
    }
    public class ErpTaxInfo
    {
        public ErpTaxInfo()
        {

        }

        public decimal Amount { get; set; }
        public string TaxCode { get; set; }
        //HK: D365 Update 10.0 Application change start
        public bool IsIncludedInPrice { get; set; }
        public decimal TaxPercentage { get; set; }
        //HK: D365 Update 10.0 Application change end
    }

    public class ErpDiscountInfo
    {
        public ErpDiscountInfo()
        {

        }

        public decimal Amount { get; set; }
        public int CustomerDiscountType { get; set; }
        public decimal DealPrice { get; set; }
        public decimal TMVTargetAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountCost { get; set; }
        public int DiscountOriginType { get; set; }
        public int ManualDiscountType { get; set; }
        public string OfferName { get; set; }
        public decimal Percentage { get; set; }
        public string PeriodicDiscountOfferId { get; set; }
        public int PeriodicDiscountType { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime? ContractValidFrom { get; set; }
        public DateTime? ContractValidTo { get; set; }
        public string DiscountMethod { get; set; }
        public string TMVPriceOverrideReasonCode { get; set; }
    }

    public class ERPSalesLine
    {
        public ERPSalesLine() { }

        public string LineMultilineDiscOnItem { get; set; }
        public ReturnLabelContent ReturnLabelProperties { get; set; }
        public ObservableCollection<ReasonCodeLine> ReasonCodeLines { get; set; }
        public ObservableCollection<DiscountLine> PeriodicDiscountPossibilities { get; set; }
        public ObservableCollection<DiscountLine> DiscountLines { get; set; }
        public UnitOfMeasureConversion UnitOfMeasureConversion { get; set; }
        public decimal? UnitQuantity { get; set; }
        public decimal? QuantityDiscounted { get; set; }
        public decimal? PeriodicPercentageDiscount { get; set; }
        public decimal? LinePercentageDiscount { get; set; }
        public DateTimeOffset? DateToActivateItem { get; set; }
        public bool? Found { get; set; }
        public string LinkedParentLineId { get; set; }
        public bool? Blocked { get; set; }
        public decimal? LoyaltyPercentageDiscount { get; set; }
        public decimal? LoyaltyDiscountAmount { get; set; }
        public string ElectronicDeliveryEmailContent { get; set; }
        public string ElectronicDeliveryEmailAddress { get; set; }
        public long? CatalogId { get; set; }
        public string UnitOfMeasureSymbol { get; set; }
        public decimal? DeliveryModeChargeAmount { get; set; }
        public string BatchId { get; set; }
        public string SerialNumber { get; set; }
        public long? RecordId { get; set; }
        public bool? IsSavedDiscount { get; set; }
        public decimal? SavedQuantity { get; set; }
        public bool? IsCustomerAccountDeposit { get; set; }
        public decimal? QuantityOrdered { get; set; }
        public int? LineMultilineDiscOnItemValue { get; set; }
        public string OriginalSalesOrderUnitOfMeasure { get; set; }
        public bool? IsReturnByReceipt { get; set; }
        public decimal? TaxRatePercent { get; set; }
        public DateTimeOffset? EndDateTime { get; set; }
        public DateTimeOffset? BeginDateTime { get; set; }
        public decimal? NetAmountWithAllInclusiveTax { get; set; }
        public decimal? TaxAmountExclusive { get; set; }
        public decimal? TaxAmountInclusive { get; set; }
        public decimal? TaxAmountExemptInclusive { get; set; }
        public ObservableCollection<TaxLine> TaxLines { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? NetAmountPerUnit { get; set; }
        public decimal? NetAmount { get; set; }
        public bool? WasChanged { get; set; }
        public string SalesOrderUnitOfMeasure { get; set; }
        public string OriginalItemTaxGroupId { get; set; }
        public string OriginalSalesTaxGroupId { get; set; }
        public string SalesTaxGroupId { get; set; }
        public string ItemTaxGroupId { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public string ItemId { get; set; }
        public ObservableCollection<AttributeValueBase> AttributeValues { get; set; }
        public string CommissionSalesGroup { get; set; }
        public bool? IsLoyaltyDiscountApplied { get; set; }
        public string TrackingId { get; set; }
        public string InventOrderUnitOfMeasure { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? QuantityInvoiced { get; set; }
        public DateTimeOffset? SalesDate { get; set; }
        public string FulfillmentStoreId { get; set; }
        public DateTimeOffset? RequestedDeliveryDate { get; set; }
        public string Comment { get; set; }
        public string DeliveryMode { get; set; }
        public Address ShippingAddress { get; set; }
        public decimal? LineManualDiscountAmount { get; set; }
        public decimal? LineManualDiscountPercentage { get; set; }
        public decimal? PeriodicDiscount { get; set; }
        public decimal? LineDiscount { get; set; }
        public decimal? TotalPercentageDiscount { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? NetAmountWithoutTax { get; set; }
        public string InventoryLocationId { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool? IsPriceOverridden { get; set; }
        public long? ListingId { get; set; }
        public long? MasterProductId { get; set; }
        public int? EntryMethodTypeValue { get; set; }
        public string Barcode { get; set; }
        public long? ProductId { get; set; }
        public bool? IsPriceKeyedIn { get; set; }
        public string TaxOverrideCode { get; set; }
        public string OriginLineId { get; set; }
        public string Description { get; set; }
        public string LineId { get; set; }
        public string StaffId { get; set; }
        public decimal? OriginalPrice { get; set; }
        public string WarehouseLocation { get; set; }
        public string InventoryStatusId { get; set; }
        public string LicensePlate { get; set; }
        public long? ReturnChannelId { get; set; }
        public string ReturnTerminalId { get; set; }
        public string ReturnStore { get; set; }
        public string ReturnInventTransId { get; set; }
        public decimal? ReturnLineNumber { get; set; }
        public string ReturnTransactionId { get; set; }
        public decimal? AdjustedPrice { get; set; }
        public decimal? AgreementPrice { get; set; }
        public decimal? BasePrice { get; set; }
        public ObservableCollection<ChargeLine> ChargeLines { get; set; }
        public bool? IsPriceLocked { get; set; }
        public bool? IsVoided { get; set; }
        public bool? IsInvoiceSettled { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string InvoiceId { get; set; }
        public bool? IsInvoiceLine { get; set; }
        public int? GiftCardOperationValue { get; set; }
        public string GiftCardCurrencyCode { get; set; }
        public string GiftCardId { get; set; }
        public bool? IsGiftCardLine { get; set; }
        public int? ProductSourceValue { get; set; }
        public int? SalesStatusValue { get; set; }
        public int? StatusValue { get; set; }
        public decimal? ReturnQuantity { get; set; }
        public decimal? LineNumber { get; set; }
        public Guid? ReservationId { get; set; }
        public string InventoryDimensionId { get; set; }
        public decimal? ReturnLineTaxAmount { get; set; }
        public ObservableCollection<CommerceProperty> ExtensionProperties { get; set; }
        //HK: D365 Update 10.0 Application change start
        public bool IsTaxOverideCodeTaxExempt { get; set; }
        public string InvoicedSalesId { get; set; }
        //HK: D365 Update 10.0 Application change end
    }

    // D365 Update 40 Platform Changes Start
    public class ErpInventoryQuantities
    {
        public string UnitOfMeasure { get; set; }
        public decimal? QuantityNotProcessed { get; set; }
        public decimal? QuantityReserved { get; set; }
        public decimal? QuantityPicked { get; set; }
        public decimal? QuantityPacked { get; set; }
        public decimal? QuantityInvoiced { get; set; }
        public decimal? QuantityOrdered { get; set; }
        public decimal? QuantityCanceled { get; set; }
        public decimal? QuantityReturned { get; set; }
    }
    // D365 Update 40 Platform Changes End
}
