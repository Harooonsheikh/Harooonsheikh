using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.ECommerceDataModels.Ingram;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums.TMV;
using VSI.EDGEAXConnector.Logging;

namespace VSI.CommerceLink.ThirdPartyAPI
{
    public class ThirdPartyApi
    {
        private StoreDto currentStore = null;
        readonly string _baseUrl;
        readonly string _apiKey;
        readonly HttpClientService _http;
        private const string OrderQueryParamPostfix = "?limit={0}&status={1}";
        private const string OrderByEnvironmentStatus = "?limit={0}&status={1}&asset.connection.type={2}";

        public ThirdPartyApi(string url, string key, StoreDto store)
        {
            _http = new HttpClientService();
            _baseUrl = url;
            _apiKey = key;
        }

        public async Task<List<IngramSalesOrderResponse>> GetIngramSaleOrders(string methodName, string environment, string status, int limit = 1000)
        {
            string logData = string.Empty;
            try
            {
                string orderUrl = _baseUrl + string.Format(OrderByEnvironmentStatus, limit, status, environment);

                string logUrl = _baseUrl + "\n" + orderUrl;
                logData = logUrl + "\n" + _apiKey;

                var ingramOrders = await _http.GetAsync(orderUrl, _apiKey);

                CustomLogger.LogRequestResponse(methodName, DataDirectionType.CLRequestToThirdParty, logData, DateTime.UtcNow, 0, ApplicationConstant.UserName, "Call to fetch third party orders", "", "", ingramOrders, DateTime.UtcNow, "GetIngramSaleOrders", "GetIngramSaleOrders", 1, 0);

                return JsonConvert.DeserializeObject<List<IngramSalesOrderResponse>>(ingramOrders);
            }
            catch (Exception ex)
            {
                CustomLogger.LogRequestResponse(methodName, DataDirectionType.CLRequestToThirdParty, logData, DateTime.UtcNow, 0, ApplicationConstant.UserName, "Call to fetch third party orders", "", "", JsonConvert.SerializeObject(ex), DateTime.UtcNow, "", "", 0, 0);

                throw;
            }
        }

        public async Task<IngramSalesOrderResponse> UpdateSaleOrderStatus(string thirdPartyId, string thirdPartyStatus, string methodName, string description = "")
        {
            string url = _baseUrl + "/" + thirdPartyId + "/" + thirdPartyStatus;

            string logData = url + "\n" + _apiKey;

            var order = await _http.PostAsync(url, GetOrderStatus(thirdPartyStatus, description), _apiKey);

            CustomLogger.LogRequestResponse(methodName, DataDirectionType.CLRequestToThirdParty, logData, DateTime.UtcNow, 0, ApplicationConstant.UserName, "Call to fetch third party orders", "", "", order, DateTime.UtcNow, thirdPartyId, thirdPartyId, 1, 0);

            var saleOrder = JsonConvert.DeserializeObject<IngramSalesOrderResponse>(order);

            return saleOrder;
        }

        public async Task<IngramSalesOrderResponse> UpdateSaleOrderParameters(string thirdPartyId, int transactionStatus, string methodName)
        {
            string url = _baseUrl + "/" + thirdPartyId;

            string logData = url + "\n" + _apiKey;

            var order = await _http.PutAsync(url, GetOrderParameter(transactionStatus), _apiKey);

            CustomLogger.LogRequestResponse(methodName, DataDirectionType.CLRequestToThirdParty, logData, DateTime.UtcNow, 0, ApplicationConstant.UserName, "Call to fetch third party orders", "", "", order, DateTime.UtcNow, thirdPartyId, thirdPartyId, 1, 0);

            var saleOrder = JsonConvert.DeserializeObject<IngramSalesOrderResponse>(order);

            return saleOrder;
        }

        private object GetOrderStatus(string status, string description = "")
        {
            if (status == ApplicationConstant.IngramOrderStatusApprove)
            {
                return new { activation_tile = !string.IsNullOrEmpty(description) ? description : "Ingram order Invoiced in D365" };
            }
            else if (status == ApplicationConstant.IngramOrderStatusFail)
            {
                return new { reason = !string.IsNullOrEmpty(description) ? description : "Ingram order cancelled in D365." };
            }
            else if (status == ApplicationConstant.IngramOrderStatusInquire)
            {
                return new { activation_tile = !string.IsNullOrEmpty(description) ? description : "Ingram order status changed to inquire." };
            }

            return new { };
        }

        private object GetOrderParameter(int transactionStatus)
        {
            var param = new UpdateParameterRequest();
            if (transactionStatus == (int)TransactionStatus.MissingParameter_EndCustomerAdminEmail)
            {
                param.asset.@params.Add(new Parameter(ApplicationConstant.IngramEndcustomerAdminEmail, ApplicationConstant.IngramEndcustomerAdminEmailErrorMessage));
            }

            return param;
        }

        public async Task<IngramSalesOrderResponse> GetSalesOrderById(string thirdPartyId, string methodName)
        {
            string url = _baseUrl + "/" + thirdPartyId;

            string logData = url + "\n" + _apiKey;

            var order = await _http.GetAsync(url, _apiKey);

            CustomLogger.LogRequestResponse(methodName, DataDirectionType.CLRequestToThirdParty, logData, DateTime.UtcNow, 0, ApplicationConstant.UserName, "Call to fetch third party orders", "", "", order, DateTime.UtcNow, thirdPartyId, thirdPartyId, 1, 0);

            var saleOrder = JsonConvert.DeserializeObject<IngramSalesOrderResponse>(order);

            return saleOrder;
        }
    }
}
