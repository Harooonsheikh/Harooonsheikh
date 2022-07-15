using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Enums.Enums.TMV;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using System.Xml.Serialization;
using System.IO;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;
using VSI.EDGEAXConnector.ERPDataModels.BoletoPayment;
using System.Globalization;
using VSI.EDGEAXConnector.Common.Constants;
using Microsoft.Dynamics.Retail.PaymentSDK.Portable;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class SaleOrderController : BaseController, ISaleOrderController
    {
        #region Public Methods

        public SaleOrderController(string storeKey) : base(storeKey)
        {
        }

        /// <summary>
        /// Search Sales Order
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public object SearchSalesOrder(string orderNo)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, orderNo.ToString());

            try
            {
                var contactPersonManager = new SalesOrderCRTManager();
                return contactPersonManager.SearchOrders(orderNo, currentStore.StoreKey);

                //NS: Call RS
                /*
                //NS: Comment Start
                long channelId = CommerceRuntimeHelper.ChannelId;
                TransactionServiceClient tsClient = new TransactionServiceClient(CommerceRuntimeHelper.RequestContext); //Utility._RequestContext

                var salesOrder = tsClient.SearchOrders(new SalesOrderSearchCriteria { SalesId = orderNo }, 10);
                //NS: Comment End
                */

                //return salesOrder;

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

                return null;
            }
            catch (Exception ex)
            {
                // REMOVE THIS LINE // CustomLogger.LogTraceInfo("Exception logged: " + ex.Message);
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return null;
            }
        }

        /// <summary>
        /// Get Sales Order Status Updates
        /// </summary>
        /// <returns></returns>
        public List<ErpSalesOrderStatus> GetSalesOrderStatusUpdate()
        {

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                // process to get sales order updates
                // check integration key for sales order other than complete.
                // get one by one and check if previous sales order status is different
                // add in the list of updated sales order 
                // send back to Magento
                // var inCompleteOrder = IntegrationManager.GetComKey

                int saleOrderStausUpdateTimeFrameInDays = configurationHelper.GetSetting(SALESORDER.Update_Status_inDays).IntValue();
                var salesOrderStatuses = new List<ErpSalesOrderStatus>();

                //NS: Comment Start
                /*
                TransactionServiceClient tsClient = new TransactionServiceClient(CommerceRuntimeHelper.RequestContext);
                
                //get sales order in date range
                string fromDate = DateTime.UtcNow.Date.AddDays(-1 * saleOrderStausUpdateTimeFrameInDays).ToString("yyyy-MM-dd HH:mm:ss"); //new DateTime(2015, 07, 10).ToString("yyyy-MM-dd h:mm:ss"); 
                string toDate = DateTime.UtcNow.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"); //new DateTime(2015, 07, 28).ToString("yyyy-MM-dd h:mm:ss");
                var containerArray = tsClient.InvokeExtensionMethod("VSIGetSalesOrderStatus", fromDate, toDate, CommerceRuntimeHelper.ChannelId);
                CustomLogger.LogTraceInfo(string.Format("{0}-{1}-{2}", fromDate, toDate, CommerceRuntimeHelper.ChannelId));
                */
                //NS: Comment Start

                string[] containerArray = new string[10];
                //int lengthOfContainer = containerArray.Count;
                int lengthOfContainer = containerArray.Length;
                for (var index = 0; index < lengthOfContainer; index++)
                {
                    string item = containerArray[index].ToString();
                    try
                    {
                        // REMOVE THIS LINE // CustomLogger.LogTraceInfo(string.Format("Order Status {0}", item));
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10801, currentStore, MethodBase.GetCurrentMethod().Name, item);

                        var values = item.Split(',');

                        var data = new ErpSalesOrderStatus
                        {
                            ChannelRefId = values[0],
                            Status = values[1].Trim(),
                            CustomerAcc = values[2],
                            SalesId = values[3]
                        };
                        index = index++;
                        salesOrderStatuses.Add(data);
                    }
                    catch (Exception ex)
                    {
                        // absorb exception and continue for other
                        // REMOVE THIS LINE // CustomLogger.LogException(new Exception("error in parsing data for " + containerArray[index] + " " + ex.Message));
                        CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                    }
                }

                salesOrderStatuses.ForEach(so =>
                {
                    IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                    var key = integrationManager.GetErpKey(Entities.SalesOrderStatus, so.ChannelRefId);
                    so.Notify = true;

                    if (key != null && key.ErpKey == so.Status && (key.Description.ToLower() == "completed" || key.Description.ToLower() == "canceled"))
                    {
                        so.Notify = false;
                    }
                });

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(salesOrderStatuses));
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

                return salesOrderStatuses;
            }
            //NS: Comment Start
            /*
            catch (CommunicationException)
            {
                return new List<ErpSalesOrderStatus>();
                // if service contains no data, it throws exception "service does not contain data. So we are not logging it"         
            }
            */
            //NS: Comment End
            catch (Exception ex)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                throw ex;
            }
        }

        /// <summary>
        /// This method supports legacy implementation by invoking the code that use Dynamics Realtime Service extension when usingCRT flag is false, otherwise Commerce Runtime API are used instead and Order Status are pulled using OrderManager.
        /// </summary>
        /// <param name="usingCrt">
        /// false to use Dynamics Realtime service extension. 
        /// true to use OrderManager Commerce Runtime class SearchOrders method. </param>
        /// <returns> List of ErpSalesOrderStatus </returns>
        public List<ErpSalesOrderStatus> GetSalesOrderStatusUpdate(bool usingCrt)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                var salesOrderOfInterest = new List<ErpSalesOrderStatus>();

                int salesOrderProcessingThreadCount = CommonUtility.StringToInt(this.configurationHelper.GetSetting(SALESORDER.Sales_Order_Processing_Thread_Count));

                ThirdPartyMessageDAL thirdPartyData = new ThirdPartyMessageDAL(currentStore.StoreKey);

                List<ThirdPartyMessage> ingramOrders = new List<ThirdPartyMessage>();
                ingramOrders.AddRange(thirdPartyData.GetSalesOrdersList(TransactionStatus.CreatedInERP, salesOrderProcessingThreadCount));
                ingramOrders.AddRange(thirdPartyData.GetSalesOrdersList(TransactionStatus.IngramCancelRequest_OrderCanceledInErp, salesOrderProcessingThreadCount));
                ingramOrders.AddRange(thirdPartyData.GetSalesOrdersList(TransactionStatus.IngramCancelRequest_SyncToErpFailedDueToExpiredTerminationDate, salesOrderProcessingThreadCount));
                ingramOrders.AddRange(thirdPartyData.GetSalesOrdersList(TransactionStatus.IngramCancelRequest_StatusUpdateInThirdPartyFailure, salesOrderProcessingThreadCount));
                ingramOrders.AddRange(thirdPartyData.GetSalesOrdersList(TransactionStatus.SynchedWithThirdPartyFailure, salesOrderProcessingThreadCount));

                List<ThirdPartyMessage> ingramTransferOrders = new List<ThirdPartyMessage>();
                ingramTransferOrders.AddRange(thirdPartyData.GetSalesOrdersList(TransactionStatus.TransferIngramRequest_OrderTransfered, salesOrderProcessingThreadCount));

                CustomLogger.LogRequestResponse(GetType().FullName,
                    DataDirectionType.CLRequestToThirdParty, // Data direction
                    "", // Data packed
                    DateTime.UtcNow, // Created on
                    currentStore.StoreId,
                    "System", // Created by
                    "Orders for which status will be synced back to ingram", // Description
                    "", // eCom Transaction Id
                    "", // Request Initialed IP
                    JsonConvert.SerializeObject(ingramOrders), // Output packed
                    DateTime.UtcNow, // Output sent at
                    "", // Identifier Key
                    "", // Identifier Value
                    1, // Success
                    0 // TotalProcessingDuration
                    );

                if (ingramOrders.Count > 0 || ingramTransferOrders.Count > 0)
                {
                    var crtSalesOrderManager = new SalesOrderCRTManager();
                    var salesOrderStatuses = new List<ErpSalesOrderStatus>();
                    string channelReferenceIds = string.Empty,
                            salesIds = string.Empty,
                            salesIdsTransfer = string.Empty;

                    if (ingramOrders.Count > 0)
                    {
                        channelReferenceIds = string.Join(",", ingramOrders.Where(x => string.IsNullOrEmpty(x.SalesId)).Select(o => o.ThirdPartyId).ToList());
                        salesIds = string.Join(",", ingramOrders.Where(x => !string.IsNullOrEmpty(x.SalesId)).Select(o => o.SalesId).ToList());

                        int saleOrderStausUpdateTimeFrameInDays = configurationHelper.GetSetting(SALESORDER.Update_Status_inDays).IntValue();

                        DateTimeOffset fromDateOff = DateTimeOffset.Now.AddDays(-1 * saleOrderStausUpdateTimeFrameInDays);
                        DateTimeOffset toDateOff = DateTimeOffset.Now.AddDays(1);

                        salesOrderStatuses.AddRange(crtSalesOrderManager.GetSalesOrderStatus(channelReferenceIds, salesIds, fromDateOff, toDateOff, currentStore.StoreKey));
                    }

                    if (ingramTransferOrders.Count > 0)
                    {
                        salesIdsTransfer = string.Join(",", ingramTransferOrders.Where(x => !string.IsNullOrEmpty(x.SalesId)).Select(o => o.SalesId).ToList());
                        salesOrderStatuses.AddRange(crtSalesOrderManager.GetSalesOrderRenewalStatus(salesIdsTransfer, currentStore.StoreKey));
                    }

                    CustomLogger.LogRequestResponse(GetType().FullName,
                    DataDirectionType.CLRequestToThirdParty, // Data direction
                    string.Format("Channel Reference Ids: {0}, SalesIds: {1}, salesIdsTransfer: {2}, Current Store Key: {3}", channelReferenceIds, salesIds, salesIdsTransfer, currentStore.StoreKey), // Data packed
                    DateTime.UtcNow, // Created on
                    currentStore.StoreId,
                    "System", // Created by
                    "List of orders that will be synced with Ingram", // Description
                    "", // eCom Transaction Id
                    "", // Request Initialed IP
                    JsonConvert.SerializeObject(salesOrderStatuses), // Output packed
                    DateTime.UtcNow, // Output sent at
                    "", // Identifier Key
                    "", // Identifier Value
                    1, // Success
                    0 // TotalProcessingDuration
                    );

                    String statusForSync = configurationHelper.GetSetting(SALESORDER.Statuses_For_Sync);

                    // Get the Order of Interest
                    foreach (ErpSalesOrderStatus erpSalesOrderStatus in salesOrderStatuses)
                    {
                        if (statusForSync.ToLower().IndexOf(erpSalesOrderStatus.Status.ToLower(), StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            salesOrderOfInterest.Add(erpSalesOrderStatus);
                        }
                    }

                    IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                    // Update Notification for each Sales Order
                    salesOrderOfInterest.ForEach(soStatus =>
                    {
                        soStatus.Notify = true;

                        var key = integrationManager.GetErpKey(Entities.SalesOrderStatus, soStatus.ChannelRefId);
                        if (key != null
                            && key.ErpKey == soStatus.Status
                            && (key.Description.Equals(ApplicationConstant.IngramD365OrderStatusCompleted, StringComparison.OrdinalIgnoreCase)
                                || key.Description.Equals(ApplicationConstant.IngramD365OrderStatusCanceled, StringComparison.OrdinalIgnoreCase)
                                || key.Description.Equals(ApplicationConstant.IngramD365OrderStatusRenewal, StringComparison.OrdinalIgnoreCase)
                               )
                           )
                        {
                            soStatus.Notify = false;
                        }
                    });

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(salesOrderStatuses));

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
                }

                return salesOrderOfInterest;
            }
            catch (Exception ex)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                CustomLogger.LogRequestResponse(GetType().FullName,
                        DataDirectionType.CLRequestToThirdParty, // Data direction
                        "", // Data packed
                        DateTime.UtcNow, // Created on
                        currentStore.StoreId,
                        "System", // Created by
                        "Exception in GetSalesOrderStatusUpdate", // Description
                        "", // eCom Transaction Id
                        "", // Request Initialed IP
                        JsonConvert.SerializeObject(ex), // Output packed
                        DateTime.UtcNow, // Output sent at
                        "", // Identifier Key
                        "", // Identifier Value
                        0, // Success
                        0 // TotalProcessingDuration
                        );
                throw;
            }
        }

        /// <summary>
        /// Update Status Integration
        /// </summary>
        /// <param name="updatedOrders"></param>
        public void UpdateStatusIntegration(Dictionary<string, string> updatedOrders)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                updatedOrders.ToList()
                    .ForEach(o => { integrationManager.UpdateIntegrationKey(Entities.SaleOrder, "", o.Key, o.Value); });

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception exp)
            {
                //CustomLogger.LogException(exp);
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(exp));
            }
        }

        /// <summary>
        /// Create Sales Order
        /// </summary>
        /// <param name="salesOrderParam"></param>
        /// <returns></returns>
        public bool CreateSaleOrders(ErpSalesOrder salesOrderParam, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(salesOrderParam));

            long recordIdForDelivery = 0;

            if (salesOrderParam == null)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL40501, currentStore, MethodBase.GetCurrentMethod().Name);
                throw new CommerceLinkError("No sales orders found");
            }
            //Managing IngegrationKey
            salesOrderParam._integrationKey = new Data.IntegrationKey()
            {
                ComKey = salesOrderParam.ChannelReferenceId
            };
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            // if order already processed and file still exists in directory, dont process it again.
            if (integrationManager.GetErpKey(Entities.SaleOrder, salesOrderParam._integrationKey.ComKey) != null)
                return false;

            //NS: if not receiving sales transaction id from ecommerce
            if (!bool.Parse(configurationHelper.GetSetting(SALESORDER.Get_SalesTransaction_Id_From_Ecom)))
            {
                //Custom logic to identify SalesOrders inserted by Connector
                var prefix = configurationHelper.GetSetting(SALESORDER.OrderPrefix);
                salesOrderParam.Id = string.Concat(prefix + Guid.NewGuid());
            }

            bool isAppleAppOrder = salesOrderParam.CustomAttributes.Count > 0 ? salesOrderParam.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVRESELLER.ToString())).Value.Equals(ThirdPartyResellers.APPLEAPP.ToString()) : false;

            bool isIngramOrder = salesOrderParam.CustomAttributes.Count > 0 ? salesOrderParam.CustomAttributes.FirstOrDefault(k => k.Key.Equals(ErpSalesOrderExtensionProperties.TMVSALESORIGIN.ToString())).Value.Equals(ThirdPartyResellers.Ingram.ToString()) : false;

            //Ax required logic to insert number with each SaleLine
            int lineNo = 1;
            if (isAppleAppOrder || isIngramOrder)
            {
                salesOrderParam.SalesLines.ToList().ForEach(line => line.LineNumber = lineNo++);
            }


            if (salesOrderParam.TenderLines.ToList().Count > 0)
            {
                //Ax required logic to insert number with each TenderLine
                lineNo = 1;
                salesOrderParam.TenderLines.ToList().ForEach(line => line.LineNumber = lineNo++);

                ProcessHeaderAndLineDiscountLength(salesOrderParam);
            }

            //recordIdForDelivery = CreateCustomerWithSalesOrder(salesOrderParam);

            //Associate customer with sales order from CL integration DB for existing customer or create new
            recordIdForDelivery = AssociateCustomWithSalesOrder(salesOrderParam, requestId);
            //salesOrderParam.CustomAttributes.RemoveAll(x => x.Key == "TMVRESELLERACCOUNT");
            //salesOrderParam.CustomAttributes.Add(new KeyValuePair<string, string>("TMVRESELLERACCOUNT", salesOrderParam.Reseller.AccountNumber));

            // Custom For MF
            // MPOS is unable to fetch OneTime address due to their custom logic so we have to create all shipment address
            if (salesOrderParam.Shipments != null)
            {
                foreach (ErpShipment shipment in salesOrderParam.Shipments)
                {
                    shipment.DeliveryAddress.EcomAddressId = shipment.ShipmentId;
                    try
                    {
                        shipment.DeliveryAddress.AddressType = (ErpAddressType)configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue();
                        shipment.DeliveryAddress.AddressTypeValue = configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue();
                        //NS:
                        //We are creation one time address which create address transaction so no need to create realtime address
                        // shipment.DeliveryAddress.RecordId = this.AssignAXAddress(shipment.DeliveryAddress, salesOrderParam.CustomerId);

                        // if we have recordId then use the real time address else one time address will be created
                        if (recordIdForDelivery > 0)
                        {
                            shipment.DeliveryAddress.RecordId = recordIdForDelivery;
                        }
                    }
                    catch (Exception ex)
                    {
                        CustomLogger.LogTraceInfo(ex.Message, currentStore.StoreId, currentStore.CreatedBy);
                    }
                }
            }
            //Assigning sales order date came from Ecom
            ErpSalesOrder salesOrder = new ErpSalesOrder();
            salesOrder = salesOrderParam;

            //We are getting this from eCom order
            var helper = new SalesOrderHelper(currentStore.StoreKey);

            helper.InventoryLocation = configurationHelper.GetSetting(INVENTORY.LocationId);
            //{
            //    //NS
            //    //If order xml have custom attribute for this or it is constant value in mapping
            //    //InventoryLocation = salesOrderParam.InventoryLocationId,
            //    InventoryLocation = configurationHelper.GetSetting(INVENTORY.LocationId)
            //};

            salesOrder = helper.PopulateSalesOrder(salesOrder, salesOrderParam, requestId);

            //Assigning SalesPersonId
            if (string.IsNullOrEmpty(salesOrder.StaffId))
            {
                salesOrder.StaffId = configurationHelper.GetSetting(ECOM.SalesPerson_Id);
            }

            if (salesOrder.SalesLines != null)
            {
                // Assigning delivery mode from first line
                salesOrder.DeliveryMode = salesOrder.SalesLines[0].DeliveryMode;

                if (!bool.Parse(configurationHelper.GetSetting(SALESORDER.Disable_Shippment_Process)))
                {
                    // Assigning shipping address from first line
                    salesOrder.ShippingAddress = salesOrder.SalesLines[0].ShippingAddress;
                    salesOrder.ContactInformationCollection = new Collection<ErpContactInformation>();
                    //adding phone no for address
                    if (!string.IsNullOrEmpty(salesOrder.ShippingAddress.Phone))
                    {
                        salesOrder.ContactInformationCollection.Add(new ErpContactInformation
                        {
                            ContactInformationType = ErpContactInformationType.Phone,
                            ContactInformationTypeValue = (int)ErpContactInformationType.Phone,
                            Value = salesOrder.ShippingAddress.Phone
                        });
                        //salesOrder.ShippingAddress.Email = salesOrder.ReceiptEmail;
                    }
                }
            }

            //NS: Get channel id from ApplicationSettings not CRT
            //salesOrder.ChannelId = CommerceRuntimeHelper.ChannelId;
            salesOrder.ChannelId = configurationHelper.GetSetting(APPLICATION.Channel_Id).LongValue();

            //salesOrder.InventoryLocationId = currentChannelState.OnlineChannelInstance.InventoryLocationId;
            salesOrder.ChannelCurrencyExchangeRate = 1.00M;

            // Gift card unlocking should be last operation before creating sales transction 
            // CU9 has issue with Validate
            // salesOrder.ValidateSalesOrder(Utility._RequestContext);


            //++ Saving object to Json before Uploading
            //var orderJson = salesOrder.SerializeToJson(1);
            // REMOVE THIS LINE // CustomLogger.LogTraceInfo(string.Format("Json Order :{0}", orderJson), salesOrder.ChannelReferenceId);
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10501, customLogger, orderJson);

            try
            {

                //Check to handle payment TenderType issue
                if (salesOrder.TenderLines.Any(tl => tl.TenderTypeId == "" || tl.TenderTypeId == null))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40502, currentStore);
                    throw new CommerceLinkError(message);
                }
                else
                {
                    //Processing Sales Order Customization
                    ProcessSalesOrderExtensions(salesOrder);

                    var salesOrderCRTManager = new SalesOrderCRTManager();

                    // re-check if order already processed, the don't process it again.
                    if (integrationManager.GetErpKey(Entities.SaleOrder, salesOrderParam._integrationKey.ComKey) != null)
                        return false;
                    salesOrder = salesOrderCRTManager.UploadOrder(salesOrder, currentStore.StoreKey, requestId);

                    integrationManager.CreateIntegrationKey(Entities.SaleOrder, salesOrder.Id, salesOrderParam._integrationKey.ComKey,
                        salesOrder.Status.ToString());
                }
            }
            catch (Exception ex)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                throw ex;
            }
            // Custom For MF

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return true;
        }

        /// <summary>
        /// Create Real Time Sales Order Transaction
        /// </summary>
        /// <param name="salesOrder"></param>
        /// <returns></returns>
        public bool CreateRealtimeSaleOrderTransaction(ErpSalesOrder salesOrder, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            return this.CreateSaleOrders(salesOrder, requestId);
        }

        /// <summary>
        /// Get list of Retail Affiliations created in ERP
        /// </summary>
        /// <returns></returns>
        public List<ErpAffiliation> GetRetailAffiliations()
        {
            var salesOrderCRTManager = new SalesOrderCRTManager();
            return salesOrderCRTManager.GetRetailAffiliations(currentStore.StoreKey);
        }

        private long AssociateCustomWithSalesOrder(ErpSalesOrder salesOrderParam, string requestId)
        {
            #region Temp
            //salesOrderParam.CustomerId = salesOrderParam.Customer.EcomCustomerId;
            //salesOrderParam.CustomerName = salesOrderParam.Customer.Name;
            //salesOrderParam.CustomerEmail = salesOrderParam.Customer.Email; 
            #endregion
            long recordIdForDelivery = 0;
            ErpCustomer customer = new ErpCustomer();
            customer.EcomCustomerId = salesOrderParam.CustomerId;

            //if (salesOrderParam.Customer != null && !string.IsNullOrEmpty(salesOrderParam.Customer.EcomCustomerId))
            //{
            //    customer.EcomCustomerId = salesOrderParam.Customer.EcomCustomerId;
            //}

            bool isMigratedOrder = false;
            bool isContractManagementOrder = false;
            bool isQuotationConvertToOrder = false;
            bool isAXCustomerProvided = false;

            if (customer.EcomCustomerId == null && bool.Parse(configurationHelper.GetSetting(SALESORDER.Use_Default_Customer)))
            {

                string defaultCustomer = configurationHelper.GetSetting(APPLICATION.ERP_Default_Customer);
                // Update customer key in customer object
                customer.AccountNumber = defaultCustomer;
                salesOrderParam.CustomerId = defaultCustomer;
            }
            else
            {
                //NS: Contract Migration SO Customization
                if (salesOrderParam.CustomAttributes.Count > 0)
                {
                    foreach (var cust in salesOrderParam.CustomAttributes)
                    {
                        if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVMIGRATEDORDERNUMBER.ToString()))
                        {
                            if (!string.IsNullOrEmpty(cust.Value))
                            {
                                isMigratedOrder = true;
                                break;
                            }
                        }
                        if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVOLDSALESORDERNUMBER.ToString()))
                        {
                            if (!string.IsNullOrEmpty(cust.Value))
                            {
                                isContractManagementOrder = true;
                                break;
                            }
                        }
                        if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVQUOTATIONID.ToString()))
                        {
                            if (!string.IsNullOrEmpty(cust.Value))
                            {
                                isQuotationConvertToOrder = true;
                                break;
                            }
                        }
                        if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVIsAXCustomerProvided.ToString()))
                        {
                            if (!string.IsNullOrEmpty(cust.Value))
                            {
                                isAXCustomerProvided = true;
                                break;
                            }
                        }
                    }
                }

                if (isMigratedOrder || isContractManagementOrder || isQuotationConvertToOrder || isAXCustomerProvided)
                {
                    //Ecommerce send D365 customer account number
                    //customer = customerController.GetCustomerData(salesOrderParam.CustomerId, 2);
                    customer.AccountNumber = salesOrderParam.CustomerId;
                }
                else if (!bool.Parse(configurationHelper.GetSetting(SALESORDER.Create_Customer_With_SalesOrder)))
                {
                    IntegrationKey customerKey = null;
                    //IntegrationKey resellerKey = null;
                    IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                    // Get Customer Key from Integation DB to check if customer already exists or not
                    customerKey = integrationManager.GetErpKey(Entities.Customer, customer.EcomCustomerId);
                    //resellerKey = integrationManager.GetErpKey(Entities.Customer, salesOrderParam.Reseller.EcomCustomerId);
                    if (customerKey == null) // New Customer
                    {
                        string customerNotFoundErrorMessage = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40301, currentStore, customer.EcomCustomerId);
                        throw new CommerceLinkError(customerNotFoundErrorMessage);
                    }
                    else
                    {
                        // Update customer key in customer object
                        customer.AccountNumber = customerKey.Description;
                        //salesOrderParam.Reseller.AccountNumber = resellerKey.Description;
                        salesOrderParam.CustomerId = customer.AccountNumber;
                    }
                }
                else
                {
                    try
                    {
                        string vatNum = salesOrderParam.isTaxExemptCustomer == "1" ? configurationHelper.GetSetting(SALESORDER.Tax_Exempt_Number) : string.Empty;

                        List<ErpAddress> addressesOnSalesOrder = new List<ErpAddress>();
                        if (salesOrderParam.BillingAddress != null)
                        {
                            // Create billing address for customer
                            ErpAddress billingAddress = new ErpAddress();
                            billingAddress.EntityName = "Address";
                            billingAddress.RecordId = salesOrderParam.BillingAddress.RecordId;
                            billingAddress.Name = salesOrderParam.BillingAddress.Name;
                            billingAddress.Street = salesOrderParam.BillingAddress.Street;
                            billingAddress.City = salesOrderParam.BillingAddress.City;
                            billingAddress.State = salesOrderParam.BillingAddress.State;
                            billingAddress.ZipCode = salesOrderParam.BillingAddress.ZipCode;
                            billingAddress.Phone = salesOrderParam.BillingAddress.Phone;
                            billingAddress.AddressType = (ErpAddressType)configurationHelper.GetSetting(SALESORDER.AX_Invoice_Address_Type).IntValue();
                            billingAddress.AddressTypeValue = configurationHelper.GetSetting(SALESORDER.AX_Invoice_Address_Type).IntValue();
                            billingAddress.IsPrimary = true; // salesOrderParam.BillingAddress.IsPrimary; // This should be true as its current/latest
                            billingAddress.County = salesOrderParam.BillingAddress.County;
                            if (salesOrderParam.BillingAddress.ThreeLetterISORegionName != null && salesOrderParam.BillingAddress.ThreeLetterISORegionName.Length < 3)
                            {
                                billingAddress.ThreeLetterISORegionName = LocalizationHelper.CountryThreeLetterISOCode(salesOrderParam.BillingAddress.ThreeLetterISORegionName);
                            }
                            else
                            {
                                billingAddress.ThreeLetterISORegionName = salesOrderParam.BillingAddress.ThreeLetterISORegionName;
                            }
                            // Add billing address to billing address list
                            addressesOnSalesOrder.Add(billingAddress);
                        }
                        ErpAddress shippingAddress = null;
                        if (salesOrderParam.Shipments != null && salesOrderParam.Shipments.Count > 0)
                        {
                            if (salesOrderParam.Shipments[0].DeliveryAddress != null)
                            {
                                // Create shipping address for customer
                                shippingAddress = new ErpAddress();
                                shippingAddress.EntityName = "Address";
                                shippingAddress.RecordId = 0;
                                shippingAddress.Name = salesOrderParam.Shipments[0].DeliveryAddress.Name;
                                shippingAddress.Street = salesOrderParam.Shipments[0].DeliveryAddress.Street;
                                shippingAddress.City = salesOrderParam.Shipments[0].DeliveryAddress.City;
                                shippingAddress.State = salesOrderParam.Shipments[0].DeliveryAddress.State;
                                shippingAddress.ZipCode = salesOrderParam.Shipments[0].DeliveryAddress.ZipCode;
                                shippingAddress.Phone = salesOrderParam.Shipments[0].DeliveryAddress.Phone;
                                shippingAddress.AddressType = (ErpAddressType)configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue();
                                shippingAddress.AddressTypeValue = configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue();
                                shippingAddress.IsPrimary = true; // salesOrderParam.BillingAddress.IsPrimary; // This should be true as its current/latest
                                shippingAddress.County = salesOrderParam.Shipments[0].DeliveryAddress.County;
                                if (salesOrderParam.Shipments[0].DeliveryAddress.ThreeLetterISORegionName != null && salesOrderParam.Shipments[0].DeliveryAddress.ThreeLetterISORegionName.Length < 3)
                                {
                                    shippingAddress.ThreeLetterISORegionName = LocalizationHelper.CountryThreeLetterISOCode(salesOrderParam.Shipments[0].DeliveryAddress.ThreeLetterISORegionName);
                                }
                                else
                                {
                                    shippingAddress.ThreeLetterISORegionName = salesOrderParam.Shipments[0].DeliveryAddress.ThreeLetterISORegionName;
                                }
                                // Add shipping address to billing address list
                                addressesOnSalesOrder.Add(shippingAddress);
                            }
                        }

                        var customerController = new CustomerController(currentStore.StoreKey);

                        string customerPhone = string.Empty;

                        if (!bool.Parse(configurationHelper.GetSetting(SALESORDER.Disable_Shippment_Process)) && salesOrderParam.Shipments != null)
                        {
                            customerPhone = salesOrderParam.Shipments.Count > 0 ? salesOrderParam.Shipments[0].DeliveryAddress.Phone : "";
                        }
                        else if (bool.Parse(configurationHelper.GetSetting(SALESORDER.Disable_Shippment_Process)) && salesOrderParam.BillingAddress != null)
                        {
                            customerPhone = salesOrderParam.BillingAddress.Phone;
                        }

                        customer = customerController.AssignCustomer(new ErpCustomer
                        {
                            EcomCustomerId = salesOrderParam.CustomerId,
                            FirstName = salesOrderParam.CustomerName,
                            Email = salesOrderParam.CustomerEmail,
                            TaxGroup = configurationHelper.GetSetting(APPLICATION.ERP_Customer_Default_TaxGroup),
                            CustomerGroup = configurationHelper.GetSetting(APPLICATION.ERP_Default_Customer_Group),
                            CurrencyCode = configurationHelper.GetSetting(CUSTOMER.Default_CurrencyCode),
                            //Addresses = new List<ErpAddress> { salesOrderParam.ShippingAddress },
                            Addresses = new List<ErpAddress>(),
                            VatNumber = vatNum,
                            // Custom For MF
                            //They need customer phone at header
                            Phone = customerPhone,
                            //NS: RS architect
                            AccountNumber = new Guid().ToString(),
                            CustomerType = ErpCustomerType.Person,
                            CustomerTypeValue = (int)ErpCustomerType.Person
                        }, requestId, true);

                        salesOrderParam.CustomerId = customer.AccountNumber;

                        if (customer.Addresses != null)
                        {
                            if (bool.Parse(configurationHelper.GetSetting(SALESORDER.Create_Customer_With_SalesOrder)))
                            {
                                customer.Addresses = new List<ErpAddress>(addressesOnSalesOrder);
                                customer = AssignCustomerAddresses(customer, requestId);
                            }

                            foreach (ErpAddress address in customer.Addresses)
                            {
                                if (address.AddressTypeValue == configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue())
                                {
                                    recordIdForDelivery = address.RecordId;
                                }
                                if (shippingAddress != null)
                                {
                                    if (address.Street == shippingAddress.Street && address.Phone == shippingAddress.Phone)
                                    {
                                        var shippingAddres = salesOrderParam.Shipments
                                            .Where(m => m.DeliveryAddress.Street == address.Street &&
                                                        m.DeliveryAddress.Phone == address.Phone && address.AddressTypeValue == configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue()).FirstOrDefault();
                                        if (shippingAddres != null)
                                        {
                                            shippingAddres.DeliveryAddress.PhoneRecordId = address.PhoneRecordId;
                                            shippingAddres.DeliveryAddress.PartyNumber = address.PartyNumber;
                                            shippingAddres.DeliveryAddress.DirectoryPartyLocationRecordId = address.DirectoryPartyLocationRecordId;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return recordIdForDelivery;
        }

        /// <summary>
        /// Create address from ErpAddress
        /// </summary>
        /// <param name="erpAddress"></param>
        /// <returns></returns>
        public ErpAddress CreateAddressFromErpAddress(ErpAddress erpAddress)
        {
            //DAI-1001 

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpAddress));

            string zipCode;

            if (Convert.ToBoolean(configurationHelper.GetSetting(APPLICATION.ZipCode_Truncate_Enable)).Equals(true))
            {
                if (erpAddress.ZipCode.Contains("-"))
                {
                    zipCode = erpAddress.ZipCode.Substring(0, erpAddress.ZipCode.IndexOf("-")).Trim();
                }
                else
                {
                    zipCode = erpAddress.ZipCode;
                }
            }
            else
            {
                zipCode = erpAddress.ZipCode;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return new ErpAddress
            {
                //NS:
                EntityName = "Address",
                RecordId = erpAddress.RecordId,
                Name = erpAddress.Name,
                Street = erpAddress.Street,
                City = erpAddress.City,
                State = erpAddress.State,
                ZipCode = zipCode,
                Phone = erpAddress.Phone,
                //DAI-866 Starts
                AddressType = (ErpAddressType)configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue(),
                AddressTypeValue = configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue(),
                //DAI-866 Ends
                IsPrimary = erpAddress.IsPrimary,
                County = erpAddress.County,
                ThreeLetterISORegionName = erpAddress.ThreeLetterISORegionName,
                PhoneRecordId = erpAddress.PhoneRecordId,
                PartyNumber = erpAddress.PartyNumber,
                DirectoryPartyLocationRecordId = erpAddress.DirectoryPartyLocationRecordId

            };
        }

        public ErpValidateVATNumberResponse ValidateVATNumber(ErpValidateVATNumberRequest request, string requestId)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, customLogger, MethodBase.GetCurrentMethod().Name);

            var salesOrderManager = new SalesOrderCRTManager();

            return salesOrderManager.ValidateVATNumber(request, currentStore.StoreKey, requestId);
        }
        #endregion

        #region Private Methods

        private string GetStoreKeyByCountryCode(string threeLetterISORegionName)
        {
            var appSettingsDAL = new ApplicationSettingsDAL(currentStore.StoreKey);
            var storeId = appSettingsDAL.GetStoreId(threeLetterISORegionName);
            var store = StoreService.GetStoreById(storeId);

            if (store == null)
                throw new CommerceLinkError("Store not found with store Id '" + store.StoreId + "'");

            return store.StoreKey;
        }

        /// <summary>
        /// Customer already exists, just create addresses
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private ErpCustomer AssignCustomerAddresses(ErpCustomer customer, string requestId)
        {
            var customerController = new CustomerController(currentStore.StoreKey);
            return customerController.CreateNewCustomer(customer, false, requestId);
        }
        //NS: PaymentSDK

        /// <summary>
        /// Process Sales Order Extensions
        /// </summary>
        /// <param name="salesOrder"></param>
        private void ProcessSalesOrderExtensions(ErpSalesOrder salesOrder)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(salesOrder));

            //Header Entension
            if (salesOrder.ExtensionProperties == null)
            {
                salesOrder.ExtensionProperties = new List<ErpCommerceProperty>();
            }

            if (salesOrder.CustomAttributes.Count > 0)
            {
                foreach (var cust in salesOrder.CustomAttributes)
                {
                    if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVRESELLERACCOUNT.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVRESELLERACCOUNT.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value ?? "" }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVDISTRIBUTORACCOUNT.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVDISTRIBUTORACCOUNT.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value ?? "" }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVINDIRECTCUSTOMER.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVINDIRECTCUSTOMER.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value ?? "" }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVINDIRECTCUSTOMEREMAIL.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVINDIRECTCUSTOMEREMAIL.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value ?? "" }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVMAINOFFERTYPE.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVMAINOFFERTYPE.ToString(), new ErpCommercePropertyValue { IntegerValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt32(cust.Value) }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVPRODUCTFAMILY.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVPRODUCTFAMILY.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVSALESORDERSUBTYPE.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVSALESORDERSUBTYPE.ToString(), new ErpCommercePropertyValue { IntegerValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt32(cust.Value) }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVINVOICESCHEDULECOMPLETE.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVINVOICESCHEDULECOMPLETE.ToString(), new ErpCommercePropertyValue { IntegerValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt32(cust.Value) }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVCONTRACTSTATUSLINE.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVCONTRACTSTATUSLINE.ToString(), new ErpCommercePropertyValue { IntegerValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt32(cust.Value) }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVSMMCAMPAIGNID.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVSMMCAMPAIGNID.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVPURCHORDERFORMNUM.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVPURCHORDERFORMNUM.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVPIT.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVPIT.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVQUOTATIONID.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVQUOTATIONID.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    //NS: Contract Migration SO Customization
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVMIGRATEDORDERNUMBER.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVMIGRATEDORDERNUMBER.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                        if (!string.IsNullOrEmpty(cust.Value))
                        {
                            salesOrder.TMVMigratedOrderNumber = cust.Value;
                        }
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVOLDSALESORDERNUMBER.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVOLDSALESORDERNUMBER.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                        if (!string.IsNullOrEmpty(cust.Value))
                        {
                            salesOrder.TMVOldSalesOrderNumber = cust.Value;
                        }
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVORIGINATINGCOUNTRY.ToString()) ||
                        cust.Key.Equals("TMVOriginatingCountry"))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVORIGINATINGCOUNTRY.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVCOMMENTFORORDER.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVCOMMENTFORORDER.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVCOMMENTFOREMAIL.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVCOMMENTFOREMAIL.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVSALESORIGIN.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVSALESORIGIN.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVPARTNERID.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVPARTNERID.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVPaymentTerms.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVPaymentTerms.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVFraudReviewStatus.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVFraudReviewStatus.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVKountScore.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVKountScore.ToString(), new ErpCommercePropertyValue { DecimalValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToDecimal(cust.Value) }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVRESELLER.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVRESELLER.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVCardHolder.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVCardHolder.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVIBAN.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVIBAN.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVSwiftCode.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVSwiftCode.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVBankName.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVBankName.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVTransferOrderAsPerOldDate.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVTransferOrderAsPerOldDate.ToString(), new ErpCommercePropertyValue { IntegerValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt32(cust.Value) }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVActivationLinkEmail.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVActivationLinkEmail.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVCustomerReference.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVCustomerReference.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVContactPersonId.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVContactPersonId.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVLoginEmail.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVLoginEmail.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    #region ALIPAY

                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVAlipayBuyerId.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVAlipayBuyerId.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVAlipayBuyerEmail.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVAlipayBuyerEmail.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVAlipayOutTradeNo.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVAlipayOutTradeNo.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVAlipayTradeNo.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVAlipayTradeNo.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }
                    else if (cust.Key.Equals(ErpSalesOrderExtensionProperties.TMVAlipayPspReference.ToString()))
                    {
                        salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVAlipayPspReference.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                    }

                    #endregion

                }
                if (!String.IsNullOrEmpty(salesOrder.CurrencyCode))
                {
                    salesOrder.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesOrderExtensionProperties.TMVCurrencyCode.ToString(), new ErpCommercePropertyValue { StringValue = salesOrder.CurrencyCode }));
                }
            }

            //Line Entension
            foreach (ErpSalesLine line in salesOrder.SalesLines)
            {
                if (line.ExtensionProperties == null)
                {
                    line.ExtensionProperties = new List<ErpCommerceProperty>();
                }
                if (line.CustomAttributes.Count > 0)
                {
                    foreach (var cust in line.CustomAttributes)
                    {
                        if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTVALIDFROM.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVCONTRACTVALIDFROM.ToString(), new ErpCommercePropertyValue { StringValue = string.IsNullOrEmpty(cust.Value) ? "1900-01-01 00:00:00.000" : cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTCALCULATEFROM.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVCONTRACTCALCULATEFROM.ToString(), new ErpCommercePropertyValue { StringValue = string.IsNullOrEmpty(cust.Value) ? "1900-01-01 00:00:00.000" : cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTVALIDTO.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVCONTRACTVALIDTO.ToString(), new ErpCommercePropertyValue { StringValue = string.IsNullOrEmpty(cust.Value) ? "1900-01-01 00:00:00.000" : cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTPOSSTERMDATE.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVCONTRACTPOSSTERMDATE.ToString(), new ErpCommercePropertyValue { StringValue = string.IsNullOrEmpty(cust.Value) ? "1900-01-01 00:00:00.000" : cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTCANCELDATE.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVCONTRACTCANCELDATE.ToString(), new ErpCommercePropertyValue { StringValue = string.IsNullOrEmpty(cust.Value) ? "1900-01-01 00:00:00.000" : cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTPOSSCANCELDATE.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVCONTRACTPOSSCANCELDATE.ToString(), new ErpCommercePropertyValue { StringValue = string.IsNullOrEmpty(cust.Value) ? "1900-01-01 00:00:00.000" : cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTTERMDATE.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVCONTRACTTERMDATE.ToString(), new ErpCommercePropertyValue { StringValue = string.IsNullOrEmpty(cust.Value) ? "1900-01-01 00:00:00.000" : cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTTERMDATEEFFECTIVE.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVCONTRACTTERMDATEEFFECTIVE.ToString(), new ErpCommercePropertyValue { StringValue = string.IsNullOrEmpty(cust.Value) ? "1900-01-01 00:00:00.000" : cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVAUTOPROLONGATION.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVAUTOPROLONGATION.ToString(), new ErpCommercePropertyValue { IntegerValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt32(cust.Value) }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVPURCHORDERFORMNUM.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVPURCHORDERFORMNUM.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVCUSTOMERREF.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVCUSTOMERREF.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVCONTRACTSTATUSLINE.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVCONTRACTSTATUSLINE.ToString(), new ErpCommercePropertyValue { IntegerValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt32(cust.Value) }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVEULAVERSION.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVEULAVERSION.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVBILLINGPERIOD.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVBILLINGPERIOD.ToString(), new ErpCommercePropertyValue { IntegerValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt32(cust.Value) }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.PACLICENSE.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.PACLICENSE.ToString(), new ErpCommercePropertyValue { StringValue = cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVORIGINALLINEAMOUNT.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVORIGINALLINEAMOUNT.ToString(), new ErpCommercePropertyValue { DecimalValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToDecimal(cust.Value) }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVLINEMODIFIED.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVLINEMODIFIED.ToString(), new ErpCommercePropertyValue { IntegerValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt32(cust.Value) }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVREVERSEDLINE.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVREVERSEDLINE.ToString(), new ErpCommercePropertyValue { IntegerValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt32(cust.Value) }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVMIGRATEDSALESLINENUMBER.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVMIGRATEDSALESLINENUMBER.ToString(), new ErpCommercePropertyValue { LongValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt64(cust.Value) }));
                            if (!string.IsNullOrEmpty(cust.Value))
                            {
                                line.TMVMigratedSalesLineNumber = Convert.ToInt64(cust.Value);
                            }
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVAFFILIATIONRECID.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVAFFILIATIONRECID.ToString(), new ErpCommercePropertyValue { LongValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt64(cust.Value) }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVOLDSALESLINENUMBER.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVOLDSALESLINENUMBER.ToString(), new ErpCommercePropertyValue { LongValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt64(cust.Value) }));
                            if (!string.IsNullOrEmpty(cust.Value))
                            {
                                line.TMVOldSalesLineNumber = Convert.ToInt64(cust.Value);
                            }
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVOLDSALESLINEACTION.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVOLDSALESLINEACTION.ToString(), new ErpCommercePropertyValue { StringValue = string.IsNullOrEmpty(cust.Value) ? "" : cust.Value }));
                            if (!string.IsNullOrEmpty(cust.Value))
                            {
                                line.TMVOldSalesLineAction = cust.Value;
                            }
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVSOURCEID.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVSOURCEID.ToString(), new ErpCommercePropertyValue { StringValue = string.IsNullOrEmpty(cust.Value) ? "" : cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVPARENT.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVPARENT.ToString(), new ErpCommercePropertyValue { LongValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt64(cust.Value) }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVCOUPONCODE.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVCOUPONCODE.ToString(), new ErpCommercePropertyValue { StringValue = string.IsNullOrEmpty(cust.Value) ? "" : cust.Value }));
                        }
                        else if (cust.Key.Equals(ErpSalesLineExtensionProperties.TMVCUSTOMERLINENUM.ToString()))
                        {
                            line.ExtensionProperties.Add(new ErpCommerceProperty(ErpSalesLineExtensionProperties.TMVCUSTOMERLINENUM.ToString(), new ErpCommercePropertyValue { IntegerValue = string.IsNullOrEmpty(cust.Value) ? 0 : Convert.ToInt32(cust.Value) }));
                        }
                    }
                }
            }

            //Paymnet Entension
            foreach (ErpTenderLine tenderLine in salesOrder.TenderLines)
            {
                if (tenderLine.ExtensionProperties == null)
                {
                    tenderLine.ExtensionProperties = new ObservableCollection<ErpCommerceProperty>();// List<ErpCommerceProperty>();
                }
                if (tenderLine.CustomAttributes != null)
                {
                    //No Customizations
                }

                if (tenderLine.TenderTypeId == ((int)PaymentCon.BOLETO).ToString())
                {
                    if (tenderLine.Boleto == null)
                    {
                        throw new CommerceLinkError("Invalid payment request.");
                    }

                    var boletoXml = ConvertBoletoToXmlString(tenderLine.Boleto);
                    tenderLine.ExtensionProperties.Add(new ErpCommerceProperty() { Key = "TMVBoletoPaymentTrans", Value = new ErpCommercePropertyValue() { StringValue = boletoXml } });

                }

            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

        }

        /// <summary>
        /// Assign AX Address
        /// </summary>
        /// <param name="address"></param>
        /// <param name="customerAccount"></param>
        /// <returns></returns>
        private long AssignAXAddress(ErpAddress address, string customerAccount)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "address: " + JsonConvert.SerializeObject(address) + ", customerAccount: " + customerAccount);

            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

            if (!string.IsNullOrEmpty(address.EcomAddressId))
            {
                IntegrationKey erpKey = null;
                erpKey = integrationManager.GetErpKey(Entities.CustomerAddress, address.EcomAddressId);
                if (erpKey != null)
                {
                    return Convert.ToInt64(erpKey.ErpKey);
                }
                else
                {
                    var customerController = new CustomerController(currentStore.StoreKey);
                    customerController.SaveAddresses(customerAccount, new List<ErpAddress> { address });
                    erpKey = integrationManager.GetErpKey(Entities.CustomerAddress, address.EcomAddressId);
                    if (erpKey != null)
                    {
                        return Convert.ToInt64(erpKey.ErpKey);
                    }
                }
            }
            else
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40701, currentStore);
                throw new CommerceLinkError(message);
            }
            return 0;
        }

        /// <summary>
        /// Process Sales Order MFI Comments
        /// </summary>
        /// <param name="salesOrderParam"></param>
        private void ProcessSalesOrderMfiComments(ErpSalesOrder salesOrderParam)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(salesOrderParam));

            string vsiMfiCommentsValue = string.Empty;
            salesOrderParam.TenderLines.ToList().ForEach(tl =>
            {
                // Setting comments of to show delivery comment at header
                vsiMfiCommentsValue += tl.TenderTypeId + " : ";

                tl.CustomAttributes.Where(c => c.Value != "").ToList().ForEach(attribute =>
                {
                    vsiMfiCommentsValue += attribute.Key + "=" + attribute.Value + ", ";
                });
            });

            KeyValuePair<string, string> mfiCommentsAttribute = new KeyValuePair<string, string>("mfiComments", vsiMfiCommentsValue);
            salesOrderParam.CustomAttributes.Add(mfiCommentsAttribute);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Process Header and Line Discount Length
        /// </summary>
        /// <param name="order"></param>
        private void ProcessHeaderAndLineDiscountLength(ErpSalesOrder order)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(order));

            //Header
            if (order.OrderDiscounts != null)
            {
                foreach (ErpDiscountLine dLine in order.OrderDiscounts)
                {
                    if (dLine != null)
                    {
                        if (dLine.OfferId != null)
                        {
                            dLine.OfferId = (dLine.OfferId.Length > 15 ? dLine.OfferId.ToString().Substring(0, 15) : dLine.OfferId);
                        }
                        if (dLine.DiscountCode != null)
                        {
                            dLine.DiscountCode = (dLine.DiscountCode.Length > 15 ? dLine.DiscountCode.ToString().Substring(0, 15) : dLine.DiscountCode);
                        }
                    }
                }
            }
            //Line
            if (order.SalesLines != null)
            {
                foreach (ErpSalesLine line in order.SalesLines)
                {
                    if (line.DiscountLines != null)
                    {
                        foreach (ErpDiscountLine dLine in line.DiscountLines)
                        {
                            if (dLine.OfferId != null)
                            {
                                dLine.OfferId = (dLine.OfferId.Length > 15 ? dLine.OfferId.ToString().Substring(0, 15) : dLine.OfferId);
                            }
                            if (dLine.DiscountCode != null)
                            {
                                dLine.DiscountCode = (dLine.DiscountCode.Length > 15 ? dLine.DiscountCode.ToString().Substring(0, 15) : dLine.DiscountCode);
                            }
                        }
                    }
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

        }

        private void SelectActiveDiscounts(List<ErpSalesOrder> salesOrders)
        {

            foreach (ErpSalesOrder salesOrder in salesOrders)
            {
                foreach (ErpSalesLine salesLine in salesOrder.SalesLines)
                {
                    List<ErpDiscountLine> activeDiscountList = null;

                    foreach (ErpDiscountLine discountLine in salesLine.DiscountLines)
                    {
                        if (discountLine.ContractValidTo >= Convert.ToDateTime(salesLine.TMVContractValidTo) ||
                                discountLine.ContractValidTo == null
                            )
                        {
                            if (activeDiscountList == null)
                            {
                                activeDiscountList = new List<ErpDiscountLine>();
                            }
                            activeDiscountList.Add(discountLine);
                        }
                    }

                    salesLine.DiscountLines = activeDiscountList;
                }
            }
        }

        private decimal GetEffectiveDiscount(List<ErpDiscountLine> discountLines)
        {
            decimal effectiveDiscount = 0;

            if (discountLines != null && discountLines.Count > 0)
            {
                List<ErpDiscountLine> activeDiscountLines =
                    discountLines.Where(d => d.ContractValidFrom <= DateTime.Now.Date && d.ContractValidTo >= DateTime.Now.Date).ToList();

                if (activeDiscountLines.FirstOrDefault(d => d.DiscountMethod == 2) != null)
                {
                    effectiveDiscount = activeDiscountLines.FirstOrDefault(d => d.DiscountMethod == 2).EffectiveAmount;
                }
                else
                {
                    effectiveDiscount = activeDiscountLines.Sum(d => d.EffectiveAmount);
                }
            }
            return effectiveDiscount;
        }

        public ERPContractSalesorderResponse GetContractSalesOrder(ContractSalesorderRequest request, string requestId)
        {
            if (string.IsNullOrEmpty(request.CustomerAccount) && !string.IsNullOrEmpty(request.EcomCustomerId))
            {
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                var customerKey = integrationManager.GetErpKey(Entities.Customer, request.EcomCustomerId);
                if (customerKey == null)
                {
                    string customerNotFoundErrorMessage = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40301, currentStore, request.EcomCustomerId);
                    throw new CommerceLinkError(customerNotFoundErrorMessage);
                }
                else
                {
                    // Update customer key in customer object
                    request.CustomerAccount = customerKey.Description;
                }
            }

            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, customLogger, MethodBase.GetCurrentMethod().Name);
            var salesOrderManager = new SalesOrderCRTManager();
            ERPContractSalesorderResponse erpContractSalesorderResponse = salesOrderManager.GetContractSalesOrder(request, currentStore.StoreKey, requestId);

            PaymentConnectorDAL paymentConnector = new PaymentConnectorDAL(currentStore.StoreKey);

            if (erpContractSalesorderResponse.Success)
            {
                foreach (var contract in erpContractSalesorderResponse.Contracts)
                {
                    // Remove all invalid discount lines
                    SelectActiveDiscounts(contract.SalesOrders);

                    LanguageCodesDAL languageCodes = null;
                    StoreCodesDAL storeCodesDAL = null;
                    string storeKeyByCountryCode = string.Empty;
                    string erpLanguage = string.Empty;
                    string ecomLanguage = string.Empty;
                    string ecomSiteCode = string.Empty;
                    string threeLetterISORegion = string.Empty;

                    foreach (ErpSalesOrder erpSalesOrder in contract.SalesOrders)
                    {
                        PaymentConnector paymentConnectorValue = paymentConnector.GetPaymentConnectorUsingErpPaymentConnector(erpSalesOrder.PaymentInfo.ProcessorId);
                        if (paymentConnectorValue != null)
                        {
                            erpSalesOrder.PaymentInfo.ProcessorId = paymentConnectorValue.EComCreditCardProcessorName;
                        }

                        if (erpSalesOrder.TMVMainOfferType == "2")
                        {
                            foreach (ErpSalesLine erpSalesLine in erpSalesOrder.SalesLines)
                            {
                                // decimal totalDiscount = erpSalesLine.DiscountLines.Sum(dis => dis.EffectiveAmount);
                                decimal totalDiscount = 0;
                                if (erpSalesLine.DiscountLines != null && erpSalesLine.DiscountLines.Count > 0)
                                {
                                    totalDiscount = GetEffectiveDiscount(erpSalesLine.DiscountLines.ToList());
                                }
                                decimal adjustedManualDiscount = ProcessSalesLineForCalculationOfManualDiscounts(erpSalesLine);

                                bool tmvAutoProlongation = erpSalesLine.TMVAutoProlongation.ToString() == "1" ? true : false;
                                // This will be false because currently time calculation code is not centeralized
                                // AX team is working on this they are using below code
                                // timeQty = this.tmvCalculateTimeQty(this.TMVContractCalculateFrom, this.TMVContractCalculateTo, this.tmvBillingPeriodDim().RetailWeight, this.TMVAutoProlongation,this.TMVCalculationType == 0,this.tmvIsSubscription());
                                tmvAutoProlongation = false;

                                //VSTS 19034: Ax has proivded method function to get time quantity in GetContractSalesOrder RTS method
                                //double timeQuantity = CalculateTimeQty(Convert.ToDateTime(DateTime.Now.Date.ToString()),
                                //    Convert.ToDateTime(erpSalesLine.TMVContractValidTo), Convert.ToInt32(erpSalesLine.TMVBillingPeriod),
                                //    true, tmvAutoProlongation);

                                //erpSalesLine.TMVTimeQuantity = Convert.ToDecimal(timeQuantity);

                                decimal discountPercentage;
                                discountPercentage = erpSalesLine.LinePercentageDiscount;
                                // discountPercentage = erpSalesLine.DiscountAmount / erpSalesLine.Price * 100;

                                decimal discountAmount;
                                discountAmount = (totalDiscount - adjustedManualDiscount) / Convert.ToInt32(erpSalesLine.Quantity);

                                /// Discount will always be 0 as new we are bringing price from column LineAmount in CustInvoiceTrans table in AX Database
                                /// This table contains the line amount after all calculations i.e. amount after discount and tax calculations

                                //VSTS 19034: Ax has proivded method function to get LineAmount in GetContractSalesOrder RTS method
                                //double lineAmount = CalculateLineAmount(Convert.ToDateTime(DateTime.Now.Date.ToString()),
                                //    Convert.ToDateTime(erpSalesLine.TMVContractValidTo), Convert.ToInt32(erpSalesLine.TMVBillingPeriod),
                                //    Convert.ToInt32(erpSalesLine.Quantity), Convert.ToDouble(erpSalesLine.Price),
                                //    Convert.ToDouble(discountAmount), Convert.ToDouble(discountPercentage),
                                //    true, tmvAutoProlongation);

                                //VSTS 19034: Ax has proivded method function to get LineAmount in GetContractSalesOrder RTS method
                                //erpSalesLine.TMVCalculateLineAmount = Convert.ToDecimal(lineAmount);
                            }
                        }

                        threeLetterISORegion = erpSalesOrder.ThreeLetterISORegionName;
                        storeKeyByCountryCode = string.IsNullOrWhiteSpace(threeLetterISORegion) ? currentStore.StoreKey : GetStoreKeyByCountryCode(erpSalesOrder.ThreeLetterISORegionName);

                        languageCodes = new LanguageCodesDAL(storeKeyByCountryCode);
                        storeCodesDAL = new StoreCodesDAL(storeKeyByCountryCode);
                        IntegrationManager integrationManager = new IntegrationManager(storeKeyByCountryCode);

                        erpLanguage = erpSalesOrder.Language;
                        ecomLanguage = languageCodes.GetEcomLanguageCode(erpLanguage);
                        ecomSiteCode = storeCodesDAL.GetEcomStoreCode(erpLanguage);
                        erpSalesOrder.Language = string.IsNullOrWhiteSpace(ecomLanguage) ? erpLanguage : ecomLanguage;
                        erpSalesOrder.SiteCode = string.IsNullOrWhiteSpace(ecomSiteCode) ? erpLanguage : ecomSiteCode;

                        foreach (var salesLine in erpSalesOrder.SalesLines)
                        {
                            string comKey = salesLine.ItemId;
                            if (!string.IsNullOrEmpty(salesLine.VariantId))
                            {
                                comKey += "_" + salesLine.VariantId;
                            }

                            var key = integrationManager.GetErpKey(Entities.Product, comKey);
                            if (key != null)
                            {
                                salesLine.ItemId = key.ComKey;
                                salesLine.ProductId = Convert.ToInt64(key.ErpKey);
                            }
                        }
                    }

                    //swap language for customer
                    threeLetterISORegion = string.Empty;
                    if (contract.Customer != null &&
                        contract.Customer.Addresses != null &&
                        contract.Customer.Addresses.Count > 0)
                    {
                        threeLetterISORegion = contract.Customer.Addresses.Where(s => s.IsPrimary = true).FirstOrDefault().ThreeLetterISORegionName;
                        storeKeyByCountryCode = string.IsNullOrWhiteSpace(threeLetterISORegion) ? currentStore.StoreKey : GetStoreKeyByCountryCode(threeLetterISORegion);
                        languageCodes = new LanguageCodesDAL(storeKeyByCountryCode);
                        erpLanguage = contract.Customer.Language;
                        ecomLanguage = languageCodes.GetEcomLanguageCode(erpLanguage);
                        contract.Customer.Language = string.IsNullOrWhiteSpace(ecomLanguage) ? erpLanguage : ecomLanguage;
                    }

                    //swap language for contact person
                    threeLetterISORegion = string.Empty;
                    if (contract.ContactPerson != null &&
                        contract.ContactPerson.Addresses != null &&
                        contract.ContactPerson.Addresses.Count > 0)
                    {
                        threeLetterISORegion = contract.ContactPerson.Addresses[0].ThreeLetterISORegionName;
                        storeKeyByCountryCode = string.IsNullOrWhiteSpace(threeLetterISORegion) ? currentStore.StoreKey : GetStoreKeyByCountryCode(threeLetterISORegion);
                        languageCodes = new LanguageCodesDAL(storeKeyByCountryCode);
                        erpLanguage = contract.ContactPerson.Language;
                        ecomLanguage = languageCodes.GetEcomLanguageCode(erpLanguage);
                        contract.ContactPerson.Language = string.IsNullOrWhiteSpace(ecomLanguage) ? erpLanguage : ecomLanguage;
                    }
                }
            }

            return erpContractSalesorderResponse;
        }

        private decimal ProcessSalesLineForCalculationOfManualDiscounts(ErpSalesLine erpSalesLine)
        {
            string manualDiscountReasonCode = configurationHelper.GetSetting(SALESORDER.TMV_ManualDiscountReasonCode);
            decimal manualAdjustedDiscount = 0;

            if (!string.IsNullOrEmpty(manualDiscountReasonCode) && erpSalesLine.DiscountLines != null)
            {
                manualAdjustedDiscount = erpSalesLine.DiscountLines.Where(d => d.TMVPriceOverrideReasonCode == manualDiscountReasonCode).Sum(dis => dis.EffectiveAmount);
            }

            return manualAdjustedDiscount;
        }

        public double CalculateTimeQty(DateTime _calculateDateFrom, DateTime _validTo, int _billingPeriod, bool _isSubscription = false,
            bool _tmvAutoProlongation = false)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var salesOrderManager = new SalesOrderCRTManager();
            return salesOrderManager.CalculateTimeQty(_calculateDateFrom, _validTo, _billingPeriod, currentStore.StoreKey, _isSubscription, _tmvAutoProlongation);
        }

        public double CalculateLineAmount(DateTime _calculateDateFrom, DateTime _validTo, int _billingPeriod, int _salesQty, double _salesPrice, double _discAmount, double _discPct, bool _isSubscription = false,
            bool _tmvAutoProlongation = false)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var salesOrderManager = new SalesOrderCRTManager();
            return salesOrderManager.CalculateLineAmount(_calculateDateFrom, _validTo, _billingPeriod, _salesQty, _salesPrice, _discAmount, _discPct, currentStore.StoreKey, _isSubscription, _tmvAutoProlongation);
        }

        public ErpContractInvoicesResponse GetContractInvoices(ContractInvoicesRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var salesOrderManager = new SalesOrderCRTManager();
            return salesOrderManager.GetContractInvoices(request, currentStore.StoreKey);
        }

        public ErpAddPaymentLinkForInvoiceResponse AddPaymentLinkForInvoice(ErpAddPaymentLinkForInvoiceRequest request, string requestId)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, customLogger, MethodBase.GetCurrentMethod().Name);

            //Generating Blob for new payment
            if (!string.IsNullOrEmpty(request.TenderLine.MaskedCardNumber) ||
                request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.SEPA.ToString()) ||
                request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.ADYEN_HPP.ToString())
               )
            {
                SalesOrderHelper soHelper = new SalesOrderHelper(currentStore.StoreKey);
                soHelper.SetupPaymentMethod(request.TenderLine, request.SalesOrder, requestId);

                var salesOrderManager = new SalesOrderCRTManager();
                return salesOrderManager.AddPaymentLinkForInvoice(request, currentStore.StoreKey, requestId);
            }
            else
            {
                return new ErpAddPaymentLinkForInvoiceResponse(false, "MaskedCardNumber is Missing in tenderLine object", null);
            }
        }

        public ErpAddPaymentLinkForInvoiceResponse AddPaymentLinkForInvoiceBoleto(ErpAddPaymentLinkForInvoiceBoletoRequest request, string requestId)
        {
            if (request.Payment.ProcessorId.ToUpper().Equals(PaymentCon.BOLETO.ToString()))
            {
                request.Payment.BoletoXml = ConvertBoletoToXmlString(request.Payment.Boleto);

                var salesOrderManager = new SalesOrderCRTManager();
                return salesOrderManager.AddPaymentLinkForInvoiceBoleto(request, currentStore.StoreKey, requestId);
            }
            else
            {
                return new ErpAddPaymentLinkForInvoiceResponse(false, "", null);
            }
        }

        public ErpCreateLicenseResponse CreateProductLicense(List<ErpCreateActionLinkRequest> licenseRequests, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            foreach (var product in licenseRequests)
            {
                var key = integrationManager.GetErpKey(Entities.Product, product.ItemId);
                if (key == null)
                {
                    return new ErpCreateLicenseResponse(false, "Product Not Found " + product.ItemId, null);
                }
                product.ProductId = long.Parse(key.ErpKey);
                string itemId = key.Description.Split(':')[0];
                string variantId = key.Description.Split(':')[1];
                product.VariantId = variantId;
                product.ItemId = itemId;
            }
            var salesOrderManager = new SalesOrderCRTManager();
            return salesOrderManager.CreateProductLicense(licenseRequests, currentStore.StoreKey, requestId);
        }

        public ErpCloseExistingOrderResponse CloseExistingOrder(string salesId, string pacLicense, string disablePacLicenseOfSalesLines = "")
        {
            var salesOrderManager = new SalesOrderCRTManager();
            return salesOrderManager.CloseExistingOrder(salesId, pacLicense, currentStore.StoreKey, disablePacLicenseOfSalesLines);
        }

        public ErpChangeContractPaymentMethodResponse ChangeContractPaymentMethod(string salesId, long newPaymentMethodRecId, string tenderTypeId, long bankAccountRecId)
        {
            var salesOrderManager = new SalesOrderCRTManager();
            return salesOrderManager.ChangeContractPaymentMethod(salesId, newPaymentMethodRecId, tenderTypeId, bankAccountRecId, currentStore.StoreKey);
        }

        public ProcessContractOperationResponse ProcessContractOperation(ProcessContractOperationRequest request, List<ErpTenderLine> tenderLines, bool isCheckoutProcessContractOperation, string requestId)
        {
            var salesOrderManager = new SalesOrderCRTManager();
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            ProcessContractOperationResponse processContractOperationResponse = null;
            bool isUpdateRequest = false, isSwitchRequest = false, isMigrationRequest = false;
            string processContractOperationXMLRequest = string.Empty;
            string finalContractAction = string.Empty;

            // if switch/migrate/update request is already processed then  don't process it again.
            if (integrationManager.GetErpKey(Entities.SaleOrder, request.RequestNumber) != null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40528, currentStore, request.RequestNumber);
                return new ProcessContractOperationResponse(false, message, null);
            }

            if (!string.IsNullOrWhiteSpace(request.PrimaryPacLicense))
            {
                isUpdateRequest = true;
            }

            foreach (var line in request.ContractLines)
            {
                if (string.IsNullOrWhiteSpace(line.SalesLineAction))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40530, currentStore, request.RequestNumber);
                    return new ProcessContractOperationResponse(false, message, null);
                }
                else if (line.SalesLineAction.Trim().Equals(ErpTMVCrosssellType.Migration.ToString()))
                {
                    isMigrationRequest = true;
                }
                else if (line.SalesLineAction.Trim().Equals(ErpTMVCrosssellType.Switch.ToString()))
                {
                    isSwitchRequest = true;
                }
                else if (line.SalesLineAction.Trim().Equals(ErpTMVCrosssellType.Transferred.ToString()))
                {
                    isSwitchRequest = true;
                }
                else if ((line.SalesLineAction.Trim().Equals(ErpTMVCrosssellType.New.ToString()) || line.SalesLineAction.Trim().Equals(ErpTMVCrosssellType.QtyUpgrade.ToString())))
                {
                    if (!string.IsNullOrWhiteSpace(request.PrimaryPacLicense))
                    {
                        isUpdateRequest = true;
                    }
                }

                line.CustomerRef = line.CustomerRef ?? string.Empty;

                var key = integrationManager.GetErpKey(Entities.Product, line.ProductId);

                if (key == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL30203, currentStore, line.ProductId);
                    message += line.ProductId;
                    throw new CommerceLinkError(message);
                }
                var temp = key.Description.Split(':');

                if (temp != null && temp.Any())
                {
                    line.ItemId = temp[0];

                    if (temp.Length > 1)
                    {
                        line.VariantId = temp[1];
                    }
                }
            }

            if ((isMigrationRequest && (isSwitchRequest || isUpdateRequest))
                || (isSwitchRequest && (isMigrationRequest || isUpdateRequest))
                || (isUpdateRequest && (isMigrationRequest || isSwitchRequest)))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40529, currentStore, request.RequestNumber);
                return new ProcessContractOperationResponse(false, message, null);
            }

            if (isMigrationRequest)
            {
                request.ContractAction = ErpTMVCrosssellType.Migration.ToString();
            }
            else if (isSwitchRequest)
            {
                request.ContractAction = ErpTMVCrosssellType.Switch.ToString();
            }
            else
            {
                request.ContractAction = string.Empty;
            }

            finalContractAction = request.ContractAction;

            #region Process Tender Lines
            if (tenderLines != null && tenderLines.Count > 0)
            {
                request.TenderLine = new List<TenderLineInformation>();
                SalesOrderHelper salesOrderHelper = new SalesOrderHelper(currentStore.StoreKey);
                foreach (var tenderLine in tenderLines)
                {
                    bool isCreditCardPayment = true;
                    if (tenderLine.TenderTypeId == PaymentCon.PURCHASEORDER.ToString() ||
                        tenderLine.TenderTypeId == PaymentCon.SEPA.ToString() ||
                        tenderLine.TenderTypeId == PaymentCon.BOLETO.ToString() ||
                        tenderLine.TenderTypeId == PaymentCon.ADYEN_HPP.ToString()
                       )
                    {
                        isCreditCardPayment = false;
                    }

                    salesOrderHelper.SetupPaymentMethod(tenderLine, new ErpSalesOrder() { CurrencyCode = request.CurrencyCode, BillingAddress = request.CustomerInformation.BillingAddress }, requestId);

                    if (isCreditCardPayment)
                    {
                        if (!string.IsNullOrWhiteSpace(tenderLine.CardToken))
                        {
                            tenderLine.UniqueCardId = PaymentProperty.ConvertXMLToPropertyArray(tenderLine.CardToken).FirstOrDefault(x => x.Name == "UniqueCardId").StringValue;
                        }
                    }

                    request.TenderLine.Add(new TenderLineInformation()
                    {
                        CardOrAccount = tenderLine.CardOrAccount,
                        CardToken = tenderLine.CardToken,
                        AuthorizationToken = tenderLine.Authorization,
                        AuthorizationAmount = tenderLine.Amount,
                        BankName = tenderLine.BankName,
                        IBAN = tenderLine.IBAN,
                        SwiftCode = tenderLine.SwiftCode,
                        TenderTypeId = tenderLine.TenderTypeId,
                        UniqueCardId = tenderLine.UniqueCardId,
                        Boleto = tenderLine.Boleto,
                        ThreeDSecure = tenderLine.ThreeDSecure
                    });
                }
            }
            #endregion

            var xmlRequest = CommonUtility.ConvertToXmlString(request);

            if (string.IsNullOrEmpty(finalContractAction)) //Call create new contract lines method in existing sales order
            {
                processContractOperationResponse = salesOrderManager.CreateNewContractLines(xmlRequest, currentStore.StoreKey);
            }
            else //Call switch/migration contract method
            {
                if (isCheckoutProcessContractOperation)
                    processContractOperationResponse = salesOrderManager.CheckoutProcessContractOperation(xmlRequest, currentStore.StoreKey);
                else
                    processContractOperationResponse = salesOrderManager.ProcessContractOperation(xmlRequest, currentStore.StoreKey);
            }

            if (processContractOperationResponse.status)
            {
                integrationManager.CreateIntegrationKey(Entities.SaleOrder, processContractOperationResponse.result.ToString(), request.RequestNumber, processContractOperationResponse.message);
            }

            return processContractOperationResponse;
        }

        public PriceResponse GetOrValidatePriceInformation(PriceRequest priceValidateRequest, bool IsErpCustomer = false)
        {
            PriceResponse processPriceValidationResponse = new PriceResponse(false, string.Empty, null);
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

            if (!IsErpCustomer)
            {
                var customerIntegrationKey = integrationManager.GetErpKey(Entities.Customer, priceValidateRequest.CustomerAccount);

                if (customerIntegrationKey != null && !string.IsNullOrWhiteSpace(customerIntegrationKey.Description))
                {
                    priceValidateRequest.CustomerAccount = customerIntegrationKey.Description;
                }
                else
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40301, currentStore, priceValidateRequest.CustomerAccount);
                    return new PriceResponse(false, message, null);
                }
            }

            foreach (var contractLine in priceValidateRequest.ContractLines)
            {
                var integrationKey = integrationManager.GetErpKey(Entities.Product, contractLine.ProductId);
                if (integrationKey == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL401200, currentStore, contractLine.ProductId);
                    return new PriceResponse(false, message, null);
                }
                var erpProduct = integrationKey.Description.Split(':');

                if (erpProduct != null && erpProduct.Any())
                {
                    contractLine.ItemId = erpProduct[0];

                    if (erpProduct.Length > 1)
                    {
                        contractLine.VariantId = erpProduct[1];
                    }
                }
            }
            var salesOrderManager = new SalesOrderCRTManager();
            processPriceValidationResponse = salesOrderManager.GetOrValidatePriceInformation(priceValidateRequest, currentStore.StoreKey);
            return processPriceValidationResponse;
        }

        public ErpReactivateContract ReactivateContract(string pacLicenseList, string subscriptionStartDate)
        {
            var salesOrderManager = new SalesOrderCRTManager();
            return salesOrderManager.ReactivateContract(pacLicenseList, subscriptionStartDate, currentStore.StoreKey);
        }

        private string ConvertBoletoToXmlString(Boleto boleto)
        {
            //33415 -Start - Convert Customer due date format DD/MM/YYYY to YYYY-MM-DD
            if (!string.IsNullOrEmpty(boleto.CustomParameters.Custom_due_date))
            {
                if (DateTime.TryParseExact(boleto.CustomParameters.Custom_due_date,
                                            "d/M/yyyy",
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None,
                    out DateTime customerDueDate))
                {
                    boleto.CustomParameters.Custom_due_date = customerDueDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    throw new CommerceLinkError(string.Format("Invalid Boleto Custom due date {0}", boleto.CustomParameters.Custom_due_date));
                }
            }
            //33415 -End - Convert Customer due date format DD/MM/YYYY to YYYY-MM-DD

            var boletoXml = string.Empty;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Boleto));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, boleto);
                boletoXml = textWriter.ToString();
            }
            return boletoXml;
        }

        public ErpCancelIngramOrderResponse CancelIngramOrder(string prNumber, string salesId, DateTimeOffset orderDate)
        {
            var salesOrderCRTManager = new SalesOrderCRTManager();
            return salesOrderCRTManager.CancelIngramOrder(prNumber, salesId, orderDate, currentStore.StoreKey);
        }

        public ErpChangeIngramOrderResponse ChangeIngramOrder(string salesOrderXML)
        {
            var salesOrderCRTManager = new SalesOrderCRTManager();
            return salesOrderCRTManager.ChangeIngramOrder(salesOrderXML, currentStore.StoreKey);
        }

        public ErpCreatePaymentJournalResponse CreatePaymentJournal(ErpCreatePaymentJournalRequest request, string requestId)
        {
            var salesOrderManager = new SalesOrderCRTManager();
            return salesOrderManager.CreatePaymentJournal(request, currentStore.StoreKey, requestId);
        }

        public ErpTransferPartnerContractResponse TransferPartnerContract(TransferPartnerContractRequest request, string requestId)
        {
            var salesOrderManager = new SalesOrderCRTManager();
            return salesOrderManager.TransferPartnerContract(request, currentStore.StoreKey, requestId);
        }

        public ErpTransferIngramOrderResponse TransferIngramOrder(string salesOrderXML)
        {
            var salesOrderCRTManager = new SalesOrderCRTManager();
            return salesOrderCRTManager.TransferIngramOrder(salesOrderXML, currentStore.StoreKey);
        }

        public ErpContractRenewalResponse ContractRenewal(ContractRenewalRequest request, string requestId)
        {
            foreach (var salesLine in request.SalesLines)
            {
                var erpProduct = salesLine.SKU.Split('_');

                if (erpProduct.Length != 2)
                {
                    string message = $"Provided SKU {salesLine.SKU} is not valid";
                    return new ErpContractRenewalResponse(false, message, string.Empty);
                }

                salesLine.ItemId = erpProduct[0];
                salesLine.VariantId = erpProduct[1];
            }

            var salesOrderManager = new SalesOrderCRTManager();
            return salesOrderManager.ContractRenewal(request, currentStore.StoreKey, requestId);
        }

        public ErpGetBoletoUrlResponse GetBoletoUrl(ErpGetBoletoUrlRequest request, string requestId)
        {
            var salesOrderManager = new SalesOrderCRTManager();
            return salesOrderManager.GetBoletoUrl(request, currentStore.StoreKey, requestId);
        }
        public ErpUpdateCustomerPortalLinkResponse UpdateCustomerPortalLink(UpdateCustomerPortalLinkRequest updateCustomerPortalLinkRequest, string storeKey)
        {
            var salesOrderManager = new SalesOrderCRTManager();            
            return salesOrderManager.UpdateCustomerPortalLink(updateCustomerPortalLinkRequest, storeKey);
        }
        #endregion

    }
}

