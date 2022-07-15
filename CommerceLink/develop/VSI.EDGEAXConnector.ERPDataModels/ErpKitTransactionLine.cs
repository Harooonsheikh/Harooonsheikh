namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpKitTransactionLine
	{
		public ErpKitTransactionLine()
		{
		}
		public string ItemId	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public string InventoryDimensionId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
