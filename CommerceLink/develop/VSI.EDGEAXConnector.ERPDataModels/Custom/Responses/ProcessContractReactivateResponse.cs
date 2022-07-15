namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ProcessContractReactivateResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public ProcessContractReactivateResponse(bool success, string message)
        {
            this.Status = success;
            this.Message = message;
        }

        public ProcessContractReactivateResponse()
        {

        }
    }
}
