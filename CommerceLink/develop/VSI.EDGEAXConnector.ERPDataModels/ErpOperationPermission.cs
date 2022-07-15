namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpOperationPermission
	{
		public ErpOperationPermission()
		{
		}
		public string OperationName	{ get; set; }//;
		public int OperationId	{ get; set; }//;
		public bool CheckUserAccess	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<string> Permissions	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
