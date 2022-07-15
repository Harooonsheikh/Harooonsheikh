namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpItem
	{
		public ErpItem()
		{
		}
		public long RecordId	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string Description	{ get; set; }//;
		public decimal BasePrice	{ get; set; }//;
		public long Product	{ get; set; }//;
		public string ChargeGroup	{ get; set; }//;
		public decimal Markup	{ get; set; }//;
		public System.Nullable<decimal> PriceQuantity	{ get; set; }//;
		public bool AllocateMarkup	{ get; set; }//;
		public string LineDiscountGroupId	{ get; set; }//;
		public string MultilineDiscountGroupId	{ get; set; }//;
		public bool IsTotalDiscountAllowed	{ get; set; }//;
		public bool NoDiscountAllowed	{ get; set; }//;
		public string ItemTaxGroupId	{ get; set; }//;
		public string SalesUnitOfMeasure	{ get; set; }//;
		public string InventoryUnitOfMeasure	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
