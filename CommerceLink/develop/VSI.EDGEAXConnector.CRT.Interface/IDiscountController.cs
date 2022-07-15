using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IDiscountController
    {
        List<ErpProductPrice> GetIndependentProductPriceDiscount(List<ErpProduct> products, long channelId, List<long> productIds, string customerAccountNumber);
        ErpGetCustomIndependentProductPriceDiscountResponse GetIndependentProductPriceDiscount(List<long> productIds, string customerAccountNumber, List<ErpAffiliationLoyaltyTier> affiliations);
    }
}
