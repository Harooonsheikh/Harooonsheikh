using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums.TMV;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.CommerceLink.ThirdPartyAdapter.Controllers
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
        public SalesOrderStatusController(string storeKey) : base(storeKey)
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
            try
            {
                CustomLogger.LogDebugInfo("Entering function UpdateOrderStatus", currentStore.StoreId, currentStore.CreatedBy);
                if (ecomUpdates != null && ecomUpdates.Count > 0)
                {
                    CustomLogger.LogDebugInfo(string.Format("ecomOrders Count are : {0}", ecomUpdates.Count.ToString()), currentStore.StoreId, currentStore.CreatedBy);

                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                        DataDirectionType.CLRequestToThirdParty, // Data direction
                        Newtonsoft.Json.JsonConvert.SerializeObject(ecomUpdates), // Data packed
                        DateTime.UtcNow, // Created on
                        currentStore.StoreId,
                        ApplicationConstant.UserName, // Created by
                        "List of orders that are received for syncing with Ingram", // Description
                        string.Empty, // eCom Transaction Id
                        string.Empty, // Request Initialed IP
                        string.Empty, // Output packed
                        DateTime.UtcNow, // Output sent at
                        string.Empty, // Identifier Key
                        string.Empty, // Identifier Value
                        1, // Success
                        0 // TotalProcessingDuration
                        );

                    ErpOrderStatus orderData = ProcessOrderStatusData(ecomUpdates);

                    CustomLogger.LogDebugInfo("Successfully exit from function ProcessOrderStatusData", currentStore.StoreId, currentStore.CreatedBy);

                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                        DataDirectionType.CLRequestToThirdParty, // Data direction
                        Newtonsoft.Json.JsonConvert.SerializeObject(ecomUpdates), // Data packed
                        DateTime.UtcNow, // Created on
                        currentStore.StoreId,
                        ApplicationConstant.UserName, // Created by
                        "Orders synced sucessfully with Ingram", // Description
                        string.Empty, // eCom Transaction Id
                        string.Empty, // Request Initialed IP
                        string.Empty, // Output packed
                        DateTime.UtcNow, // Output sent at
                        string.Empty, // Identifier Key
                        string.Empty, // Identifier Value
                        1, // Success
                        0 // TotalProcessingDuration
                        );
                }
                else
                {
                    CustomLogger.LogDebugInfo(string.Format("ecomOrders Count are : {0}", ecomUpdates == null ? "ecomOrders is null" : ecomUpdates.Count.ToString()), currentStore.StoreId, currentStore.CreatedBy);

                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                        DataDirectionType.CLRequestToThirdParty, // Data direction
                        Newtonsoft.Json.JsonConvert.SerializeObject(string.Format("ecomOrders Count are : {0}", ecomUpdates == null ? "ecomOrders is null" : ecomUpdates.Count.ToString())), // Data packed
                        DateTime.UtcNow, // Created on
                        currentStore.StoreId,
                        ApplicationConstant.UserName, // Created by
                        "No orders received for syncing with Ingram", // Description
                        string.Empty, // eCom Transaction Id
                        string.Empty, // Request Initialed IP
                        string.Empty, // Output packed
                        DateTime.UtcNow, // Output sent at
                        string.Empty, // Identifier Key
                        string.Empty, // Identifier Value
                        1, // Success
                        0 // TotalProcessingDuration
                        );
                }
                return ecomUpdates;
            }
            catch (Exception)
            {

                throw;
            }
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
                    string fileNameOrderStatus = configurationHelper.GetSetting(SALESORDER.Status_File_Name) + currentStore.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + ".xml";
                    XmlTemplateHelper xmlHelper = new XmlTemplateHelper(currentStore);
                    xmlHelper.GenerateXmlUsingTemplate(fileNameOrderStatus, this.configurationHelper.GetDirectory(configurationHelper.GetSetting(SALESORDER.Status_local_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, orderData);

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
            CustomLogger.LogDebugInfo("Entering function ProcessOrderStatusData", currentStore.StoreId, currentStore.CreatedBy);

            ErpOrderStatus orderData = new ErpOrderStatus();

            ErpSalesOrderCustomStatus orderItem;
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            foreach (ErpSalesOrderStatus ord in ecomOrders)
            {
                try
                {
                    orderItem = ProcessOrderStatus(ord);
                    orderItem.orderNo = ord.ChannelRefId;
                    orderItem.salesId = ord.SalesId;
                    orderItem.TrackingNumber = ResolveTrackingNumber(ord.Shipments);
                    orderData.ordersStatus.Add(orderItem);

                    string methodName = MethodBase.GetCurrentMethod().Name;
                    var thirdPartyAPI = new ThirdPartyAPI.ThirdPartyApi(thirdPartyApiUrl, thirdPartyApiKey, currentStore);
                    var messageDAL = new ThirdPartyMessageDAL(currentStore.StoreKey);

                    ThirdPartyMessage thirdPartyMessage = messageDAL.GetThirdPartyMessage(ord.ChannelRefId, ord.SalesId);

                    if (string.IsNullOrEmpty(ord.ChannelRefId))
                    {
                        ord.ChannelRefId = thirdPartyMessage.ThirdPartyId;
                    }

                    if (thirdPartyMessage.OrderType.Equals(ApplicationConstant.IngramOrderTypePurchase, StringComparison.OrdinalIgnoreCase)
                        || thirdPartyMessage.OrderType.Equals(ApplicationConstant.IngramOrderTypeChange, StringComparison.OrdinalIgnoreCase))
                    {
                        CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} which is: {1}", ord.ChannelRefId, ord.Status), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                            DataDirectionType.CLRequestToThirdParty, // Data direction
                            string.Empty, // Data packed
                            DateTime.UtcNow, // Created on
                            currentStore.StoreId,
                            ApplicationConstant.UserName, // Created by
                            string.Format("Sales order status update for {0} which is: {1}", ord.ChannelRefId, ord.Status), // Description
                            string.Empty, // eCom Transaction Id
                            string.Empty, // Request Initialed IP
                            string.Empty, // Output packed
                            DateTime.UtcNow, // Output sent at
                            ord.ChannelRefId, // Identifier Key
                            ord.ChannelRefId, // Identifier Value
                            1, // Success
                            0 // TotalProcessingDuration
                            );

                        if (ord.Status == ApplicationConstant.IngramD365OrderStatusInvoiced)
                        {
                            var ingramSalesOrderResponse = thirdPartyAPI.UpdateSaleOrderStatus(ord.ChannelRefId, ApplicationConstant.IngramOrderStatusApprove, methodName, thirdPartyMessage.Description).Result;

                            if (ingramSalesOrderResponse.status == ApplicationConstant.IngramResponseOrderStatusApproved)
                            {
                                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.SynchedWithThirdParty, ApplicationConstant.UserName);
                                messageDAL.UpdateThirdPartyStatus(ord.ChannelRefId, ApplicationConstant.IngramResponseOrderStatusApproved, ApplicationConstant.UserName);

                                CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} sucessfully set to approved", ord.ChannelRefId), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                    DataDirectionType.CLRequestToThirdParty, // Data direction
                                    string.Empty, // Data packed
                                    DateTime.UtcNow, // Created on
                                    currentStore.StoreId,
                                    ApplicationConstant.UserName, // Created by
                                    string.Format("Sales order status update for {0} sucessfully set to approved", ord.ChannelRefId), // Description
                                    string.Empty, // eCom Transaction Id
                                    string.Empty, // Request Initialed IP
                                    string.Empty, // Output packed
                                    DateTime.UtcNow, // Output sent at
                                    ord.ChannelRefId, // Identifier Key
                                    ord.ChannelRefId, // Identifier Value
                                    1, // is Success
                                    0 // TotalProcessingDuration
                                    );

                                integrationManager.CreateIntegrationKey(Entities.SalesOrderStatus, ord.Status, ord.ChannelRefId, orderItem.status.ToString());
                            }
                            else
                            {
                                string errors = string.Join(",", ingramSalesOrderResponse.errors.Select(e => e).ToList());
                                Exception ex = new Exception(string.Format("Ingram order {0} sync failed with error-code: {1} and error message(s): {2} ", ord.ChannelRefId, ingramSalesOrderResponse.error_code, errors));
                                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy, "Ingram Sales Order Status Sync");
                                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.SynchedWithThirdPartyFailure, ApplicationConstant.UserName);

                                CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                    DataDirectionType.CLRequestToThirdParty, // Data direction
                                    string.Empty, // Data packed
                                    DateTime.UtcNow, // Created on
                                    currentStore.StoreId,
                                    ApplicationConstant.UserName, // Created by
                                    string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), // Description
                                    string.Empty, // eCom Transaction Id
                                    string.Empty, // Request Initialed IP
                                    string.Empty, // Output packed
                                    DateTime.UtcNow, // Output sent at
                                    ord.ChannelRefId, // Identifier Key
                                    ord.ChannelRefId, // Identifier Value
                                    0, // Success
                                    0 // TotalProcessingDuration
                                    );
                            }
                        }
                        else if (ord.Status == ApplicationConstant.IngramD365OrderStatusCanceled)
                        {
                            var ingramSalesOrderResponse = thirdPartyAPI.UpdateSaleOrderStatus(ord.ChannelRefId, ApplicationConstant.IngramOrderStatusFail, methodName, thirdPartyMessage.Description).Result;

                            if (ingramSalesOrderResponse.status == ApplicationConstant.IngramResponseOrderStatusFailed)
                            {
                                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.SynchedWithThirdParty, ApplicationConstant.UserName);
                                messageDAL.UpdateThirdPartyStatus(ord.ChannelRefId, ApplicationConstant.IngramResponseOrderStatusFailed, ApplicationConstant.UserName);

                                CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} sucessfully set to failed", ord.ChannelRefId), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                    DataDirectionType.CLRequestToThirdParty, // Data direction
                                    string.Empty, // Data packed
                                    DateTime.UtcNow, // Created on
                                    currentStore.StoreId,
                                    ApplicationConstant.UserName, // Created by
                                    string.Format("Sales order status update for {0} sucessfully set to failed", ord.ChannelRefId), // Description
                                    string.Empty, // eCom Transaction Id
                                    string.Empty, // Request Initialed IP
                                    string.Empty, // Output packed
                                    DateTime.UtcNow, // Output sent at
                                    ord.ChannelRefId, // Identifier Key
                                    ord.ChannelRefId, // Identifier Value
                                    1, // Success
                                    0 // TotalProcessingDuration
                                    );

                                integrationManager.CreateIntegrationKey(Entities.SalesOrderStatus, ord.Status, ord.ChannelRefId, orderItem.status.ToString());
                            }
                            else
                            {
                                string errors = string.Join(",", ingramSalesOrderResponse.errors.Select(e => e).ToList());
                                Exception ex = new Exception(string.Format("Ingram order {0} sync failed with error-code: {1} and error message(s): {2} ", ord.ChannelRefId, ingramSalesOrderResponse.error_code, errors));
                                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy, "Ingram Sales Order Status Sync");
                                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.SynchedWithThirdPartyFailure, ApplicationConstant.UserName);

                                CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                    DataDirectionType.CLRequestToThirdParty, // Data direction
                                    string.Empty, // Data packed
                                    DateTime.UtcNow, // Created on
                                    currentStore.StoreId,
                                    ApplicationConstant.UserName, // Created by
                                    string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), // Description
                                    string.Empty, // eCom Transaction Id
                                    string.Empty, // Request Initialed IP
                                    string.Empty, // Output packed
                                    DateTime.UtcNow, // Output sent at
                                    ord.ChannelRefId, // Identifier Key
                                    ord.ChannelRefId, // Identifier Value
                                    0, // Success
                                    0 // TotalProcessingDuration
                                    );
                            }
                        }
                        else if (ord.Status == ApplicationConstant.IngramMissingParameterStatus)
                        {
                            SetMissingParamterErrorAndStatus(ord, methodName, thirdPartyAPI, messageDAL, thirdPartyMessage);
                        }
                    }
                    else if (thirdPartyMessage.OrderType.Equals(ApplicationConstant.IngramOrderTypeCancel, StringComparison.OrdinalIgnoreCase))
                    {
                        CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} which is: {1}", ord.ChannelRefId, ord.Status), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                            DataDirectionType.CLRequestToThirdParty, // Data direction
                            string.Empty, // Data packed
                            DateTime.UtcNow, // Created on
                            currentStore.StoreId,
                            ApplicationConstant.UserName, // Created by
                            string.Format("Sales order status update for {0} which is: {1}", ord.ChannelRefId, ord.Status), // Description
                            string.Empty, // eCom Transaction Id
                            string.Empty, // Request Initialed IP
                            string.Empty, // Output packed
                            DateTime.UtcNow, // Output sent at
                            ord.ChannelRefId, // Identifier Key
                            ord.ChannelRefId, // Identifier Value
                            1, // Success
                            0 // TotalProcessingDuration
                            );

                        if (ord.Status.Equals("Canceled", StringComparison.OrdinalIgnoreCase) ||
                            ord.Status.Equals("Terminated", StringComparison.OrdinalIgnoreCase))
                        {
                            var ingramSalesOrderResponse = thirdPartyAPI.UpdateSaleOrderStatus(ord.ChannelRefId, "approve", methodName, thirdPartyMessage.Description).Result;

                            if (ingramSalesOrderResponse.status == ApplicationConstant.IngramResponseOrderStatusApproved)
                            {
                                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.IngramCancelRequest_StatusUpdatedInThirdParty, ApplicationConstant.UserName);
                                messageDAL.UpdateThirdPartyStatus(ord.ChannelRefId, ApplicationConstant.IngramResponseOrderStatusApproved, ApplicationConstant.UserName);

                                CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} sucessfully set to rejected", ord.ChannelRefId), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                    DataDirectionType.CLRequestToThirdParty, // Data direction
                                    string.Empty, // Data packed
                                    DateTime.UtcNow, // Created on
                                    currentStore.StoreId,
                                    ApplicationConstant.UserName, // Created by
                                    string.Format("Sales order status update for {0} sucessfully set to rejected", ord.ChannelRefId), // Description
                                    string.Empty, // eCom Transaction Id
                                    string.Empty, // Request Initialed IP
                                    string.Empty, // Output packed
                                    DateTime.UtcNow, // Output sent at
                                    ord.ChannelRefId, // Identifier Key
                                    ord.ChannelRefId, // Identifier Value
                                    1, //
                                    0 // TotalProcessingDuration
                                    );

                                integrationManager.CreateIntegrationKey(Entities.SalesOrderStatus, ord.Status, ord.ChannelRefId, orderItem.status.ToString());
                            }
                            else
                            {
                                string errors = string.Join(",", ingramSalesOrderResponse.errors.Select(e => e).ToList());
                                Exception ex = new Exception(string.Format("Ingram order {0} sync failed with error-code: {1} and error message(s): {2} ", ord.ChannelRefId, ingramSalesOrderResponse.error_code, errors));
                                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy, "Ingram Sales Order Status Sync");
                                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.IngramCancelRequest_StatusUpdateInThirdPartyFailure, ApplicationConstant.UserName);

                                CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                    DataDirectionType.CLRequestToThirdParty, // Data direction
                                    string.Empty, // Data packed
                                    DateTime.UtcNow, // Created on
                                    currentStore.StoreId,
                                    ApplicationConstant.UserName, // Created by
                                    string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), // Description
                                    string.Empty, // eCom Transaction Id
                                    string.Empty, // Request Initialed IP
                                    string.Empty, // Output packed
                                    DateTime.UtcNow, // Output sent at
                                    ord.ChannelRefId, // Identifier Key
                                    ord.ChannelRefId, // Identifier Value
                                    0, //
                                    0 // TotalProcessingDuration
                                    );
                            }
                        }
                        else if (ord.Status == ApplicationConstant.IngramD365OrderStatusInvoiced)
                        {
                            // Order in ERP was not canceled or terminated because its possible expiration date has expired so cancel the PR. Asset will be set to Active
                            var ingramSalesOrderResponse = thirdPartyAPI.UpdateSaleOrderStatus(ord.ChannelRefId, ApplicationConstant.IngramOrderStatusFail, methodName, thirdPartyMessage.Description).Result;

                            if (ingramSalesOrderResponse.status == ApplicationConstant.IngramResponseOrderStatusFailed)
                            {
                                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.IngramCancelRequest_StatusUpdatedInThirdPartyOrderTerminationDateExpired, ApplicationConstant.UserName);
                                messageDAL.UpdateThirdPartyStatus(ord.ChannelRefId, ApplicationConstant.IngramResponseOrderStatusFailed, ApplicationConstant.UserName);

                                CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} sucessfully set to failed", ord.ChannelRefId), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                    DataDirectionType.CLRequestToThirdParty, // Data direction
                                    string.Empty, // Data packed
                                    DateTime.UtcNow, // Created on
                                    currentStore.StoreId,
                                    ApplicationConstant.UserName, // Created by
                                    string.Format("Sales order status update for {0} sucessfully set to failed as posible termination period has expired", ord.ChannelRefId), // Description
                                    string.Empty, // eCom Transaction Id
                                    string.Empty, // Request Initialed IP
                                    string.Empty, // Output packed
                                    DateTime.UtcNow, // Output sent at
                                    ord.ChannelRefId, // Identifier Key
                                    ord.ChannelRefId, // Identifier Value
                                    1, //
                                    0 // TotalProcessingDuration
                                    );

                                integrationManager.CreateIntegrationKey(Entities.SalesOrderStatus, ord.Status, ord.ChannelRefId, orderItem.status.ToString());
                            }
                            else
                            {
                                string errors = string.Join(",", ingramSalesOrderResponse.errors.Select(e => e).ToList());
                                Exception ex = new Exception(string.Format("Ingram order {0} sync failed with error-code: {1} and error message(s): {2} ", ord.ChannelRefId, ingramSalesOrderResponse.error_code, errors));
                                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy, "Ingram Sales Order Status Sync");
                                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.IngramCancelRequest_StatusUpdateInThirdPartyFailure, ApplicationConstant.UserName);

                                CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                    DataDirectionType.CLRequestToThirdParty, // Data direction
                                    string.Empty, // Data packed
                                    DateTime.UtcNow, // Created on
                                    currentStore.StoreId,
                                    ApplicationConstant.UserName, // Created by
                                    string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), // Description
                                    string.Empty, // eCom Transaction Id
                                    string.Empty, // Request Initialed IP
                                    string.Empty, // Output packed
                                    DateTime.UtcNow, // Output sent at
                                    ord.ChannelRefId, // Identifier Key
                                    ord.ChannelRefId, // Identifier Value
                                    0, // Success
                                    0 // TotalProcessingDuration
                                    );
                            }
                        }
                        else
                        {
                            messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.IngramCancelRequest_StatusForThirdPartyNotFound, ApplicationConstant.UserName);
                            messageDAL.UpdateThirdPartyStatus(ord.ChannelRefId, ApplicationConstant.IngramResponseOrderStatusApproved, ApplicationConstant.UserName);

                            CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                DataDirectionType.CLRequestToThirdParty, // Data direction
                                string.Empty, // Data packed
                                DateTime.UtcNow, // Created on
                                currentStore.StoreId,
                                ApplicationConstant.UserName, // Created by
                                string.Format("Sales order status not update for {0}", ord.ChannelRefId), // Description
                                string.Empty, // eCom Transaction Id
                                string.Empty, // Request Initialed IP
                                string.Empty, // Output packed
                                DateTime.UtcNow, // Output sent at
                                ord.ChannelRefId, // Identifier Key
                                ord.ChannelRefId, // Identifier Value
                                1, // Success
                                0 // TotalProcessingDuration
                                );
                        }
                    }
                    else if (thirdPartyMessage.OrderType.Equals(ApplicationConstant.IngramOrderTypeTransfer, StringComparison.OrdinalIgnoreCase))
                    {
                        if (thirdPartyMessage.TransactionStatus == (int)TransactionStatus.TransferIngramRequest_OrderTransfered)
                        {
                            var ingramSalesOrderResponse = thirdPartyAPI.UpdateSaleOrderStatus(ord.ChannelRefId, ApplicationConstant.IngramOrderStatusApprove, methodName, thirdPartyMessage.Description).Result;
                            if (ingramSalesOrderResponse.status == ApplicationConstant.IngramResponseOrderStatusApproved)
                            {
                                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.TransferIngramRequest_OrderTransferedSynchInThirdParty, ApplicationConstant.UserName);
                                messageDAL.UpdateThirdPartyStatus(ord.ChannelRefId, ApplicationConstant.IngramResponseOrderStatusApproved, ApplicationConstant.UserName);

                                CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} sucessfully set to rejected", ord.ChannelRefId), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                    DataDirectionType.CLRequestToThirdParty, // Data direction
                                    string.Empty, // Data packed
                                    DateTime.UtcNow, // Created on
                                    currentStore.StoreId,
                                    ApplicationConstant.UserName, // Created by
                                    string.Format("Sales order status update for {0} sucessfully set to rejected", ord.ChannelRefId), // Description
                                    string.Empty, // eCom Transaction Id
                                    string.Empty, // Request Initialed IP
                                    string.Empty, // Output packed
                                    DateTime.UtcNow, // Output sent at
                                    ord.ChannelRefId, // Identifier Key
                                    ord.ChannelRefId, // Identifier Value
                                    1, // Success
                                    0 // TotalProcessingDuration
                                    );

                                integrationManager.CreateIntegrationKey(Entities.SalesOrderStatus, ord.Status, ord.ChannelRefId, orderItem.status.ToString());
                            }
                            else
                            {
                                string errors = string.Join(",", ingramSalesOrderResponse.errors.Select(e => e).ToList());
                                Exception ex = new Exception(string.Format("Ingram order {0} sync failed with error-code: {1} and error message(s): {2} ", ord.ChannelRefId, ingramSalesOrderResponse.error_code, errors));
                                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy, "Ingram Sales Order Status Sync");
                                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.TransferIngramRequest_OrderTransferedSynchInThirdPartyFailed, ApplicationConstant.UserName);

                                CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                    DataDirectionType.CLRequestToThirdParty, // Data direction
                                    string.Empty, // Data packed
                                    DateTime.UtcNow, // Created on
                                    currentStore.StoreId,
                                    ApplicationConstant.UserName, // Created by
                                    string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), // Description
                                    string.Empty, // eCom Transaction Id
                                    string.Empty, // Request Initialed IP
                                    errors, // Output packed
                                    DateTime.UtcNow, // Output sent at
                                    ord.ChannelRefId, // Identifier Key
                                    ord.ChannelRefId, // Identifier Value
                                    0, // Success
                                    0 // TotalProcessingDuration
                                    );
                            }
                        }
                        else if (thirdPartyMessage.TransactionStatus == (int)TransactionStatus.TransferIngramRequest_ValidationFailed)
                        {
                            var ingramSalesOrderResponse = thirdPartyAPI.UpdateSaleOrderStatus(ord.ChannelRefId, ApplicationConstant.IngramOrderStatusFail, methodName, thirdPartyMessage.Description).Result;

                            if (ingramSalesOrderResponse.status == ApplicationConstant.IngramResponseOrderStatusFailed)
                            {
                                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.TransferIngramRequest_OrderTransferedSynchInThirdParty, ApplicationConstant.UserName);
                                messageDAL.UpdateThirdPartyStatus(ord.ChannelRefId, ApplicationConstant.IngramResponseOrderStatusFailed, ApplicationConstant.UserName);

                                CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} sucessfully set to failed", ord.ChannelRefId), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                    DataDirectionType.CLRequestToThirdParty, // Data direction
                                    string.Empty, // Data packed
                                    DateTime.UtcNow, // Created on
                                    currentStore.StoreId,
                                    ApplicationConstant.UserName, // Created by
                                    string.Format("Sales order status update for {0} sucessfully set to failed as posible incorrect validation", ord.ChannelRefId), // Description
                                    string.Empty, // eCom Transaction Id
                                    string.Empty, // Request Initialed IP
                                    string.Empty, // Output packed
                                    DateTime.UtcNow, // Output sent at
                                    ord.ChannelRefId, // Identifier Key
                                    ord.ChannelRefId, // Identifier Value
                                    1, // Success
                                    0 // TotalProcessingDuration
                                    );

                                integrationManager.CreateIntegrationKey(Entities.SalesOrderStatus, ApplicationConstant.IngramResponseOrderStatusFailed, ord.ChannelRefId, orderItem.status.ToString());
                            }
                            else
                            {
                                string errors = string.Join(",", ingramSalesOrderResponse.errors.Select(e => e).ToList());
                                Exception ex = new Exception(string.Format("Ingram order {0} sync failed with error-code: {1} and error message(s): {2} ", ord.ChannelRefId, ingramSalesOrderResponse.error_code, errors));
                                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy, "Ingram Sales Order Status Sync");
                                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.TransferIngramRequest_OrderTransferedSynchInThirdPartyFailed, ApplicationConstant.UserName);

                                CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                    DataDirectionType.CLRequestToThirdParty, // Data direction
                                    string.Empty, // Data packed
                                    DateTime.UtcNow, // Created on
                                    currentStore.StoreId,
                                    ApplicationConstant.UserName, // Created by
                                    string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), // Description
                                    string.Empty, // eCom Transaction Id
                                    string.Empty, // Request Initialed IP
                                    string.Empty, // Output packed
                                    DateTime.UtcNow, // Output sent at
                                    ord.ChannelRefId, // Identifier Key
                                    ord.ChannelRefId, // Identifier Value
                                    0, // Success
                                    0 // TotalProcessingDuration
                                    );
                            }
                        }
                        else if (thirdPartyMessage.TransactionStatus == (int)TransactionStatus.MissingParameter_EndCustomerAdminEmail)
                        {
                            SetMissingParamterErrorAndStatus(ord, methodName, thirdPartyAPI, messageDAL, thirdPartyMessage);
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                        DataDirectionType.CLRequestToThirdParty, // Data direction
                        string.Empty, // Data packed
                        DateTime.UtcNow, // Created on
                        currentStore.StoreId,
                        ApplicationConstant.UserName, // Created by
                        string.Format("Order {0} status updated failed with exception", ord.ChannelRefId, ex.Message), // Description
                        string.Empty, // eCom Transaction Id
                        string.Empty, // Request Initialed IP
                        Newtonsoft.Json.JsonConvert.SerializeObject(ex), // Output packed
                        DateTime.UtcNow, // Output sent at
                        ord.ChannelRefId, // Identifier Key
                        ord.ChannelRefId, // Identifier Value
                        0, // Success
                        0 // TotalProcessingDuration
                        );

                    // throw;
                    continue;
                }
            }
            return orderData;
        }

        private void SetMissingParamterErrorAndStatus(ErpSalesOrderStatus ord, string methodName, ThirdPartyAPI.ThirdPartyApi thirdPartyAPI, ThirdPartyMessageDAL messageDAL, ThirdPartyMessage thirdPartyMessage)
        {
            try
            {
                var salesOrder = thirdPartyAPI.GetSalesOrderById(ord.ChannelRefId, methodName).Result;

                if (salesOrder != null && (salesOrder.errors == null || salesOrder.errors.Count == 0))
                {
                    if (salesOrder.asset.@params.FirstOrDefault(p => p.name.Equals(ApplicationConstant.IngramEndcustomerAdminEmail)) != null)
                    {
                        var ingramEndcustomerAdminEmail = salesOrder.asset.@params.FirstOrDefault(p => p.name.Equals(ApplicationConstant.IngramEndcustomerAdminEmail)).value.ToString();

                        if (string.IsNullOrWhiteSpace(ingramEndcustomerAdminEmail) && salesOrder.status == ApplicationConstant.IngramResponseOrderStatusPending)
                        {
                            var updateParameterResponse = thirdPartyAPI.UpdateSaleOrderParameters(ord.ChannelRefId, thirdPartyMessage.TransactionStatus, methodName).Result;
                            if (updateParameterResponse != null && (updateParameterResponse.errors == null || updateParameterResponse.errors.Count == 0))
                            {
                                CustomLogger.LogRequestResponse(new LoggingContext()
                                {
                                    MethodName = MethodBase.GetCurrentMethod().Name,
                                    DataDirectionId = DataDirectionType.CLRequestToThirdParty,
                                    DataPacket = string.Empty,
                                    CreatedOn = DateTime.UtcNow,
                                    StoreId = currentStore.StoreId,
                                    CreatedBy = ApplicationConstant.UserName,
                                    Description = string.Format("Ingram order {0} update sales order parameter error description successfull", ord.ChannelRefId),
                                    EcomTransactionId = string.Empty,
                                    RequestInitiatedIP = string.Empty,
                                    OutputPacket = JsonConvert.SerializeObject(updateParameterResponse),
                                    OutputSentAt = DateTime.UtcNow,
                                    IdentifierKey = ord.ChannelRefId,
                                    IdentifierValue = ord.ChannelRefId,
                                    IsSuccess = 1,
                                    TotalProcessingDuration = 0
                                });

                                if (salesOrder.status == ApplicationConstant.IngramResponseOrderStatusPending)
                                {
                                    var ingramSalesOrderResponse = thirdPartyAPI.UpdateSaleOrderStatus(ord.ChannelRefId, ApplicationConstant.IngramOrderStatusInquire, methodName, ApplicationConstant.IngramEndcustomerAdminEmailErrorMessage).Result;
                                    if (ingramSalesOrderResponse.status == ApplicationConstant.IngramResponseOrderStatusInquiring)
                                    {
                                        CustomLogger.LogRequestResponse(new LoggingContext()
                                        {
                                            MethodName = MethodBase.GetCurrentMethod().Name,
                                            DataDirectionId = DataDirectionType.CLRequestToThirdParty,
                                            DataPacket = string.Empty,
                                            CreatedOn = DateTime.UtcNow,
                                            StoreId = currentStore.StoreId,
                                            CreatedBy = ApplicationConstant.UserName,
                                            Description = string.Format("Sales order {0} status sucessfully Changed to {1}.", ord.ChannelRefId, ApplicationConstant.IngramOrderStatusInquire),
                                            EcomTransactionId = string.Empty,
                                            RequestInitiatedIP = string.Empty,
                                            OutputPacket = JsonConvert.SerializeObject(ingramSalesOrderResponse),
                                            OutputSentAt = DateTime.UtcNow,
                                            IdentifierKey = ord.ChannelRefId,
                                            IdentifierValue = ord.ChannelRefId,
                                            IsSuccess = 1,
                                            TotalProcessingDuration = 0
                                        });
                                    }
                                    else
                                    {
                                        string errors = string.Empty;
                                        if (ingramSalesOrderResponse.errors != null)
                                        {
                                            errors = string.Join(",", ingramSalesOrderResponse.errors?.Select(e => e).ToList());
                                        }
                                        Exception ex = new Exception(string.Format("Ingram order {0} sync failed with error-code: {1} and error message(s): {2} ", ord.ChannelRefId, ingramSalesOrderResponse.error_code, errors));
                                        CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy, "Ingram Sales Order Status Sync");


                                        CustomLogger.LogDebugInfo(string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                            DataDirectionType.CLRequestToThirdParty, // Data direction
                                            string.Empty, // Data packed
                                            DateTime.UtcNow, // Created on
                                            currentStore.StoreId,
                                            ApplicationConstant.UserName, // Created by
                                            string.Format("Sales order status update for {0} failed to update with exception: {1}", ord.ChannelRefId, errors), // Description
                                            string.Empty, // eCom Transaction Id
                                            string.Empty, // Request Initialed IP
                                            JsonConvert.SerializeObject(ingramSalesOrderResponse), // Output packed
                                            DateTime.UtcNow, // Output sent at
                                            ord.ChannelRefId, // Identifier Key
                                            ord.ChannelRefId, // Identifier Value
                                            0, // Success
                                            0 // TotalProcessingDuration
                                            );
                                    }
                                }

                            }
                            else
                            {
                                string errors = string.Empty;
                                if (updateParameterResponse.errors != null)
                                {
                                    errors = string.Join(",", updateParameterResponse.errors?.Select(e => e).ToList());
                                }
                                Exception ex = new Exception(string.Format("Ingram order {0} update sales order parameter error description failed with error codes {1}", ord.ChannelRefId, errors));
                                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy, "Ingram Sales Order Status Sync");

                                CustomLogger.LogDebugInfo(string.Format("Ingram order {0} update sales order parameter error description failed with error codes {1}", ord.ChannelRefId, errors), currentStore.StoreId, currentStore.CreatedBy, ord.ChannelRefId);

                                CustomLogger.LogRequestResponse(new LoggingContext()
                                {
                                    MethodName = MethodBase.GetCurrentMethod().Name,
                                    DataDirectionId = DataDirectionType.CLRequestToThirdParty,
                                    DataPacket = string.Empty,
                                    CreatedOn = DateTime.UtcNow,
                                    StoreId = currentStore.StoreId,
                                    CreatedBy = ApplicationConstant.UserName,
                                    Description = string.Format("Ingram order {0} update sales order parameter error description failed with error codes {1}", ord.ChannelRefId, errors),
                                    EcomTransactionId = string.Empty,
                                    RequestInitiatedIP = string.Empty,
                                    OutputPacket = JsonConvert.SerializeObject(updateParameterResponse),
                                    OutputSentAt = DateTime.UtcNow,
                                    IdentifierKey = ord.ChannelRefId,
                                    IdentifierValue = ord.ChannelRefId,
                                    IsSuccess = 0,
                                    TotalProcessingDuration = 0
                                });
                            }

                        }
                    }
                }
            }
            catch (Exception exception)
            {
                CustomLogger.LogRequestResponse(new LoggingContext()
                {
                    MethodName = MethodBase.GetCurrentMethod().Name,
                    DataDirectionId = DataDirectionType.CLRequestToThirdParty,
                    DataPacket = string.Empty,
                    CreatedOn = DateTime.UtcNow,
                    StoreId = currentStore.StoreId,
                    CreatedBy = ApplicationConstant.UserName,
                    Description = string.Format("Sales order {0} failed to update", ord.ChannelRefId),
                    EcomTransactionId = string.Empty,
                    RequestInitiatedIP = string.Empty,
                    OutputPacket = JsonConvert.SerializeObject(exception),
                    OutputSentAt = DateTime.UtcNow,
                    IdentifierKey = ord.ChannelRefId,
                    IdentifierValue = ord.ChannelRefId,
                    IsSuccess = 0,
                    TotalProcessingDuration = 0
                });
            }
            finally
            {
                messageDAL.UpdateTransactionStatus(ord.ChannelRefId, TransactionStatus.MissingParameter_IngramOrderMarkedDeleted, ApplicationConstant.UserName);
            }
        }

        private string ResolveTrackingNumber(IList<ErpShipment> shipmentItems)
        {
            string returnValue = string.Empty;

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
                    if (returnValue == string.Empty)
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

            // In Transfer order validation error we are not getting status from ERP.
            if (orderStatus.Status == null) return status;

            if (orderStatus.Status.Equals(ApplicationConstant.IngramD365OrderStatusDelivered, StringComparison.CurrentCultureIgnoreCase))
            {
                status.status = simpleTypeOrderOrderStatus.OPEN;
                status.shippingStatus = simpleTypeOrderShippingStatus.SHIPPED;
            }
            else if (orderStatus.Status.Equals(ApplicationConstant.IngramD365OrderStatusInvoiced, StringComparison.CurrentCultureIgnoreCase) ||
                     orderStatus.Status.Equals(ApplicationConstant.IngramD365OrderStatusRenewal, StringComparison.CurrentCultureIgnoreCase)
                    )
            {
                status.status = simpleTypeOrderOrderStatus.COMPLETED;
                status.shippingStatus = simpleTypeOrderShippingStatus.SHIPPED;
            }
            else if (orderStatus.Status.Equals(ApplicationConstant.IngramD365OrderStatusCanceled, StringComparison.CurrentCultureIgnoreCase))
            {
                status.status = simpleTypeOrderOrderStatus.CANCELLED;
                status.shippingStatus = simpleTypeOrderShippingStatus.NOT_SHIPPED;
            }

            return status;
        }

        #endregion
    }
}
