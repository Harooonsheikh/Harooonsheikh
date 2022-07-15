using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IPriceController
    {
        List<ErpProductPrice> GetActiveProductPrice(long channelId, long catalogId, List<long> productIds, DateTime date, string customerAccountNumber);
    }
}
