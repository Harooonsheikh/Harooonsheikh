namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomDiscountLine
    {
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public string DiscountCode { get; set; }
        public string OfferId { get; set; }
        public string CouponId { get; set; }
        public string OfferName { get; set; }
        public decimal EffectiveAmount { get; set; }
        public decimal Percentage { get; set; }
        public int DiscountLineTypeValue { get; set; }
        public int ManualDiscountTypeValue { get; set; }
        public int CustomerDiscountTypeValue { get; set; }
        public int PeriodicDiscountTypeValue { get; set; }

    }
}
