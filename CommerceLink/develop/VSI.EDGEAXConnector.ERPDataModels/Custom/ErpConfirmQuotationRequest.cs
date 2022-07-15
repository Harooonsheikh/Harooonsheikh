using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpConfirmQuotationRequest
    {
        public ErpConfirmQuotationRequest()
        {
            this.TenderLine = new ErpTenderLine();
            this.SalesOrder = new ErpSalesOrder();
            this.Customer = new ConfirmQuotationCustomerDetail();
        }
        public string QuotationId { get; set; }
        public string Currency { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public string TransactionId { get; set; }
        public string TMVSalesOrigin { get; set; }
        public string TMVFraudReviewStatus { get; set; }
        public string TMVKountScore { get; set; }
        public ErpTenderLine TenderLine { get; set; }
        public ErpSalesOrder SalesOrder { get; set; }
        public ConfirmQuotationCustomerDetail Customer { get; set; }
        public string ChannelReferenceId { get; set; }        
    }
    public class ConfirmQuotationCustomerDetail
    {
        public ConfirmQuotationCustomerDetail()
        {
            this.BillingAddress = new ErpAddress();
        }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public ErpAddress BillingAddress { get; set; }
    }
}
