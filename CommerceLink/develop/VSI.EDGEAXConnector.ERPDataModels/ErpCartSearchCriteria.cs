using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
//HK: D365 Update 10.0 Application change start
    public class ErpCartSearchCriteria
    {
        public string CartId { get; set; }
        public long? ExpectedCartVersion { get; set; }
        public string CustomerAccountNumber { get; set; }
        public bool? IncludeAnonymous { get; set; }
        public bool? SuspendedOnly { get; set; }
        public int? CartTypeValue { get; set; }
    }
//HK: D365 Update 10.0 Application change end
}
