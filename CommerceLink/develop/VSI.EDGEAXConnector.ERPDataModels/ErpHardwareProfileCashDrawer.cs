namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpHardwareProfileCashDrawer
	{
		public ErpHardwareProfileCashDrawer()
		{
		}
		public string ProfileId	{ get; set; }//;
		public ErpDeviceType DeviceType	{ get; set; }//;
		public int DeviceTypeValue	{ get; set; }//;
		public string DeviceName	{ get; set; }//;
		public string DeviceDescription	{ get; set; }//;
		public string DeviceMake	{ get; set; }//;
		public string DeviceModel	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
