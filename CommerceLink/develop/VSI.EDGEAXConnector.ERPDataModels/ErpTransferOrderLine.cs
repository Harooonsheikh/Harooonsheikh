namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTransferOrderLine
	{
		public ErpTransferOrderLine()
		{
		}
		public string RecordId	{ get; set; }//;
		public string OrderId	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string ItemName	{ get; set; }//;
		public string InventDimId	{ get; set; }//;
		public decimal QuantityTransferred	{ get; set; }//;
		public decimal QuantityShipped	{ get; set; }//;
		public decimal QuantityReceived	{ get; set; }//;
		public decimal QuantityShipNow	{ get; set; }//;
		public decimal QuantityReceiveNow	{ get; set; }//;
		public decimal QuantityRemainShip	{ get; set; }//;
		public decimal QuantityRemainReceive	{ get; set; }//;
		public string PurchaseUnit	{ get; set; }//;
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
