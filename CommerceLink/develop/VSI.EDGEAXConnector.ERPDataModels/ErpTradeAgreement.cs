namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTradeAgreement
	{
		public ErpTradeAgreement()
		{
		}
		public long RecordId	{ get; set; }//;
		public ErpPriceDiscountItemCode ItemCode	{ get; set; }//;
		public ErpPriceDiscountAccountCode AccountCode	{ get; set; }//;
		public string ItemRelation	{ get; set; }//;
		public string AccountRelation	{ get; set; }//;
		public decimal QuantityAmountFrom	{ get; set; }//;
		public decimal QuantityAmountTo	{ get; set; }//;
		public System.DateTimeOffset FromDate	{ get; set; }//;
		public System.DateTimeOffset ToDate	{ get; set; }//;
		public decimal Amount	{ get; set; }//;
		public string Currency	{ get; set; }//;
		public decimal PercentOne	{ get; set; }//;
		public decimal PercentTwo	{ get; set; }//;
		public bool ShouldSearchAgain	{ get; set; }//;
		public decimal PriceUnit	{ get; set; }//;
		public ErpPriceDiscountType Relation	{ get; set; }//;
		public string UnitOfMeasureSymbol	{ get; set; }//;
		public decimal MarkupAmount	{ get; set; }//;
		public bool ShouldIncludeMarkup	{ get; set; }//;
		public string InventoryDimensionId	{ get; set; }//;
		public string ConfigId	{ get; set; }//;
		public string SizeId	{ get; set; }//;
		public string StyleId	{ get; set; }//;
		public string ColorId	{ get; set; }//;
		public decimal MaximumRetailPriceIndia	{ get; set; }//;
		public bool IsVariant	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
