namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public enum ErpGiftCardOperationType
    {
        None = 0,
        Issue = 1,
        AddTo = 2,
		//HK: D365 Update 10.0 Application change start
        CashOut = 3
		//HK: D365 Update 10.0 Application change end
    }
}
