using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.MagentoAPI.Entities
{
    public class ShippingAssignment
    {
        public Shipping shipping { get; set; }
        public List<Item2> items { get; set; }
    }
}
