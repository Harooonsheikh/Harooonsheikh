using System;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface IDiscountWithAffiliationController : IDisposable
    {
        void PushAllProductDiscountWithAffiliations(ErpDiscountWithAffiliation erpDiscountWithAffiliation);
    }
}
