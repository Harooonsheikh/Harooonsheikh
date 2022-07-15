namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCreatePaymentJournalResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ErpCreatePaymentJournalResponse(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }
    }
}
