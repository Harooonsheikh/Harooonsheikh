using AutoMapper;
using EdgeAXCommerceLink.Commerce.RetailProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRTAX7.Controllers
{
    public class SalesOrderController : BaseController, ISalesOrderController
    {

        #region "Public"

        public List<SalesOrder> SearchOrders(string orderNumber)
        {
            return null;
        }

        public ErpSalesOrder UploadOrder(ErpSalesOrder salesOrder)
        {
            SalesOrder objRSSalesOrder;
            int response;

            try
            {
                if (salesOrder.AffiliationLoyaltyTierLines != null)
                {
                    foreach (var affiliation in salesOrder.AffiliationLoyaltyTierLines)
                    {
                        affiliation.ChannelId = baseChannelId;
                    }
                }

                //Required to push sales order via RS because it accept JSON objects
                salesOrder = SalesOrderMappingHelper.SetNullValues(salesOrder);

                String strSalesOrder = Newtonsoft.Json.JsonConvert.SerializeObject(salesOrder);

                objRSSalesOrder = Mapper.Map<SalesOrder>(salesOrder);

                objRSSalesOrder = SalesOrderMappingHelper.MapMissingProperties(salesOrder, objRSSalesOrder);

                // Create Json String of SalesOrder
                String strRSSalesOrder = Newtonsoft.Json.JsonConvert.SerializeObject(objRSSalesOrder);

                //Store RS SalesOrder json for reference in database
                CustomLogger.LogDebugInfo("Json of RS-SalesOrder before Upload : " + strRSSalesOrder, objRSSalesOrder.ChannelReferenceId);

                // Upload Sales Order
                response = AsyncUploadSalesOrder(objRSSalesOrder).Result;

                if (response == 0)
                {
                    //Sales order has been uploaded without extensions
                }
                else if (response == 1)
                {
                    //Sales order has been uploaded with extensions
                }

                // Return the Erp SalesOrder object, no need to reverse mapping
                return salesOrder;
            }
            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);
                throw exp;
            }
        }

        public List<ErpSalesOrderStatus> GetSalesOrderStatus(DateTimeOffset fromDateOff, DateTimeOffset toDateOff)
        {
            try
            {
                // Query Result Settings
                QueryResultSettings queryResultSettings = new QueryResultSettings();
                queryResultSettings.Paging = new PagingInfo();
                queryResultSettings.Paging.Skip = 0;
                queryResultSettings.Paging.Top = (long?)Convert.ToInt64(ConfigurationHelper.GetSetting(SALESORDER.Retail_Server_Paging));

                PagedResult<SalesOrder> salesOrderReturn;
                List<ErpSalesOrderStatus> erpSalesOrderStatuses = new List<ErpSalesOrderStatus>();

                // Start without skipping
                queryResultSettings.Paging.Skip = 0;

                // AX Call
                do
                {
                    salesOrderReturn = AsyncSearchSalesOrder(fromDateOff, toDateOff, queryResultSettings).Result;

                    queryResultSettings.Paging.Skip += queryResultSettings.Paging.Top;

                    // Map each SalesOrder to EroSalesOrderStatus and add to return object
                    foreach (SalesOrder order in salesOrderReturn)
                    {
                        ErpSalesOrderStatus erpSalesOrderStatus = new ErpSalesOrderStatus();

                        erpSalesOrderStatus.ChannelRefId = order.ChannelReferenceId;
                        erpSalesOrderStatus.CustomerAcc = order.CustomerId;
                        ErpSalesStatus statusOfSalesOrder = (ErpSalesStatus)order.StatusValue;
                        erpSalesOrderStatus.Status = statusOfSalesOrder.ToString();
                        erpSalesOrderStatus.SalesId = order.SalesId;
                        erpSalesOrderStatus.Notify = false;

                        erpSalesOrderStatuses.Add(erpSalesOrderStatus);
                    }

                }
                while (salesOrderReturn.Count() == queryResultSettings.Paging.Top);

                // Return the ErpSalesOrderStatus
                return erpSalesOrderStatuses;
            }

            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);
                throw exp;
            }
        }

        public ErpValidateVATNumberResponse ValidateVATNumber(ErpValidateVATNumberRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region "Private"

        /// <summary>
        /// Asynchronous sales order upload
        /// </summary>
        /// <param name="salesOrder"></param>
        /// <returns></returns>
        private async Task<int> AsyncUploadSalesOrder(SalesOrder salesOrder)
        {
            // Create Sales Order Manager object
            var salesOrderManager = RPFactory.GetManager<ISalesOrderManager>();

            // Upload SalesOrder and return the result
            int response = await salesOrderManager.UploadSalesOrder(salesOrder); // New Method

            // return the SalesOrder object
            return response;
        }

        /// <summary>
        /// Asynchronous sales order search
        /// </summary>
        /// <param name="orderNumber">Order Number</param>
        /// <param name="queryResultSettings">Query Result Settings for asynchronous call</param>
        /// <returns></returns>
        private async Task<PagedResult<SalesOrder>> AsyncSearchSalesOrder(DateTimeOffset fromDateOff, DateTimeOffset toDateOff, QueryResultSettings queryResultSettings)
        {
            // Create sales order manager object
            var salesOrderManager = RPFactory.GetManager<ISalesOrderManager>();

            // Sales order search criteria
            SalesOrderSearchCriteria salesOrderSearchCriteria = new SalesOrderSearchCriteria();

            salesOrderSearchCriteria.StartDateTime = fromDateOff;
            salesOrderSearchCriteria.EndDateTime = toDateOff;
            salesOrderSearchCriteria.SearchTypeValue = (int) OrderSearchType.SalesOrder;
            salesOrderSearchCriteria.SalesTransactionTypeValues =
                new System.Collections.ObjectModel.ObservableCollection<int> { (int)SalesTransactionType.PendingSalesOrder };

            // Search the sales orders in SalesOrder object
            PagedResult<SalesOrder> salesOrderReturn = await salesOrderManager.SearchSalesOrders(salesOrderSearchCriteria, queryResultSettings);

            // Return the SalesOrder object
            return salesOrderReturn;
        }

        object ISalesOrderController.SearchOrders(string orderNumber)
        {
            throw new NotImplementedException();
        }

        public ERPContractSalesorderResponse GetContractSalesOrder(ContractSalesorderRequest request)
        {
            throw new NotImplementedException();
        }

        public double CalculateTimeQty(DateTime _calculateDateFrom, DateTime _validTo, int _billingPeriod, bool _isSubscription = false, bool _tmvAutoProlongation = false)
        {
            throw new NotImplementedException();
        }

        public ErpContractInvoicesResponse GetContractInvoices(ContractInvoicesRequest request)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
