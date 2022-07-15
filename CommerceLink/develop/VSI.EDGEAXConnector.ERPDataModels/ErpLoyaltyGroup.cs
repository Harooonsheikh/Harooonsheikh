namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpLoyaltyGroup
	{
		public ErpLoyaltyGroup()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string Description	{ get; set; }//;
		public System.Collections.Generic.IList<ErpLoyaltyTier> LoyaltyTiers	{ get; set; }//;
		public System.Collections.Generic.IList<ErpLoyaltyCardTier> LoyaltyCardTiers	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
//HK: D365 Update 10.0 Application change start
        public int HighestActiveLoyaltyCardTier { get; set; }
//HK: D365 Update 10.0 Application change end
    }
}
