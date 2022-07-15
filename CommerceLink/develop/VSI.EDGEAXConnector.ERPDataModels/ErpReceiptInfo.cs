namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReceiptInfo
	{
		public ErpReceiptInfo()
		{
		}
		public int PrintAsSlip	{ get; set; }//;
		public int PrintBehavior	{ get; set; }//;
		public string Title	{ get; set; }//;
		public string Description	{ get; set; }//;
		public int Uppercase	{ get; set; }//;
		public string ReceiptLayoutId	{ get; set; }//;
		public int HeaderLines	{ get; set; }//;
		public int Bodylines	{ get; set; }//;
		public int FooterLines	{ get; set; }//;
		public bool IsCopy	{ get; set; }//;
		public string HeaderTemplate	{ get; set; }//;
		public string BodyTemplate	{ get; set; }//;
		public string FooterTemplate	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
