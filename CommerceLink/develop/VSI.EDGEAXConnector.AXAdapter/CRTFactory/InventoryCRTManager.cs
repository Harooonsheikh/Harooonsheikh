using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class InventoryCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public InventoryCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public List<ErpProduct> GetProductAvailabilities(List<ErpProduct> erpProducts, List<long> productIds, long channelId, string storeKey)
        {
            var inventoryController = _crtFactory.CreateInventoryController(storeKey);
            return inventoryController.GetProductAvailabilities(erpProducts, productIds, channelId);
        }
    }
}
