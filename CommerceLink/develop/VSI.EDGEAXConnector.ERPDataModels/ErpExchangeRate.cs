namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpExchangeRate
	{
		public ErpExchangeRate()
		{
		}
		public long RecordId	{ get; set; }//;
		public decimal Rate	{ get; set; }//;
		public string FromCurrency	{ get; set; }//;
		public string ToCurrency	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
