namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpStorageLookup
	{
		public ErpStorageLookup()
		{
		}
		public long ChannelId	{ get; set; }//;
		public string OperatingUnitNumber	{ get; set; }//;
		public string ConnectionString	{ get; set; }//;
		public bool IsPublishedToLocal	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
