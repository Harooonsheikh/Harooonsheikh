using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomCustomerUpdateRequest
    {
        public EcomCustomerUpdateRequest()
        {
            this.Customer = new EcomCustomer();
            // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in updateCustomerAPI - Start
            this.Customer.ExtensionProperties = new List<EcomCommerceProperty>();
            // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in updateCustomerAPI - End

            this.CreateNewContactPerson = false;
        }

        public EcomCustomer Customer { get; set; }
        public bool UseMapping { get; set; }
        [Required]
        public string CustomerAccountNumber { get; set; }
        public bool CreateNewContactPerson { get; set; }
        public EcomContactPerson ContactPerson { get; set; }
    }
}
