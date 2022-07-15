namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ProcessContractTerminateResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public ProcessContractTerminateResponse(bool success, string message)
        {
            this.Status = success;
            this.Message = message;
        }

        public ProcessContractTerminateResponse()
        {

        }
    }
}