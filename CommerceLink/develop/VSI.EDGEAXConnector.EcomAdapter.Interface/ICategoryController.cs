using System;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface ICategoryController : IDisposable
    {
        void PushCategories(ErpCatalog catalog);
    }
}