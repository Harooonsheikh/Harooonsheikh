namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpOrgUnitLocation
	{
		public ErpOrgUnitLocation()
		{
		}
		public System.Collections.ObjectModel.Collection<ErpOrgUnitContact> Contacts	{ get; set; }//;
		public long ChannelId	{ get; set; }//;
		public decimal Latitude	{ get; set; }//;
		public decimal Longitude	{ get; set; }//;
		public string OrgUnitName	{ get; set; }//;
		public string OrgUnitNumber	{ get; set; }//;
		public string Street	{ get; set; }//;
		public string City	{ get; set; }//;
		public string Zip	{ get; set; }//;
		public string County	{ get; set; }//;
		public string State	{ get; set; }//;
		public string Country	{ get; set; }//;
		public double Distance	{ get; set; }//;
		public string InventoryLocationId	{ get; set; }//;
		public string InventorySiteId	{ get; set; }//;
		public long PostalAddressId	{ get; set; }//;
		public int OpenFrom	{ get; set; }//;
		public int OpenTo	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
