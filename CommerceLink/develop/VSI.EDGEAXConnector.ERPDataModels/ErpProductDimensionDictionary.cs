namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductDimensionDictionary
	{
		public ErpProductDimensionDictionary()
		{
		}
		public System.Collections.Generic.ICollection<ErpProductDimensionSet> DimensionsAsList	{ get; set; }//;
		public System.Collections.Generic.IEqualityComparer<string> Comparer	{ get; set; }//;
		public int Count	{ get; set; }//;
		public System.Collections.Generic.Dictionary<string, ErpProductDimensionValueDictionaryEntry>.KeyCollection Keys	{ get; set; }//;
		public System.Collections.Generic.Dictionary<string, ErpProductDimensionValueDictionaryEntry>.ValueCollection Values	{ get; set; }//;
		public ErpProductDimensionValueDictionaryEntry Item	{ get; set; }//;
	}
}
