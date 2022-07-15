using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
//HK: D365 Update 10.0 Application change start
    public class ErpFiscalIntegrationRegistrationSettings
    {
        public string SkipReasonCode { get; set; }
        public string MarkAsRegisteredReasonCode { get; set; }
    }
//HK: D365 Update 10.0 Application change end
}
