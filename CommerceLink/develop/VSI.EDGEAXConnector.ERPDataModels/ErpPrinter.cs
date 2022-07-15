namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpPrinter
	{
		public ErpPrinter()
		{
		}
		public string Name	{ get; set; }//;
		public int PrinterType	{ get; set; }//;
		public long Terminal	{ get; set; }//;
		public ErpPrintBehavior PrintBehavior	{ get; set; }//;
		public int PrintBehaviorValue	{ get; set; }//;
		public string ReceiptLayoutId	{ get; set; }//;
		public string HardwareProfileId	{ get; set; }//;
		public ErpReceiptType ReceiptType	{ get; set; }//;
		public int ReceiptTypeValue	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
