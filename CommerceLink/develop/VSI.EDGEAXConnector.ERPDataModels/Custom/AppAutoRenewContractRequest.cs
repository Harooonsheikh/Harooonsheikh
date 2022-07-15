namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class AppAutoRenewContractRequest
    {
        public string ChannelReferenceId { get; set; }
        public bool? AutoRenew { get; set; }
        public string SalesOrderId { get; set; }
        public string PONumber { get; set; }
        public bool IsUpdateRenewalPrice { get; set; }
    }
}
