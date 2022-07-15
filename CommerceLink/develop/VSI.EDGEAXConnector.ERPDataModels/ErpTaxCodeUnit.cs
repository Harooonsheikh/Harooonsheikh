namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTaxCodeUnit
	{
		public ErpTaxCodeUnit()
		{
		}
		public string TaxCode	{ get; set; }//;
		public decimal TaxRoundOff	{ get; set; }//;
		public int TaxRoundOffType	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
