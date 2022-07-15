using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomSalesOrder
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string ChannelReferenceId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public DateTimeOffset OrderPlacedDate { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal NetAmountWithNoTax { get; set; }
        public decimal NetAmountWithTax { get; set; }

        public EcomAddress BillingAddress { get; set; }
        public IList<EcomSalesLine> SalesLines { get; set; }
        public IList<EcomTenderLine> TenderLines { get; set; }
        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }
        
    }
}
