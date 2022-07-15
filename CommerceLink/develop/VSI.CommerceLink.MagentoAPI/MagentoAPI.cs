using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VSI.CommerceLink.MagentoAPI.Entities;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;

namespace VSI.CommerceLink.MagentoAPI
{
    public class MagentoAPI
    {
        public  string Token { get; set; }
        public string BaseUrl { get; set; }
        public StoreDto currentStore = null; 
        private const string TOKENPOSTFIX = "integration/admin/token";
        private const string ORDERPOSTFIX = "orders/";
        private const string ORDERCHANGESTATUSPOSTFIX = "orders/create";

        public MagentoAPI(string url,string userName,string password,StoreDto store)
        {
            //++setting store and customlogger
            currentStore = store;
            CustomLogger.LogTraceInfo("MagentoAPI Authorization Start - " + DateTime.UtcNow.ToShortTimeString(), currentStore.StoreId, currentStore.CreatedBy);
            try
            {
                BaseUrl = url;
                var http = (HttpWebRequest)WebRequest.Create(new Uri(BaseUrl + TOKENPOSTFIX));
                http.Accept = "application/json";
                http.ContentType = "application/json";
                http.Method = "POST";

                string parsedContent = "{ \"username\":\"" + userName + "\",\"password\":\"" + password + "\"}";
                ASCIIEncoding encoding = new ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(parsedContent);

                Stream newStream = http.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                var response = http.GetResponse();
                var stream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    Token = content.Replace("\"", "");
                    CustomLogger.LogTraceInfo("MagentoAPI Authorization End - " + DateTime.UtcNow.ToShortTimeString(),currentStore.StoreId ,currentStore.CreatedBy);
                }
                
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy);
            }
        }

        public RootObject SalesOrder(string id)
        {
            CustomLogger.LogTraceInfo("MagentoAPI.SalesOrder Entered - " + DateTime.UtcNow.ToShortTimeString(), currentStore.StoreId, currentStore.CreatedBy);
            RootObject salesOrder = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUrl + ORDERPOSTFIX + id);
                request.Method = "GET";
                request.Headers.Add((HttpRequestHeader.Authorization), "Bearer " + Token);
                String salesOrderJson = string.Empty;
                using (HttpWebResponse response1 = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response1.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    salesOrderJson = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                }
                salesOrder = JsonConvert.DeserializeObject<RootObject>(salesOrderJson);
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy);
            }
            CustomLogger.LogTraceInfo("MagentoAPI.SalesOrder Exit - " + DateTime.UtcNow.ToShortTimeString(), currentStore.StoreId, currentStore.CreatedBy);
            return salesOrder;
        }

        public RootObject ChangeOrderStatus(string entityId, string orderId, string status)
        {
            RootObject salesOrder = null;
            CustomLogger.LogTraceInfo("MagentoAPI.ChangeOrderStatus Entered - " + DateTime.UtcNow.ToShortTimeString(), currentStore.StoreId, currentStore.CreatedBy);
            try
            {
                var http = (HttpWebRequest)WebRequest.Create(new Uri(BaseUrl + ORDERCHANGESTATUSPOSTFIX));
                http.Accept = "application/json";
                http.ContentType = "application/json";
                http.Method = "PUT";
                http.Headers.Add((HttpRequestHeader.Authorization), "Bearer " + Token);
                string json = "{\"entity\": {\"entity_id\": \"" + entityId + "\",\"increment_id\":\"" + orderId + "\",\"status\": \"" + status + "\"}}";

                ASCIIEncoding encoding = new ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(json);

                Stream newStream = http.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                var response = http.GetResponse();
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                salesOrder = JsonConvert.DeserializeObject<RootObject>(content);

            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy);
            }
            CustomLogger.LogTraceInfo("MagentoAPI.ChangeOrderStatus Exit - " + DateTime.UtcNow.ToShortTimeString(), currentStore.StoreId, currentStore.CreatedBy);
            return salesOrder;
        }
    }
}
