using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class QuantityDiscountWithAffiliationCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public QuantityDiscountWithAffiliationCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public List<ErpProductQuantityDiscountWithAffiliation> GetQuantityDiscountWithAffiliation(string storeKey)
        {
            var quantityDiscountWithAffiliationController = _crtFactory.CreateQuantityDiscountWithAffiliationController(storeKey);
            return quantityDiscountWithAffiliationController.GetQuantityDiscountWithAffiliation();
        }
    }
}
