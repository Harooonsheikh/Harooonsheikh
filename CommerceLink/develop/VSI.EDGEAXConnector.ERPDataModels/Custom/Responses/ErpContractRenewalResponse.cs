namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpContractRenewalResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string SalesOrderId { get; set; }

        public ErpContractRenewalResponse(bool success, string message, string salesOrderId)
        {
            Success = success;
            Message = message;
            SalesOrderId = salesOrderId;
        }
    }
}
