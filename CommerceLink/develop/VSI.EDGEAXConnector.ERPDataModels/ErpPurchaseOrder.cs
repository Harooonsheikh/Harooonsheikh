namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpPurchaseOrder
	{
		public ErpPurchaseOrder()
		{
		}
		public string OrderId	{ get; set; }//;
		public string RecordId	{ get; set; }//;
		public string Status	{ get; set; }//;
		public decimal Lines	{ get; set; }//;
		public decimal TotalItems	{ get; set; }//;
		public ErpPurchaseTransferOrderType OrderType	{ get; set; }//;
		public int OrderTypeValue	{ get; set; }//;
		public decimal TotalReceived	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpPurchaseOrderLine> OrderLines	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
