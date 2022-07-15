using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCustomerInoviceResponse
    {
        public bool Status { get; set; }
        public List<ErpCustomerInvoice> CustomerInvoices { get; set; }
        public string Message { get; set; }

        public ErpCustomerInoviceResponse(bool status, List<ErpCustomerInvoice> customerInvoice, string message)
        {
            this.Status = status;
            this.CustomerInvoices = customerInvoice;
            this.Message = message;
        }
    }
}
