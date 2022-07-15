using System.ComponentModel.DataAnnotations;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class UpdateBillingAddressRequest
    {
        [Required]
        public string SalesOrderId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string VATNumber { get; set; }
        [Required]
        public string LocalTaxId { get; set; }
        public BillingAddress BillingAddress { get; set; }
    }

    public class UpdateBillingAddressResponse
    {
        public UpdateBillingAddressResponse(bool status, string message)
        {
            Status = status;
            Message = message;
        }
        public bool Status { get; set; }
        public string Message { get; set; }
    }

    public class BillingAddress
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ThreeLetterISORegionName { get; set; }
    }

    
}
