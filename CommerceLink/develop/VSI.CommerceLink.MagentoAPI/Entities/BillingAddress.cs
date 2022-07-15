using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.MagentoAPI.Entities
{
    public class BillingAddress
    {
        public string address_type { get; set; }
        public string city { get; set; }
        public string country_id { get; set; }
        public string email { get; set; }
        public string entity_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string parent_id { get; set; }
        public string postcode { get; set; }
        public string region { get; set; }
        public string region_code { get; set; }
        public string region_id { get; set; }
        public List<string> street { get; set; }
        public string telephone { get; set; }
    }
}
