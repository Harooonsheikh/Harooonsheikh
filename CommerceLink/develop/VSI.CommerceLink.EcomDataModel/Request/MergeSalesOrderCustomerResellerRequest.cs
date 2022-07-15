using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Request
{
  public class MergeSalesOrderCustomerResellerRequest
    {
        public MergeSalesOrderCustomerResellerRequest()
        {
            this.UseMapping = false;
        }
        public EcomCustomer Customer { get; set; }
        public EcomCustomer Reseller { get; set; }
        [Required]
        public bool UseMapping { get; set; }
        [Required]
        public string SalesOrder { get; set; }
    }
}
