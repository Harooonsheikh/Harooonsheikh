namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpAttributeNameTranslation
	{
		public ErpAttributeNameTranslation()
		{
		}
		public string Language	{ get; set; }//;
		public string FriendlyName	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}