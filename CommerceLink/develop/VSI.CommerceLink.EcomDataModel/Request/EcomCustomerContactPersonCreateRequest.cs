using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomCustomerContactPersonCreateRequest
    {
        public EcomCustomerContactPersonCreateRequest()
        {
            this.CustomerInfo = new EcomCustomerCreateRequest();
        }
        public bool UseMapping { get; set; }
        public EcomCustomerCreateRequest CustomerInfo { get; set; }
        public EcomContactPerson ContactPerson { get; set; }
    }
}
