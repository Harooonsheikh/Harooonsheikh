namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpDistrictInfo
	{
		public ErpDistrictInfo()
		{
		}
		public string Name	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string CountryRegionId	{ get; set; }//;
		public string StateId	{ get; set; }//;
		public string CountyId	{ get; set; }//;
		public string CityName	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
