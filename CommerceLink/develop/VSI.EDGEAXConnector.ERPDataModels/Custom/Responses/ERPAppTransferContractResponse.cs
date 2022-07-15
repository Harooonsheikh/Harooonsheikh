namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ERPAppTransferContractResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ERPAppTransferContractResponse(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }
       
    }
}
