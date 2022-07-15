using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 8.1 Application change start
    public class ErpOrgUnitAvailabilitySearchCriteria
    {
        public ErpOrgUnitAvailabilitySearchCriteria()
        { }
        public string OrgUnitNumber { get; set; }
        public string OrgUnitName { get; set; }
    }
    //NS: D365 Update 8.1 Application change end
}
