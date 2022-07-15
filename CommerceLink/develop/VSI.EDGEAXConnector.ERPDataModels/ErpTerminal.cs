namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTerminal
	{
		public ErpTerminal()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string TerminalId	{ get; set; }//;
		public long ChannelId	{ get; set; }//;
		public string HardwareProfile	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
