using System.Collections.Generic;
using System.Collections.ObjectModel;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface IStoreController
    {
       List<ErpStore> GetStores();

        ReadOnlyCollection<object> InventoryLookup(string itemId,string variantId);
        ReadOnlyCollection<object> InvokeExtensionMethod(string methodName, params object[] parameters);

        List<ErpInventoryInfo> GetStoreAvailability(string itemId, string variantId);
        ErpChannelCustomDiscountThresholdResponse GetDiscountThreshold();
    }
}
