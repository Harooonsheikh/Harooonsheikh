namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpShipment
	{
		public ErpShipment()
		{
		}
		public string SalesId	{ get; set; }//;
		public string ShipmentId	{ get; set; }//;
		public ErpAddress DeliveryAddress	{ get; set; }//;
		public System.DateTimeOffset ShipDate	{ get; set; }//;
		public string DeliveryMode	{ get; set; }//;
		public string TermsOfDelivery	{ get; set; }//;
		public string TrackingNumber	{ get; set; }//;
		public string TrackingUrl	{ get; set; }//;
		public ErpTrackingInfo LatestCarrierTrackingInfo	{ get; set; }//;
		public string TransactionId	{ get; set; }//;
		public string CarrierId	{ get; set; }//;
		public string CarrierName	{ get; set; }//;
		public System.Collections.ObjectModel.Collection<ErpShipmentLine> ShipmentLines	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
//HK: D365 Update 10.0 Application change start
        public string WeightUnit { get; set; }
        public decimal ShippingWeight { get; set; }
//HK: D365 Update 10.0 Application change end

    }
}
