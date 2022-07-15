namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTransactionProperty
	{
		public ErpTransactionProperty()
		{
		}
		public string TransactionId	{ get; set; }//;
		public decimal SalesLineNumber	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string Value	{ get; set; }//;
		public bool IsHeaderProperty	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
