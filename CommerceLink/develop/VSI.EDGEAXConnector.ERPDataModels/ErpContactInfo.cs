namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpContactInfo
	{
		public ErpContactInfo()
		{
		}
		public ErpContactInfoType AddressType	{ get; set; }//;
		public long RecordId	{ get; set; }//;
		public long PartyLocationRecordId	{ get; set; }//;
		public long PartyRecordId	{ get; set; }//;
		public string PartyNumber	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string Value	{ get; set; }//;
		public string ValueExtension	{ get; set; }//;
		public string LogisticsLocationId	{ get; set; }//;
		public long LogisticsLocationRecordId	{ get; set; }//;
		public long ParentLocation	{ get; set; }//;
		public bool IsPrimary	{ get; set; }//;
		public bool IsMobilePhone	{ get; set; }//;
		public bool IsPrivate	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
