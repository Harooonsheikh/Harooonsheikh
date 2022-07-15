using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
  public  class ErpUpdateCustomerContactPersonResponse
    {
        //public bool Success { get; set; }
        //public string Message { get; set; }
        //public ErpCustomer ErpCustomer { get; set; }
        //public ErpContactPerson ErpContactPerson { get; set; }

        //public ErpUpdateCustomerContactPersonResponse(bool success, string message, ErpCustomer erpCustomer,ErpContactPerson erpContactPerson)
        //{
        //    this.ErpCustomer = erpCustomer;
        //    this.ErpContactPerson=erpContactPerson;
        //    this.Success = success;
        //    this.Message = message;
        //}
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Customer { get; set; }
        public object ContactPerson { get; set; }

        public ErpUpdateCustomerContactPersonResponse(bool success, string message, object customer, object contactPerson)
        {
            this.Success = success;
            this.Message = message;
            this.Customer = customer;
            this.ContactPerson = contactPerson;
        }
    }
}
