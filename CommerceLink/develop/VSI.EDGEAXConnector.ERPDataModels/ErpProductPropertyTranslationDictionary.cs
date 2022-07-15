namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductPropertyTranslationDictionary
	{
		public ErpProductPropertyTranslationDictionary()
		{
		}
		public System.Collections.Generic.ICollection<ErpProductPropertyTranslation> ProductPropertiesAsList	{ get; set; }//;
		public System.Collections.Generic.IEqualityComparer<string> Comparer	{ get; set; }//;
		public int Count	{ get; set; }//;
		public System.Collections.Generic.Dictionary<string, ErpProductPropertyDictionary>.KeyCollection Keys	{ get; set; }//;
		public System.Collections.Generic.Dictionary<string, ErpProductPropertyDictionary>.ValueCollection Values	{ get; set; }//;
		public ErpProductPropertyDictionary Item	{ get; set; }//;
	}
}
