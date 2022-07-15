namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTenderLineBase
	{
		public ErpTenderLineBase()
		{
		}
		public string TenderLineId	{ get; set; }//;
		public decimal Amount	{ get; set; }//;
		public decimal CashBackAmount	{ get; set; }//;
		public decimal AmountInTenderedCurrency	{ get; set; }//;
		public decimal AmountInCompanyCurrency	{ get; set; }//;
		public string Currency	{ get; set; }//;
		public decimal ExchangeRate	{ get; set; }//;
		public decimal CompanyCurrencyExchangeRate	{ get; set; }//;
		public string TenderTypeId	{ get; set; }//;
		public string SignatureData	{ get; set; }//;
		public System.Collections.Generic.IList<ErpReasonCodeLine> ReasonCodeLines	{ get; set; }//;
		public decimal LineNumber	{ get; set; }//;
		public string GiftCardId	{ get; set; }//;
		public string CreditMemoId	{ get; set; }//;
		public string CustomerId	{ get; set; }//;
		public string LoyaltyCardId	{ get; set; }//;
		public string CardTypeId	{ get; set; }//;
		public bool IsChangeLine	{ get; set; }//;
		public ErpTenderLineStatus Status	{ get; set; }//;
		public bool IsVoided	{ get; set; }//;
		public bool IsHistorical	{ get; set; }//;
		public bool IsVoidable	{ get; set; }//;
		public int StatusValue	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
