using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IShippingController
    {
        IEnumerable<ErpCommerceProperty> GetShippingCharges(string shippingMethod, IEnumerable<ErpCommerceProperty> productIdWithQuantity, string zipCode, string countryCode, string address1, string address2);
        IEnumerable<ErpCommerceProperty> GetShippingTrackingId(string SalesOrderId);
    }
}
