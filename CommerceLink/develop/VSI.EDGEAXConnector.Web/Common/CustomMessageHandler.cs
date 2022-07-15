using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using Newtonsoft.Json;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Web.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiCustomMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // Get request ID from request
            var requestId = "";
            if (request.Properties.ContainsKey("RequestId"))
            {
                requestId = request.Properties["RequestId"].ToString();
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                request.Properties.Remove(HttpPropertyKeys.NoRouteMatched);
                return request.CreateResponse(response.StatusCode, new ApplicationResponse(false, "Resource not found", null));
            }

            
            bool.TryParse(Convert.ToString(ConfigurationManager.AppSettings["DisableCustomMessage"]), 
                out bool disableCustomMessage);

            //In case custom messages are disabled or response is not error response, return actual response.
            if (response.StatusCode != HttpStatusCode.InternalServerError || disableCustomMessage) return response;
            {
                var message = "An unexpected error has occurred while processing the request.";

                var exception = response.Content.ReadAsAsync<HttpError>(cancellationToken).Result;
                var tempException = exception;

                do
                {
                    // In case of CommerceLinkError return actual exception message.
                    if (tempException.ExceptionType == typeof(CommerceLinkError).ToString())
                    {
                        return request.CreateResponse(HttpStatusCode.OK, new ApplicationResponse(false, tempException.ExceptionMessage, ""));
                    }

                    tempException = tempException.InnerException;

                } while (tempException != null);

                
                request.Properties.Remove(HttpPropertyKeys.IncludeErrorDetailKey);

                try
                {
                    message = CommerceLinkLogger.LogException(1, "Global Handler", JsonConvert.SerializeObject(exception), requestId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                return request.CreateResponse(HttpStatusCode.OK, new ApplicationResponse(false, message, null));

            }
        }


        /// <summary>
        /// ApplicationResponse a generic response type 
        /// </summary>
        public class ApplicationResponse
        {
            /// <summary>
            /// Contructor
            /// </summary>
            /// <param name="success"></param>
            /// <param name="message"></param>
            /// <param name="data"></param>
            public ApplicationResponse(bool success, string message, string data)
            {
                Success = success;
                Message = message;
                Data = data;
            }
            /// <summary>
            /// Success
            /// </summary>
            public bool Success { get; set; }
            /// <summary>
            /// Message
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// Data
            /// </summary>
            public string Data { get; set; }
        }
    }

    public class ReturnMessage
    {
        public ReturnMessage(string message)
        {
            Message = message;
        }

        public ReturnMessage()
        {

        }
        public string Message { get; set; }
    }
}