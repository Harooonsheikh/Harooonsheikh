using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class DiscountWithAffiliationCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public DiscountWithAffiliationCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public List<ErpProductPrice> GetIndependentProductPriceDiscount(List<ErpProduct> products, long channelId, List<long> productIds, string customerAccountNumber, string storeKey)
        {
            var discountWithAffiliationController = _crtFactory.CreateDiscountWithAffiliationController(storeKey);
            return discountWithAffiliationController.GetIndependentProductPriceDiscount(products, channelId, productIds, customerAccountNumber);
        }

        public ErpGetCustomIndependentProductPriceDiscountResponse GetIndependentProductPriceDiscount(List<long> productIds, string customerAccountNumber, List<ErpAffiliationLoyaltyTier> affiliations, string storeKey)
        {
            var discountController = _crtFactory.CreateDiscountController(storeKey);
            return discountController.GetIndependentProductPriceDiscount(productIds, customerAccountNumber, affiliations);
        }

        public List<ErpRetailDiscountWithAffiliationItem> GetDiscountWithAffiliationsByProductIds(long productId, string affiliationName, string currency, long channelId, string storeKey)
        {
            var discountController = _crtFactory.CreateDiscountWithAffiliationController(storeKey);
            return discountController.GetDiscountWithAffiliationsByProductIds(productId, affiliationName, currency, channelId);
        }
    }
}
