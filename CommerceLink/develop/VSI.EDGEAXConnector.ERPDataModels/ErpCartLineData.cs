namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCartLineData
	{
		public ErpCartLineData()
		{
		}
		public string LineId	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string Barcode	{ get; set; }//;
		public string InventoryDimensionId	{ get; set; }//;
		public long ProductId	{ get; set; }//;
		public string WarehouseId	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public decimal Price	{ get; set; }//;
		public decimal ExtendedPrice	{ get; set; }//;
		public decimal TaxAmount	{ get; set; }//;
		public decimal TaxAmountExclusive	{ get; set; }//;
		public decimal TaxAmountInclusive	{ get; set; }//;
		public decimal TotalAmount	{ get; set; }//;
		public decimal DiscountAmount	{ get; set; }//;
		public decimal LineDiscount	{ get; set; }//;
		public decimal LinePercentageDiscount	{ get; set; }//;
		public decimal LineManualDiscountPercentage	{ get; set; }//;
		public decimal LineManualDiscountAmount	{ get; set; }//;
		public string SerialNumber	{ get; set; }//;
		public ErpAddress ShippingAddress	{ get; set; }//;
		public string DeliveryMode	{ get; set; }//;
		public System.Nullable<decimal> DeliveryModeChargeAmount	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> RequestedDeliveryDate	{ get; set; }//;
		public string ReturnTransactionId	{ get; set; }//;
		public string Comment	{ get; set; }//;
		public decimal ReturnLineNumber	{ get; set; }//;
		public string ReturnInventoryTransactionId	{ get; set; }//;
		public string StoreNumber	{ get; set; }//;
		public ErpTransactionStatus Status	{ get; set; }//;
		public bool IsReturnByReceipt	{ get; set; }//;
		public bool IsProductLine	{ get; set; }//;
		public bool IsGiftCardLine	{ get; set; }//;
		public string GiftCardId	{ get; set; }//;
		public string GiftCardCurrencyCode	{ get; set; }//;
		public ErpGiftCardOperationType GiftCardOperation	{ get; set; }//;
		public bool IsInvoiceLine	{ get; set; }//;
		public string InvoiceId	{ get; set; }//;
		public decimal InvoiceAmount	{ get; set; }//;
		public decimal TaxRatePercent	{ get; set; }//;
		public string ElectronicDeliveryEmail	{ get; set; }//;
		public string ElectronicDeliveryEmailContent	{ get; set; }//;
		public bool IsVoided	{ get; set; }//;
		public decimal QuantityOrdered	{ get; set; }//;
		public decimal QuantityInvoiced	{ get; set; }//;
		public System.Collections.Generic.Dictionary<string, ErpLinkedProduct> LineIdsLinkedProductMap	{ get; set; }//;
		public string LinkedParentLineId	{ get; set; }//;
		public System.Collections.ObjectModel.Collection<ErpDiscountLine> DiscountLines	{ get; set; }//;
		public System.Collections.ObjectModel.Collection<ErpReasonCodeLine> ReasonCodeLines	{ get; set; }//;
		public System.Collections.ObjectModel.Collection<ErpChargeLine> ChargeLines	{ get; set; }//;
		public string UnitOfMeasureSymbol	{ get; set; }//;
		public System.Collections.ObjectModel.Collection<string> PromotionLines	{ get; set; }//;
		public bool IsPriceOverridden	{ get; set; }//;
		public System.Nullable<decimal> OriginalPrice	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
