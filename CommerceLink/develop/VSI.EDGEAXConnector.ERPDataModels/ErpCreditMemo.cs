namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCreditMemo
	{
		public ErpCreditMemo()
		{
		}
		public string Id	{ get; set; }//;
		public decimal Balance	{ get; set; }//;
		public string CurrencyCode	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
