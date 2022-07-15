namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpPeriodicDiscount
	{
		public ErpPeriodicDiscount()
		{
		}
		public long RecordId	{ get; set; }//;
		public string OfferId	{ get; set; }//;
		public string CurrencyCode	{ get; set; }//;
		public System.DateTimeOffset ValidFromDate	{ get; set; }//;
		public System.DateTimeOffset ValidToDate	{ get; set; }//;
		public ErpConcurrencyMode ConcurrencyMode	{ get; set; }//;
		public int DateValidationType	{ get; set; }//;
		public string ValidationPeriodId	{ get; set; }//;
		public int DiscountMethod	{ get; set; }//;
		public decimal OfferPrice	{ get; set; }//;
		public decimal OfferPriceIncludingTax	{ get; set; }//;
		public decimal DiscountPercent	{ get; set; }//;
		public decimal DiscountAmount	{ get; set; }//;
		public string Name	{ get; set; }//;
		public ErpPeriodicDiscountOfferType PeriodicDiscountType	{ get; set; }//;
		public bool IsDiscountCodeRequired	{ get; set; }//;
		public int DiscountType	{ get; set; }//;
		public decimal MixAndMatchDealPrice	{ get; set; }//;
		public decimal MixAndMatchDiscountPercent	{ get; set; }//;
		public decimal MixAndMatchDiscountAmount	{ get; set; }//;
		public int MixAndMatchNumberOfLeastExpensiveLines	{ get; set; }//;
		public int MixAndMatchNumberOfTimeApplicable	{ get; set; }//;
		public decimal DiscountLineNumber	{ get; set; }//;
		public decimal DiscountLinePercentOrValue	{ get; set; }//;
		public string MixAndMatchLineGroup	{ get; set; }//;
		public int MixAndMatchLineSpecificDiscountType	{ get; set; }//;
		public int MixAndMatchLineNumberOfItemsNeeded	{ get; set; }//;
		public string UnitOfMeasureSymbol	{ get; set; }//;
		public long ProductId	{ get; set; }//;
		public long DistinctProductVariantId	{ get; set; }//;
		public int ShouldCountNonDiscountItems	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string InventoryDimensionId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
