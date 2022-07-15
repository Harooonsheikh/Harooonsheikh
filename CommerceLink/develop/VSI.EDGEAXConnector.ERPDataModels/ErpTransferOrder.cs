using System;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTransferOrder
	{
		public ErpTransferOrder()
		{
		}
		public string OrderId	{ get; set; }//;
		public string RecordId	{ get; set; }//;
		public string Status	{ get; set; }//;
		public decimal Lines	{ get; set; }//;
		public decimal TotalItems	{ get; set; }//;
		public ErpPurchaseTransferOrderType OrderType	{ get; set; }//;
		public int OrderTypeValue	{ get; set; }//;
		public string InventLocationIdFrom	{ get; set; }//;
		public string InventLocationIdTo	{ get; set; }//;
		public decimal QuantityShipped	{ get; set; }//;
		public decimal QuantityReceived	{ get; set; }//;
		public decimal QuantityShipNow	{ get; set; }//;
		public decimal QuantityReceiveNow	{ get; set; }//;
		public decimal QuantityShipRemaining	{ get; set; }//;
		public decimal QuantityReceiveRemaining	{ get; set; }//;
		public System.DateTimeOffset ShipDate	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpTransferOrderLine> OrderLines	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start
        public DateTimeOffset ReceiveDate { get; set; }
        //NS: D365 Update 12 Platform change end

        //NS: D365 Update 8.1 Application change start

        public string DeliveryModeId { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}
