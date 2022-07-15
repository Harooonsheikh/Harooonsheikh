using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface IDiscountController
    {
        // List<ErpPriceAdjustment> GetDiscounts();
        List<ErpProductDiscount> GetDiscounts();

        /// <summary>
        /// Get Product Price Disocunt
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="customerAccountNumber"></param>
        /// <param name="affiliations"></param>
        /// <returns></returns>
        ErpGetCustomIndependentProductPriceDiscountResponse GetIndependentProductPriceDiscount(List<long> productIds
            , string customerAccountNumber, List<ErpAffiliationLoyaltyTier> affiliations);
    }
}
