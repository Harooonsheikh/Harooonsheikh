namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpProduct
	{
		public ErpProduct()
		{
		}
		public long RecordId	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string Locale	{ get; set; }//;
		public string ProductNumber	{ get; set; }//;
		public ErpProductRules Rules	{ get; set; }//;
		public bool HasLinkedProducts	{ get; set; }//;
		public bool IsMasterProduct	{ get; set; }//;
		public bool IsKit	{ get; set; }//;
		public bool IsRemote	{ get; set; }//;
        public ErpProductChangeTrackingInformation ChangeTrackingInformation	{ get; set; }//;
		public string ProductName	{ get; set; }//;
		public string Description	{ get; set; }//;
		public ErpRichMediaLocations Image	{ get; set; }//;
		public System.Collections.Generic.ICollection<string> UnitsOfMeasureSymbol	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpLinkedProduct> LinkedProducts	{ get; set; }//;
		public decimal BasePrice	{ get; set; }//;
		public decimal Price	{ get; set; }//;
		public decimal AdjustedPrice	{ get; set; }//;
		public ErpProjectionDomain Context	{ get; set; }//;
		public long PrimaryCategoryId	{ get; set; }//;
		public System.Collections.Generic.ICollection<long> CategoryIds	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpRelatedProduct> RelatedProducts	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpRelatedProduct> ProductsRelatedToThis	{ get; set; }//;
        //public ErpProductPropertySchema IndexedProductSchema	{ get; set; }//;
		public System.Collections.Generic.ICollection<string> ProductSchema	{ get; set; }//;

        //AF:Start

        // again commented the property , this property was causing issue in old architecture when un scommented 
        public ErpProductPropertyTranslationDictionary IndexedProductProperties	{ get; set; }//;

        //AF:End
        //public System.Collections.Generic.ICollection<ErpProductPropertyTranslation> ProductProperties	{ get; set; }//;
        public ErpProductCompositionInformation CompositionInformation	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
        public System.Collections.Generic.ICollection<ErpProductVariant> Variants { get; set; }

        public string OfferId { get; set; }

        public string ProductImageUrl { get; set; }

        public string AvaTax_TaxClassId { get; set; }
      
    }
}
