namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 9 Platform new enum
    public enum ErpAuditEventType
    {
        Unknown = 0,
        UserLogOn = 1,
        UserLogOff = 2,
        ManagerOverride = 3,
        ItemVoid = 4,
        TransactionVoid = 5,
        PrintReceiptCopy = 6,
        //NS: D365 Update 12 Platform change start
        PriceCheck = 7,
        TaxOverride = 8,
        QuantityCorrection = 9,
        PurgeTransactionsData = 10
        //NS: D365 Update 12 Platform change end
    }
}
