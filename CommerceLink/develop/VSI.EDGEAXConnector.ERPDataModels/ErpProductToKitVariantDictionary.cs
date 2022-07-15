namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductToKitVariantDictionary
	{
		public ErpProductToKitVariantDictionary()
		{
		}
		public System.Collections.Generic.ICollection<ErpComponentKitVariantSet> ProductToKitVariantDictionaryAsList	{ get; set; }//;
		public System.Collections.Generic.IEqualityComparer<long> Comparer	{ get; set; }//;
		public int Count	{ get; set; }//;
		public System.Collections.Generic.Dictionary<long, ErpComponentKitVariantSet>.KeyCollection Keys	{ get; set; }//;
		public System.Collections.Generic.Dictionary<long, ErpComponentKitVariantSet>.ValueCollection Values	{ get; set; }//;
		public ErpComponentKitVariantSet Item	{ get; set; }//;
	}
}
