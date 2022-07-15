using System;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomDiscountInfo
    {
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountCode { get; set; }
        public string OfferName { get; set; }
        public decimal Percentage { get; set; }
        public string PeriodicDiscountOfferId { get; set; }
        public int CustomerDiscountType { get; set; }
        public int DiscountOriginType { get; set; }
        public int ManualDiscountType { get; set; }
    }
}
