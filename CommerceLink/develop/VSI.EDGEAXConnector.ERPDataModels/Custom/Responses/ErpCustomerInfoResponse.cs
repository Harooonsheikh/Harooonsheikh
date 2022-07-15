using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCustomerInfoResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public CustomerInfo CustomerInfo { get; set; }

        public ErpCustomerInfoResponse(bool status, string message, CustomerInfo customerInfo)
        {
            this.Status = status;
            this.Message = message;
            this.CustomerInfo = customerInfo;
        }
    }
    public class CustomerInfo
    {
        public string InvoiceId { get; set; }
        public string SalesId { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
