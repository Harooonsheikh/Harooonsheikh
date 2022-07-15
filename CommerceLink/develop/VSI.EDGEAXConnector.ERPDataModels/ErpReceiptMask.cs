namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReceiptMask
	{
		public ErpReceiptMask()
		{
		}
		public long RecordId	{ get; set; }//;
		public string FunctionalityProfileId	{ get; set; }//;
		public ErpReceiptTransactionType ReceiptTransactionType	{ get; set; }//;
		public string Mask	{ get; set; }//;
		public bool IsIndependent	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
