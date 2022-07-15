using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class StoreCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public StoreCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public List<ErpStore> SearchStores(string storeKey)
        {
            var storeController = _crtFactory.CreateStoreController(storeKey);
            return storeController.SearchStores();
        }

        public List<ErpCategory> GetChannelCategoryHierarchy(string storeKey)
        {
            var storeController = _crtFactory.CreateStoreController(storeKey);
            return storeController.GetChannelCategoryHierarchy();
        }
        public List<ErpAttributeCategory> GetChannelCategoryAttributes(ErpCategory category, string storeKey)
        {
            var storeController = _crtFactory.CreateStoreController(storeKey);
            return storeController.GetChannelCategoryAttributes(category);
        }

        public List<ErpInventoryInfo> GetStoreAvailability(string itemId, string variantId, string storeKey)
        {
            var storeController = _crtFactory.CreateStoreController(storeKey);
            return storeController.GetStoreAvailability(itemId, variantId);
        }

        public ErpChannelCustomDiscountThresholdResponse GetDiscountThreshold(string storeKey)
        {
            var storeController = _crtFactory.CreateStoreController(storeKey);
            return storeController.GetDiscountThreshold();
        }
    }
}
