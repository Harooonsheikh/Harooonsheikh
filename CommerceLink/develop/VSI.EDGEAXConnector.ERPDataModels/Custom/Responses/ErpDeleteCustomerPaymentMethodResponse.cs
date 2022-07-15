using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpDeleteCustomerPaymentMethodResponse
    {
        public ErpDeleteCustomerPaymentMethodResponse(bool status, string message)
        {
            this.Status = status;
            this.Message = message;
        }

        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
