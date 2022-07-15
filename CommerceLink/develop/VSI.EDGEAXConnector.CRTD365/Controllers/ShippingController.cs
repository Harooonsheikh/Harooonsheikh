using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class ShippingController : BaseController, IShippingController
    {

        public ShippingController(string storeKey) : base(storeKey)
        {

        }
        public IEnumerable<ErpCommerceProperty> GetShippingCharges(string shippingMethod, IEnumerable<ErpCommerceProperty> productIdWithQuantity, string zipCode, string countryCode, string address1, string address2)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<ErpCommerceProperty> GetShippingTrackingId(string SalesOrderId)
        {
            throw new NotImplementedException();
        }
    }
}
