using NewRelic.Api.Agent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Web.ActionFilters
{
    public class RequestResponseLogAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // LogRequest(actionContext);
            actionContext.Request.Properties["CreatedOn"] = DateTime.UtcNow;
            actionContext.Request.Properties["RequestId"] = Guid.NewGuid().ToString();
            SetRequestIdNewrelic(actionContext.Request.Properties["RequestId"].ToString());
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500000, GetRequestGUID(actionContext.Request), actionContext.ActionDescriptor.ActionName, DateTime.UtcNow);         
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //LogResponse(actionExecutedContext);
            if (LogMapperHelper.IsLogginEnabled)
            {
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500004, GetRequestGUID(actionExecutedContext.Request), actionExecutedContext.ActionContext.ActionDescriptor.ActionName, DateTime.UtcNow);
                LogRequestResponse(actionExecutedContext);
            }
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500001, GetRequestGUID(actionExecutedContext.Request), actionExecutedContext.ActionContext.ActionDescriptor.ActionName, DateTime.UtcNow);
        }
        private void LogRequest(HttpActionContext actionContext)
        {
            StoreDto store = null;
            try
            {
                string identifierKey = null;
                string identifierValue = null;
                string httpMethod = actionContext.Request.Method.Method;
                string salesOrderRequest = null;
                IEnumerable<string> storeKey = null;
                var actionName = actionContext.ActionDescriptor.ActionName;
                string requestBody = "";
                LogMapper logMapper = null;
                if (actionContext.Request.Headers.TryGetValues("x-api-key", out storeKey))
                {
                    if (storeKey != null)
                    {
                        LoggingDAL loggingDAL = new LoggingDAL(storeKey.FirstOrDefault());
                        logMapper = LogMapperHelper.GetLogSettings(actionName);
                        identifierKey = logMapper?.IdentifierKey;
                    }
                }


                foreach (var argument in actionContext.ActionArguments)
                {
                    requestBody += argument.Value == null ? "" : JsonConvert.SerializeObject(argument.Value);
                    requestBody += Environment.NewLine;
                    if (argument.Key == identifierKey)
                    {
                        identifierValue = argument.Value?.ToString();
                    }
                }
                if (actionName == "CreateSalesOrderTransaction")
                {
                    salesOrderRequest = requestBody;
                    if (!string.IsNullOrWhiteSpace(requestBody) && requestBody != "null")
                    {
                        JObject salesOrder = JsonConvert.DeserializeObject<JObject>(requestBody);
                        requestBody = salesOrder["salesOrderJSON"].ToString();
                    }
                }
                else if (actionName == "CreateMergeSalesOrderTransaction")
                {
                    salesOrderRequest = requestBody;
                    if (!string.IsNullOrWhiteSpace(requestBody) && requestBody != "null")
                    {
                        JObject salesOrder = JsonConvert.DeserializeObject<JObject>(requestBody);
                        requestBody = salesOrder["SalesOrder"]["salesOrderJSON"].ToString();
                    }
                }
                if (storeKey != null && !string.IsNullOrWhiteSpace(storeKey.FirstOrDefault()))
                {
                    store = StoreService.GetStoreByKey(storeKey.FirstOrDefault());
                    if (store != null)
                    {
                        RequestResponse requestResponseLog = new RequestResponse()
                        {

                            ApplicationName = ConfigurationManager.AppSettings["ApplicationName"],
                            CreatedBy = store.CreatedBy,
                            Description = string.Empty,
                            EcomTransactionId = string.Empty,
                            DataDirectionId = (int)DataDirectionType.EcomRequestToCL,
                            IdentifierKey = identifierKey,
                            IdentifierValue = httpMethod.Equals("GET", StringComparison.InvariantCultureIgnoreCase) ? identifierValue : GetIdentifierValue(requestBody, logMapper?.IdentifierPath),
                            MethodName = actionName,
                            DataPacket = (actionName.Equals("CreateSalesOrderTransaction", StringComparison.InvariantCultureIgnoreCase) || actionName.Equals("CreateMergeSalesOrderTransaction", StringComparison.InvariantCultureIgnoreCase)) ? salesOrderRequest : requestBody,
                            CreatedOn = DateTime.Now,
                            RequestInitiatedIP = GetClientIp(actionContext.Request),
                            StoreId = store.StoreId,

                        };
                        LoggingDAL loggingDAL = new LoggingDAL(storeKey.FirstOrDefault());
                        loggingDAL.LogRequest(requestResponseLog);
                        actionContext.Request.Properties["RequestResponseId"] = requestResponseLog.RequestResponseId;
                    }
                }
                else
                {
                    throw new CommerceLinkError("Store Key not found");
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, store != null ? store.StoreId : 0, store != null ? store.CreatedBy : string.Empty);
            }
        }
        private void LogResponse(HttpActionExecutedContext actionExecutedContext)
        {
            StoreDto store = null;
            try
            {
                if (actionExecutedContext != null)
                {
                    bool? isSuccess = false;
                    IEnumerable<string> storeKey = null;
                    if (actionExecutedContext.Request.Headers.TryGetValues("x-api-key", out storeKey))
                    {
                        var id = actionExecutedContext.Request.Properties["RequestResponseId"].ToString();
                        string response = string.Empty;
                        if (actionExecutedContext.Response != null)
                        {
                            response = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                        }
                        if (!string.IsNullOrWhiteSpace(response))
                        {
                            var status = GetSuccessValue(response);
                            if (status != null)
                            {
                                isSuccess = status.Equals("true", comparisonType: StringComparison.InvariantCultureIgnoreCase) ? true : false;
                            }
                            else
                            {
                                isSuccess = null;
                            }
                        }
                        else
                        {
                            isSuccess = false;
                            if (actionExecutedContext.Exception != null)
                            {
                                response = actionExecutedContext.Exception.ToString();
                            }
                        }
                        store = StoreService.GetStoreByKey(storeKey.FirstOrDefault());
                        LoggingDAL loggingDAL = new LoggingDAL(storeKey.FirstOrDefault());
                        loggingDAL.LogResponse(id, isSuccess, response);
                    }

                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp,store != null ? store.StoreId : 0, store != null ? store.CreatedBy : string.Empty);
            }
        }
        private void LogRequestResponse(HttpActionExecutedContext actionExecutedContext)
        {
            StoreDto store = null;
            try
            {
                if (actionExecutedContext != null)
                {
                    DateTime dateTime = DateTime.UtcNow;
                    string identifierKey = null;
                    string identifierValue = null;
                    string httpMethod = actionExecutedContext.Request.Method.Method;
                    string salesOrderRequest = null;
                    IEnumerable<string> storeKey = null;
                    
                    var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
                    string requestBody = "";
                    LogMapper logMapper = null;
                    bool? isSuccess = false;

                    // Extract the EcomTransactionId from Header
                    IEnumerable<string> ecomTransactionIds = null;
                    string ecomTransactionId = string.Empty;
                    actionExecutedContext.Request.Headers.TryGetValues("TransactionId", out ecomTransactionIds);
                    if (ecomTransactionIds != null && ecomTransactionIds.Count() > 0)
                    {
                        ecomTransactionId = ecomTransactionIds.First();
                    }

                    foreach (var argument in actionExecutedContext.ActionContext.ActionArguments)
                    {
                        requestBody += argument.Value == null ? "" : JsonConvert.SerializeObject(argument.Value);
                        requestBody += Environment.NewLine;
                        if (argument.Key == identifierKey)
                        {
                            identifierValue = argument.Value?.ToString();
                        }
                    }
                    if (actionName == "CreateSalesOrderTransaction")
                    {
                        salesOrderRequest = requestBody;
                        if (!string.IsNullOrWhiteSpace(requestBody) && requestBody != "null")
                        {
                            JObject salesOrder = JsonConvert.DeserializeObject<JObject>(requestBody);
                            requestBody = salesOrder["salesOrderJSON"].ToString();
                        }
                    }
                    else if (actionName == "CreateMergeSalesOrderTransaction")
                    {
                        salesOrderRequest = requestBody;
                        if (!string.IsNullOrWhiteSpace(requestBody) && requestBody != "null")
                        {
                            JObject salesOrder = JsonConvert.DeserializeObject<JObject>(requestBody);
                            requestBody = salesOrder["SalesOrder"]["salesOrderJSON"].ToString();
                        }
                    }
                    if (actionExecutedContext.Request.Headers.TryGetValues("x-api-key", out storeKey))
                    {
                        if (storeKey != null)
                        {
                            logMapper = LogMapperHelper.GetLogSettings(actionName);
                            identifierKey = logMapper?.IdentifierKey;

                            string response = string.Empty;
                            if (actionExecutedContext.Response != null)
                            {
                                response = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                            }
                            if (!string.IsNullOrWhiteSpace(response))
                            {
                                var status = GetSuccessValue(response);
                                if (status != null)
                                {
                                    isSuccess = status.Equals("true", comparisonType: StringComparison.InvariantCultureIgnoreCase) ? true : false;
                                }
                                else
                                {
                                    isSuccess = null;
                                }
                            }
                            else
                            {
                                isSuccess = false;
                                if (actionExecutedContext.Exception != null)
                                {
                                    response = actionExecutedContext.Exception.ToString();
                                }
                            }
                            store = StoreService.GetStoreByKey(storeKey.FirstOrDefault());

                            RequestResponse requestResponseLog = new RequestResponse();
                            requestResponseLog.CreatedBy = store.CreatedBy;
                            requestResponseLog.Description = GetRequestGUID(actionExecutedContext.Request);
                            requestResponseLog.EcomTransactionId = ecomTransactionId;
                            requestResponseLog.DataDirectionId = (int)DataDirectionType.EcomRequestToCL;
                            requestResponseLog.IdentifierKey = identifierKey;
                            requestResponseLog.IdentifierValue = httpMethod.Equals("GET", StringComparison.InvariantCultureIgnoreCase) ? identifierValue : GetIdentifierValue(requestBody, logMapper?.IdentifierPath);
                            requestResponseLog.MethodName = actionName;
                            requestResponseLog.DataPacket = (actionName.Equals("CreateSalesOrderTransaction", StringComparison.InvariantCultureIgnoreCase) || actionName.Equals("CreateMergeSalesOrderTransaction", StringComparison.InvariantCultureIgnoreCase)) ? salesOrderRequest : requestBody;
                            requestResponseLog.CreatedOn = actionExecutedContext.Request.Properties.ContainsKey("CreatedOn") ? (DateTime)actionExecutedContext.Request.Properties["CreatedOn"] : DateTime.UtcNow;
                            requestResponseLog.RequestInitiatedIP = GetClientIp(actionExecutedContext.Request);
                            requestResponseLog.StoreId = store.StoreId;
                            requestResponseLog.OutputPacket = response;
                            requestResponseLog.OutputSentAt = dateTime;
                            var timespan = (requestResponseLog.OutputSentAt - requestResponseLog.CreatedOn);
                            requestResponseLog.TotalProcessingDuration = timespan.HasValue ? Convert.ToDecimal(timespan.Value.TotalMilliseconds) : 0;
                            requestResponseLog.IsSuccess = isSuccess;
                            
                            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500006, GetRequestGUID(actionExecutedContext.Request), actionExecutedContext.ActionContext.ActionDescriptor.ActionName, DateTime.UtcNow);

                            Task.Factory.StartNew(() => { LogRequestResponseAsync(requestResponseLog, store, storeKey.FirstOrDefault()); });
                            
                            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500007, GetRequestGUID(actionExecutedContext.Request), actionExecutedContext.ActionContext.ActionDescriptor.ActionName, DateTime.UtcNow);


                            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500007, GetRequestGUID(actionExecutedContext.Request), actionExecutedContext.ActionContext.ActionDescriptor.ActionName, DateTime.UtcNow);

                        }
                    }
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, store != null ? store.StoreId : 0, store != null ? store.CreatedBy : string.Empty);
            }
        }

        public void LogRequestResponseAsync(RequestResponse requestResponseLog, StoreDto store, string storeKey)
        {
            try
            {
                LoggingDAL loggingDAL = new LoggingDAL(storeKey);
                loggingDAL.LogRequestResponse(requestResponseLog);
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, store != null ? store.StoreId : 0, store != null ? store.CreatedBy : string.Empty);
            }
        }

        private string GetClientIp(HttpRequestMessage request)
        {
            if (request != null)
            {

                var xforwardForIP = GetXForwardForIP(request);
                if (!string.IsNullOrWhiteSpace(xforwardForIP))
                {
                    return "x-" + xforwardForIP;
                }
                else if (request.Properties.ContainsKey("MS_HttpContext"))
                {
                    return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                }
                else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
                {
                    RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                    return prop.Address;
                }
                else if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Request.UserHostAddress;
                }
                else
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public static string GetXForwardForIP(HttpRequestMessage request)
        {
            var ip = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null
                      && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (ip.Contains(","))
                ip = ip.Split(',').First().Trim();
            return ip;
        }

        private string GetIdentifierValue(string requestBody, string identifierPath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(requestBody) && requestBody != "null")
                {
                    requestBody = requestBody.Trim();
                    JObject jObject = JObject.Parse(requestBody);
                    JToken jToken = jObject.SelectToken(identifierPath);
                    return jToken?.ToString();
                }
                return null;
            }
            catch (Exception exception)
            {
                return null;
            }
        }
        private string GetSuccessValue(string response)
        {
            var status = JsonConvert.DeserializeObject<JObject>(response).GetValue("status");
            if (status == null)
            {
                status = JsonConvert.DeserializeObject<JObject>(response).GetValue("Status");
            }
            if (status == null)
            {
                status = JsonConvert.DeserializeObject<JObject>(response).GetValue("Success");
            }
            if (status != null)
            {
                return status.ToString();
            }
            return null;
        }

        public string GetRequestGUID(HttpRequestMessage request)
        {
            if (request != null)
            {
                if (request.Properties.ContainsKey("RequestId"))
                {
                    return request.Properties["RequestId"].ToString();
                }
            }
            return string.Empty;
        }
        public void SetRequestIdNewrelic(string requestId)
        {
            try
            {
                var agent = NewRelic.Api.Agent.NewRelic.GetAgent();
                ITransaction transaction = agent.CurrentTransaction;
                transaction.AddCustomAttribute("RequestId", requestId);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
