using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpSalesLine
	{
		public ErpSalesLine()
		{
            this.TaxMeasure = new ErpTaxMeasure();
		}
		public string LineId	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string OriginLineId	{ get; set; }//;
		public string TaxOverrideCode	{ get; set; }//;
		public long ProductId	{ get; set; }//;
		public string Barcode	{ get; set; }//;
		public long MasterProductId	{ get; set; }//;
		public long ListingId	{ get; set; }//;
		public bool IsPriceOverridden	{ get; set; }//;
		public System.Nullable<decimal> OriginalPrice	{ get; set; }//;
		public ErpProductVariant Variant	{ get; set; }//;
		public decimal TotalAmount	{ get; set; }//;
		public decimal DiscountAmount	{ get; set; }//;
		public decimal TotalDiscount	{ get; set; }//;
		public decimal TotalPercentageDiscount	{ get; set; }//;
		public decimal LineDiscount	{ get; set; }//;
		public decimal PeriodicDiscount	{ get; set; }//;
		public decimal LineManualDiscountPercentage	{ get; set; }//;
		public decimal LineManualDiscountAmount	{ get; set; }//;
		public ErpAddress ShippingAddress	{ get; set; }//;
		public string DeliveryMode	{ get; set; }//;
		public string Comment	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> RequestedDeliveryDate	{ get; set; }//;
		public string InventoryLocationId	{ get; set; }//;
		public string InventoryDimensionId	{ get; set; }//;
		public System.Guid ReservationId	{ get; set; }//;
		public decimal LineNumber	{ get; set; }//;
		public decimal ReturnQuantity	{ get; set; }//;
		public ErpTransactionStatus Status	{ get; set; }//;
		public int StatusValue	{ get; set; }//;
		public ErpProductSource ProductSource	{ get; set; }//;
		public int ProductSourceValue	{ get; set; }//;
		public bool IsGiftCardLine	{ get; set; }//;
		public string GiftCardId	{ get; set; }//;
		public string GiftCardCurrencyCode	{ get; set; }//;
		public ErpGiftCardOperationType GiftCardOperation	{ get; set; }//;
		public bool IsInvoiceLine	{ get; set; }//;
		public string InvoiceId	{ get; set; }//;
		public decimal InvoiceAmount	{ get; set; }//;
		public bool IsInvoiceSettled	{ get; set; }//;
		public bool IsVoided	{ get; set; }//;
		public bool IsPriceLocked	{ get; set; }//;
		public System.Collections.Generic.IList<ErpChargeLine> ChargeLines	{ get; set; }//;
		public decimal BasePrice	{ get; set; }//;
		public decimal AgreementPrice	{ get; set; }//;
		public decimal AdjustedPrice	{ get; set; }//;
		public string ReturnTransactionId	{ get; set; }//;
		public decimal ReturnLineNumber	{ get; set; }//;
		public string ReturnInventTransId	{ get; set; }//;
		public string ReturnStore	{ get; set; }//;
		public string ReturnTerminalId	{ get; set; }//;
		public long ReturnChannelId	{ get; set; }//;
		public string Store	{ get; set; }//;
		public string TerminalId	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> SalesDate	{ get; set; }//;
		public decimal QuantityInvoiced	{ get; set; }//;
		public decimal QuantityOrdered	{ get; set; }//;
		public long RecordId	{ get; set; }//;
		public string SerialNumber	{ get; set; }//;
		public string BatchId	{ get; set; }//;
		public System.Nullable<decimal> DeliveryModeChargeAmount	{ get; set; }//;
		public string UnitOfMeasureSymbol	{ get; set; }//;
		public long CatalogId	{ get; set; }//;
		public System.Collections.Generic.ISet<long> CatalogIds	{ get; set; }//;
		public string ElectronicDeliveryEmailAddress	{ get; set; }//;
		public string ElectronicDeliveryEmailContent	{ get; set; }//;
		public decimal LoyaltyDiscountAmount	{ get; set; }//;
		public decimal LoyaltyPercentageDiscount	{ get; set; }//;
		public bool Blocked	{ get; set; }//;
		public bool Found	{ get; set; }//;
		public System.DateTimeOffset DateToActivateItem	{ get; set; }//;
		public decimal LinePercentageDiscount	{ get; set; }//;
		public decimal PeriodicPercentageDiscount	{ get; set; }//;
		public decimal QuantityDiscounted	{ get; set; }//;
		public decimal UnitQuantity	{ get; set; }//;
		public ErpUnitOfMeasureConversion UnitOfMeasureConversion	{ get; set; }//;
		public System.Collections.Generic.IList<ErpDiscountLine> DiscountLines	{ get; set; }//;
		public System.Collections.Generic.IList<ErpDiscountLine> PeriodicDiscountPossibilities	{ get; set; }//;
		public System.Collections.Generic.IList<ErpReasonCodeLine> ReasonCodeLines	{ get; set; }//;
		public ErpLineMultilineDiscountOnItem LineMultilineDiscOnItem	{ get; set; }//;
		public System.Collections.Generic.Dictionary<string, ErpLinkedProduct> LineIdsLinkedProductMap	{ get; set; }//;
		public string LinkedParentLineId	{ get; set; }//;
		public int LineMultilineDiscOnItemValue	{ get; set; }//;
		public bool WasChanged	{ get; set; }//;
		public string OriginalSalesOrderUnitOfMeasure	{ get; set; }//;
		public string InventOrderUnitOfMeasure	{ get; set; }//;
		public ErpParameterSet Properties	{ get; set; }//;
		public bool IsLoyaltyDiscountApplied	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public decimal Price	{ get; set; }//;
		public string ItemTaxGroupId	{ get; set; }//;
		public string SalesTaxGroupId	{ get; set; }//;
		public decimal TaxAmount	{ get; set; }//;
		public string SalesOrderUnitOfMeasure	{ get; set; }//;
		public decimal NetAmount	{ get; set; }//;
		public decimal NetAmountPerUnit	{ get; set; }//;
		public decimal GrossAmount	{ get; set; }//;
		public System.Collections.Generic.IList<ErpTaxLine> TaxLines	{ get; set; }//;
		public decimal TaxAmountExemptInclusive	{ get; set; }//;
		public decimal TaxAmountInclusive	{ get; set; }//;
		public decimal TaxAmountExclusive	{ get; set; }//;
		public decimal NetAmountWithAllInclusiveTax	{ get; set; }//;
		public decimal NetAmountWithAllInclusiveTaxPerUnit	{ get; set; }//;
		public System.DateTimeOffset BeginDateTime	{ get; set; }//;
		public System.DateTimeOffset EndDateTime	{ get; set; }//;
		public decimal TaxRatePercent	{ get; set; }//;
		public bool IsReturnByReceipt	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start

        public decimal BarcodeEmbeddedPrice { get; set; }
        public decimal QuantityRemained { get; set; }
        public decimal QuantityShipped { get; set; }
        public decimal QuantityCanceled { get; set; }
        public decimal GiftCardBalance { get; set; }
        public bool PriceInBarcode { get; set; }
        public int LineVersion { get; set; }
        public ObservableCollection<ErpPriceLine> PriceLines { get; set; }
        public ObservableCollection<string> RelatedDiscountedLineIds { get; set; }
        public ErpThirdPartyGiftCardInfo ThirdPartyGiftCardInfo { get; set; }
        public ObservableCollection<ErpTaxLine> ReturnTaxLines { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }
        //NS: D365 Update 12 Platform change end

        //NS: D365 Update 8.1 Application change start
        public int ItemType { get; set; }
        public int InvoiceTypeValue { get; set; }
        public decimal QuantityReturnable { get; set; }
        public ErpTaxMeasure TaxMeasure { get; set; }

        //NS: D365 Update 8.1 Application change ends

        #region Checkout Process Contract Operation
        public decimal TargetPrice { get; set; }
        public string SalesLineAction { get; set; }
        public string OldLinePacLicense { get; set; }
        #endregion
    }
}
