namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductVariantDictionary
	{
		public ErpProductVariantDictionary()
		{
		}
		public System.Collections.Generic.ICollection<ErpProductVariant> ProductVariantsAsList	{ get; set; }//;
		public System.Collections.Generic.IEqualityComparer<long> Comparer	{ get; set; }//;
		public int Count	{ get; set; }//;
		public System.Collections.Generic.Dictionary<long, ErpProductVariant>.KeyCollection Keys	{ get; set; }//;
		public System.Collections.Generic.Dictionary<long, ErpProductVariant>.ValueCollection Values	{ get; set; }//;
		public ErpProductVariant Item	{ get; set; }//;
	}
}
