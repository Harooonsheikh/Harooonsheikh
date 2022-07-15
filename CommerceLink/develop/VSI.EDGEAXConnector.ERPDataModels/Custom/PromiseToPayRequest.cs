using System.ComponentModel.DataAnnotations;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class PromiseToPayRequest
    {
        [Required]
        public string InvoiceId { get; set; }
    }

    public class PromiseToPayResponse
    {
        public PromiseToPayResponse(bool status, string message)
        {
            Status = status;
            Message = message;
        }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}