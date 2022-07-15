namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpSalesTransactionData
	{
		public ErpSalesTransactionData()
		{
		}
		public long ByteLength	{ get; set; }//;
		public byte[] TransactionData	{ get; set; }//;
		public string Id	{ get; set; }//;
		public string Comment	{ get; set; }//;
		public string CustomerId	{ get; set; }//;
		public string TerminalId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public long ChannelId	{ get; set; }//;
		public decimal Amount	{ get; set; }//;
		public string StaffId	{ get; set; }//;
		public byte[] Version	{ get; set; }//;
		public ErpCartType Type	{ get; set; }//;
		public System.DateTimeOffset ModifiedDateTime	{ get; set; }//;
		public bool IsSuspended	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
