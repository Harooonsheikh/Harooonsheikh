namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ERPAppAutoRenewContractResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ERPAppAutoRenewContractResponse(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }
       
    }
}
