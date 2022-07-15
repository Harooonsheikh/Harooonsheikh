namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpContactInformation
	{
		public ErpContactInformation()
		{
		}
		public string Value	{ get; set; }//;
		public ErpContactInformationType ContactInformationType	{ get; set; }//;
		public int ContactInformationTypeValue	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
