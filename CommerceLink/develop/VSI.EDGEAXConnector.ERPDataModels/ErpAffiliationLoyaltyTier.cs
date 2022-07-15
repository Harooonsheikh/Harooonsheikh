namespace VSI.EDGEAXConnector.ERPDataModels
{
		public class ErpAffiliationLoyaltyTier
	{
		public ErpAffiliationLoyaltyTier()
		{
		}
		public long AffiliationId	{ get; set; }//;
		public long LoyaltyTierId	{ get; set; }//;
		public ErpRetailAffiliationType AffiliationType	{ get; set; }//;
		public System.Collections.ObjectModel.Collection<ErpReasonCodeLine> ReasonCodeLines	{ get; set; }//;
		public string CustomerId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
