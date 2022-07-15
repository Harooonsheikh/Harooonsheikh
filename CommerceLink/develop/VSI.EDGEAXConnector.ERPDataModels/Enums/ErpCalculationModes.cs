namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public enum ErpCalculationModes
	{
        None = 0,
        Prices = 1,
        Discounts = 2,
        Charges = 4,
        Taxes = 8,
        Totals = 16,
        Deposit = 32,
        AmountDue = 64,
        All = 127
    }
}
