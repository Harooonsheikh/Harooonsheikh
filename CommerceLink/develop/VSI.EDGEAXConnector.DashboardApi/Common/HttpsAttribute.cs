using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace VSI.EDGEAXConnector.DashboardApi.Common
{

    /// <summary>
    /// HttpsAttribute class provides details for authorization filter for HTTPS.
    /// </summary>
    public class HttpsAttribute : AuthorizationFilterAttribute
    {

        #region Public Methods

        /// <summary>
        /// OnAuthorization is called when a process requests authorization.
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "HTTPS Required"
                };
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }

        #endregion

    }
}