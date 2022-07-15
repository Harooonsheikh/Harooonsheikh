using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums.Enums.TMV;
using VSI.EDGEAXConnector.Mapper;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.ECommerceDataModels.Ingram;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.Logging;

namespace VSI.CommerceLink.ThirdPartyAdapter.Controllers
{
    public class SaleOrderController : BaseController, ISaleOrderController
    {
        #region Constructor
        public SaleOrderController(string storeKey) : base(storeKey)
        {
        }

        #endregion

        #region Public Methods
        public ErpSalesOrder GetSalesOrderFromXML(XmlDocument xmlDoc)
        {
            ErpSalesOrder order = new ErpSalesOrder();
            try
            {
                XmlTemplateHelper xmlHelper = new XmlTemplateHelper(currentStore);
                xmlHelper.GenerateObjectByTemplateIngram(xmlDoc, XmlTemplateHelper.XmlSourceDirection.READ, order);
            }
            catch (Exception ex)
            {
                CustomLogger.LogFatal(string.Concat("Exception logged:  : {0}", ex.Message), currentStore.StoreId, currentStore.CreatedBy);
                order = null;
            }

            return order;
        }

        public ErpSalesOrder GetSalesOrders(string saleOrderJson)
        {
            try
            {
                ErpSalesOrder erpSalesOrder = new ErpSalesOrder();

                var salesOrderXML = JsonConvert.DeserializeXmlNode(saleOrderJson, "orders");

                erpSalesOrder = this.GetSalesOrderFromXML(salesOrderXML);

                return erpSalesOrder;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SaveThirdPartySalesOrder()
        {
            try
            {
                var messageList = new List<ThirdPartyMessage>();

                var messageDAL = new ThirdPartyMessageDAL(currentStore.StoreKey);
                string methodName = MethodBase.GetCurrentMethod().Name;

                ThirdPartyEnvironmentWithStatusDAL thirdPartyEnvironmentWithStatusDAL = new ThirdPartyEnvironmentWithStatusDAL();
                List<ThirdPartyEnvironmentWithStatus> thirdPartyEnvironmentWithStatuses = thirdPartyEnvironmentWithStatusDAL.GetActiveEnvironments();

                var thirdPartyAPI = new ThirdPartyAPI.ThirdPartyApi(thirdPartyApiUrl, thirdPartyApiKey, currentStore);
                List<IngramSalesOrderResponse> salesOrderList = new List<IngramSalesOrderResponse>();
                foreach (var environment in thirdPartyEnvironmentWithStatuses)
                {
                    salesOrderList.AddRange(thirdPartyAPI.GetIngramSaleOrders(methodName, environment.EnvironmentName.Trim(), environment.Status.Trim(), CommonUtility.StringToInt(thirdPartyApiLimit)).Result);
                }
                var saleOrderids = salesOrderList.Select(so => so.id).ToList();
                var saleOrderidsNotExist = messageDAL.SaleOrderIdsNotExist(saleOrderids);
                var saleOrderToSave = salesOrderList.Where(so => saleOrderidsNotExist.Contains(so.id)).ToList();

                foreach (var saleOrder in saleOrderToSave)
                {
                    try
                    {
                        #region get distributor specific data
                        var distributor = $"{saleOrder.asset.connection.type}:{saleOrder.marketplace.id}:{saleOrder.contract.id}";

                        ThirdPartyDistributorDetail distributorDetail = ThirdPartyMessageHelper.ThirdPartyDistributorDetails.FirstOrDefault(k => k.DistributorKey == distributor);

                        if (distributorDetail == null)
                        {
                            string distributorStoreKey = null;
                            string distributorStoreCurrency = null;

                            // 1. Get distributer storekey 
                            IntegrationKey distributorIntegrationKey = IntegrationManager.GetIntegrationKey(EDGEAXConnector.Enums.Entities.Customer, distributor);
                            if(distributorIntegrationKey == null)
                            {
                                var distributorNotFoundMessage = $"Distributor not found with distributor key: {distributor}";
                                CustomLogger.LogRequestResponse(methodName, EDGEAXConnector.Enums.DataDirectionType.ThirdPartyRequestToCL, JsonConvert.SerializeObject(saleOrder), DateTime.UtcNow, currentStore.StoreId, "System", distributorNotFoundMessage, "", "", distributorNotFoundMessage, DateTime.UtcNow, saleOrder.id, saleOrder.id, 0, 0);
                                CustomLogger.LogException(new Exception(distributorNotFoundMessage), currentStore.StoreId, currentStore.CreatedBy, saleOrder.id);
                                continue;
                            }

                            var distributorStore = StoreService.GetStoreById(distributorIntegrationKey.StoreId.Value);

                            if (distributorStore != null)
                            {
                                distributorStoreKey = distributorStore.StoreKey;

                                // 2. Get currency using target store (fetched in step 1)
                                ConfigurationHelper configurationHelperForTargetStore = new ConfigurationHelper(distributorStoreKey);

                                //34050 - Ingram distributor currency can be change from channel
                                distributorStoreCurrency = configurationHelperForTargetStore.GetSetting(APPLICATION.Ingram_Default_Currency_Code);
                            }

                            distributorDetail = new ThirdPartyDistributorDetail(distributor, distributorStoreKey, distributorStoreCurrency);

                            ThirdPartyMessageHelper.ThirdPartyDistributorDetails.Add(distributorDetail);
                        }
                        #endregion

                        // VSTS 33374 Begin
                        string pacLicense = string.Empty;
                        if (saleOrder.asset.@params.FirstOrDefault(p => p.name.Equals(ApplicationConstant.IngramPacLicenseParameterName)) != null)
                        {
                            pacLicense = saleOrder.asset.@params.FirstOrDefault(p => p.name.Equals(ApplicationConstant.IngramPacLicenseParameterName)).value.ToString();
                        }
                        // VSTS 33374 End

                        var message = new ThirdPartyMessage()
                        {
                            EntityId = (int)Entities.SaleOrder,
                            ThirdPartyId = saleOrder.id,
                            Content = JsonConvert.SerializeObject(saleOrder),
                            ThirdPartyStatus = saleOrder.status,
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = "System",
                            TransactionStatus = (int)TransactionStatus.Created,
                            DestinationStoreKey = distributorDetail != null ? distributorDetail.StoreKey : null,
                            Currency = distributorDetail != null ? distributorDetail.Currency : null,
                            AssetId = saleOrder.asset.id,
                            OrderType = string.IsNullOrEmpty(pacLicense) ? saleOrder.type : ApplicationConstant.IngramOrderTypeTransfer,
                            RetryCount = !String.IsNullOrWhiteSpace(configurationHelper.GetSetting(INGRAM.Status_Update_Retry_Count)) ? Convert.ToByte(configurationHelper.GetSetting(INGRAM.Status_Update_Retry_Count)) : (byte)3,
                            PacLicense = pacLicense
                        };

                        CustomLogger.LogDebugInfo(string.Format("Ingram order {0} fetched and added in CommerceLink DB", saleOrder.id), currentStore.StoreId, currentStore.CreatedBy);

                        messageList.Add(message);
                    }
                    catch (Exception ex)
                    {
                        Exception ingramOrderSaveException = new Exception(string.Format("Exception when extracing ingram order. Order message: {0}", Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder)));
                        CustomLogger.LogException(ingramOrderSaveException, currentStore.StoreId, currentStore.CreatedBy, "Ingram order fetching", saleOrder.id);
                        CustomLogger.LogRequestResponse(methodName, EDGEAXConnector.Enums.DataDirectionType.CLRequestToThirdParty, JsonConvert.SerializeObject(saleOrder), DateTime.UtcNow, currentStore.StoreId, "System", "Error extracting ingram order from request", "", "", JsonConvert.SerializeObject(ex), DateTime.UtcNow, saleOrder.id, saleOrder.id, 0, 0);
                        continue;
                    }
                }
                if (messageList.Count > 0)
                {
                    try
                    {
                        messageDAL.Add(messageList);
                    }
                    catch (Exception ex)
                    {
                        Exception ingramOrderSaveException = new Exception(string.Format("Exception when adding ingram orders in database. Orders: {0}", Newtonsoft.Json.JsonConvert.SerializeObject(messageList)));
                        CustomLogger.LogException(ingramOrderSaveException, currentStore.StoreId, currentStore.CreatedBy, "Ingram order fetching");
                        CustomLogger.LogRequestResponse(methodName, EDGEAXConnector.Enums.DataDirectionType.CLRequestToThirdParty, JsonConvert.SerializeObject(messageList), DateTime.UtcNow, currentStore.StoreId, "System", "Error inserting ingram orders in database", "", "", JsonConvert.SerializeObject(ex), DateTime.UtcNow, "", "", 0, 0);
                        throw;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}
