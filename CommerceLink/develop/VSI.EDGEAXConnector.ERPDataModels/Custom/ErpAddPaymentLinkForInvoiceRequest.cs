using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpAddPaymentLinkForInvoiceRequest
    {
        public ErpAddPaymentLinkForInvoiceRequest()
        {
            this.TenderLine = new ErpTenderLine();
            this.SalesOrder = new ErpSalesOrder();
            this.Customer = new CustomerDetail();
        }
        public string SalesId { get; set; }
        public string InvoiceId { get; set; }
        public string Currency { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public string TransactionId { get; set; }
        public ErpTenderLine TenderLine { get; set; }
        public ErpSalesOrder SalesOrder { get; set; }
        public CustomerDetail Customer { get; set; }
        public string ChannelReferenceId { get; set; }
    }

    public class CustomerDetail
    {
        public CustomerDetail()
        {
            this.BillingAddress = new ErpAddress();
        }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public ErpAddress BillingAddress { get; set; }
    }
}
