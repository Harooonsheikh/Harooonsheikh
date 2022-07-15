using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomCustomerCreateRequest
    {
        public EcomCustomerCreateRequest()
        {
            this.Customer = new EcomCustomer();
            //this.Customer.SwapLanguage = true;
            this.Customer.ExtensionProperties = new List<EcomCommerceProperty>();
            this.UseMapping = false;
        }
        public EcomCustomer Customer { get; set; }
        [Required]
        public string CustomerAccountNumber { get; set; }
        public bool UseMapping { get; set; }
    }
}
