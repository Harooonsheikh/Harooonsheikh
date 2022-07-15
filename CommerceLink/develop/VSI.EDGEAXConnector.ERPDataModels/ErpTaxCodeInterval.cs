namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTaxCodeInterval
	{
		public ErpTaxCodeInterval()
		{
		}
		public string TaxItemGroup	{ get; set; }//;
		public string TaxCode	{ get; set; }//;
		public decimal TaxValue	{ get; set; }//;
		public decimal TaxLimitMinimum	{ get; set; }//;
		public decimal TaxLimitMaximum	{ get; set; }//;
		public bool IsTaxExempt	{ get; set; }//;
		public bool IsGroupRounding	{ get; set; }//;
		public string TaxCurrencyCode	{ get; set; }//;
		public int TaxBase	{ get; set; }//;
		public int TaxLimitBase	{ get; set; }//;
		public int TaxCalculationMethod	{ get; set; }//;
		public string TaxOnTax	{ get; set; }//;
		public string TaxUnit	{ get; set; }//;
		public decimal TaxMaximum	{ get; set; }//;
		public decimal TaxMinimum	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
