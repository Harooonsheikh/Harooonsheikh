namespace VSI.EDGEAXConnector.ERPDataModels.BoletoPayment
{
    public class BoletoResultDetail
    {
        public string ExtendedDescription { get; set; }
        public string ConnectorTxID1 { get; set; }
        public string MerchantAccount { get; set; }
        public string ConnectorInstanceId { get; set; }
        public string AcquirerReturnCode { get; set; }
        public string ConnectorVersion { get; set; }
        public string AcquirerResponse { get; set; }
        public string EXTERNAL_SYSTEM_LINK { get; set; }
        public string ConnectorName { get; set; }
        public string AcquirerReturnMessage { get; set; }
        public string AcquirerTxId1 { get; set; }
        public string AcquirerTxId2 { get; set; }
    }
}
