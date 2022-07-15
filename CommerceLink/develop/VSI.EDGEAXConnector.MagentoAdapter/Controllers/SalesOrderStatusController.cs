using System;
using System.Collections.Generic;
using System.Linq;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
{

    /// <summary>
    /// SalesOrderStatusController class performs Sales Order Status related activities.
    /// </summary>
    public class SalesOrderStatusController : BaseController, ISalesOrderStatusController
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SalesOrderStatusController(string storeKey) : base(false, storeKey)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// UpdateOrderStatus updated sale order status.
        /// </summary>
        /// <param name="ecomOrders"></param>
        /// <returns></returns>
        public List<ErpSalesOrderStatus> UpdateOrderStatus(List<ErpSalesOrderStatus> ecomUpdates)
        {
            CustomLogger customLogger = new CustomLogger();

            customLogger.LogDebugInfo("Entering function UpdateOrderStatus");
            if (ecomUpdates != null && ecomUpdates.Count > 0)
            {
                customLogger.LogDebugInfo(string.Format("ecomOrders Count are : {0}", ecomUpdates.Count.ToString()));
                ErpOrderStatus orderData = this.ProcessOrderStatusData(ecomUpdates);
                customLogger.LogDebugInfo("Successfully exit from function ProcessOrderStatusData");


                this.CreateOrderStatusFile(orderData);
            }
            else
            {
                customLogger.LogDebugInfo(string.Format("ecomOrders Count are : {0}", ecomUpdates== null ? "ecomOrders is null" : ecomUpdates.Count.ToString()));
            }
            return ecomUpdates;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This function creates product Sales Order Status CSV files.
        /// </summary>
        /// <param name="products"></param>
        private void CreateOrderStatusFile(ErpOrderStatus orderData)
        {
            try
            {
                Boolean includeERPOrderNumberInStatus = false;
                includeERPOrderNumberInStatus = configurationHelper.GetSetting(SALESORDER.Include_ERP_Order_Number_in_Status).BoolValue();
                Boolean includeTrackingInfoInStatus = false;
                includeTrackingInfoInStatus = configurationHelper.GetSetting(SALESORDER.Include_Tracking_Info_in_Status).BoolValue();
                orderData.ordersStatus.ForEach(x => x.IncludeERPOrderNumberInStatus = includeERPOrderNumberInStatus);
                orderData.ordersStatus.ForEach(x => x.IncludeTrackingInfoInStatus = includeTrackingInfoInStatus);
                if (orderData != null)
                {
                    string fileNameOrderStatus = configurationHelper.GetSetting(SALESORDER.Status_File_Name) + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";
                    XmlTemplateHelper xmlHelper = new XmlTemplateHelper();
                    xmlHelper.GenerateXmlUsingTemplate(fileNameOrderStatus, ConfigurationHelper.GetDirectory(configurationHelper.GetSetting(SALESORDER.Status_local_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, orderData);

                    TransactionLogging obj = new TransactionLogging(currentStore.StoreKey);
                    obj.LogTransaction(SyncJobs.SalesOrderStatusSync, "Sales Order Status Sync XML generated Successfully", DateTime.UtcNow, null);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// FormatProductCSVDataExt format and arrange Products as per requirements for CSV Export.
        /// </summary>
        /// <param name="ecomOrders"></param>
        /// <param name="csvProducts"></param>
        /// <param name="csvRelatedProducts"></param>
        /// <param name="csvProductImages"></param>
        private ErpOrderStatus ProcessOrderStatusData(List<ErpSalesOrderStatus> ecomOrders)
        {
            CustomLogger customLogger = new CustomLogger();
            customLogger.LogDebugInfo("Entering function ProcessOrderStatusData");
            ErpOrderStatus orderData = this.IntializeOrderStatusData();

            ErpSalesOrderCustomStatus orderItem;

            foreach (ErpSalesOrderStatus ord in ecomOrders)
            {
                orderItem = this.ProcessOrderStatus(ord);
                orderItem.orderNo = ord.ChannelRefId;
                orderItem.salesId = ord.SalesId;
                orderItem.TrackingNumber = ResolveTrackingNumber(ord.Shipments);
                orderData.ordersStatus.Add(orderItem);

                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                integrationManager.CreateIntegrationKey(Entities.SalesOrderStatus, ord.Status, ord.ChannelRefId, orderItem.status.ToString());            
            }

            return orderData;
        }

        private string ResolveTrackingNumber(IList<ErpShipment> shipmentItems)
        {
            string returnValue = "";

            if (shipmentItems == null) return string.Empty;

            if (shipmentItems[0].TransactionId.ToString().Contains("ERROR:"))
            {
                returnValue = shipmentItems[0].TransactionId.ToString();
            }
            else
            {
                var trackingNumbers = shipmentItems.GroupBy(gr => gr.TrackingNumber).ToList();

                foreach (var val in trackingNumbers)
                {
                    if (returnValue == "")
                        returnValue = val.Key.ToString();
                    else
                        returnValue = returnValue + "," + val.Key.ToString();
                }
            }

            return returnValue;
        }

        private ErpSalesOrderCustomStatus ProcessOrderStatus(ErpSalesOrderStatus orderStatus)
        {
            ErpSalesOrderCustomStatus status = new ErpSalesOrderCustomStatus();

            if (orderStatus.Status.Equals("Delivered", StringComparison.CurrentCultureIgnoreCase))
            {
                status.status = simpleTypeOrderOrderStatus.OPEN;
                status.shippingStatus = simpleTypeOrderShippingStatus.SHIPPED;
            }
            else if (orderStatus.Status.Equals("Invoiced", StringComparison.CurrentCultureIgnoreCase))
            {
                status.status = simpleTypeOrderOrderStatus.COMPLETED;
                status.shippingStatus = simpleTypeOrderShippingStatus.SHIPPED;
            }
            else if (orderStatus.Status.Equals("Canceled", StringComparison.CurrentCultureIgnoreCase))
            {
                status.status = simpleTypeOrderOrderStatus.CANCELLED;
                status.shippingStatus = simpleTypeOrderShippingStatus.NOT_SHIPPED;
            }
            return status;
        }

        private ErpOrderStatus IntializeOrderStatusData()
        {
            ErpOrderStatus orderData = new ErpOrderStatus();

            return orderData;
        }
        #endregion
    }
}
