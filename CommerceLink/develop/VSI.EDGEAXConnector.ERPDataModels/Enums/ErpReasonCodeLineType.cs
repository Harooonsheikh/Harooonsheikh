namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public enum ErpReasonCodeLineType
	{
        Header = 0,
        Sales = 1,
        Payment = 2,
        IncomeExpense = 3,
        NoSale = 4,
        Affiliation = 5,
		//HK: D365 Update 10.0 Application change start
        Fiscal = 6
		//HK: D365 Update 10.0 Application change end
    }
}
