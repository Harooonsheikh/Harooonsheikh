using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VSI.CommerceLink.EcomDataModel.Enum;

namespace VSI.CommerceLink.EcomDataModel.Request
{
    public class CreateMergedCartRequest : EcomBaseRequest
    {
        [Required]
        public string CartId { get; set; }
        public long AffiliationId { get; set; }
        public EcomCalculationModes CalculationModes { get; set; }
        public List<EcomCartLine> CartLines { get; set; }
        public EcomDeliverySpecification DeliverySpecification { get; set; }
        public bool isLegacyDiscountCode { get; set; }
        public List<string> CouponCodes { get; set; }
    }
}
