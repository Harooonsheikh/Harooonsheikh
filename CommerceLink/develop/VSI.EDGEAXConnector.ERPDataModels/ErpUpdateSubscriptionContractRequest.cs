using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpUpdateSubscriptionContractRequest
    {
        public string RequestNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public string ContractAction { get; set; }
        public string PrimaryPacLicense { get; set; }
        public bool UseOldContractDates { get; set; }
        public string CustomerReference { get; set; }
        public List<ErpCoupon> CouponCodes { get; set; }
        public List<ErpContractLine> ContractLines { get; set; }

    }
}
