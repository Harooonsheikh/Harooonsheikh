using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface IShippingController
    {

        bool CreateShipping(ErpSalesLineShippingRate salesLineShippingRate);

        bool CreateShippingController();

        IEnumerable<ErpCommerceProperty> GetShippingCharges(string ShippingMethod, IEnumerable<ErpCommerceProperty> ProductIdWithQty, string ZipCode, string CountryCode, string Address1, string Address2);

    }
}
