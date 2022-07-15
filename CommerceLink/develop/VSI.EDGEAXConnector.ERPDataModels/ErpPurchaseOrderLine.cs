namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpPurchaseOrderLine
	{
		public ErpPurchaseOrderLine()
		{
		}
		public string RecordId	{ get; set; }//;
		public string OrderId	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string ItemName	{ get; set; }//;
		public string InventDimId	{ get; set; }//;
		public decimal QuantityOrdered	{ get; set; }//;
		public decimal PurchaseQuantity	{ get; set; }//;
		public string PurchaseUnit	{ get; set; }//;
		public decimal PurchaseReceived	{ get; set; }//;
		public decimal PurchaseReceivedNow	{ get; set; }//;
		public string InventBatchId	{ get; set; }//;
		public string WMSLocationId	{ get; set; }//;
		public string WMSPalletId	{ get; set; }//;
		public string InventSiteId	{ get; set; }//;
		public string InventLocationId	{ get; set; }//;
		public string ConfigId	{ get; set; }//;
		public string InventSerialId	{ get; set; }//;
		public string InventSizeId	{ get; set; }//;
		public string InventColorId	{ get; set; }//;
		public string InventStyleId	{ get; set; }//;
		public bool IsCommitted	{ get; set; }//;
		public string Message	{ get; set; }//;
		public string Guid	{ get; set; }//;
		public string DeliveryMethod	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
