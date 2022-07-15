namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ERPAppReactivateContractResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ERPAppReactivateContractResponse(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }
       
    }
}
