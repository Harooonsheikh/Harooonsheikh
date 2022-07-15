using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
//HK: D365 Update 10.0 Application change start
    public class ErpChargeLineOverride
    {
        public decimal? OriginalAmount { get; set; }
        public decimal? OverrideAmount { get; set; }
        public string OverrideReasonDescription { get; set; }
        public DateTimeOffset? OverrideDateTime { get; set; }
        public string UserId { get; set; }
    }
//HK: D365 Update 10.0 Application change end
}
