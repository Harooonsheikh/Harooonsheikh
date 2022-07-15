namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductPropertySchema
	{
		public ErpProductPropertySchema()
		{
		}
		public System.Collections.Generic.IEqualityComparer<string> Comparer	{ get; set; }//;
		public int Count	{ get; set; }//;
		public System.Collections.Generic.Dictionary<string, int>.KeyCollection Keys	{ get; set; }//;
		public System.Collections.Generic.Dictionary<string, int>.ValueCollection Values	{ get; set; }//;
		public int Item	{ get; set; }//;
	}
}
