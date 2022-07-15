namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTaxableItem
	{
		public ErpTaxableItem()
		{
		}
		public string ItemId	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public decimal Price	{ get; set; }//;
		public string ItemTaxGroupId	{ get; set; }//;
		public string SalesTaxGroupId	{ get; set; }//;
		public decimal TaxAmount	{ get; set; }//;
		public string SalesOrderUnitOfMeasure	{ get; set; }//;
		public decimal NetAmount	{ get; set; }//;
		public decimal NetAmountPerUnit	{ get; set; }//;
		public decimal GrossAmount	{ get; set; }//;
		public System.Collections.Generic.IList<ErpTaxLine> TaxLines	{ get; set; }//;
		public decimal TaxAmountExemptInclusive	{ get; set; }//;
		public decimal TaxAmountInclusive	{ get; set; }//;
		public decimal TaxAmountExclusive	{ get; set; }//;
		public decimal NetAmountWithAllInclusiveTax	{ get; set; }//;
		public decimal NetAmountWithAllInclusiveTaxPerUnit	{ get; set; }//;
		public System.DateTimeOffset BeginDateTime	{ get; set; }//;
		public System.DateTimeOffset EndDateTime	{ get; set; }//;
		public decimal TaxRatePercent	{ get; set; }//;
		public bool IsReturnByReceipt	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
