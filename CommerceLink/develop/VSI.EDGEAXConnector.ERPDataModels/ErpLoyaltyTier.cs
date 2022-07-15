namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpLoyaltyTier
	{
		public ErpLoyaltyTier()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string TierId	{ get; set; }//;
		public decimal TierLevel	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
