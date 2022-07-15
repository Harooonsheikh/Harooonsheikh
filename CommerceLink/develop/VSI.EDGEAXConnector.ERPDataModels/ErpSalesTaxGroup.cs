namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpSalesTaxGroup
	{
		public ErpSalesTaxGroup()
		{
		}
		public long RecordId	{ get; set; }//;
		public string TaxGroup	{ get; set; }//;
		public string TaxGroupName	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
