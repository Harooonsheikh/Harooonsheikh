using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class ShippingCRTManager
    {

        private readonly ICRTFactory _crtFactory;

        public ShippingCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        /// <summary>
        /// Get shipping charges against each product
        /// </summary>
        /// <param name="ShippingMethod"></param>
        /// <param name="ProductIdWithQty"></param>
        /// <param name="ZipCode"></param>
        /// <param name="CountryCode"></param>
        /// <param name="Address1"></param>
        /// <param name="Address2"></param>
        /// <returns></returns>
        public IEnumerable<ErpCommerceProperty> GetShippingCharges(string ShippingMethod, IEnumerable<ErpCommerceProperty> ProductIdWithQty, string ZipCode, string CountryCode, string Address1, string Address2, string storeKey)
        {
            var storeController = _crtFactory.CreateShippingController(storeKey);
            var data = storeController.GetShippingCharges(ShippingMethod, ProductIdWithQty, ZipCode, CountryCode, Address1, Address2);
            return data;
        }

        /// <summary>
        /// Get tracking data against sales order
        /// </summary>
        /// <param name="SalesOrderId"></param>
        /// <returns></returns>
        public IEnumerable<ErpCommerceProperty> GetShippingTrackingId(string SalesOrderId, string storeKey)
        {
            var storeController = _crtFactory.CreateShippingController(storeKey);
            return storeController.GetShippingTrackingId(SalesOrderId);
        }

    }
}
