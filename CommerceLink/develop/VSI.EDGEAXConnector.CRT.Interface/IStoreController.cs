using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.CRTClasses;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IStoreController
    {
        List<ErpStore> SearchStores();
        List<ErpCategory> GetChannelCategoryHierarchy();
        List<ErpAttributeCategory> GetChannelCategoryAttributes(ErpCategory category);

        List<ErpInventoryInfo> GetStoreAvailability(string itemId, string variantId);
        ErpChannelCustomDiscountThresholdResponse GetDiscountThreshold();
    }
}
