namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ERPAppCancelContractResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ERPAppCancelContractResponse(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }
       
    }
}
