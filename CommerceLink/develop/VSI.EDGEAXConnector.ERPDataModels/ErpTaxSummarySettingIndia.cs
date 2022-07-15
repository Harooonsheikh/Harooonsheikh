namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTaxSummarySettingIndia
	{
		public ErpTaxSummarySettingIndia()
		{
		}
		public ErpReceiptTaxDetailsTypeIndia TaxDetailsType	{ get; set; }//;
		public bool ShowTaxonTax	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
