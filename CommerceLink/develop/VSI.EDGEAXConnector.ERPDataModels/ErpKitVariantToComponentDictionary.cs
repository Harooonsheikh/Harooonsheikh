namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpKitVariantToComponentDictionary
	{
		public ErpKitVariantToComponentDictionary()
		{
		}
		public System.Collections.Generic.ICollection<ErpKitVariantContent> KitVariantToComponentDictionaryAsList	{ get; set; }//;
		public System.Collections.Generic.IEqualityComparer<long> Comparer	{ get; set; }//;
		public int Count	{ get; set; }//;
		public System.Collections.Generic.Dictionary<long, ErpKitVariantContent>.KeyCollection Keys	{ get; set; }//;
		public System.Collections.Generic.Dictionary<long, ErpKitVariantContent>.ValueCollection Values	{ get; set; }//;
		public ErpKitVariantContent Item	{ get; set; }//;
	}
}
