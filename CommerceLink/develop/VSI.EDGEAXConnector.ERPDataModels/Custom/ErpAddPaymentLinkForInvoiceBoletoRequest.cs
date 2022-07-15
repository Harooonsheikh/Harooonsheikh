using System;
using VSI.EDGEAXConnector.ERPDataModels.BoletoPayment;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpAddPaymentLinkForInvoiceBoletoRequest
    {
        public string SalesId { get; set; }
        public string InvoiceId { get; set; }
        public string ChannelReferenceId { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public ErpPaymentDetail Payment { get; set; } 
    }

    public class ErpPaymentDetail
    {
        public decimal Amount { get; set; }
        public string ProcessorId { get; set; }
        public Boleto Boleto { get; set; }
        public string BoletoXml { get; set; }
    }
}
