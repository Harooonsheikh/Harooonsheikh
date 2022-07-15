namespace VSI.EDGEAXConnector.ERPDataModels
{

    //NS: D365 Update 9 Platform new enum
    public enum ErpReasonCodeInputRequiredType
	{
        None = 0,
        SubCode = 1,
        Date = 2,
        Numeric = 3,
        Item = 4,
        Customer = 5,
        Staff = 6,
        Text = 9,
        SubCodeButtons = 10,
        AgeLimit = 11,
        //NS: D365 Update 12 Platform change start
        CompositeSubCodes = 12
        //NS: D365 Update 12 Platform change end
    }
}
