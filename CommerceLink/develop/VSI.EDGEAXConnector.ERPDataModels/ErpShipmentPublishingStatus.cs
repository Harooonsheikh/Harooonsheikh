namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpShipmentPublishingStatus
	{
		public ErpShipmentPublishingStatus()
		{
		}
		public string SalesId	{ get; set; }//;
		public string ShipmentId	{ get; set; }//;
		public ErpShipmentPublishingActionStatus PublishingStatus	{ get; set; }//;
		public string PublishingMessage	{ get; set; }//;
		public string ChannelBatchId	{ get; set; }//;
		public string ChannelReferenceId	{ get; set; }//;
		public System.DateTimeOffset LastModifiedDateTime	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
