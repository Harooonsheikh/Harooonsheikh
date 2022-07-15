namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReceiptProfile
	{
		public ErpReceiptProfile()
		{
		}
		public string ReceiptLayoutId	{ get; set; }//;
		public string ProfileId	{ get; set; }//;
		public string ReceiptType	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
