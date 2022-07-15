using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
//using Autofac;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Configuration;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class QuantityDiscountController : ProductController, IQuantityDiscountController
    {
        public QuantityDiscountController(string storeKey) : base(storeKey)
        {

        }
    
        public List<ErpProductQuantityDiscount> GetQuantityDiscount()
        {
            var crtQuantityDiscountManager = new QuantityDiscountCRTManager();
            return crtQuantityDiscountManager.GetQuantityDiscount(currentStore.StoreKey);
        }
    }
}
