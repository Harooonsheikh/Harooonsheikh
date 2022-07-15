namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpKitLineProductPropertyDictionary
	{
		public ErpKitLineProductPropertyDictionary()
		{
		}
		public System.Collections.Generic.ICollection<ErpKitLineProductProperty> ComponentPropertiesAsList	{ get; set; }//;
		public System.Collections.Generic.IEqualityComparer<long> Comparer	{ get; set; }//;
		public int Count	{ get; set; }//;
		public System.Collections.Generic.Dictionary<long, ErpKitLineProductProperty>.KeyCollection Keys	{ get; set; }//;
		public System.Collections.Generic.Dictionary<long, ErpKitLineProductProperty>.ValueCollection Values	{ get; set; }//;
		public ErpKitLineProductProperty Item	{ get; set; }//;
	}
}
