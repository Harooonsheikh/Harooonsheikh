using AutoMapper;
using EdgeAXCommerceLink.Commerce.RetailProxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRTAX7.Controllers
{
    public class ShippingController : BaseController, IShippingController
    {

        #region "Public"

        /// <summary>
        /// Get shipping charges against each product
        /// </summary>
        /// <param name="shippingMethod"></param>
        /// <param name="productIdWithQuantity"></param>
        /// <param name="zipCode"></param>
        /// <param name="countryCode"></param>
        /// <param name="address1"></param>
        /// <param name="address2"></param>
        /// <param name="legalCompany"></param>
        /// <returns></returns>
        public IEnumerable<ErpCommerceProperty> GetShippingCharges(string shippingMethod, IEnumerable<ErpCommerceProperty> productIdWithQuantity, string zipCode, string countryCode, string address1, string address2)
        {
            string legalCompany = "";

            legalCompany = ConfigurationHelper.GetSetting(APPLICATION.ERP_Legal_Company);

            try
            {
                // Create Sales Order Manager object
                var salesOrderManager = RPFactory.GetManager<ISalesOrderManager>();

                // Query Search Result
                QueryResultSettings queryResultSettings = new QueryResultSettings();
                queryResultSettings.Paging = new PagingInfo();
                queryResultSettings.Paging.Skip = Paging_0_1000.Skip;
                queryResultSettings.Paging.Top = Paging_0_1000.Top;

                // Map
                IEnumerable<CommerceProperty> ProductIdWithQuantity;
                ProductIdWithQuantity = Mapper.Map<IEnumerable<ErpCommerceProperty>, IEnumerable<CommerceProperty>>(productIdWithQuantity);

                // Get Shipping Charges
                List<CommerceProperty> salesLinesShippingRatesReturn = new List<CommerceProperty>();

                PagedResult<CommerceProperty> salesLinesShippingCharges;
                salesLinesShippingCharges = AsyncGetShippingCharges(shippingMethod, ProductIdWithQuantity, zipCode, countryCode, address1, address2, legalCompany, queryResultSettings).Result;
                foreach (CommerceProperty commerceProperty in salesLinesShippingCharges)
                {
                    salesLinesShippingRatesReturn.Add(commerceProperty);
                }

                // Reverse Map
                IEnumerable<ErpCommerceProperty> erpSalesLineShippingRatesReturn;
                erpSalesLineShippingRatesReturn = Mapper.Map<IEnumerable<CommerceProperty>, IEnumerable<ErpCommerceProperty>>(salesLinesShippingRatesReturn);

                // Return
                return erpSalesLineShippingRatesReturn;

            }
            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);
                throw exp;
            }
        }

        /// <summary>
        /// Get tracking data against sales order
        /// </summary>
        /// <param name="SalesOrderId"></param>
        /// <returns></returns>
        public IEnumerable<ErpCommerceProperty> GetShippingTrackingId(string SalesOrderId)
        {
            try
            {
                // Create Sales Order Manager object
                var salesOrderManager = RPFactory.GetManager<ISalesOrderManager>();

                // Query Search Result
                QueryResultSettings queryResultSettings = new QueryResultSettings();
                queryResultSettings.Paging = new PagingInfo();
                queryResultSettings.Paging.Skip = Paging_0_1000.Skip;
                queryResultSettings.Paging.Top = Paging_0_1000.Top;

                PagedResult<CommerceProperty> salesOrderTrackingIds = new PagedResult<CommerceProperty>();

                salesOrderTrackingIds = AsyncGetShippingTrackingId(SalesOrderId, queryResultSettings).Result;

                IEnumerable<ErpCommerceProperty> erpSalesOrderTrackingIdsReturn;
                erpSalesOrderTrackingIdsReturn = Mapper.Map<IEnumerable<CommerceProperty>, IEnumerable<ErpCommerceProperty>>(salesOrderTrackingIds);

                // Return
                return erpSalesOrderTrackingIdsReturn;

            }

            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);
                throw exp;
            }
        }

        #endregion

        #region "Private"

        /// <summary>
        /// Async call to get shipping charges against each product
        /// </summary>
        /// <param name="shippingMethod"></param>
        /// <param name="productIdWithQuantity"></param>
        /// <param name="zipCode"></param>
        /// <param name="countryCode"></param>
        /// <param name="address1"></param>
        /// <param name="address2"></param>
        /// <param name="queryResultSettings"></param>
        /// <returns></returns>
        private async Task<PagedResult<CommerceProperty>> AsyncGetShippingCharges(string ShippingMethod, IEnumerable<CommerceProperty> ProductIdWithQuantity, string ZipCode, string CountryCode, string Address1, string Address2, string legalCompany, QueryResultSettings queryResultSettings)
        {
            var salesOrderManager = RPFactory.GetManager<ISalesOrderManager>();
            PagedResult<CommerceProperty> salesLinesShippingRates = new PagedResult<CommerceProperty>();
            salesLinesShippingRates = Task.Run(async () => await salesOrderManager.GetShippingCharges(ShippingMethod, ProductIdWithQuantity, ZipCode, CountryCode, Address1, Address2, legalCompany, queryResultSettings)).Result;
            return salesLinesShippingRates;
        }

        /// <summary>
        /// Async call to get tracking data against sales order
        /// </summary>
        /// <param name="SalesOrderId"></param>
        /// <param name="queryResultSettings"></param>
        /// <returns></returns>
        private async Task<PagedResult<CommerceProperty>> AsyncGetShippingTrackingId(string SalesOrderId, QueryResultSettings queryResultSettings)
        {
            var salesOrderManager = RPFactory.GetManager<ISalesOrderManager>();
            PagedResult<CommerceProperty> salesOrderTrackingIds;
            salesOrderTrackingIds = Task.Run(async () => await salesOrderManager.GetSalesOrderTrackingId(SalesOrderId, queryResultSettings)).Result;
            return salesOrderTrackingIds;
        }

        #endregion

    }
}
