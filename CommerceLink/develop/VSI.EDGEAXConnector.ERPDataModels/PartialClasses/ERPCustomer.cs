using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpCustomer
    {
        public string EcomCustomerId { get; set; }
        public string SLBirthMonth { get; set; }
        public List<ErpAddress> CustomerAddresses { get; set; }
        public bool? IsAsyncCustomer { get; set; }
        public List<string> LicenceNumber { get; set; }
        public string ContactPersonId { get; set; }

        // Custom property for ingram
        public ErpAddress Address { get; set; }
        // [MB] - TV - BR 3.0 - 15007 - Start
        public bool SwapLanguage { get; set; }
        // [MB] - TV - BR 3.0 - 15007 - End
    }
}
