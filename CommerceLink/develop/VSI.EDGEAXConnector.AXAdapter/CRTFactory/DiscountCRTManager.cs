using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class DiscountCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public DiscountCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public List<ErpProductPrice> GetIndependentProductPriceDiscount(List<ErpProduct> products, long channelId, List<long> productIds, string customerAccountNumber, string storeKey)
        {
            var discountController = _crtFactory.CreateDiscountController(storeKey);
            return discountController.GetIndependentProductPriceDiscount(products, channelId, productIds, customerAccountNumber);
        }

        public ErpGetCustomIndependentProductPriceDiscountResponse GetIndependentProductPriceDiscount(List<long> productIds, string customerAccountNumber, List<ErpAffiliationLoyaltyTier> affiliations, string storeKey)
        {
            var discountController = _crtFactory.CreateDiscountController(storeKey);
            return discountController.GetIndependentProductPriceDiscount(productIds, customerAccountNumber, affiliations);
        }
    }
}
