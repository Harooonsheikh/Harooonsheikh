namespace VSI.EDGEAXConnector.ERPDataModels
{

    //NS: D365 Update 9 Platform new enum
    public enum ErpReceiptType
	{
        Unknown = 0,
        SalesReceipt = 1,
        CardReceiptForShop = 2,
        CardReceiptForCustomer = 3,
        CardReceiptForShopReturn = 4,
        CardReceiptForCustomerReturn = 5,
        CustomerAccountReceiptForShop = 7,
        CustomerAccountReceiptForCustomer = 8,
        CustomerAccountReceiptForShopReturn = 9,
        CustomerAccountReceiptForCustomerReturn = 10,
        CustomerAccountDeposit = 14,
        CreditMemo = 15,
        SalesOrderReceipt = 18,
        GiftCertificate = 20,
        QuotationReceipt = 21,
        PackingSlip = 22,
        PickupReceipt = 23,
        XReport = 24,
        ZReport = 25,
        SafeDrop = 26,
        BankDrop = 27,
        TenderDeclaration = 28,
        RemoveTender = 29,
        FloatEntry = 30,
        StartingAmount = 31,
        OrderSummaryReceipt = 32,
        GiftReceipt = 33,
        ReturnLabel = 34,
        EFDocDANFESimplified = 35,
        EFDocDANFEDetailed = 36,
        //NS: D365 Update 12 Platform change start
        PickingList = 37,
        //NS: D365 Update 12 Platform change end

        //NS: D365 Update 8.1 Application change start
        OpenDrawer = 38,
        //NS: D365 Update 8.1 Application change end
		//HK: D365 Update 10.0 Application change start
        SuspendedTransaction = 39,
        CardTerminationReceiptForShop = 40,
        CardTerminationReceiptForCustomer = 41,
		//HK: D365 Update 10.0 Application change end
        CustomReceipt1 = 101,
        CustomReceipt2 = 102,
        CustomReceipt3 = 103,
        CustomReceipt4 = 104,
        CustomReceipt5 = 105,
        CustomReceipt6 = 106,
        CustomReceipt7 = 107,
        CustomReceipt8 = 108,
        CustomReceipt9 = 109,
        CustomReceipt10 = 110,
        CustomReceipt11 = 111,
        CustomReceipt12 = 112,
        CustomReceipt13 = 113,
        CustomReceipt14 = 114,
        CustomReceipt15 = 115,
        CustomReceipt16 = 116,
        CustomReceipt17 = 117,
        CustomReceipt18 = 118,
        CustomReceipt19 = 119,
        CustomReceipt20 = 120
	}
}
