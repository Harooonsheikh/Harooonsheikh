namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductCatalog
	{
		public ErpProductCatalog()
		{
		}
		public long RecordId	{ get; set; }//;
		public long ProductId	{ get; set; }//;
		public long ChannelId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string Language	{ get; set; }//;
		public bool IsSnapshotEnabled	{ get; set; }//;
		public System.DateTimeOffset ValidFrom	{ get; set; }//;
		public System.DateTimeOffset ValidTo	{ get; set; }//;
		public System.DateTimeOffset CreatedOn	{ get; set; }//;
		public System.DateTimeOffset ModifiedOn	{ get; set; }//;
		public System.DateTimeOffset PublishedOn	{ get; set; }//;
		public ErpRichMediaLocations Image	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
