namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpProductDimensionValueSet
	{
		public ErpProductDimensionValueSet()
		{
		}
		public string DimensionValue	{ get; set; }//;
		public System.Collections.Generic.ICollection<long> VariantSet	{ get; set; }//;


    }
}
