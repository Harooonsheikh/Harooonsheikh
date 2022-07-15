namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpLoyaltyRewardPoint
	{
		public ErpLoyaltyRewardPoint()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Description	{ get; set; }//;
		public ErpDayMonthYear ExpirationTimeUnit	{ get; set; }//;
		public int ExpirationTimeValue	{ get; set; }//;
		public bool IsRedeemable	{ get; set; }//;
		public int RedeemRanking	{ get; set; }//;
		public string RewardPointCurrency	{ get; set; }//;
		public string RewardPointId	{ get; set; }//;
		public ErpLoyaltyRewardPointType RewardPointType	{ get; set; }//;
		public decimal IssuedPoints	{ get; set; }//;
		public decimal UsedPoints	{ get; set; }//;
		public decimal ExpiredPoints	{ get; set; }//;
		public decimal ActivePoints	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
