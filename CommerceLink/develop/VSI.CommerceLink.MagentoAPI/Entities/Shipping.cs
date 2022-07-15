using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.MagentoAPI.Entities
{
    public class Shipping
    {
        public Address address { get; set; }
        public string method { get; set; }
        public Total total { get; set; }
    }
}
