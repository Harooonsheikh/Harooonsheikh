namespace VSI.EDGEAXConnector.ERPDataModels
{
    public enum ErpSalesTransactionType
	{
        None = -1,
        Sales = 2,
        CustomerAccountDeposit = 3,
        SalesInvoice = 15,
        IncomeExpense = 18,
        CustomerOrder = 19,
        PendingSalesOrder = 27,
        AsyncCustomerQuote = 31,
        AsyncCustomerOrder = 33,
        //HK: D365 Update 10.0 Application change start
        SuspendedSalesTransaction = 36
		//HK: D365 Update 10.0 Application change end
    }
}
