using System;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpLoyaltyCard
	{
		public ErpLoyaltyCard()
		{
		}
		public long RecordId	{ get; set; }//;
		public string CardNumber	{ get; set; }//;
		public ErpLoyaltyCardTenderType CardTenderType	{ get; set; }//;
		public string CustomerAccount	{ get; set; }//;
		public string CustomerDataAreaId	{ get; set; }//;
		public long PartyRecordId	{ get; set; }//;
		public string PartyNumber	{ get; set; }//;
		public System.Collections.Generic.IList<ErpLoyaltyGroup> LoyaltyGroups	{ get; set; }//;
		public System.Collections.Generic.IList<ErpLoyaltyRewardPoint> RewardPoints	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 8.1 Application change start
        public DateTimeOffset LoyaltyEnrollmentDate { get; set; }
        public long OmOperatingUnitId { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}
