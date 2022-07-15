namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpDeviceConfiguration
	{
		public ErpDeviceConfiguration()
		{
		}
		public bool AllowItemsAggregation	{ get; set; }//;
		public bool AggregateItemsForPrinting	{ get; set; }//;
		public bool AggregatePayments	{ get; set; }//;
		public bool AuditEnabled	{ get; set; }//;
		public string DiscountAtTotal	{ get; set; }//;
		public bool DisplaySecondaryTotalCurrency	{ get; set; }//;
		public string EndOfTransaction	{ get; set; }//;
		public bool LimitStaffListToStore	{ get; set; }//;
		public string LineItemTaxChange	{ get; set; }//;
		public decimal MaximumPrice	{ get; set; }//;
		public decimal MaximumQuantity	{ get; set; }//;
		public int MinimumPasswordLength	{ get; set; }//;
		public bool MustKeyInPriceIfZero	{ get; set; }//;
		public string FunctionalityProfileDescription	{ get; set; }//;
		public string OverridePrice	{ get; set; }//;
		public bool PrintXZReportsOnTerminal	{ get; set; }//;
		public string ProfileId	{ get; set; }//;
		public string RefundSale	{ get; set; }//;
		public string SalesPerson	{ get; set; }//;
		public string SecondaryTotalCurrency	{ get; set; }//;
		public bool ShowStaffListAtLogOn	{ get; set; }//;
		public bool StaffBarcodeLogOn	{ get; set; }//;
		public bool StaffBarcodeLogOnRequiresPassword	{ get; set; }//;
		public bool StaffCardLogOn	{ get; set; }//;
		public bool StaffCardLogOnRequiresPassword	{ get; set; }//;
		public string StartOfTransaction	{ get; set; }//;
		public bool EnableTimeRegistration	{ get; set; }//;
		public string TenderDeclaration	{ get; set; }//;
		public string TransactionTaxChange	{ get; set; }//;
		public string VoidItem	{ get; set; }//;
		public string VoidPayment	{ get; set; }//;
		public string VoidTransaction	{ get; set; }//;
		public string CultureName	{ get; set; }//;
		public bool HideTrainingMode	{ get; set; }//;
		public string StorePhone	{ get; set; }//;
		public ErpStatementMethod StatementMethod	{ get; set; }//;
		public bool StatementPostingAsBusinessDay	{ get; set; }//;
		public int StatementCalculationBatchEndTimeInSeconds	{ get; set; }//;
		public string StoreNumber	{ get; set; }//;
		public long ChannelId	{ get; set; }//;
		public string StoreTaxGroup	{ get; set; }//;
		public string TaxIdNumber	{ get; set; }//;
		public long TaxOverrideGroup	{ get; set; }//;
		public int TenderDeclarationCalculation	{ get; set; }//;
		public bool UseCustomerBasedTax	{ get; set; }//;
		public bool UseDestinationBasedTax	{ get; set; }//;
		public int AutoLogOffTimeoutInMinutes	{ get; set; }//;
		public string CustomerDisplayText1	{ get; set; }//;
		public string CustomerDisplayText2	{ get; set; }//;
		public long EFTStoreId	{ get; set; }//;
		public string EFTTerminalId	{ get; set; }//;
		public bool ExitAfterEachTransaction	{ get; set; }//;
		public string HardwareProfile	{ get; set; }//;
		public string Placement	{ get; set; }//;
		public string TerminalDescription	{ get; set; }//;
		public bool OpenDrawerAtLogOnLogOff	{ get; set; }//;
		public bool PrintTaxRefundChecks	{ get; set; }//;
		public bool StandAlone	{ get; set; }//;
		public string TerminalId	{ get; set; }//;
		public bool TerminalStatement	{ get; set; }//;
		public string VisualProfile	{ get; set; }//;
		public string Currency	{ get; set; }//;
		public string InventLocationId	{ get; set; }//;
		public bool IncludeKitComponents	{ get; set; }//;
		public int AccentColor	{ get; set; }//;
		public string Theme	{ get; set; }//;
		public string LogOnBackgroundPictureAsBase64	{ get; set; }//;
		public string BackgroundPictureAsBase64	{ get; set; }//;
		public bool RequireAmountDeclaration	{ get; set; }//;
		public int MaxTransactionSearchResults	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start

        public string SystemLegalTermsUrl { get; set; }
        public string SystemPrivacyStatementUrl { get; set; }
        public string RejectOrderFulfillment { get; set; }
        public int AutoExitMethodValue { get; set; }
        public int DenominationsToDisplayValue { get; set; }
        public int EmployeeLogonTypeValue { get; set; }
        public bool ShowAppBarLabel { get; set; }

        //NS: D365 Update 12 Platform change end


        //NS: D365 Update 8.1 Application change start
        public bool ManuallyCalculateTaxes { get; set; }
        public string FiscalRegistrationProcessId { get; set; }
        public bool IncludeSalesOrderInvoices { get; set; }
        public bool IncludeFreeTextInvoices { get; set; }
        public bool IncludeProjectInvoices { get; set; }
        public bool IncludeCreditNoteInvoices { get; set; }

        //NS: D365 Update 8.1 Application change end
		//HK: D365 Update 10.0 Application change start
        public bool DisplayTaxExemptInLineDetails { get; set; }
        public string TaxExemptReceiptIndicator { get; set; }
        public bool ManuallyCalculateCharges { get; set; }
        public string ChargeOverrideReasonCode { get; set; }
		//HK: D365 Update 10.0 Application change end
    }
}