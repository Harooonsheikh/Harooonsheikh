using System.ComponentModel.DataAnnotations;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class AssignCustomerPortalAccessRequest
    {
        [Required]
        public string PACLicense { get; set; }

        [Required]
        public string InvoiceId { get; set; }
    }

    public class AssignCustomerPortalAccessResponse
    {
        public AssignCustomerPortalAccessResponse(bool status, string message)
        {
            Status = status;
            Message = message;
            StatusCode = "ERROR";
        }

        public AssignCustomerPortalAccessResponse(bool status, string message, string statusCode)
        {
            Status = status;
            Message = message;
            StatusCode = statusCode;
        }
        
        public bool Status { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }

    }
}