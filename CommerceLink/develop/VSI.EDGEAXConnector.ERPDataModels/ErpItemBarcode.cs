namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpItemBarcode
	{
		public ErpItemBarcode()
		{
		}
		public long RecordId	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string BarcodeSetupId	{ get; set; }//;
		public string ItemBarcodeValue	{ get; set; }//;
		public string InventoryDimensionId	{ get; set; }//;
		public int Blocked	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public bool RetailShowForItem	{ get; set; }//;
		public string RetailVariantId	{ get; set; }//;
		public string UnitId	{ get; set; }//;
		public bool UseForInput	{ get; set; }//;
		public string UseForPrinting	{ get; set; }//;
		public string ConfigId	{ get; set; }//;
		public string InventoryStyleId	{ get; set; }//;
		public string InventoryColorId	{ get; set; }//;
		public string InventorySizeId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
