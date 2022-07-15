using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Hosting;
using Newtonsoft.Json;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.DashboardApi.CustomMessageHandler
{
    public class WebApiCustomMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            
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
                        return request.CreateResponse(HttpStatusCode.BadRequest, new ApplicationResponse(false, tempException.ExceptionMessage, ""));
                    }

                    tempException = tempException.InnerException;

                } while (tempException != null);


                request.Properties.Remove(HttpPropertyKeys.IncludeErrorDetailKey);
                
                return request.CreateResponse(HttpStatusCode.BadRequest, new ApplicationResponse(false, message, null));

            }
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