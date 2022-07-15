namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCountyInfo
	{
		public ErpCountyInfo()
		{
		}
		public string CountyId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string CountryRegionId	{ get; set; }//;
		public string StateId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
