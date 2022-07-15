namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpLoyaltyRewardPointLine
	{
		public ErpLoyaltyRewardPointLine()
		{
		}
		public string TransactionId	{ get; set; }//;
		public long LoyaltyGroupRecordId	{ get; set; }//;
		public string LoyaltyCardNumber	{ get; set; }//;
		public string CustomerAccount	{ get; set; }//;
		public System.DateTimeOffset EntryDate	{ get; set; }//;
		public int EntryTime	{ get; set; }//;
		public ErpLoyaltyRewardPointEntryType EntryType	{ get; set; }//;
		public int EntryTypeValue	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> ExpirationDate	{ get; set; }//;
		public decimal LineNumber	{ get; set; }//;
		public long LoyaltyTierRecordId	{ get; set; }//;
		public long RewardPointRecordId	{ get; set; }//;
		public decimal RewardPointAmountQuantity	{ get; set; }//;
		public string RewardPointId	{ get; set; }//;
		public bool RewardPointIsRedeemable	{ get; set; }//;
		public ErpLoyaltyRewardPointType RewardPointType	{ get; set; }//;
		public string RewardPointCurrency	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
