using System;
using System.ComponentModel.DataAnnotations;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomCreateContractNewPaymentMethodRequest : EcomBaseRequest
    {
        [Required]
        public string SalesId { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public EcomCreateContractNewPaymentMethodCustomer Customer { get; set; }
        public EcomTenderLine TenderLine { get; set; }
    }
}
