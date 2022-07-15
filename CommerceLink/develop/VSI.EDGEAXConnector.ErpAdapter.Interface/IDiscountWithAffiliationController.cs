using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface IDiscountWithAffiliationController
    {
        // List<ErpPriceAdjustment> GetDiscounts();
        List<ErpProductDiscountWithAffiliation> GetDiscountsWithAffiliation();

        /// <summary>
        /// Get Product Price Disocunt
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="customerAccountNumber"></param>
        /// <param name="affiliations"></param>
        /// <returns></returns>
        ErpGetCustomIndependentProductPriceDiscountResponse GetIndependentProductPriceDiscount(List<long> productIds
            , string customerAccountNumber, List<ErpAffiliationLoyaltyTier> affiliations);

        /// <summary>
        /// Get Discounts With Affiliation by productIds
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        List<ErpProductDiscountWithAffiliation> GetDiscountsWithAffiliationByProductIds(string ItemId, string Variant, string AffiliationId, string Currency);
    }
}
