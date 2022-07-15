namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpProductPrice
	{
		public ErpProductPrice()
		{
		}
		public long ProductId	{ get; set; }//;
		public long ListingId	{ get; set; }//;
		public decimal BasePrice	{ get; set; }//;
		public decimal TradeAgreementPrice	{ get; set; }//;
		public decimal AdjustedPrice	{ get; set; }//;
		public decimal CustomerContextualPrice	{ get; set; }//;
		public string CurrencyCode	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string InventoryDimensionId	{ get; set; }//;
		public string UnitOfMeasure	{ get; set; }//;
		public System.DateTime ValidFrom	{ get; set; }//;
		public long ProductLookupId	{ get; set; }//;
		public long ChannelId	{ get; set; }//;
		public long CatalogId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
        public string AffiliationName { get; set; }
        public long AffiliationId { get; set; }
        public int PeriodicDiscountType { get; set; }
        public int DiscountType { get; set; }
        public int LineType { get; set; }
    }
}
