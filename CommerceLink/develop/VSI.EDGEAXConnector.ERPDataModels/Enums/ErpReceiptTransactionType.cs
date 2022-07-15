namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public enum ErpReceiptTransactionType
	{
		//HK: D365 Update 10.0 Application change start
        None = 0,
        Sale = 1,
        Return = 2,
        Payment = 5,
        SalesOrder = 6,
        Quote = 7,
        SuspendedTransaction = 9
		//HK: D365 Update 10.0 Application change end
    }
}
