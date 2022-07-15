namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTrackingInfo
	{
		public ErpTrackingInfo()
		{
		}
		public string TrackingNumber	{ get; set; }//;
		public string ServiceType	{ get; set; }//;
		public ErpWeight PackageWeight	{ get; set; }//;
		public System.Nullable<System.DateTime> ShippedOnDate	{ get; set; }//;
		public string Status	{ get; set; }//;
		public System.Nullable<System.DateTime> DeliveredOnDate	{ get; set; }//;
		public System.Nullable<System.DateTime> EstimatedDeliveryDate	{ get; set; }//;
		public string TrackingUrl	{ get; set; }//;
		public string PackagingType	{ get; set; }//;
		public ErpAddress DestinationAddress	{ get; set; }//;
		public ErpAddress OriginAddress	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpShipmentProgress> ShipmentProgress	{ get; set; }//;
		public decimal ShippingCharge	{ get; set; }//;
	}
}
