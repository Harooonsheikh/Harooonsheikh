using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCustomerInoviceDetailResponse
    {
        public bool Status { get; set; }
        public ErpCustomer CustomerInfo { get; set; }
        public ErpCustomerInvoiceDetail CustomerInvoiceDetails { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }

        public ErpCustomerInoviceDetailResponse(bool status, ErpCustomer customerInfo, ErpCustomerInvoiceDetail customerInvoice, string message, string errorCode = "")
        {
            this.Status = status;
            this.CustomerInfo = customerInfo;
            this.CustomerInvoiceDetails = customerInvoice;
            this.Message = message;
            this.ErrorCode = errorCode;
        }
    }
}
