using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface IDiscountController : IDisposable
    {
        string PushAllProductDiscounts(ErpDiscount erpDiscount, string fileName);
    

    
    }
}
