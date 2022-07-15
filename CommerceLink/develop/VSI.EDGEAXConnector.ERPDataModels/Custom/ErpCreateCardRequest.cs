using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpCreateCardRequest
    {
        public ErpCreateCardRequest()
        {
            this.TenderLine = new ErpTenderLine();
            this.SalesOrder = new ErpSalesOrder();
            this.Customer = new CustomerData();
        }
        public string Currency { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public string TransactionId { get; set; }
        public ErpTenderLine TenderLine { get; set; }
        public ErpSalesOrder SalesOrder { get; set; }
        public CustomerData Customer { get; set; }
    }

    public class CustomerData
    {
        public CustomerData()
        {
            this.BillingAddress = new ErpAddress();
        }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public ErpAddress BillingAddress { get; set; }
    }
}
