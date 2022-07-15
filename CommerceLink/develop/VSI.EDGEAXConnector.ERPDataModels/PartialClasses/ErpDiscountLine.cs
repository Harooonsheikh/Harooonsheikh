
using System;
namespace VSI.EDGEAXConnector.ERPDataModels
{

    public partial class ErpDiscountLine
    {
        // Custom For VW  
        public string RebateCode { get; set; }
        public string SppNumber { get; set; }
        public string CouponId { get; set; }
        public decimal Tax { get; set; }
        public string TMVPriceOverrideReasonCode { get; set; }
        // public int PeriodicDiscountType { get; set; }
        public decimal TMVTargetAmount { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime? ContractValidFrom { get; set; }
        public DateTime? ContractValidTo { get; set; }
        public DateTime? ContractCalculateFrom { get; set; }
        public DateTime? ContractCalculateTo { get; set; }
        public int DiscountMethod { get; set; }
    }
}
