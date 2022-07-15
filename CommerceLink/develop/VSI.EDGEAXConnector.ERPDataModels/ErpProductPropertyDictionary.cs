namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductPropertyDictionary
	{
		public ErpProductPropertyDictionary()
		{
		}
		public System.Collections.Generic.IEqualityComparer<string> Comparer	{ get; set; }//;
		public int Count	{ get; set; }//;
		public System.Collections.Generic.Dictionary<string, ErpProductProperty>.KeyCollection Keys	{ get; set; }//;
		public System.Collections.Generic.Dictionary<string, ErpProductProperty>.ValueCollection Values	{ get; set; }//;
		public ErpProductProperty Item	{ get; set; }//;
	}
}
