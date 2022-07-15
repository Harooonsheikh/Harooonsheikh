using System;
namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpGiftCard
	{
		public ErpGiftCard()
		{
		}
		public string Id	{ get; set; }//;
		public decimal Balance	{ get; set; }//;
		public string BalanceCurrencyCode	{ get; set; }//;
		public decimal BalanceInCardCurrency	{ get; set; }//;
		public string CardCurrencyCode	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
		//HK: D365 Update 10.0 Application change start
        public decimal GiftCardIssueAmount { get; set; }
        public DateTime? GiftCardActiveFrom { get; set; }
        public DateTime? GiftCardExpireDate { get; set; }
        public string GiftCardHistoryDetails { get; set; }
		//HK: D365 Update 10.0 Application change end

	}
}
