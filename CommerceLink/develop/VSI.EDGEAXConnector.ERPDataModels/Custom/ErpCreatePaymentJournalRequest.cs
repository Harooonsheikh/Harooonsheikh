using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpCreatePaymentJournalRequest
    {
        public ErpCreatePaymentJournalRequest()
        {
        }
        [Required]
        public string SalesId { get; set; }
        [Required]
        public string InvoiceId { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public string TransactionDate { get; set; }

        public PaymentJournalInfo Payment { get; set; }
    }

    public class PaymentJournalInfo
    {
        public decimal Amount { get; set; }
    }
}
