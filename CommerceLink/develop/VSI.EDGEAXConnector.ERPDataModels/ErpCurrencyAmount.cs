namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCurrencyAmount
	{
		public ErpCurrencyAmount()
		{
		}
		public string CurrencyCode	{ get; set; }//;
		public string CurrencySymbol	{ get; set; }//;
		public decimal ConvertedAmount	{ get; set; }//;
		public decimal RoundedConvertedAmount	{ get; set; }//;
		public decimal ExchangeRate	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
