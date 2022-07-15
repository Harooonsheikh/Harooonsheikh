namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReasonCodeSettings
	{
		public ErpReasonCodeSettings()
		{
		}
		public ErpReasonCodeSettings DefaultSettings	{ get; set; }//;
		public string Item	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
	}
}
