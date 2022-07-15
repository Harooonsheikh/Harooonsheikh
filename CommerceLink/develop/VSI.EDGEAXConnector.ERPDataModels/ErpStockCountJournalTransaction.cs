namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpStockCountJournalTransaction
	{
		public ErpStockCountJournalTransaction()
		{
		}
		public string JournalId	{ get; set; }//;
		public long RecordId	{ get; set; }//;
		public int OperationType	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string ItemName	{ get; set; }//;
		public string InventDimId	{ get; set; }//;
		public decimal Counted	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public string Unit	{ get; set; }//;
		public string UserId	{ get; set; }//;
		public string TerminalId	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> CountedDate	{ get; set; }//;
		public int Status	{ get; set; }//;
		public string InventBatchId	{ get; set; }//;
		public string WarehouseLocationId	{ get; set; }//;
		public string WarehousePalletId	{ get; set; }//;
		public string InventSiteId	{ get; set; }//;
		public string InventLocationId	{ get; set; }//;
		public string ConfigId	{ get; set; }//;
		public string InventSizeId	{ get; set; }//;
		public string InventColorId	{ get; set; }//;
		public string InventStyleId	{ get; set; }//;
		public string InventSerialId	{ get; set; }//;
		public System.Guid TrackingGuid	{ get; set; }//;
		public bool UpdatedInAx	{ get; set; }//;
		public string Message	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
