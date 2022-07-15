namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpHardwareProfileScanner
	{
		public ErpHardwareProfileScanner()
		{
		}
		public string ProfileId	{ get; set; }//;
		public ErpDeviceType DeviceType	{ get; set; }//;
		public int DeviceTypeValue	{ get; set; }//;
		public string DeviceName	{ get; set; }//;
		public string DeviceDescription	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
        
        //NS: D365 Update 8.1 Application change start
        public bool DecodeData { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}
