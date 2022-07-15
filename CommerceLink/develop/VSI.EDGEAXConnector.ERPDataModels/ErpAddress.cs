namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpAddress
	{
		public ErpAddress()
		{
		}
		public string Name	{ get; set; }
		public string FullAddress	{ get; set; }
		public long RecordId	{ get; set; }
		public string Street	{ get; set; }
		public string StreetNumber	{ get; set; }
		public string County	{ get; set; }
		public string City	{ get; set; }
		public string DistrictName	{ get; set; }
		public string State	{ get; set; }
		public string ZipCode	{ get; set; }
		public string ThreeLetterISORegionName	{ get; set; }
		public string Phone	{ get; set; }
		public long PhoneRecordId	{ get; set; }
		public string PhoneExt	{ get; set; }
		public string Email	{ get; set; }
		public string EmailContent	{ get; set; }
		public long EmailRecordId	{ get; set; }
		public string Url	{ get; set; }
		public long UrlRecordId	{ get; set; }
		public string TwoLetterISORegionName	{ get; set; }
		public bool Deactivate	{ get; set; }
		public string AttentionTo	{ get; set; }
		public string BuildingCompliment	{ get; set; }
		public string Postbox	{ get; set; }
		public string TaxGroup	{ get; set; }
		public ErpAddressType AddressType	{ get; set; }
		public int AddressTypeValue	{ get; set; }

        //[MB] - Custom for TV - 14719 - Customer address type string value - Start
        public string AddressTypeStrValue { get; set; }
        //[MB] - Custom for TV - 14719 - Customer address type string value - End

        public bool IsPrimary	{ get; set; }
		public bool IsPrivate	{ get; set; }
		public string PartyNumber	{ get; set; }
		public long DirectoryPartyTableRecordId	{ get; set; }
		public long DirectoryPartyLocationRecordId	{ get; set; }
		public long DirectoryPartyLocationRoleRecordId	{ get; set; }
		public string LogisticsLocationId	{ get; set; }
		public long LogisticsLocationRecordId	{ get; set; }
		public long LogisticsLocationExtRecordId	{ get; set; }
		public long LogisticsLocationRoleRecordId	{ get; set; }
		public long PhoneLogisticsLocationRecordId	{ get; set; }
		public string PhoneLogisticsLocationId	{ get; set; }
		public long EmailLogisticsLocationRecordId	{ get; set; }
		public string EmailLogisticsLocationId	{ get; set; }
		public long UrlLogisticsLocationRecordId	{ get; set; }
		public string UrlLogisticsLocationId	{ get; set; }
		public long ExpireRecordId	{ get; set; }
		public int SortOrder	{ get; set; }
		public string EntityName	{ get; set; }
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }
	    public System.Collections.Generic.List<ErpCommerceProperty> ExtensionProperties	{ get; set; }
		public object Item	{ get; set; }
        public bool IsAsyncAddress { get; set; }
        public string CountyName { get; set; }
        public string StateName { get; set; }

        //NS: D365 Update 8.1 Application change start
        public int RoleCount { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}
