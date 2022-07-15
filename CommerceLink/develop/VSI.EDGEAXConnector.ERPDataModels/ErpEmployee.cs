namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpEmployee
	{
		public ErpEmployee()
		{
		}
		public string StaffId	{ get; set; }//;
		public string NameOnReceipt	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string CultureName	{ get; set; }//;
		public ErpEmployeePermissions Permissions	{ get; set; }//;
		public ErpRichMediaLocations Image	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
