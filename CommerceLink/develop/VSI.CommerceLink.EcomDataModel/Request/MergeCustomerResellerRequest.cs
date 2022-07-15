using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Request
{
  public class MergeCustomerResellerRequest
    {
        public MergeCustomerResellerRequest()
        {
            this.UseMapping = false;
        }
        public EcomCustomer Customer { get; set; }
        public EcomCustomer Reseller { get; set; }
        public bool UseMapping { get; set; }
    }
}
