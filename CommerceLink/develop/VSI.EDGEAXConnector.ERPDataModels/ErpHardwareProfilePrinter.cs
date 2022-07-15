namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpHardwareProfilePrinter
	{
		public ErpHardwareProfilePrinter()
		{
		}
		public string ProfileId	{ get; set; }//;
		public ErpDeviceType DeviceType	{ get; set; }//;
		public int DeviceTypeValue	{ get; set; }//;
		public string DeviceName	{ get; set; }//;
		public string DeviceMake	{ get; set; }//;
		public string DeviceModel	{ get; set; }//;
		public string DeviceDescription	{ get; set; }//;
		public int CharacterSet	{ get; set; }//;
		public string ReceiptProfileId	{ get; set; }//;
		public bool BinaryConversion	{ get; set; }//;
		public int DocInsertRemovalTimeout	{ get; set; }//;
		public ErpPrinterLogotype Logo	{ get; set; }//;
		public ErpPrinterLogoAlignmentType LogoAlignment	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
