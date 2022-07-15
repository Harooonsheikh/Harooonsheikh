namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpLoyaltySchemeLineRedeem
	{
		public ErpLoyaltySchemeLineRedeem()
		{
		}
		public long RecordId	{ get; set; }//;
		public long LoyaltyGroupRecordId	{ get; set; }//;
		public long LoyaltyTierRecordId	{ get; set; }//;
		public long FromRewardPointRecordId	{ get; set; }//;
		public string FromRewardPointId	{ get; set; }//;
		public bool FromRewardPointIsRedeemable	{ get; set; }//;
		public ErpLoyaltyRewardPointType FromRewardPointType	{ get; set; }//;
		public decimal FromRewardPointAmountQuantity	{ get; set; }//;
		public string FromRewardPointCurrency	{ get; set; }//;
		public long ToCategoryRecordId	{ get; set; }//;
		public long ToProductRecordId	{ get; set; }//;
		public long ToVariantRecordId	{ get; set; }//;
		public string ToRewardAmountCurrency	{ get; set; }//;
		public decimal ToRewardAmountQuantity	{ get; set; }//;
		public ErpLoyaltyRewardType ToRewardType	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
