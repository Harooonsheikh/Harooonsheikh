namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpSalesInvoice
	{
		public ErpSalesInvoice()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Id	{ get; set; }//;
		public string SalesId	{ get; set; }//;
		public ErpSalesInvoiceType SalesType	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> InvoiceDate	{ get; set; }//;
		public string CurrencyCode	{ get; set; }//;
		public decimal Amount	{ get; set; }//;
		public decimal AmountPaid	{ get; set; }//;
		public string Account	{ get; set; }//;
		public string Name	{ get; set; }//;
		public decimal TotalManualDiscountPercentage	{ get; set; }//;
		public decimal TotalManualDiscountAmount	{ get; set; }//;
		public System.Collections.Generic.IList<ErpSalesInvoiceLine> SalesInvoiceLine	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
