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
    public class QuantityDiscountCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public QuantityDiscountCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public List<ErpProductQuantityDiscount> GetQuantityDiscount(string storeKey)
        {
            var quantityDiscountController = _crtFactory.CreateQuantityDiscountController(storeKey);
            return quantityDiscountController.GetQuantityDiscount();
        }
    }
}
