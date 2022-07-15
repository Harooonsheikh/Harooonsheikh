namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpLoyaltyCardTier
	{
		public ErpLoyaltyCardTier()
		{
		}
		public long RecordId	{ get; set; }//;
		public long LoyaltyTierRecordId	{ get; set; }//;
		public string TierId	{ get; set; }//;
		public System.DateTimeOffset ValidFrom	{ get; set; }//;
		public System.DateTimeOffset ValidTo	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
