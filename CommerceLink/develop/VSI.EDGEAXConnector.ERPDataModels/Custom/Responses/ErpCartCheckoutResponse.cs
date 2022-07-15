using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCartCheckoutResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErpSalesOrder SalesOrder { get; set; }

        public ErpCartCheckoutResponse(bool success, string message, ErpSalesOrder salesOrder)
        {
            this.SalesOrder = salesOrder;
            this.Success = success;
            this.Message = message;
        }
    }
}
