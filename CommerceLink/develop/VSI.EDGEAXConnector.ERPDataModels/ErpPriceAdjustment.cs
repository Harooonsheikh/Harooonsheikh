namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpPriceAdjustment
	{
		public ErpPriceAdjustment()
		{
		}
		public long RecordId	{ get; set; }//;
		public string OfferId	{ get; set; }//;
		public System.DateTimeOffset ValidFromDate	{ get; set; }//;
		public System.DateTimeOffset ValidToDate	{ get; set; }//;
		public ErpConcurrencyMode ConcurrencyMode	{ get; set; }//;
		public int DateValidationType	{ get; set; }//;
		public string ValidationPeriodId	{ get; set; }//;
		public ErpDiscountOfferMethod DiscountMethod	{ get; set; }//;
		public decimal OfferPrice	{ get; set; }//;
		public decimal OfferPriceIncludingTax	{ get; set; }//;
		public decimal DiscountPercent	{ get; set; }//;
		public decimal DiscountAmount	{ get; set; }//;
		public long ProductId	{ get; set; }//;
		public long ProductVariantId	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string InventoryDimensionId	{ get; set; }//;
		public string UnitOfMeasure	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        public string SKU { get; set; }
        public string SpecialPrice { get; set; }
	}
}
