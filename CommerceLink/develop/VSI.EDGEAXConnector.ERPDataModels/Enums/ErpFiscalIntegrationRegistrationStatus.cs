using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Enums
{
//HK: D365 Update 10.0 Application change start
    public enum ErpFiscalIntegrationRegistrationStatus
    {
        None = 0,
        Completed = 1,
        Skipped = 2,
        MarkedAsRegistered = 3
    }
//HK: D365 Update 10.0 Application change end
}
