using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpAddPaymentLinkForInvoiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public ErpAddPaymentLinkForInvoiceResponse(bool success, string message,string errorcode)
        {
            this.Success = success;
            this.Message = message;
            this.ErrorCode = errorcode;
        }
    }
}
