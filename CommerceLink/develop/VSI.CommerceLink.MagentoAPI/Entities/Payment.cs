using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.MagentoAPI.Entities
{
    public class Payment
    {
        public object account_status { get; set; }
        public List<string> additional_information { get; set; }
        public string amount_authorized { get; set; }
        public string amount_ordered { get; set; }
        public string base_amount_authorized { get; set; }
        public string base_amount_ordered { get; set; }
        public string base_shipping_amount { get; set; }
        public object cc_last4 { get; set; }
        public string entity_id { get; set; }
        public string method { get; set; }
        public string parent_id { get; set; }
        public string shipping_amount { get; set; }
        public List<object> extension_attributes { get; set; }
    }

}
