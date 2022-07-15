using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomCustomerAttribute
    {
        public EcomCustomerAttribute()
        {

        }
        public string Name { get; set; }
        public EcomCommercePropertyValue AttributeValue { get; set; }
    }

}
