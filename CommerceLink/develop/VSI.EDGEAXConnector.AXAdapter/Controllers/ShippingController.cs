//using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class ShippingController : BaseController, IShippingController
    {
        public ShippingController(string storeKey) : base(storeKey)
        {
        }

        public bool CreateShipping(ErpSalesLineShippingRate salesLineShippingRate)
        {
            throw new NotImplementedException();
        }

        public bool CreateShippingController()
        {
            return false;
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
        public IEnumerable<ErpCommerceProperty> GetShippingCharges(string ShippingMethod, IEnumerable<ErpCommerceProperty> ProductIdWithQty, string ZipCode, string CountryCode, string Address1, string Address2)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "ShippingMethod: " + ShippingMethod + ",ProductIdWithQty: " + JsonConvert.SerializeObject(ProductIdWithQty) + ",ZipCode: " + ZipCode + ",CountryCode: " + CountryCode
                + ",Address1: " + Address1 + ",Address2: " + Address2);

            var shippingCRTManager = new ShippingCRTManager();
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

            foreach (ErpCommerceProperty product in ProductIdWithQty)
            {
                ErpCommerceProperty convertedProduct = new ErpCommerceProperty();

                string erpKey = "";
                IntegrationKey integrationKey;

                // Extract ErpKey from database
                integrationKey = integrationManager.GetErpKey(Enums.Entities.Product, product.Key);
                erpKey = (integrationKey == null) ? product.Key : integrationKey.Description; // Check if ErpKey is null

                product.Key = erpKey;
            }

            IEnumerable<ErpCommerceProperty> returnedProductIdWithQuantity = new List<ErpCommerceProperty>();

            returnedProductIdWithQuantity = shippingCRTManager.GetShippingCharges(ShippingMethod, ProductIdWithQty, ZipCode, CountryCode, Address1, Address2, currentStore.StoreKey);

            foreach (ErpCommerceProperty product in returnedProductIdWithQuantity)
            {
                if (!product.Key.ToString().Contains("ERROR"))
                {
                    //IntegrationKey integrationKey;
                    //integrationKey = IntegrationManager.GetKeyByDescription(Enums.Entities.Product, product.Key);
                    //product.Key = integrationKey.ComKey;

                    ///// TODO (BEGIN): Discuss with Noman and Ms. Anam
                    ///// Database returns Master Product so letter "M" is removed from Key
                    //product.Key = product.Key.Replace("M", "");
                    ///// TODO (END)
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(returnedProductIdWithQuantity));

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return returnedProductIdWithQuantity;
        }

        /// <summary>
        /// Get tracking data against sales order
        /// </summary>
        /// <param name="SalesOrderId">Sales Order ID</param>
        /// <returns></returns>
        public IEnumerable<ErpCommerceProperty> GetShippingTrackingId(string SalesOrderId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var shippingCRTManager = new ShippingCRTManager();
            return shippingCRTManager.GetShippingTrackingId(SalesOrderId, currentStore.StoreKey);
        }

    }
}
