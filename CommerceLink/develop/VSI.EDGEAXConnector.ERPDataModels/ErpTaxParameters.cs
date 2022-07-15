namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTaxParameters
	{
		public ErpTaxParameters()
		{
		}
		public bool VATIndia	{ get; set; }//;
		public bool ServiceTaxIndia	{ get; set; }//;
		public bool SalesTaxIndia	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
