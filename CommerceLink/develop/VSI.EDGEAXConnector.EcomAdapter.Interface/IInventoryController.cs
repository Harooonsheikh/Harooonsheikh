using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface IInventoryController : IDisposable
    {
       void PushAllProductInventory(List<EcomcatalogProductCreateEntity> products);

        void PushAllProductInventory(ErpInventoryProducts inventoryProducts);


    }
}
