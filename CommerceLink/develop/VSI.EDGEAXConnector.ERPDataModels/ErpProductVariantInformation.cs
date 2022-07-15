namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductVariantInformation
	{
		public ErpProductVariantInformation()
		{
		}
		public ErpProductVariantDictionary IndexedVariants	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpProductVariant> Variants	{ get; set; }//;
		public ErpProductDimensionDictionary IndexedDimensions	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpProductDimensionSet> Dimensions	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public long MasterProductId	{ get; set; }//;
		public long ActiveVariantProductId	{ get; set; }//;
	}
}
