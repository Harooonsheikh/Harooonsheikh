using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.MagentoAPI.Entities
{
    public class StatusHistory
    {
        public object comment { get; set; }
        public string created_at { get; set; }
        public string entity_id { get; set; }
        public string entity_name { get; set; }
        public string is_customer_notified { get; set; }
        public string is_visible_on_front { get; set; }
        public string parent_id { get; set; }
        public string status { get; set; }
    }
}
