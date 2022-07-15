using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface IQuantityDiscountWithAffiliationController : IDisposable
    {
        void PushAllQuantityDiscountWithAffiliations(ErpQuantityDiscountWithAffiliation erpQuantityDiscountWithAffiliation, List<ErpProduct> erpProductList);
    }
}
