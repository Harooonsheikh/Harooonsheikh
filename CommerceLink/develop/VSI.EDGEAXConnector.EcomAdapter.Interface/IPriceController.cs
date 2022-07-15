using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface IPriceController : IDisposable
    {
        string PushAllProductPrice(ErpPrice price, string fileName);
    }
}
