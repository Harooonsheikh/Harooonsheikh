namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpTransferPartnerContractResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string MessageCode { get; set; }
        public string SalesOrderId { get; set; }

        public ErpTransferPartnerContractResponse(bool success, string message, string messageCode, string salesOrderId)
        {
            this.Success = success;
            this.Message = message;
            this.MessageCode = messageCode;
            this.SalesOrderId = salesOrderId;
        }
    }
}
