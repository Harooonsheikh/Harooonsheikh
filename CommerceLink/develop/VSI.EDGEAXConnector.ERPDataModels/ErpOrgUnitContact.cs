using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public sealed class ErpOrgUnitContact
    {
        public ErpOrgUnitContact() { }

        public ErpContactInfoType ContactType { get; internal set; }
        public string Description { get; internal set; }
        public long LocationRecordId { get; internal set; }
        public string Locator { get; internal set; }

    }
}
