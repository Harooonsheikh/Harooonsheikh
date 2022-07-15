namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpItemReservation
	{
		public ErpItemReservation()
		{
		}
		public System.Guid ReservationId	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string WarehouseInventoryDimensionId	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public System.DateTimeOffset ExpireDateTime	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
