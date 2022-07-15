namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpAddressFormattingInfo
	{
		public ErpAddressFormattingInfo()
		{
		}
		public string AddressComponentName	{ get; set; }//;
		public int LineNumber	{ get; set; }//;
		public bool IsDataEntryOnly	{ get; set; }//;
		public string CountryRegionId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
