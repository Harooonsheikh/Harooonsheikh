using System.ComponentModel.DataAnnotations;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class UnblockContractRequest
    {
        [Required]
        public string SalesOrderId { get; set; }
        [Required]
        public string InvoiceId { get; set; }
    }

    public class UnblockContractResponse
    {
        public UnblockContractResponse(bool status, string message)
        {
            Status = status;
            Message = message;
        }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}