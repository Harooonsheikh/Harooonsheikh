namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTaxLine
	{
		public ErpTaxLine()
		{
		}
		public string TaxGroup	{ get; set; }//;
		public string TaxCode	{ get; set; }//;
		public decimal Percentage	{ get; set; }//;
		public decimal Amount	{ get; set; }//;
		public bool IsExempt	{ get; set; }//;
		public decimal TaxBasis	{ get; set; }//;
		public bool IsIncludedInPrice	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
