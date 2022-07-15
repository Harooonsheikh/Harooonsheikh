namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReceiptHeaderTaxInfoIndia
	{
		public ErpReceiptHeaderTaxInfoIndia()
		{
		}
		public string ValueAddedTaxTINNumber	{ get; set; }//;
		public string CentralSalesTaxTINNumber	{ get; set; }//;
		public string ServiceTaxNumber	{ get; set; }//;
		public string ExciseTaxNumber	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
