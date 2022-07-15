namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpProductVariant
	{
		public ErpProductVariant()
		{
		}
		public string ItemId	{ get; set; }//;
		public long MasterProductId	{ get; set; }//;
		public string VariantId	{ get; set; }//;
		public string InventoryDimensionId	{ get; set; }//;
		public long DistinctProductVariantId	{ get; set; }//;
		public string SizeId	{ get; set; }//;
		public string ColorId	{ get; set; }//;
		public string StyleId	{ get; set; }//;
		public string ConfigId	{ get; set; }//;
		public string ProductNumber	{ get; set; }//;
		public System.Collections.Generic.IEnumerable<ErpRichMediaLocations> Images	{ get; set; }//;
		public decimal BasePrice	{ get; set; }//;
		public decimal Price	{ get; set; }//;
		public decimal AdjustedPrice	{ get; set; }//;
		public string Color	{ get; set; }//;
		public string Size	{ get; set; }//;
		public string Style	{ get; set; }//;
		public string Configuration	{ get; set; }//;
		public ErpProductPropertyTranslationDictionary IndexedProperties	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpProductPropertyTranslation> PropertiesAsList	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
