using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpGetCustomerQuotationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErpCustomer Customer { get; set; }
        public ErpContactPerson ContactPerson { get; set; }
        public List<ErpCustomerOrderInfo> Quotations { get; set; }

        public ErpGetCustomerQuotationResponse(bool success, string message, ErpCustomer customer, ErpContactPerson contactPerson, List<ErpCustomerOrderInfo> quotation)
        {
            this.Success = success;
            this.Message = message;
            this.Customer = customer;
            this.ContactPerson = contactPerson;
            this.Quotations = quotation;
        }
    }
}
