namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTaxComponentIndia
	{
		public ErpTaxComponentIndia()
		{
		}
		public string TaxCode	{ get; set; }//;
		public string TaxType	{ get; set; }//;
		public string TaxComponent	{ get; set; }//;
		public string DataAreaId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
