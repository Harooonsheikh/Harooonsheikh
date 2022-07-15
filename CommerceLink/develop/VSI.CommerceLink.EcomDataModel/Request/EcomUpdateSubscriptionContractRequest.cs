using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class EcomUpdateSubscriptionContractRequest : EcomBaseRequest
    {
        [Required]
        public string RequestNumber { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        [Required]
        public string ContractAction { get; set; }
        [Required]
        public string PrimaryPacLicense { get; set; }
        public bool UseOldContractDates { get; set; }
        [Required]
        public string CustomerReference { get; set; } = "";
        public List<EcomCoupon> CouponCodes { get; set; }
        public List<EcomContractLine> ContractLines { get; set; }
    }
}
