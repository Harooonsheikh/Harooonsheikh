using System;
using System.Collections.Generic;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpUpdateSubscriptionContract
    {
        public string RequestNumber { get; set; }
        public string RequestDate { get; set; }
        public string ContractAction { get; set; }
        public string PrimaryPACLicense { get; set; }
        public bool UseOldContractDates { get; set; }
        public string CouponCodes { get; set; }
        public string CustomerReference { get; set; }
        public List<ErpContractLine> ContractLines { get; set; }

    }

}
