using System.Collections.Generic;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ERPContractSalesorderResponse 
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Contract> Contracts { get; set; } 

        public ERPContractSalesorderResponse(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
            this.Contracts = new List<Contract>();
        }
    }

    public class Contract
    {
        public ErpCustomer Customer { get; set; }
        public ErpContactPerson ContactPerson { get; set; }
        public List<ErpSalesOrder> SalesOrders { get; set; }

        public Contract(ErpCustomer customer, ErpContactPerson contactPerson, List<ErpSalesOrder> salesOrders)
        {
            this.Customer = customer;
            this.ContactPerson = contactPerson;
            this.SalesOrders = salesOrders;
        }
    }

}
