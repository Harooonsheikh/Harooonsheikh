namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpZipCodeInfo
	{
		public ErpZipCodeInfo()
		{
		}
		public string ZipPostalCode	{ get; set; }//;
		public string StreetName	{ get; set; }//;
		public int TimeZone	{ get; set; }//;
		public string CountryRegionId	{ get; set; }//;
		public string StateId	{ get; set; }//;
		public string CountyId	{ get; set; }//;
		public string CityName	{ get; set; }//;
		public string CityAlias	{ get; set; }//;
		public string District	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
