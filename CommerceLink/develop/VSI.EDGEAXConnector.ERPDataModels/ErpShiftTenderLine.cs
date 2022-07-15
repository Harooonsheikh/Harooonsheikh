namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpShiftTenderLine
	{
		public ErpShiftTenderLine()
		{
		}
		public string TenderTypeId	{ get; set; }//;
		public string TenderTypeName	{ get; set; }//;
		public ErpTransactionType TransactionType	{ get; set; }//;
		public bool ChangeLine	{ get; set; }//;
		public string CardTypeId	{ get; set; }//;
		public string CardTypeName	{ get; set; }//;
		public string TenderCurrency	{ get; set; }//;
		public bool CountingRequired	{ get; set; }//;
		public decimal StartingAmountOfStoreCurrency	{ get; set; }//;
		public decimal StartingAmountOfTenderCurrency	{ get; set; }//;
		public decimal FloatingEntryAmountOfStoreCurrency	{ get; set; }//;
		public decimal FloatingEntryAmountOfTenderCurrency	{ get; set; }//;
		public decimal SafeDropAmountOfStoreCurrency	{ get; set; }//;
		public decimal SafeDropAmountOfTenderCurrency	{ get; set; }//;
		public decimal BankDropAmountOfStoreCurrency	{ get; set; }//;
		public decimal BankDropAmountOfTenderCurrency	{ get; set; }//;
		public decimal RemoveTenderAmountOfStoreCurrency	{ get; set; }//;
		public decimal RemoveTenderAmountOfTenderCurrency	{ get; set; }//;
		public decimal DeclareTenderAmountOfStoreCurrency	{ get; set; }//;
		public decimal DeclareTenderAmountOfTenderCurrency	{ get; set; }//;
		public decimal TenderedAmountOfStoreCurrency	{ get; set; }//;
		public decimal TenderedAmountOfTenderCurrency	{ get; set; }//;
		public decimal ChangeAmountOfStoreCurrency	{ get; set; }//;
		public decimal ChangeAmountOfTenderCurrency	{ get; set; }//;
		public int Count	{ get; set; }//;
		public decimal ShiftAmountInStoreCurrency	{ get; set; }//;
		public decimal ShiftAmountOfTenderCurrency	{ get; set; }//;
		public decimal AddToTenderAmountOfStoreCurrency	{ get; set; }//;
		public decimal AddToTenderAmountOfTenderCurrency	{ get; set; }//;
		public decimal RemoveFromTenderAmountOfStoreCurrency	{ get; set; }//;
		public decimal RemoveFromTenderAmountOfTenderCurrency	{ get; set; }//;
		public decimal TotalRemovedFromTenderAmountOfStoreCurrency	{ get; set; }//;
		public decimal TotalRemovedFromTenderAmountOfTenderCurrency	{ get; set; }//;
		public decimal OverShortAmountOfStoreCurrency	{ get; set; }//;
		public decimal OverShortAmountOfTenderCurrency	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
