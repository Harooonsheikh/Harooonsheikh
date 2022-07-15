namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpSalesInvoiceLine
	{
		public ErpSalesInvoiceLine()
		{
		}
		public long RecordId	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string ProductName	{ get; set; }//;
		public string InventDimensionId	{ get; set; }//;
		public string InventTransactionId	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public decimal Price	{ get; set; }//;
		public decimal DiscountPercent	{ get; set; }//;
		public decimal DiscountAmount	{ get; set; }//;
		public string BatchId	{ get; set; }//;
		public decimal NetAmount	{ get; set; }//;
		public string Site	{ get; set; }//;
		public string Warehouse	{ get; set; }//;
		public string SerialId	{ get; set; }//;
		public string ColorId	{ get; set; }//;
		public string SizeId	{ get; set; }//;
		public string StyleId	{ get; set; }//;
		public string ConfigId	{ get; set; }//;
		public string ColorName	{ get; set; }//;
		public string SizeName	{ get; set; }//;
		public string StyleName	{ get; set; }//;
		public string ConfigName	{ get; set; }//;
		public string SalesTaxGroup	{ get; set; }//;
		public string ItemTaxGroup	{ get; set; }//;
		public decimal SalesMarkup	{ get; set; }//;
		public decimal TotalDiscount	{ get; set; }//;
		public decimal TotalPercentageDiscount	{ get; set; }//;
		public decimal LineDiscount	{ get; set; }//;
		public decimal PeriodicDiscount	{ get; set; }//;
		public decimal PeriodicPercentageDiscount	{ get; set; }//;
		public decimal LineManualDiscountPercentage	{ get; set; }//;
		public decimal LineManualDiscountAmount	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
