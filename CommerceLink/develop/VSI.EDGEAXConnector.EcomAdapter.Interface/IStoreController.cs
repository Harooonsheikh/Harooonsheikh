using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface IStoreController : IDisposable
    {
        void CreateStores(List<EcomstoreEntity> stores);

        void PushStoresInfo(ErpStoreInfo erpStores);
    }
}
