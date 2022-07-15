namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpShift
	{
		public ErpShift()
		{
		}
		public string CashDrawer	{ get; set; }//;
		public long ShiftId	{ get; set; }//;
		public long StoreRecordId	{ get; set; }//;
		public string StoreId	{ get; set; }//;
		public string TerminalId	{ get; set; }//;
		public string StaffId	{ get; set; }//;
		public string CurrentStaffId	{ get; set; }//;
		public ErpShiftStatus Status	{ get; set; }//;
		public int StatusValue	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> StartDateTime	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> CloseDateTime	{ get; set; }//;
		public string ClosedAtTerminalId	{ get; set; }//;
		public string CurrentTerminalId	{ get; set; }//;
		public decimal SalesTotal	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> StatusDateTime	{ get; set; }//;
		public decimal ReturnsTotal	{ get; set; }//;
		public decimal PaidToAccountTotal	{ get; set; }//;
		public decimal TaxTotal	{ get; set; }//;
		public decimal DiscountTotal	{ get; set; }//;
		public decimal RoundedAmountTotal	{ get; set; }//;
		public int CustomerCount	{ get; set; }//;
		public int SaleTransactionCount	{ get; set; }//;
		public int NoSaleTransactionCount	{ get; set; }//;
		public int VoidTransactionCount	{ get; set; }//;
		public int LogOnTransactionCount	{ get; set; }//;
		public int SuspendedTransactionCount	{ get; set; }//;
		public int TransactionCount	{ get; set; }//;
		public System.Collections.Generic.IList<ErpShiftTenderLine> TenderLines	{ get; set; }//;
		public System.Collections.Generic.IList<ErpShiftAccountLine> AccountLines	{ get; set; }//;
		public decimal StartingAmountTotal	{ get; set; }//;
		public decimal FloatingEntryAmountTotal	{ get; set; }//;
		public decimal AddToTenderAmountTotal	{ get; set; }//;
		public decimal SafeDropTotal	{ get; set; }//;
		public decimal BankDropTotal	{ get; set; }//;
		public decimal RemoveTenderAmountTotal	{ get; set; }//;
		public decimal DeclareTenderAmountTotal	{ get; set; }//;
		public decimal OverShortTotal	{ get; set; }//;
		public decimal TenderedTotal	{ get; set; }//;
		public decimal ChangeTotal	{ get; set; }//;
		public decimal IncomeAccountTotal	{ get; set; }//;
		public decimal ExpenseAccountTotal	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start
        public decimal VoidedSalesTotal { get; set; }
        //NS: D365 Update 12 Platform change end

        //NS: D365 Update 8.1 Application change start

        public decimal ShiftSalesTotal { get; set; }
        public decimal ShiftReturnsTotal { get; set; }

        //NS: D365 Update 8.1 Application change end
//HK: D365 Update 10.0 Application change start
        public decimal ChargeTotal { get; set; }
        public decimal GiftCardCashOutTotal { get; set; }
//HK: D365 Update 10.0 Application change end
    }
}
