namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ProcessContractOperationResponse
    {
        public ProcessContractOperationResponse(bool status, string message, object result)
        {
            this.status = status;
            this.message = message;
            this.result = result;
        }

        public bool status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
}
