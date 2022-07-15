namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public enum ErpTransactionType
	{
        None = -1,
        LogOff = 0,
        LogOn = 1,
        Sales = 2,
        Payment = 3,
        RemoveTender = 4,
        FloatEntry = 5,
        ChangeTender = 6,
        TenderDeclaration = 7,
        OpenDrawer = 9,
        SalesOrder = 14,
        SalesInvoice = 15,
        BankDrop = 16,
        SafeDrop = 17,
        IncomeExpense = 18,
        CustomerOrder = 19,
        StartingAmount = 20,
        SuspendShift = 21,
        BlindCloseShift = 22,
        CloseShift = 23,
        PrintX = 24,
        PrintZ = 25,
        PendingSalesOrder = 27,
        KitDisassembly = 28,
        AsyncCustomerQuote = 31,
        AsyncCustomerOrder = 33,
		//HK: D365 Update 10.0 Application change start
        ForceDeleteShift = 34,
        GiftCardInquiry = 35,
        SuspendedTransaction = 36
		//HK: D365 Update 10.0 Application change end
    }
}
