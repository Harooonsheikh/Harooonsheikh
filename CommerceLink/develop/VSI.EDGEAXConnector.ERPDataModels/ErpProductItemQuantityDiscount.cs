using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    /// <summary>
    /// This class will be used for creation of QuantityDiscount xml file in Magento Adapter
    /// </summary>
    public class ErpProductItemQuantityDiscount
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
    }
}
