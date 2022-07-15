namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCurrency
	{
		public ErpCurrency()
		{
		}
		public string CurrencyCode	{ get; set; }//;
		public string CurrencySymbol	{ get; set; }//;
		public decimal RoundOffPrice	{ get; set; }//;
		public decimal RoundOffSales	{ get; set; }//;
		public int RoundOffTypePrice	{ get; set; }//;
		public int RoundOffTypeSales	{ get; set; }//;
		public short NumberOfDecimals	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
