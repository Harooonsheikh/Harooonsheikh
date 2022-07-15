using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpProductItemQuantityDiscountWithAffiliation
    {
        public string SKU { get; set; }
        public string TierPriceWebsiste { get; set; }
        public string TierPriceCustomerGroup { get; set; }
        public decimal TierPriceQuantity { get; set; }
        public decimal TierPrice { get; set; }
        public ErpMultiBuyDiscountType TierPriceValueType { get; set; }
        public string OfferId { get; set; }
        public string DiscountName { get; set; }
        public int PeriodicDiscountType { get; set; }
        public int DiscountType { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long AffiliationId { get; set; }
        public string AffiliationName { get; set; }
        public string TierPriceValueTypeUpdatedValue { get; set; }
        public string DiscountPercentageUpdatedValue { get; set; }
        public string UnitPriceUpdatedValue { get; set; }
        public string ValidationFrom { get; set; }
        public string ValidationTo { get; set; }
        public string CurrencyCode { get; set; }
    }
}
