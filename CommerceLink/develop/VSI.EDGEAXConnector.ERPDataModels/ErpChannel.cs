namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpChannel
	{
		public ErpChannel()
		{
		}
		public string EventNotificationProfileId	{ get; set; }//;
		public long RecordId	{ get; set; }//;
		public ErpRetailChannelType OrgUnitType	{ get; set; }//;
		public int OrgUnitTypeValue	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string DefaultCustomerAccount	{ get; set; }//;
		public long CategoryHierarchyId	{ get; set; }//;
		public string InventoryLocationId	{ get; set; }//;
		public ErpChannelProfile ChannelProfile	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpChannelProperty> ChannelProperties	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpChannelLanguage> ChannelLanguages	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
