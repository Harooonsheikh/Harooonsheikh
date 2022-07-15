namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpRetailDiscountLine
	{
		public ErpRetailDiscountLine()
		{
		}
		public string OfferId	{ get; set; }//;
		public decimal DiscountLineNumber	{ get; set; }//;
		public decimal DiscountLinePercentOrValue	{ get; set; }//;
		public string MixAndMatchLineGroup	{ get; set; }//;
		public int MixAndMatchLineSpecificDiscountType	{ get; set; }//;
		public int MixAndMatchLineNumberOfItemsNeeded	{ get; set; }//;
		public int DiscountMethod	{ get; set; }//;
		public decimal DiscountAmount	{ get; set; }//;
		public decimal DiscountPercent	{ get; set; }//;
		public decimal OfferPrice	{ get; set; }//;
		public decimal OfferPriceIncludingTax	{ get; set; }//;
		public string UnitOfMeasureSymbol	{ get; set; }//;
		public long CategoryId	{ get; set; }//;
		public long ProductId	{ get; set; }//;
		public long DistinctProductVariantId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
