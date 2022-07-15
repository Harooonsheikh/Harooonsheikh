namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpFormulaIndia
	{
		public ErpFormulaIndia()
		{
		}
		public bool SupportedTaxBasis	{ get; set; }//;
		public bool SupportedTaxBasisForMiscCharge	{ get; set; }//;
		public int Id	{ get; set; }//;
		public ErpTaxableBasisIndia TaxableBasis	{ get; set; }//;
		public bool PriceIncludesTax	{ get; set; }//;
		public string CalculationExpression	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
