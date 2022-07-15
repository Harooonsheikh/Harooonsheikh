using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpMergeSalesOrderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Customer { get; set; }
        public object ContactPerson { get; set; }
        public object SalesOrder { get; set; }
        public ErpMergeSalesOrderResponse(bool success, string message, object customer, object contactPerson, object SalesOrder)
        {
            this.Success = success;
            this.Message = message;
            this.Customer = customer;
            this.ContactPerson = contactPerson;
            this.SalesOrder = SalesOrder;
        }
    }
}
