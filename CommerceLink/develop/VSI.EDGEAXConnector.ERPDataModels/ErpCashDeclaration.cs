namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCashDeclaration
	{
		public ErpCashDeclaration()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Currency	{ get; set; }//;
		public decimal Amount	{ get; set; }//;
		public ErpCashType CashType	{ get; set; }//;
		public int CashTypeValue	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
