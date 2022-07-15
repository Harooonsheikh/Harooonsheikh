namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReceipt
	{
		public ErpReceipt()
		{
		}
		public string TransactionId	{ get; set; }//;
		public string Header	{ get; set; }//;
		public string Body	{ get; set; }//;
		public string Footer	{ get; set; }//;
		public string ReceiptId	{ get; set; }//;
		public string LayoutId	{ get; set; }//;
		public ErpReceiptType ReceiptType	{ get; set; }//;
		public string ReceiptTitle	{ get; set; }//;
		public int ReceiptTypeValue	{ get; set; }//;
		public string ReceiptTypeStrValue	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpPrinter> Printers	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
