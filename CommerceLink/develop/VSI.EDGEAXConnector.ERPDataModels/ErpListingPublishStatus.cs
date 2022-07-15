namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpListingPublishStatus
	{
		public ErpListingPublishStatus()
		{
		}
		public ErpListingPublishingActionStatus PublishStatus	{ get; set; }//;
		public System.DateTimeOffset ListingModifiedDateTime	{ get; set; }//;
		public string ChannelListingId	{ get; set; }//;
		public long ChannelId	{ get; set; }//;
		public long CatalogId	{ get; set; }//;
		public long ProductId	{ get; set; }//;
		public string ChannelBatchId	{ get; set; }//;
		public string ChannelState	{ get; set; }//;
		public ErpPublishingAction AppliedAction	{ get; set; }//;
		public System.DateTimeOffset StatusDateTime	{ get; set; }//;
		public string StatusMessage	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
