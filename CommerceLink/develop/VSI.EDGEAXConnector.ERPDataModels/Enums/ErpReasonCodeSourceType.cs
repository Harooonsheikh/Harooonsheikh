namespace VSI.EDGEAXConnector.ERPDataModels
{
    public enum ErpReasonCodeSourceType
	{
        None = 0,
        AddSalesperson = 1,
        EndOfTransaction = 2,
        ItemDiscount = 3,
        ItemNotOnFile = 4,
        LineItemTaxChange = 5,
        Markup = 6,
        NegativeAdjustment = 7,
        NfcEContingencyModeEnabled = 8,
        NfcEVoided = 9,
        OpenDrawer = 10,
        OverridePrice = 11,
        ReturnItem = 12,
        ReturnTransaction = 13,
        SerialNumber = 14,
        StartOfTransaction = 15,
        TenderDeclaration = 16,
        TotalDiscount = 17,
        TransactionTaxChange = 18,
        VoidItem = 19,
        VoidPayment = 20,
        VoidTransaction = 21,
        //NS: D365 Update 12 Platform change start
        OrderFulfillment = 22,
        //NS: D365 Update 12 Platform change end
		//HK: D365 Update 10.0 Application change start
        ManualCharge = 23
		//HK: D365 Update 10.0 Application change end
    }
}
