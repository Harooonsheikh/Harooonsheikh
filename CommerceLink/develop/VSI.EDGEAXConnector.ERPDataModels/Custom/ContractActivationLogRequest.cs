using System;
using System.ComponentModel.DataAnnotations;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ContractActivationLogRequest
    {
        [Required]
        public string SalesId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime? RequestedDate { get; set; }
    }

    public class ContractActivationLogResponse
    {
        public ContractActivationLogResponse(bool status, string message, string result)
        {
            Status = status;
            Message = message;
            Result = result;
        }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }
    }
}