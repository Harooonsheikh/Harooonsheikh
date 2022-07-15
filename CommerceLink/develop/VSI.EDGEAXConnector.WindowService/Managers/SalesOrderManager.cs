using MapsterMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.Enums.Enums.TMV;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.WindowService.Managers;

namespace VSI.EDGEAXConnector.WindowService
{
    public class SalesOrderManager : IJobManager
    {
        public static readonly string IDENTIFIER = "SalesOrderSync";
        public static readonly string GROUP = "Synchronization";
        private readonly IEComAdapterFactory _eComAdapterFactory;
        private readonly IErpAdapterFactory _erpAdapterFactory;
        StoreDto _store;
        ConfigurationHelper _configurationHelper;
        EmailSender _emailSender;
        FileHelper _fileHelper;
        IMapper _mapper;

        bool _isLoadSalesOrderFromDb;
        int _salesOrderProcessingThreadCount;

        public SalesOrderManager(IErpAdapterFactory erpAdapterFactory, IEComAdapterFactory eComAdapterFactory)
        {
            _erpAdapterFactory = new ErpAdapterFactory();
            _eComAdapterFactory = new EComAdapterFactory();
            _mapper = AutoMapBootstrapper.MapsterInstance;
        }

        public SalesOrderManager()
        {
            _erpAdapterFactory = new ErpAdapterFactory();
            _eComAdapterFactory = new EComAdapterFactory();
            _mapper = AutoMapBootstrapper.MapsterInstance;
        }


        // Custom For MF
        private bool CheckOrderIsMFICoreToProcess(ErpSalesOrder erpSalesOrder)
        {
            if (!string.IsNullOrEmpty(erpSalesOrder.CustomAttributes.FirstOrDefault(k => k.Key == "isMFICore").Value) && Convert.ToBoolean(erpSalesOrder.CustomAttributes.FirstOrDefault(k => k.Key == "isMFICore").Value))
            {
                return Convert.ToBoolean(erpSalesOrder.CustomAttributes.FirstOrDefault(k => k.Key == "isMFICore").Value);
            }
            else
            {
                return false;
            }
        }

        public bool SyncOrderStatus()
        {
            try
            {
                CustomLogger.LogDebugInfo("Syn Order Status Started", _store.StoreId, _store.CreatedBy);
                var erpSalesOrderController = _erpAdapterFactory.CreateSalesOrderController(_store.StoreKey);
                CustomLogger.LogDebugInfo("erpSalesOrderController Instance Created", _store.StoreId, _store.CreatedBy);
                bool useCRT = !(System.Configuration.ConfigurationManager.AppSettings["IsLoadSalesOrderStatusUsingCRT"] != null && System.Configuration.ConfigurationManager.AppSettings["IsLoadSalesOrderStatusUsingCRT"].ToString() == "0");
                CustomLogger.LogDebugInfo(string.Format("Value of useCRT is {0}", useCRT.ToString()), _store.StoreId, _store.CreatedBy);
                var orderUpdates = erpSalesOrderController.GetSalesOrderStatusUpdate(useCRT);
                CustomLogger.LogDebugInfo("Successfully return form function erpSalesOrderController.GetSalesOrderStatusUpdate(useCRT).", _store.StoreId, _store.CreatedBy);
                var ecomUpdates = (orderUpdates.Where(o => o.Notify == true).ToList());
                CustomLogger.LogDebugInfo(ecomUpdates, _store.StoreId, _store.CreatedBy, "ecomUpdates to Json");
                using (var eComSalesOrderController = _eComAdapterFactory.CreateSalesOrderStatusController(_store.StoreKey))
                {
                    CustomLogger.LogDebugInfo("eComSalesOrderController Instance Created", _store.StoreId, _store.CreatedBy);
                    var updatesFromCom = eComSalesOrderController.UpdateOrderStatus(ecomUpdates);
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, _store.StoreId, _store.CreatedBy);
                return false;
            }
        }

        private string GetOrderNumber(string file)
        {
            string orderId = string.Empty;
            if (!string.IsNullOrEmpty(file))
            {
                orderId = string.Format("Order No {0} ", Path.GetFileNameWithoutExtension(file));
            }
            return orderId;
        }
        #region Test Code
        public void SearchSalesOrder(string orderNo)
        {
            try
            {
                var erpController = _erpAdapterFactory.CreateSalesOrderController(_store.StoreKey);
                var salesOrders = erpController.SearchSalesOrder(orderNo);
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, _store.StoreId, _store.CreatedBy);

            }
        }

        public bool Sync()
        {
            try
            {
                CustomLogger.LogDebugInfo("Sync Sales Order Started", _store.StoreId, _store.CreatedBy);

                _isLoadSalesOrderFromDb = bool.Parse(_configurationHelper.GetSetting(SALESORDER.Is_Load_Sales_Order_From_DB));
                _salesOrderProcessingThreadCount = CommonUtility.StringToInt(_configurationHelper.GetSetting(SALESORDER.Sales_Order_Processing_Thread_Count));
                var messageDal = new ThirdPartyMessageDAL(_store.StoreKey);

                if (_isLoadSalesOrderFromDb) // for Ingram
                {
                    CustomLogger.LogDebugInfo("thirdPartySalesOrderController Instance Created", _store.StoreId, _store.CreatedBy);

                    var thirdPartyOrdersList = GetOrderToProcessInErp();

                    foreach (var thirdPartyOrder in thirdPartyOrdersList)
                    {
                        CustomLogger.LogDebugInfo(string.Format("Sales Order {0} retrieved for processing from DB", thirdPartyOrder.ThirdPartyId), _store.StoreId, _store.CreatedBy, thirdPartyOrder.ThirdPartyId);
                        try
                        {
                            var orderTargetStore = thirdPartyOrder.DestinationStoreKey;
                            if (orderTargetStore == null)
                            {
                                // Ingram order must have destination store key for placing order
                                Exception missingTargetStoreKeyException = new Exception(string.Format("Destination store key not found for ingram order {0}", thirdPartyOrder.ThirdPartyId));
                                CustomLogger.LogException(missingTargetStoreKeyException, _store.StoreId, _store.CreatedBy, "Ingram Sales Order Processing", thirdPartyOrder.ThirdPartyId);
                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL,
                                            thirdPartyOrder.Content, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                            string.Format("Destination store key not found for ingram order {0}", thirdPartyOrder.ThirdPartyId), thirdPartyOrder.ThirdPartyId, string.Empty,
                                            string.Empty, DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);
                                continue;
                            }

                            CustomLogger.LogDebugInfo(string.Format("Target store for Sales Order {0} is {1}", thirdPartyOrder.ThirdPartyId, orderTargetStore), _store.StoreId, _store.CreatedBy, thirdPartyOrder.ThirdPartyId);

                            CustomLogger.LogDebugInfo("erpSalesOrderController Instance Created", _store.StoreId, _store.CreatedBy);
                            var erpSalesOrderController = _erpAdapterFactory.CreateSalesOrderController(orderTargetStore);
                            var eComSalesOrderController = _eComAdapterFactory.CreateSalesOrderController(orderTargetStore);

                            ErpSalesOrder erpSalesOrder = eComSalesOrderController.GetSalesOrders(thirdPartyOrder.Content);
                            IntegrationManager integrationManager = new IntegrationManager(orderTargetStore);

                            // if order already processed, then dont process it again.
                            if (integrationManager.GetErpKey(Entities.SaleOrder, thirdPartyOrder.ThirdPartyId) != null)
                            {
                                messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.SaleOrderAlreadyProcessed, ApplicationConstant.UserName);
                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL,
                                            thirdPartyOrder.Content, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                            string.Format("Ingram order already processed {0}", thirdPartyOrder.ThirdPartyId), thirdPartyOrder.ThirdPartyId, string.Empty,
                                            string.Empty, DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);
                                continue;
                            }

                            // AQ-Begin: As per discussion with Baidar, default currency code for ingram order will be Channel Currency Code of Target Store
                            if (string.IsNullOrEmpty(erpSalesOrder.CurrencyCode))
                            {
                                erpSalesOrder.CurrencyCode = thirdPartyOrder.Currency;
                            }
                            // AQ-End

                            if (thirdPartyOrder.OrderType.ToUpper().Equals(ApplicationConstant.IngramOrderTypeCancel, StringComparison.OrdinalIgnoreCase)) // Cancel Request
                            {
                                messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.IngramCancelRequest_PreparingSyncToErp, ApplicationConstant.UserName);

                                // code to cancel/terminate conttract comes here
                                ThirdPartyMessage lastThirdPartyMessage = SetupIngramCancelOrderRequest(erpSalesOrder, orderTargetStore);
                                string prNumber = lastThirdPartyMessage.ThirdPartyId;
                                string salesId = lastThirdPartyMessage.SalesId;

                                DateTimeOffset currentPrDateTime = erpSalesOrder.OrderPlacedDate;

                                // send the cancellation/termination request
                                ErpCancelIngramOrderResponse result = erpSalesOrderController.CancelIngramOrder(prNumber, salesId, currentPrDateTime);

                                string salesIdResult = null;
                                if (!string.IsNullOrEmpty(result.Result))
                                {
                                    salesIdResult = result.Result;
                                }

                                if (result.Success)
                                {
                                    messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.IngramCancelRequest_OrderCanceledInErp, ApplicationConstant.UserName, salesIdResult);
                                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL,
                                            thirdPartyOrder.Content, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                            string.Format("Order {0} canceled/terminated", thirdPartyOrder.ThirdPartyId), thirdPartyOrder.ThirdPartyId, string.Empty,
                                            JsonConvert.SerializeObject(result), DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 1, 0);
                                }
                                else
                                {
                                    if (result.Code == IngramCancelTerminationCodes.TerminationDateExpired)
                                    {
                                        messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.IngramCancelRequest_SyncToErpFailedDueToExpiredTerminationDate, ApplicationConstant.UserName, salesIdResult);
                                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL,
                                                thirdPartyOrder.Content, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                                string.Format("Order {0} not canceled/terminated due to termination date expired", thirdPartyOrder.ThirdPartyId), thirdPartyOrder.ThirdPartyId, string.Empty,
                                                JsonConvert.SerializeObject(result), DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);
                                    }
                                    else // Result code is Other
                                    {
                                        messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.IngramCancelRequest_SyncToErpFailed, ApplicationConstant.UserName, salesIdResult);
                                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL,
                                                thirdPartyOrder.Content, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                                string.Format("Order {0} not canceled/terminated with exception {1}", thirdPartyOrder.ThirdPartyId, result.Message), thirdPartyOrder.ThirdPartyId, string.Empty,
                                                JsonConvert.SerializeObject(result), DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);
                                    }
                                }

                                continue;
                            }
                            else if (thirdPartyOrder.OrderType.ToUpper().Equals(ApplicationConstant.IngramOrderTypeChange, StringComparison.OrdinalIgnoreCase)) // Change Request
                            {
                                messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.PreparingToSendToErp, ApplicationConstant.UserName);

                                // Populate sales lines
                                erpSalesOrder = SetupIngramChangeOrderRequest(erpSalesOrder, orderTargetStore);

                                // Check if we have valid
                                if (erpSalesOrder.SalesLines.Count == 0)
                                {
                                    string exceptionMessage = string.Format("No valid sales lines found in order {0}", thirdPartyOrder.ThirdPartyId);
                                    Exception exception = new Exception(exceptionMessage);
                                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL,
                                            thirdPartyOrder.Content, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                            exceptionMessage, thirdPartyOrder.ThirdPartyId, string.Empty,
                                            exceptionMessage, DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);
                                    throw exception;
                                }

                                IngramSalesOrder salesOrder = _mapper.Map<IngramSalesOrder>(erpSalesOrder);

                                string ingramChangeOrderXmlRequest;

                                XmlSerializer xmlSerializer = new XmlSerializer(typeof(IngramSalesOrder));

                                using (StringWriter textWriter = new StringWriter())
                                {
                                    xmlSerializer.Serialize(textWriter, salesOrder);
                                    ingramChangeOrderXmlRequest = textWriter.ToString();
                                }

                                // Send the change/update request
                                ErpChangeIngramOrderResponse result = erpSalesOrderController.ChangeIngramOrder(ingramChangeOrderXmlRequest);

                                string salesId = null;
                                if (!string.IsNullOrEmpty(result.Result))
                                {
                                    salesId = result.Result;
                                }

                                if (result.Success)
                                {
                                    messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.CreatedInERP, ApplicationConstant.UserName, salesId);
                                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL,
                                           ingramChangeOrderXmlRequest, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                           string.Format("Order {0} synced", thirdPartyOrder.ThirdPartyId), thirdPartyOrder.ThirdPartyId, string.Empty,
                                           JsonConvert.SerializeObject(result), DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 1, 0);
                                }
                                else
                                {
                                    messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.ErrorInSalesOrderSycnhing, ApplicationConstant.UserName, salesId);
                                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL,
                                           ingramChangeOrderXmlRequest, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                           string.Format("Order {0} failed to sync", thirdPartyOrder.ThirdPartyId), thirdPartyOrder.ThirdPartyId, string.Empty,
                                           JsonConvert.SerializeObject(result), DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);
                                }

                                continue;
                            }
                            else if (thirdPartyOrder.OrderType.ToUpper().Equals(ApplicationConstant.IngramOrderTypeTransfer, StringComparison.OrdinalIgnoreCase)) // VSTS 33374: Ingram Transfer Code
                            {
                                messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.PreparingToSendToErp, ApplicationConstant.UserName);

                                LogRequestResponseIngramSuccess(erpSalesOrder, null,
                                    string.Format("Starting setup for transfer for sales order id {0}", thirdPartyOrder.ThirdPartyId),
                                    "PR/channelReferenceID", thirdPartyOrder.ThirdPartyId);

                                // Remove products with 0 quantity // Ingram orders in test mode provides the list of all products. Only required products have quantity greater then 0
                                erpSalesOrder = RemoveProductsWithZeroQuantity(erpSalesOrder);

                                if (erpSalesOrder.SalesLines.Count() == 0)
                                {
                                    string exceptionMessage = string.Format("No valid sales lines found in order {0}", thirdPartyOrder.ThirdPartyId);
                                    Exception exception = new Exception(exceptionMessage);
                                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL,
                                            thirdPartyOrder.Content, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                            exceptionMessage, thirdPartyOrder.ThirdPartyId, string.Empty,
                                            exceptionMessage, DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);
                                    LogRequestResponseIngramFailure(erpSalesOrder, null,
                                        string.Format("No valid sales lines found in order {0}", thirdPartyOrder.ThirdPartyId),
                                        "PR/channelReferenceID", thirdPartyOrder.ThirdPartyId);
                                    throw exception;
                                }

                                // Trim the ItemId as they are containing extra space when configured in Ingram
                                erpSalesOrder = TrimIngramSalesOrderItemIds(erpSalesOrder);

                                // Extract and setup Products and Variants
                                foreach (ErpSalesLine sl in erpSalesOrder.SalesLines)
                                {
                                    sl.ItemId = sl.Description.Substring(0, sl.Description.IndexOf('_'));
                                    sl.VariantId = sl.Description.Substring(sl.Description.IndexOf('_') + 1);
                                }

                                // Update the AddressType and Primary Property of Reseller and Customer Address for Ingram order
                                var salesOrigin = erpSalesOrder.CustomAttributes.FirstOrDefault(a => a.Key.Equals("TMVSALESORIGIN"));
                                
                                var endCustomerAdminEmail = string.Empty;
                                if (erpSalesOrder.Parameters.FirstOrDefault(p => p.Name.Equals(ApplicationConstant.IngramEndcustomerAdminEmail)) != null)
                                {
                                    endCustomerAdminEmail = erpSalesOrder.Parameters.FirstOrDefault(p => p.Name.Equals(ApplicationConstant.IngramEndcustomerAdminEmail)).Value;

                                    if (string.IsNullOrWhiteSpace(endCustomerAdminEmail))
                                    {
                                        messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.MissingParameter_EndCustomerAdminEmail, ApplicationConstant.UserName);
                                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.CLRequestToThirdParty, string.Empty, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName, string.Format("Missing parameter MissingParameter_EndCustomerAdminEmail in request {0}", thirdPartyOrder.ThirdPartyId), thirdPartyOrder.ThirdPartyId, string.Empty, string.Empty, DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);
                                        continue;
                                    }
                                }

                                // 1. Create Reseller and 2. Create Indirect Customer
                                LogRequestResponseIngramSuccess(erpSalesOrder, null,
                                    string.Format("Setup completed and going to create Customer and Reseller for sales order id {0}", thirdPartyOrder.ThirdPartyId),
                                    "PR/channelReferenceID", thirdPartyOrder.ThirdPartyId);

                                if (!CreateCustomerOrResellerWithContactPerson(erpSalesOrder))
                                {
                                    messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.CustomerOrResellerCreationFailed, ApplicationConstant.UserName);
                                    CustomLogger.LogFatal(string.Format("Customer Or Reseller Creation failed, Sale order id {0}", thirdPartyOrder.ThirdPartyId), _store.StoreId, _store.CreatedBy, thirdPartyOrder.ThirdPartyId);
                                    LogRequestResponseIngramFailure(JsonConvert.SerializeObject(erpSalesOrder), null,
                                        string.Format("Customer Or Reseller Creation failed, Sale order id {0}", thirdPartyOrder.ThirdPartyId),
                                        "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId);
                                    continue;
                                }

                                LogRequestResponseIngramSuccess(erpSalesOrder, null,
                                    string.Format("Customer and Reseller created for sales order id {0}", thirdPartyOrder.ThirdPartyId),
                                    "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId);

                                // 3. Get Distributor details
                                string distributorComKey = $"{erpSalesOrder.IngramAssetType}:{erpSalesOrder.IngramMarketPlaceId}:{erpSalesOrder.IngramContractId}";
                                IntegrationKey distributorKey = integrationManager.GetErpKey(Entities.Customer, distributorComKey);
                                string distributorAccountNumber = distributorKey.Description;

                                // 4. Create XML for RTS Method
                                IngramTransferOrder salesOrder = _mapper.Map<IngramTransferOrder>(erpSalesOrder);

                                string pacLicense = string.Empty;
                                if (erpSalesOrder.Parameters.FirstOrDefault(p => p.Name.Equals(ApplicationConstant.IngramPacLicenseParameterName)) != null)
                                {
                                    pacLicense = erpSalesOrder.Parameters.FirstOrDefault(p => p.Name.Equals(ApplicationConstant.IngramPacLicenseParameterName)).Value.ToString();
                                }
                                salesOrder.PACLicense = pacLicense;
                                salesOrder.Distributor = distributorAccountNumber;
                                salesOrder.Reseller = erpSalesOrder.Reseller.AccountNumber;
                                salesOrder.IndirectCustomer = erpSalesOrder.Customer.AccountNumber;
                                salesOrder.ActivationLinkEmail = endCustomerAdminEmail;
                                salesOrder.SalesOrigin = salesOrigin.Value;

                                string ingramTransferOrderXmlRequest = CommonUtility.ConvertToXmlString(salesOrder);

                                LogRequestResponseIngramSuccess(ingramTransferOrderXmlRequest, null,
                                    string.Format("Sending transfer requset to D365, Sale order id {0}", thirdPartyOrder.ThirdPartyId),
                                    "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId);

                                // 4. Send call to D365
                                ErpTransferIngramOrderResponse response = erpSalesOrderController.TransferIngramOrder(ingramTransferOrderXmlRequest);

                                if (response.Success)
                                {
                                    messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.TransferIngramRequest_OrderTransfered, ApplicationConstant.UserName, response.Result, response.RenewalDate.Value.UtcDateTime, response.Message);

                                    LogRequestResponseIngramSuccess(ingramTransferOrderXmlRequest, response,
                                    string.Format("Order {0} synced", thirdPartyOrder.ThirdPartyId),
                                    "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId);
                                }
                                else
                                {
                                    var transactionStatus = TransactionStatus.TransferIngramRequest_None;

                                    switch (response.Code)
                                    {
                                        case IngramTransferCodes.ValidationFailed:
                                            transactionStatus = TransactionStatus.TransferIngramRequest_ValidationFailed;
                                            break;
                                        case IngramTransferCodes.Other:
                                            transactionStatus = TransactionStatus.TransferIngramRequest_Other;
                                            break;
                                        case IngramTransferCodes.None:
                                            transactionStatus = TransactionStatus.TransferIngramRequest_None;
                                            break;
                                    }

                                    messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, transactionStatus, ApplicationConstant.UserName, response.Result, response.RenewalDate.Value.UtcDateTime, response.Message);

                                    LogRequestResponseIngramFailure(ingramTransferOrderXmlRequest, response,
                                        string.Format("Order {0} failed to sync", thirdPartyOrder.ThirdPartyId),
                                        "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId);
                                }

                                continue;

                            }
                            else if (thirdPartyOrder.OrderType.Equals(ApplicationConstant.IngramOrderTypePurchase, StringComparison.OrdinalIgnoreCase)) // else ThirdPartyType is purchase, so create order
                            {
                                messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.PreparingToSendToErp, ApplicationConstant.UserName);

                                if (erpSalesOrder != null)
                                {
                                    CustomLogger.LogDebugInfo($"Sales Order {thirdPartyOrder.ThirdPartyId} converted to ErpSalesOrder object. Object json is: {JsonConvert.SerializeObject(erpSalesOrder)}", _store.StoreId, _store.CreatedBy, thirdPartyOrder.ThirdPartyId);

                                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL,
                                                thirdPartyOrder.Content, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                                string.Format("Sales Order {0} converted to ErpSalesOrder object. Object json is: {1}", thirdPartyOrder.ThirdPartyId, JsonConvert.SerializeObject(erpSalesOrder)), thirdPartyOrder.ThirdPartyId, string.Empty,
                                                string.Empty, DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 1, 0);

                                    // Remove products with 0 quantity // Ingram orders in test mode provides the list of all products. Only required products have quantity greater then 0
                                    erpSalesOrder = RemoveProductsWithZeroQuantity(erpSalesOrder);

                                    // Trim the ItemId as they are containing extra space when configured in Ingram
                                    erpSalesOrder = TrimIngramSalesOrderItemIds(erpSalesOrder);

                                    // Set the SEPA custom properties if applicable
                                    AddPaymentCustomProperties(erpSalesOrder);

                                    if (!CreateCustomerOrResellerWithContactPerson(erpSalesOrder))
                                    {
                                        messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.CustomerOrResellerCreationFailed, ApplicationConstant.UserName);
                                        CustomLogger.LogFatal($"Customer Or Reseller Creation failed, Sale order id {thirdPartyOrder.ThirdPartyId}", _store.StoreId, _store.CreatedBy, thirdPartyOrder.ThirdPartyId);
                                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                            DataDirectionType.ThirdPartyResponseToCL, // Data direction
                                            JsonConvert.SerializeObject(erpSalesOrder), // Data packed
                                            DateTime.UtcNow, // Created on
                                            _store.StoreId,
                                            ApplicationConstant.UserName, // Created by
                                            $"Customer Or Reseller Creation failed, Sale order id {thirdPartyOrder.ThirdPartyId}", // Description
                                            thirdPartyOrder.ThirdPartyId, // eCom Transaction Id
                                            string.Empty, // Request Initialed IP
                                            string.Empty, // Output packed
                                            DateTime.UtcNow, // Output sent at
                                            "PR/ChannelReferenceID", // Identifier Key
                                            thirdPartyOrder.ThirdPartyId, // Identifier Value
                                            0, // Success
                                            0 // TotalProcessingDuration
                                            );
                                        continue;
                                    }

                                    CustomLogger.LogDebugInfo($"CreateCustomerOrResellerWithContactPerson created for Sales Order {thirdPartyOrder.ThirdPartyId}", _store.StoreId, _store.CreatedBy, thirdPartyOrder.ThirdPartyId);

                                    // AQ-Begin: Extract customer from integration key. Baider will provide the data that will be inserted in Integration Key table
                                    string distributor = $"{erpSalesOrder.IngramAssetType}:{erpSalesOrder.IngramMarketPlaceId}:{erpSalesOrder.IngramContractId}";

                                    // Here we set Customer/Distributor Property
                                    erpSalesOrder.CustomerId = distributor;
                                    var distributionIntegrationKey = integrationManager.GetErpKey(Entities.Customer, distributor);
                                    if (distributionIntegrationKey == null)
                                    {
                                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.CLRequestToThirdParty, string.Empty, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName, string.Format("Distributor {0} not found in Integration DB", distributor), thirdPartyOrder.ThirdPartyId, string.Empty, string.Empty, DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);
                                        messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.DistributerNotFound, ApplicationConstant.UserName);
                                        continue;
                                    }

                                    TransactionStatus transactionStatus;
                                    if (!SetSalesOrderCustomAttributes(erpSalesOrder, distributionIntegrationKey.Description, out transactionStatus))
                                    {
                                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.CLRequestToThirdParty, string.Empty, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName, string.Format("Missing parameter MissingParameter_EndCustomerAdminEmail in request {0}", thirdPartyOrder.ThirdPartyId), thirdPartyOrder.ThirdPartyId, string.Empty, string.Empty, DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);
                                        messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, transactionStatus, ApplicationConstant.UserName);
                                        continue;
                                    }

                                    if (erpSalesOrder.SalesLines != null && erpSalesOrder.SalesLines.Count > 0)
                                    {
                                        PriceRequest priceValidateRequest = new PriceRequest
                                        {
                                            IsValidateRequest = false,
                                            Currency = erpSalesOrder.CurrencyCode,
                                            CustomerAccount = distributor
                                        };

                                        if (erpSalesOrder.Reseller != null && !string.IsNullOrEmpty(erpSalesOrder.Reseller.AccountNumber))
                                        {
                                            priceValidateRequest.ResellerAccount = erpSalesOrder.Reseller.AccountNumber;
                                        }

                                        priceValidateRequest.IndirectCustomerAccount = erpSalesOrder.Customer.AccountNumber;
                                        priceValidateRequest.RequestDate = erpSalesOrder.OrderPlacedDate.ToString("yyyy-MM-dd");

                                        foreach (var salesLine in erpSalesOrder.SalesLines)
                                        {
                                            priceValidateRequest.ContractLines.Add(new PriceContractLine()
                                            {
                                                LineNumber = salesLine.LineNumber,
                                                ProductId = salesLine.ItemId.Trim(),
                                                Quantity = salesLine.Quantity,
                                                TargetPrice = salesLine.BasePrice,
                                            });
                                        }

                                        PriceResponse response;

                                        try
                                        {
                                            response = erpSalesOrderController.GetOrValidatePriceInformation(priceValidateRequest);
                                            CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL,
                                                JsonConvert.SerializeObject(priceValidateRequest), DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                                string.Empty, thirdPartyOrder.ThirdPartyId, string.Empty,
                                                JsonConvert.SerializeObject(response), DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 1, 0);
                                        }
                                        catch (Exception ex)
                                        {
                                            CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL,
                                                JsonConvert.SerializeObject(priceValidateRequest), DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                                string.Empty, thirdPartyOrder.ThirdPartyId, string.Empty,
                                                JsonConvert.SerializeObject(ex), DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);

                                            messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.ErrorInSalesOrderSycnhing, ApplicationConstant.UserName);
                                            continue;
                                        }

                                        if (response.Status)
                                        {
                                            if (response.Result != null)
                                            {
                                                // Populate the header data
                                                erpSalesOrder.NetAmountWithNoTax = response.Result.NetAmountWithNoTax;
                                                erpSalesOrder.NetAmountWithTax = response.Result.NetAmountWithTax;
                                                erpSalesOrder.TaxAmount = response.Result.TaxAmount;
                                                erpSalesOrder.TotalAmount = response.Result.TotalAmount;

                                                erpSalesOrder.TenderLines[0].Amount = response.Result.TotalAmount;

                                                // Populate details
                                                if (response.Result.SaleLines?.Items != null)
                                                {
                                                    foreach (ErpSalesLine erpSalesLine in erpSalesOrder.SalesLines)
                                                    {
                                                        ItemInformation productInformation = response.Result.SaleLines.Items.FirstOrDefault(sli => sli.ItemId.ToString() + "_" + sli.VariantId.ToString() == erpSalesLine.Description);

                                                        if (productInformation != null)
                                                        {
                                                            erpSalesLine.BasePrice = productInformation.BasePrice;
                                                            erpSalesLine.Price = productInformation.BasePrice;
                                                            erpSalesLine.NetAmount = productInformation.NetAmount;
                                                            erpSalesLine.TaxAmount = productInformation.TaxAmount;
                                                            erpSalesLine.TaxRatePercent = productInformation.TaxRatePercent;
                                                            erpSalesLine.TotalAmount = productInformation.TotalAmount;
                                                            erpSalesLine.UnitOfMeasureSymbol = productInformation.UnitOfMeasureSymbol;
                                                            erpSalesLine.SalesOrderUnitOfMeasure = productInformation.UnitOfMeasureSymbol;
                                                            erpSalesLine.VariantId = productInformation.VariantId;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Response was false
                                            Exception ex = new Exception(string.Format("Product prices not fetched for ingram order {0}", erpSalesOrder.ChannelReferenceId));
                                            CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL, JsonConvert.SerializeObject(priceValidateRequest), DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName, string.Empty, thirdPartyOrder.ThirdPartyId, string.Empty, JsonConvert.SerializeObject(ex), DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 1, 0);
                                            messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.ProductPricesCallFailed, ApplicationConstant.UserName);
                                            continue;
                                        }
                                    }
                                    // AQ-End 

                                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.CLRequestToThirdParty, string.Empty, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName, string.Format("Sales order {0} to be created in D365: {1}", thirdPartyOrder.ThirdPartyId, JsonConvert.SerializeObject(erpSalesOrder)), thirdPartyOrder.ThirdPartyId, string.Empty, string.Empty, DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 1, 0);

                                    if (!erpSalesOrderController.CreateSaleOrders(erpSalesOrder, string.Empty))
                                    {
                                        messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.SalesOrderCreationFailed, ApplicationConstant.UserName);
                                        CustomLogger.LogFatal($"Sale order Creation failed or sale order already exists, Sale order id {thirdPartyOrder.ThirdPartyId}", _store.StoreId, _store.CreatedBy);
                                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.CLRequestToThirdParty, string.Empty, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                            $"Sale order Creation failed or sale order already exists, Sale order id {thirdPartyOrder.ThirdPartyId}",
                                            thirdPartyOrder.ThirdPartyId, string.Empty, string.Empty, DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);
                                        continue;
                                    }

                                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.CLRequestToThirdParty, string.Empty, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                            $"Sale Order {thirdPartyOrder.ThirdPartyId} created in D365",
                                            thirdPartyOrder.ThirdPartyId, string.Empty, string.Empty, DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 1, 0);

                                    messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.CreatedInERP, ApplicationConstant.UserName);
                                }
                            }
                            else
                            {
                                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.CLRequestToThirdParty, string.Empty, DateTime.UtcNow, _store.StoreId, ApplicationConstant.UserName,
                                        $"Unable to read order from XML, order No: {thirdPartyOrder.ThirdPartyId}",
                                        thirdPartyOrder.ThirdPartyId, string.Empty, string.Empty, DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);
                            }
                        }
                        catch (Exception ex)
                        {
                            CustomLogger.LogException(ex, _store.StoreId, _store.CreatedBy, thirdPartyOrder.ThirdPartyId, thirdPartyOrder.ThirdPartyId);

                            CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name, DataDirectionType.ThirdPartyResponseToCL, JsonConvert.SerializeObject(thirdPartyOrder), DateTime.UtcNow, _store.StoreId, "Sales Order Manager", "Fatal Exception", thirdPartyOrder.ThirdPartyId, string.Empty, JsonConvert.SerializeObject(ex), DateTime.UtcNow, "PR/ChannelReferenceID", thirdPartyOrder.ThirdPartyId, 0, 0);

                            messageDal.UpdateTransactionStatus(thirdPartyOrder.ThirdPartyId, TransactionStatus.ErrorInSalesOrderSycnhing, ApplicationConstant.UserName);

                            continue;
                        }
                    }
                }
                else
                {
                    Exception ex = new Exception("SALESORDER.Is_Load_Sales_Order_From_DB is not set for Ingram Store");
                    CustomLogger.LogException(ex, _store.StoreId, _store.CreatedBy);
                    throw ex;
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, _store.StoreId, _store.CreatedBy);

                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                    DataDirectionType.ThirdPartyResponseToCL, // Data direction
                    string.Empty, // Data packed
                    DateTime.UtcNow, // Created on
                    _store.StoreId,
                    ApplicationConstant.UserName, // Created by
                    "Sales Order Sync", // Description
                    string.Empty, // eCom Transaction Id
                    string.Empty, // Request Initialed IP
                    JsonConvert.SerializeObject(ex), // Output packed
                    DateTime.UtcNow, // Output sent at
                    string.Empty, // Identifier Key
                    string.Empty, // Identifier Value
                    0, // Success
                    0 // TotalProcessingDuration
                    );

                _emailSender.NotifyThroughEmail(string.Empty, ex.ToString(), string.Empty, (int)Common.Enums.EmailTemplateId.SaleOrder);

                throw;
            }
        }

        private List<ThirdPartyMessage> GetOrderToProcessInErp()
        {
            var messageDal = new ThirdPartyMessageDAL(_store.StoreKey);
            List<ThirdPartyMessage> thirdPartyOrdersList = new List<ThirdPartyMessage>();

            thirdPartyOrdersList.AddRange(messageDal.GetSalesOrdersList(TransactionStatus.Created, _salesOrderProcessingThreadCount));
            thirdPartyOrdersList.AddRange(messageDal.GetSalesOrdersList(TransactionStatus.ErrorInSalesOrderSycnhing, _salesOrderProcessingThreadCount));
            thirdPartyOrdersList.AddRange(messageDal.GetSalesOrdersList(TransactionStatus.TransferIngramRequest_Other, _salesOrderProcessingThreadCount));
            thirdPartyOrdersList.AddRange(messageDal.GetSalesOrdersList(TransactionStatus.TransferIngramRequest_None, _salesOrderProcessingThreadCount));

            return thirdPartyOrdersList;
        }

        /// <summary>
        /// Setup Ingram order for cancellation or termination
        /// </summary>
        /// <param name="ingramOrder"></param>
        /// <param name="targetStoreKey"></param>
        /// <returns></returns>
        private ThirdPartyMessage SetupIngramCancelOrderRequest(ErpSalesOrder ingramOrder, string targetStoreKey)
        {
            ThirdPartyMessageDAL thirdPartyDal = new ThirdPartyMessageDAL(targetStoreKey);
            return thirdPartyDal.GetThirdPartyIdUsingAssetID(ingramOrder.IngramAssetId, ingramOrder.ChannelReferenceId);
        }

        /// <summary>
        /// Setup Ingram order for update or change
        /// </summary>
        /// <param name="order"></param>
        /// <param name="targetStoreKey"></param>
        /// <returns></returns>
        private ErpSalesOrder SetupIngramChangeOrderRequest(ErpSalesOrder order, string targetStoreKey)
        {
            // Validate sales lines for down sell cases
            ErpSalesLine faultySalesLine = order.SalesLines.FirstOrDefault(sl => sl.OldQuantity > sl.Quantity);
            if (faultySalesLine != null)
            {
                string exceptionMessage = string.Format("Quantity ({0}) for product {1} is lesser than old quantity ({2})", faultySalesLine.Quantity, faultySalesLine.ItemId, faultySalesLine.OldQuantity);
                Exception exception = new Exception(exceptionMessage);
                throw exception;
            }

            ThirdPartyMessageDAL thirdPartyDal = new ThirdPartyMessageDAL(targetStoreKey);
            ThirdPartyMessage lastThirdPartyMessage = thirdPartyDal.GetThirdPartyIdUsingAssetID(order.IngramAssetId, order.ChannelReferenceId);
            order.ChannelReferenceId = lastThirdPartyMessage.ThirdPartyId ?? string.Empty;
            order.SalesId = lastThirdPartyMessage.SalesId ?? string.Empty;

            // Filter and update sales lines that will be send for processing
            order = SelectValidSalesLines(order);

            // Check if we have valid
            if (order.SalesLines.Count == 0)
            {
                string exceptionMessage = string.Format("No valid sales lines found in order {0}", order.ChannelReferenceId);
                Exception exception = new Exception(exceptionMessage);
                throw exception;
            }

            // Trim the ItemId as they are containing extra space when configured in Ingram
            order = TrimIngramSalesOrderItemIds(order);

            // Update product quantity
            order = UpdateProductQuantity(order);

            foreach (ErpSalesLine sl in order.SalesLines)
            {
                sl.ItemId = sl.Description.Substring(0, sl.Description.IndexOf('_'));
                sl.VariantId = sl.Description.Substring(sl.Description.IndexOf('_') + 1);
            }

            return order;
        }

        private void AddPaymentCustomProperties(ErpSalesOrder salesOrderPrams)
        {
            if (salesOrderPrams.TenderLines[0].TenderTypeId == PaymentCon.SEPA.ToString())
            {
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVCardHolder", salesOrderPrams.TenderLines[0].CardOrAccount));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVIBAN", salesOrderPrams.TenderLines[0].IBAN));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVSwiftCode", salesOrderPrams.TenderLines[0].SwiftCode));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVBankName", salesOrderPrams.TenderLines[0].BankName));
            }
            else
            {
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVCardHolder", string.Empty));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVIBAN", string.Empty));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVSwiftCode", string.Empty));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVBankName", string.Empty));
            }

            if (salesOrderPrams.TenderLines[0].TenderTypeId == PaymentCon.ADYEN_HPP.ToString())
            {
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayPspReference", salesOrderPrams.TenderLines[0].PspReference));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayBuyerId", salesOrderPrams.TenderLines[0].Alipay?.BuyerId ?? string.Empty));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayBuyerEmail", salesOrderPrams.TenderLines[0].Alipay?.BuyerEmail ?? string.Empty));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayOutTradeNo", salesOrderPrams.TenderLines[0].Alipay?.OutTradeNo ?? string.Empty));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayTradeNo", salesOrderPrams.TenderLines[0].Alipay?.TradeNo ?? string.Empty));
            }
            else
            {
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayBuyerId", string.Empty));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayBuyerEmail", string.Empty));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayOutTradeNo", string.Empty));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayTradeNo", string.Empty));
                salesOrderPrams.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAlipayPspReference", string.Empty));
            }
        }

        #region Ingram specific code

        /// <summary>
        /// Remove products with 0 quantity
        /// </summary>
        /// <param name="salesOrder"></param>
        /// <returns></returns>
        private ErpSalesOrder RemoveProductsWithZeroQuantity(ErpSalesOrder salesOrder)
        {
            ErpSalesOrder returnSalesOrder = salesOrder;

            var salesLinesToRemove = returnSalesOrder.SalesLines.Where(sl => sl.Quantity < 1).ToList();

            foreach (ErpSalesLine salesLineToRemove in salesLinesToRemove)
            {
                returnSalesOrder.SalesLines.Remove(salesLineToRemove);
            }

            return returnSalesOrder;
        }

        /// <summary>
        /// Filter and update sales lines that will be send for processing
        /// </summary>
        /// <param name="salesOrder"></param>
        /// <returns></returns>
        private ErpSalesOrder SelectValidSalesLines(ErpSalesOrder salesOrder)
        {
            var validSalesLines = salesOrder.SalesLines.Where(sl => sl.OldQuantity < sl.Quantity).ToList();
            salesOrder.SalesLines = null;
            salesOrder.SalesLines = new List<ErpSalesLine>();
            salesOrder.SalesLines = validSalesLines;
            return salesOrder;
        }

        /// <summary>
        /// Update product quantity for Ingram order change
        /// </summary>
        /// <param name="salesOrder"></param>
        /// <returns></returns>
        private ErpSalesOrder UpdateProductQuantity(ErpSalesOrder salesOrder)
        {
            foreach (ErpSalesLine salesLine in salesOrder.SalesLines)
            {
                salesLine.Quantity = salesLine.Quantity - salesLine.OldQuantity;
            }

            return salesOrder;
        }

        /// <summary>
        /// Trim the product ItemId and Description
        /// </summary>
        /// <param name="salesOrder"></param>
        /// <returns></returns>
        private ErpSalesOrder TrimIngramSalesOrderItemIds(ErpSalesOrder salesOrder)
        {
            ErpSalesOrder returnSalesOrder = salesOrder;

            if (returnSalesOrder.SalesLines != null)
            {
                foreach (ErpSalesLine salesLine in returnSalesOrder.SalesLines)
                {
                    salesLine.Description = salesLine.Description.Trim();
                    salesLine.ItemId = salesLine.ItemId.Trim();
                }
            }

            return returnSalesOrder;
        }

        /// <summary>
        /// Update address type and is primary attribute
        /// </summary>
        /// <param name="address"></param>
        /// <param name="addressType"></param>
        /// <param name="isPrimary"></param>
        /// <returns></returns>
        private ErpAddress UpdateAddressType(ErpAddress address, ErpAddressType addressType, bool isPrimary)
        {
            address.AddressType = addressType;
            address.AddressTypeValue = (int)addressType;
            address.IsPrimary = isPrimary;
            return address;
        }

        private bool SetSalesOrderCustomAttributes(ErpSalesOrder erpSalesOrder, string distributorAccount, out TransactionStatus transactionStatus)
        {
            transactionStatus = TransactionStatus.None;

            var mappedResellerAccount = erpSalesOrder.CustomAttributes.FirstOrDefault(c => c.Key == ErpSalesOrderExtensionProperties.TMVRESELLERACCOUNT.ToString());
            erpSalesOrder.CustomAttributes.Remove(mappedResellerAccount);

            var resellerAccount = new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVRESELLERACCOUNT.ToString(), erpSalesOrder.Reseller.AccountNumber);
            erpSalesOrder.CustomAttributes.Add(resellerAccount);

            var mappedCustomerAccount = erpSalesOrder.CustomAttributes.FirstOrDefault(c => c.Key == ErpSalesOrderExtensionProperties.TMVINDIRECTCUSTOMER.ToString());
            erpSalesOrder.CustomAttributes.Remove(mappedCustomerAccount);

            var indirectCustomerAccount = new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVINDIRECTCUSTOMER.ToString(), erpSalesOrder.Customer.AccountNumber);
            erpSalesOrder.CustomAttributes.Add(indirectCustomerAccount);

            var distributorAccountAttribute = new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVDISTRIBUTORACCOUNT.ToString(), distributorAccount);
            erpSalesOrder.CustomAttributes.Add(distributorAccountAttribute);

            if (erpSalesOrder.Parameters.FirstOrDefault(p => p.Name.Equals(ApplicationConstant.IngramEndcustomerAdminEmail)) != null)
            {
                var endCustomerAdminEmail = erpSalesOrder.Parameters.FirstOrDefault(p => p.Name.Equals(ApplicationConstant.IngramEndcustomerAdminEmail)).Value.ToString();

                if (string.IsNullOrWhiteSpace(endCustomerAdminEmail))
                {
                    transactionStatus = TransactionStatus.MissingParameter_EndCustomerAdminEmail;
                    return false;
                }

                var activationLinkEmail = erpSalesOrder.CustomAttributes.FirstOrDefault(c => c.Key == ErpSalesOrderExtensionProperties.TMVActivationLinkEmail.ToString());
                erpSalesOrder.CustomAttributes.Remove(activationLinkEmail);

                var tmvActivationLinkEmail = new KeyValuePair<string, string>(ErpSalesOrderExtensionProperties.TMVActivationLinkEmail.ToString(), endCustomerAdminEmail);
                erpSalesOrder.CustomAttributes.Add(tmvActivationLinkEmail);

            }

            return true;
        }

        #endregion

        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(_store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.SalesOrderSync);
            return job;
        }

        public void JobLog(Job jb, int status)
        {
            JobRepository jobRepo = new JobRepository(_store.StoreKey);
            jobRepo.JobLog(jb.JobID, status);
        }

        public string GetIdentifier()
        {
            return IDENTIFIER;
        }

        public string GetGroup()
        {
            return GROUP;
        }

        public void SetStore(StoreDto store)
        {
            this._store = store;
        }

        public void UpdateJobStatus(JobSchedule jobSchedule, Common.Enums.SynchJobStatus status)
        {
            JobRepository jobRepo = new JobRepository(_store.StoreKey);
            jobRepo.UpdateJobStatus(jobSchedule.JobId, (int)status, _store.StoreId);
        }

        public void JobLog(JobSchedule jobSchedule, int status)
        {
            JobRepository jobRepo = new JobRepository(_store.StoreKey);
            jobRepo.JobLog(jobSchedule.JobId, status);
        }
        public bool IsJobCompletedTodayInJobLog(JobSchedule jobSchedule, int jobStatus)
        {
            JobRepository jobRepo = new JobRepository(_store.StoreKey);
            return jobRepo.IsJobCompletedTodayInJobLog(jobSchedule.JobId, jobStatus);
        }

        public JobSchedule GetSchedule()
        {
            JobRepository jobRepo = new JobRepository(_store.StoreKey);
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.SalesOrderSync, _store.StoreId);
        }

        public void InitializeParameter()
        {
            _configurationHelper = new ConfigurationHelper(_store.StoreKey);
            _fileHelper = new FileHelper(_store.StoreKey);
        }

        #endregion

        private bool CreateCustomerOrResellerWithContactPerson(ErpSalesOrder erpSalesOrder)
        {
            IntegrationManager integrationManager = new IntegrationManager(_store.StoreKey);
            var erpCustomerController = _erpAdapterFactory.CreateCustomerController(_store.StoreKey);
            var erpContactPersonController = _erpAdapterFactory.CreateContactPersonController(_store.StoreKey);

            ErpCustomer erpResellerCustomer;
            ErpContactPerson erpResellerContactPerson = new ErpContactPerson();

            ErpCustomer erpCustomer;

            #region Customer Or Reseller Validation Only

            // Only create reseller if IsCreateResellerCustomer is true
            if (bool.Parse(_configurationHelper.GetSetting(SALESORDER.IsCreateResellerCustomer)))
            {
                if (erpSalesOrder.Reseller == null)
                {
                    CustomLogger.LogFatal("Reseller Not exists.", _store.StoreId, _store.CreatedBy);
                    throw new Exception("Reseller does not exists");
                }

                if (!ValidateCreateCustomer(erpSalesOrder.Reseller))
                {
                    CustomLogger.LogFatal("Invalid Reseller Customer.", _store.StoreId, _store.CreatedBy);
                    return false;
                }

                erpResellerContactPerson = new ErpContactPerson()
                {
                    FirstName = erpSalesOrder.Reseller.FirstName,
                    LastName = erpSalesOrder.Reseller.LastName,
                    Email = erpSalesOrder.Reseller.Email,
                    Phone = erpSalesOrder.Reseller.Phone,
                    Language = erpSalesOrder.Reseller.Language,
                    InActive = false,
                };

                if (!ValidateContactPerson(erpResellerContactPerson))
                {
                    CustomLogger.LogFatal("Invalid Reseller Contact Person.", _store.StoreId, _store.CreatedBy);
                    return false;
                }

            }

            if (erpSalesOrder.Customer == null)
            {
                CustomLogger.LogFatal("Customer Not exists.", _store.StoreId, _store.CreatedBy);
                throw new Exception("Customer does not exists");
            }

            if (!ValidateCreateCustomer(erpSalesOrder.Customer))
            {
                return false;
            }

            var erpContactPerson = new ErpContactPerson()
            {
                FirstName = erpSalesOrder.Customer.FirstName,
                LastName = erpSalesOrder.Customer.LastName,
                Email = erpSalesOrder.Customer.Email,
                Phone = erpSalesOrder.Customer.Phone,
                Language = erpSalesOrder.Customer.Language,
                InActive = false,
            };

            if (!ValidateContactPerson(erpContactPerson))
            {
                return false;
            }

            #endregion

            // Update the AddressType and Primary Property of Reseller and Customer Address for Ingram order
            var salesOrigin = erpSalesOrder.CustomAttributes.FirstOrDefault(a => a.Key.Equals(ApplicationConstant.SalesOrderCustomAttributeTmvSalesOrigion, StringComparison.OrdinalIgnoreCase));
            if (salesOrigin.Value.Equals(ApplicationConstant.SalesOrderCustomAtrributeSalesOriginIngram, StringComparison.OrdinalIgnoreCase))
            {
                if (erpSalesOrder.Customer != null && erpSalesOrder.Customer.Address != null)
                {
                    UpdateAddressType(erpSalesOrder.Customer.Address, ErpAddressType.Business, true);
                }

                if (erpSalesOrder.Reseller != null && erpSalesOrder.Reseller.Address != null)
                {
                    UpdateAddressType(erpSalesOrder.Reseller.Address, ErpAddressType.Business, true);
                }
            }

            if (bool.Parse(_configurationHelper.GetSetting(SALESORDER.IsCreateResellerCustomer)))
            {
                if (erpSalesOrder.Reseller != null)
                {
                    var reseller = integrationManager.GetErpKey(Entities.Customer, erpSalesOrder.Reseller.EcomCustomerId);

                    if (reseller != null)
                    {
                        CustomLogger.LogDebugInfo(string.Format("Reseller Customer already exists : {0}", erpSalesOrder.Reseller.EcomCustomerId), _store.StoreId, _store.CreatedBy);

                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                            DataDirectionType.ThirdPartyResponseToCL, // Data direction
                            JsonConvert.SerializeObject(erpSalesOrder), // Data packed
                            DateTime.UtcNow, // Created on
                            _store.StoreId,
                            ApplicationConstant.UserName, // Created by
                            string.Format("Reseller {0} already exists for Sale order id {1}", reseller.ComKey, erpSalesOrder.ChannelReferenceId), // Description
                            erpSalesOrder.ChannelReferenceId, // eCom Transaction Id
                            string.Empty, // Request Initialed IP
                            string.Empty, // Output packed
                            DateTime.UtcNow, // Output sent at
                            erpSalesOrder.ChannelReferenceId, // Identifier Key
                            erpSalesOrder.ChannelReferenceId, // Identifier Value
                            1, // Success
                            0 // TotalProcessingDuration
                        );

                        erpSalesOrder.Reseller.AccountNumber = reseller.Description;
                    }
                    else
                    {
                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                            DataDirectionType.ThirdPartyResponseToCL, // Data direction
                            JsonConvert.SerializeObject(erpSalesOrder), // Data packed
                            DateTime.UtcNow, // Created on
                            _store.StoreId,
                            ApplicationConstant.UserName, // Created by
                            string.Format("Reseller does not exist for Sale order id {0}", erpSalesOrder.ChannelReferenceId), // Description
                            erpSalesOrder.ChannelReferenceId, // eCom Transaction Id
                            string.Empty, // Request Initialed IP
                            string.Empty, // Output packed
                            DateTime.UtcNow, // Output sent at
                            erpSalesOrder.ChannelReferenceId, // Identifier Key
                            erpSalesOrder.ChannelReferenceId, // Identifier Value
                            1, // Success
                            0 // TotalProcessingDuration
                        );

                        if (erpSalesOrder.Reseller.Attributes == null)
                            erpSalesOrder.Reseller.Attributes = new List<ErpCustomerAttribute>();

                        erpSalesOrder.Reseller.ExtensionProperties = new List<ErpCommerceProperty>();
                        erpSalesOrder.Reseller.ExtensionProperties.Add(new ErpCommerceProperty(ApplicationConstant.SalesOrderCustomAttributeRelationshipType, new ErpCommercePropertyValue { StringValue = ApplicationConstant.SalesOrderCustomAttributeRelationshipTypeReseller }));

                        erpResellerCustomer = erpCustomerController.CreateCustomerThridParty(erpSalesOrder.Reseller, erpSalesOrder.CurrencyCode);

                        if (erpResellerCustomer == null)
                        {
                            CustomLogger.LogFatal("Reseller Customer Create failed.", _store.StoreId, _store.CreatedBy);

                            CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                                DataDirectionType.ThirdPartyResponseToCL, // Data direction
                                JsonConvert.SerializeObject(erpSalesOrder), // Data packed
                                DateTime.UtcNow, // Created on
                                _store.StoreId,
                                ApplicationConstant.UserName, // Created by
                                string.Format("Reseller creation failed for Sale order id {0}", erpSalesOrder.ChannelReferenceId), // Description
                                erpSalesOrder.ChannelReferenceId, // eCom Transaction Id
                                string.Empty, // Request Initialed IP
                                string.Empty, // Output packed
                                DateTime.UtcNow, // Output sent at
                                erpSalesOrder.ChannelReferenceId, // Identifier Key
                                erpSalesOrder.ChannelReferenceId, // Identifier Value
                                0, // Success
                                0 // TotalProcessingDuration
                            );

                            return false;
                        }

                        CustomLogger.LogDebugInfo("Reseller Customer Created.", _store.StoreId, _store.CreatedBy);

                        CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                            DataDirectionType.ThirdPartyResponseToCL, // Data direction
                            JsonConvert.SerializeObject(erpSalesOrder), // Data packed
                            DateTime.UtcNow, // Created on
                            _store.StoreId,
                            ApplicationConstant.UserName, // Created by
                            string.Format("Reseller {0} created for Sale order id {1}", erpResellerCustomer.AccountNumber, erpSalesOrder.ChannelReferenceId), // Description
                            erpSalesOrder.ChannelReferenceId, // eCom Transaction Id
                            string.Empty, // Request Initialed IP
                            string.Empty, // Output packed
                            DateTime.UtcNow, // Output sent at
                            erpSalesOrder.ChannelReferenceId, // Identifier Key
                            erpSalesOrder.ChannelReferenceId, // Identifier Value
                            1, // Success
                            0 // TotalProcessingDuration
                        );

                        // Setting reseller D365 account number in ERPSalesOrder reseller object
                        erpSalesOrder.Reseller.AccountNumber = erpResellerCustomer.AccountNumber;

                        erpResellerContactPerson.CustAccount = erpResellerCustomer.AccountNumber;
                        erpResellerContactPerson.ContactForParty = erpResellerCustomer.DirectoryPartyRecordId;
                        erpResellerContactPerson.PhoneRecordId = erpResellerCustomer.PhoneRecordId;
                        erpResellerContactPerson.TMVSourceSystem = string.IsNullOrWhiteSpace(erpResellerContactPerson.TMVSourceSystem) ? SourceSystem.WEB.ToString() : erpResellerContactPerson.TMVSourceSystem.Trim();

                        erpResellerContactPerson.Language = erpResellerCustomer.Language;

                        erpContactPersonController.CreateContactPerson(erpResellerContactPerson, string.Empty);
                        CustomLogger.LogDebugInfo("Reseller Contact Person Created.", _store.StoreId, _store.CreatedBy);
                    }
                }
            }

            var customer = integrationManager.GetErpKey(Entities.Customer, erpSalesOrder.Customer.EcomCustomerId);

            if (customer != null)
            {
                CustomLogger.LogDebugInfo(string.Format("Customer already exists : {0}", erpSalesOrder.Customer.EcomCustomerId), _store.StoreId, _store.CreatedBy);

                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                        DataDirectionType.ThirdPartyResponseToCL, // Data direction
                        JsonConvert.SerializeObject(erpSalesOrder), // Data packed
                        DateTime.UtcNow, // Created on
                        _store.StoreId,
                        ApplicationConstant.UserName, // Created by
                        string.Format("Indirect customer {0} already exists for Sale order id {1}", customer.ComKey, erpSalesOrder.ChannelReferenceId), // Description
                        erpSalesOrder.ChannelReferenceId, // eCom Transaction Id
                        string.Empty, // Request Initialed IP
                        string.Empty, // Output packed
                        DateTime.UtcNow, // Output sent at
                        erpSalesOrder.ChannelReferenceId, // Identifier Key
                        erpSalesOrder.ChannelReferenceId, // Identifier Value
                        1, // Success
                        0 // TotalProcessingDuration
                        );

                erpSalesOrder.Customer.AccountNumber = customer.Description;
            }
            else
            {
                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                    DataDirectionType.ThirdPartyResponseToCL, // Data direction
                    JsonConvert.SerializeObject(erpSalesOrder), // Data packed
                    DateTime.UtcNow, // Created on
                    _store.StoreId,
                    ApplicationConstant.UserName, // Created by
                    string.Format("Indirect customer does not exist for Sale order id {0}", erpSalesOrder.ChannelReferenceId), // Description
                    erpSalesOrder.ChannelReferenceId, // eCom Transaction Id
                    string.Empty, // Request Initialed IP
                    string.Empty, // Output packed
                    DateTime.UtcNow, // Output sent at
                    erpSalesOrder.ChannelReferenceId, // Identifier Key
                    erpSalesOrder.ChannelReferenceId, // Identifier Value
                    1, // Success
                    0 // TotalProcessingDuration
                    );

                erpSalesOrder.Customer.ExtensionProperties = new List<ErpCommerceProperty>();
                erpSalesOrder.Customer.ExtensionProperties.Add(new ErpCommerceProperty("RelationshipType", new ErpCommercePropertyValue { StringValue = "End-User Reseller" }));
                erpCustomer = erpCustomerController.CreateCustomerThridParty(erpSalesOrder.Customer, erpSalesOrder.CurrencyCode);
                if (erpCustomer == null)
                {
                    CustomLogger.LogFatal("Ingram indirect customer creation failed.", _store.StoreId, _store.CreatedBy);
                    return false;
                }

                CustomLogger.LogDebugInfo("Ingram indirect customer created.", _store.StoreId, _store.CreatedBy);

                // Setting reseller D365 account number in ERPSalesOrder reseller object
                erpSalesOrder.Customer.AccountNumber = erpCustomer.AccountNumber;

                erpContactPerson.CustAccount = erpCustomer.AccountNumber;
                erpContactPerson.ContactForParty = erpCustomer.DirectoryPartyRecordId;
                erpContactPerson.PhoneRecordId = erpCustomer.PhoneRecordId;
                erpContactPerson.TMVSourceSystem = string.IsNullOrWhiteSpace(erpContactPerson.TMVSourceSystem) ? SourceSystem.WEB.ToString() : erpContactPerson.TMVSourceSystem.Trim();

                erpContactPerson.Language = erpCustomer.Language;

                erpContactPersonController.CreateContactPerson(erpContactPerson, string.Empty);

                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                        DataDirectionType.ThirdPartyResponseToCL, // Data direction
                        JsonConvert.SerializeObject(erpSalesOrder), // Data packed
                        DateTime.UtcNow, // Created on
                        _store.StoreId,
                        ApplicationConstant.UserName, // Created by
                        string.Format("Indirect customer created for Sale order id {0}", erpSalesOrder.ChannelReferenceId), // Description
                        erpSalesOrder.ChannelReferenceId, // eCom Transaction Id
                        string.Empty, // Request Initialed IP
                        string.Empty, // Output packed
                        DateTime.UtcNow, // Output sent at
                        erpSalesOrder.ChannelReferenceId, // Identifier Key
                        erpSalesOrder.ChannelReferenceId, // Identifier Value
                        1, // Success
                        0 // TotalProcessingDuration
                        );

                CustomLogger.LogDebugInfo("Reseller Contact Person Created.", _store.StoreId, _store.CreatedBy);
            }

            return true;
        }

        private bool ValidateCreateCustomer(ErpCustomer erpCustomer)
        {
            if (erpCustomer == null)
            {
                throw new Exception("Invalid Customer");
            }

            //Validation for Person type customer
            if (!string.IsNullOrWhiteSpace(erpCustomer.CustomerType.ToString()) && erpCustomer.CustomerType.ToString() == ErpCustomerType.Person.ToString())
            {
                if (string.IsNullOrWhiteSpace(erpCustomer.FirstName))
                {
                    throw new Exception("Invalid Customer FirstName");
                }
                else if (string.IsNullOrWhiteSpace(erpCustomer.LastName))
                {
                    throw new Exception("Invalid Customer LastName");
                }
            }

            if (string.IsNullOrEmpty(erpCustomer.EcomCustomerId))
            {
                throw new Exception("Invalid Customer EcomCustomerId");
            }
            else if (erpCustomer.CustomerType.ToString() == ErpCustomerType.Organization.ToString() && string.IsNullOrWhiteSpace(erpCustomer.Name))
            {
                throw new Exception("Invalid Organization Name");
            }

            //Length issue server side validation
            if (!string.IsNullOrWhiteSpace(erpCustomer.Name) && erpCustomer.Name.Length > 100)
            {
                erpCustomer.Name = erpCustomer.Name.Substring(0, 100);
            }
            if (!string.IsNullOrWhiteSpace(erpCustomer.FirstName) && erpCustomer.FirstName.Length > 25)
            {
                erpCustomer.FirstName = erpCustomer.FirstName.Substring(0, 25);
            }
            if (!string.IsNullOrWhiteSpace(erpCustomer.MiddleName) && erpCustomer.MiddleName.Length > 25)
            {
                erpCustomer.LastName = erpCustomer.LastName.Substring(0, 25);
            }
            if (!string.IsNullOrWhiteSpace(erpCustomer.LastName) && erpCustomer.LastName.Length > 25)
            {
                erpCustomer.LastName = erpCustomer.LastName.Substring(0, 25);
            }
            if (!string.IsNullOrWhiteSpace(erpCustomer.Email) && erpCustomer.Email.Length > 255)
            {
                throw new Exception("Invalid Customer Email with Length greater than 255");
            }
            if (!string.IsNullOrWhiteSpace(erpCustomer.VatNumber) && erpCustomer.VatNumber.Length > 20)
            {
                throw new Exception("Invalid Customer VatNumber with Length greater than 20");
            }
            if (!string.IsNullOrWhiteSpace(erpCustomer.Language) && erpCustomer.Language.Length > 7)
            {
                throw new Exception("Invalid Customer Language with Length greater than 7");
            }

            if (erpCustomer.Addresses != null)
            {
                foreach (var address in erpCustomer.Addresses)
                {
                    if (!string.IsNullOrEmpty(address.Name) && address.Name.Length > 60)
                    {
                        address.Name = address.Name.Substring(0, 60);
                    }

                    if (!string.IsNullOrWhiteSpace(address.ZipCode) && address.ZipCode.Length > 10)
                    {
                        throw new Exception("Invalid Customer address ZipCode with Length greater than 10");
                    }
                }
            }

            if (erpCustomer != null && erpCustomer.Address != null)
            {
                if (!string.IsNullOrEmpty(erpCustomer.Address.Name) && erpCustomer.Address.Name.Length > 60)
                {
                    erpCustomer.Address.Name = erpCustomer.Address.Name.Substring(0, 60);
                }

                if (!string.IsNullOrWhiteSpace(erpCustomer.Address.ZipCode) && erpCustomer.Address.ZipCode.Length > 10)
                {
                    throw new Exception("Invalid Customer address ZipCode with Length greater than 10");
                }
            }

            return true;

        }

        private bool ValidateContactPerson(ErpContactPerson erpContactPerson)
        {
            if (erpContactPerson == null)
            {
                throw new Exception("Invalid ContactPerson");
            }

            if (string.IsNullOrWhiteSpace(erpContactPerson.FirstName))
            {
                throw new Exception("Invalid ContactPerson FirstName");
            }
            else if (string.IsNullOrWhiteSpace(erpContactPerson.LastName))
            {
                throw new Exception("Invalid ContactPerson LastName");
            }
            //Length issue server side validation
            else if (!string.IsNullOrWhiteSpace(erpContactPerson.FirstName) && erpContactPerson.FirstName.Length > 25)
            {
                erpContactPerson.FirstName = erpContactPerson.FirstName.Substring(0, 25);
            }
            if (!string.IsNullOrWhiteSpace(erpContactPerson.MiddleName) && erpContactPerson.MiddleName.Length > 25)
            {
                erpContactPerson.MiddleName = erpContactPerson.MiddleName.Substring(0, 25);
            }
            if (!string.IsNullOrWhiteSpace(erpContactPerson.LastName) && erpContactPerson.LastName.Length > 25)
            {
                erpContactPerson.LastName = erpContactPerson.LastName.Substring(0, 25);
            }
            if (!string.IsNullOrWhiteSpace(erpContactPerson.Language) && erpContactPerson.Language.Length > 7)
            {
                throw new Exception("Invalid ContactPerson Language with Length greater than 7");
            }

            return true;
        }

        /// <summary>
        /// Logging in Request Response Ingram
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="description"></param>
        /// <param name="identifierKey"></param>
        /// <param name="identifierValue"></param>
        /// <param name="success"></param>
        private void LogRequestResponseIngram(object input, object output, string description, string identifierKey, string identifierValue, int success)
        {
            CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                DataDirectionType.ThirdPartyResponseToCL, // Data direction
                JsonConvert.SerializeObject(input), // Data packed
                DateTime.UtcNow, // Created on
                _store.StoreId,
                ApplicationConstant.UserName, // Created by
                string.Format(description), // Description
                identifierValue, // eCom Transaction Id
                string.Empty, // Request Initialed IP
                JsonConvert.SerializeObject(output), // Output packed
                DateTime.UtcNow, // Output sent at
                identifierKey, // Identifier Key
                identifierValue, // Identifier Value
                success, // Success
                0 // TotalProcessingDuration
                );
        }

        /// <summary>
        /// Logging in Request Response Ingram Success
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="description"></param>
        /// <param name="identifierKey"></param>
        /// <param name="identifierValue"></param>
        private void LogRequestResponseIngramSuccess(object input, object output, string description, string identifierKey, string identifierValue)
        {
            LogRequestResponseIngram(input, output, description, identifierKey, identifierValue, 1);
        }

        /// <summary>
        /// Logging in Request Response Ingram Failure
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="description"></param>
        /// <param name="identifierKey"></param>
        /// <param name="identifierValue"></param>
        private void LogRequestResponseIngramFailure(object input, object output, string description, string identifierKey, string identifierValue)
        {
            LogRequestResponseIngram(input, output, description, identifierKey, identifierValue, 0);
        }

    }

}
