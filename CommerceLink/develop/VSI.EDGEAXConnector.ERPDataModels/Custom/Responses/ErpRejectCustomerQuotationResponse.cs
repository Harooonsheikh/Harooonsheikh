using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpRejectCustomerQuotationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string QuotationId { get; set; }

        public ErpRejectCustomerQuotationResponse(bool success, string message, string quotationId)
        {
            this.Success = success;
            this.Message = message;
            this.QuotationId = quotationId;
        }
    }
}
