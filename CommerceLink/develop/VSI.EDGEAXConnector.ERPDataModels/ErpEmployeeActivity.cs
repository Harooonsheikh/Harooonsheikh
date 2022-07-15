namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpEmployeeActivity
	{
		public ErpEmployeeActivity()
		{
		}
		public string StaffId	{ get; set; }//;
		public string StaffName	{ get; set; }//;
		public string Activity	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> ActivityDateTimeOffset	{ get; set; }//;
		public string StoreNumber	{ get; set; }//;
		public ErpEmployeeActivityType EmployeeActivityType	{ get; set; }//;
		public string JobId	{ get; set; }//;
		public string BreakCategory	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
