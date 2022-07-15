namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpDevice
	{
		public ErpDevice()
		{
		}
		public long RecordId	{ get; set; }//;
		public string DeviceNumber	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string DeviceType	{ get; set; }//;
		public long TerminalRecordId	{ get; set; }//;
		public long ChannelId	{ get; set; }//;
		public string TerminalId	{ get; set; }//;
		public string ChannelName	{ get; set; }//;
		public bool IsInUse	{ get; set; }//;
		public string Token	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 8.1 Application change start
        public bool AllowMassActivation { get; set; }
        public string CountryRegionIsoCode { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}
