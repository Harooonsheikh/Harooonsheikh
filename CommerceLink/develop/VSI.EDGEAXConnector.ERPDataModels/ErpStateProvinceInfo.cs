namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpStateProvinceInfo
	{
		public ErpStateProvinceInfo()
		{
		}
		public string CountryRegionId	{ get; set; }//;
		public string StateId	{ get; set; }//;
		public string StateName	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
