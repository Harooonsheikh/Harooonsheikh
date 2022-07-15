using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IInventoryController
    {
        List<ErpProduct> GetProductAvailabilities(List<ErpProduct> erpProducts, List<long> productIds, long channelId);
    }
}
