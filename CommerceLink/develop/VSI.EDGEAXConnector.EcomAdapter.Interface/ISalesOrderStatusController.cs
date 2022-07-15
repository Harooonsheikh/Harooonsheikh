using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface ISalesOrderStatusController : IDisposable
    {
        List<ErpSalesOrderStatus> UpdateOrderStatus(List<ErpSalesOrderStatus> orderStatuses);    
    }
}
