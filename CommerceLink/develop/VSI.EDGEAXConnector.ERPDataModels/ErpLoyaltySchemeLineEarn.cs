namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpLoyaltySchemeLineEarn
	{
		public ErpLoyaltySchemeLineEarn()
		{
		}
		public long RecordId	{ get; set; }//;
		public long LoyaltyGroupRecordId	{ get; set; }//;
		public long LoyaltyTierRecordId	{ get; set; }//;
		public string FromActivityAmountCurrency	{ get; set; }//;
		public decimal FromActivityAmountQuantity	{ get; set; }//;
		public ErpLoyaltyActivityType FromActivityType	{ get; set; }//;
		public long FromCategoryRecordId	{ get; set; }//;
		public long FromProductRecordId	{ get; set; }//;
		public long FromVariantRecordId	{ get; set; }//;
		public long ToRewardPointRecordId	{ get; set; }//;
		public string ToRewardPointId	{ get; set; }//;
		public ErpLoyaltyRewardPointType ToRewardPointType	{ get; set; }//;
		public bool ToRewardPointIsRedeemable	{ get; set; }//;
		public string ToRewardPointCurrency	{ get; set; }//;
		public decimal ToRewardPointAmountQuantity	{ get; set; }//;
		public ErpDayMonthYear ToRewardPointExpirationTimeUnit	{ get; set; }//;
		public int ToRewardPointExpirationTimeValue	{ get; set; }//;
		public System.DateTimeOffset ValidFrom	{ get; set; }//;
		public System.DateTimeOffset ValidTo	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
