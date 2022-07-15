namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpInsufficientCredentialLengthException
    {
        public ErpInsufficientCredentialLengthException()
        {

        }
        public int ActualLength { get; set; }
        public int MinLength { get; set; }
    }
}
