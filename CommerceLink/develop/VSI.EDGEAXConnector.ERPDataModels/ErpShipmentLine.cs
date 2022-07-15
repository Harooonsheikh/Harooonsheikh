namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpShipmentLine
	{
		public ErpShipmentLine()
		{
		}
		public string ShipmentLineId	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string InventDimId	{ get; set; }//;
		public decimal OrderedQuantity	{ get; set; }//;
		public decimal DeliveredQuantity	{ get; set; }//;
		public decimal RemainingQuantity	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
