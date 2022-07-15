using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.MagentoAPI.Entities
{
    public class Total
    {
        public string base_shipping_amount { get; set; }
        public string base_shipping_discount_amount { get; set; }
        public string base_shipping_incl_tax { get; set; }
        public string base_shipping_tax_amount { get; set; }
        public string shipping_amount { get; set; }
        public string shipping_discount_amount { get; set; }
        public string shipping_discount_tax_compensation_amount { get; set; }
        public string shipping_incl_tax { get; set; }
        public string shipping_tax_amount { get; set; }
    }
}
