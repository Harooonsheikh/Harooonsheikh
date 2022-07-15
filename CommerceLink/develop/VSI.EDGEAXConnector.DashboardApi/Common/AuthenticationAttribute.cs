using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.DashboardApi.Common
{

    /// <summary>
    /// RequireHttpsAttribute class provides details for authorization filter for HTTPS.
    /// </summary>
    public class AuthenticationAttribute : Attribute, IAuthenticationFilter
    {

        #region Public Methods

        /// <summary>
        /// AuthenticateAsync Authenticates the request.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // 1. Look for credentials in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;
            

            String staticKeyValue = ConfigurationManager.AppSettings["apiKeyECommerce"];
            String apiKeyValue = "";

            // 2. If there are no credentials, do nothing.
            if (authorization == null)
            {
                try
                {
                    IEnumerable<String> apiKey = request.Headers.GetValues("x-api-key");
                    if (apiKey == null || !apiKey.GetEnumerator().MoveNext()) // will throw exception if no element found
                    { // empty {
                        return; // no auth
                    }
                    else
                    {
                        //apiKeyValue = apiKey.GetEnumerator().Current;
                        var apiKeyValues = apiKey.GetEnumerator();
                        apiKeyValues.MoveNext();
                        apiKeyValue = apiKeyValues.Current;
                    }
                }
                catch (System.InvalidOperationException)
                {
                    return;
                }

                // lookup from static access to web.config
                if (apiKeyValue == staticKeyValue)
                {
                    IIdentity identity = new GenericIdentity(GetUserIP(), "ClientSystem");  // remote IP address of eCom system
                    // TODO check against whitelist IP addresses
                    IPrincipal principal = new GenericPrincipal(identity, new string[] { "eCommerce" });
                    context.Principal = principal;
                    return;
                } else
                {
                    //CustomLogger logger = new CustomLogger();
                    //logger.LogWarn("Wrong API key received:" + apiKeyValue); //TODO:Shan
                    return;
                }
            }

            // 3. If there are credentials but the filter does not recognize the 
            //    authentication scheme, do nothing.
            if (authorization.Scheme != "Basic")
            {
                return;
            }

            // 4. If there are credentials that the filter understands, try to validate them.
            // 5. If the credentials are bad, set the error result.
            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            //Tuple<string, string> userNameAndPasword = ExtractUserNameAndPassword(authorization.Parameter);
            //if (userNameAndPasword == null)
            //{
            //    context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
            //}

            //string userName = userNameAndPasword.Item1;
            //string password = userNameAndPasword.Item2;

            //IPrincipal principal = await AuthenticateAsync(userName, password, cancellationToken);
            //if (principal == null)
            //{
            //    context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
            //}

            //// 6. If the credentials are valid, set principal.
            //else
            //{
            //    context.Principal = principal;
            //}

            await Task.FromResult(0); // to resolve this Warning (This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls,
            //    or 'await Task.Run(...)' to do CPU-bound work on a background thread)

        }

        /// <summary>
        /// It challenge requests.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, System.Threading.CancellationToken cancellationToken)
        {
        //    //var challenge = new AuthenticationHeaderValue("Basic");
        //    //context.Result = new ChallengeUnauthorizedResult(challenge, context.Result);
        //    //return Task.FromResult(0);

            await Task.FromResult(0);  // to resolve this Warning (This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls,
            //    or 'await Task.Run(...)' to do CPU-bound work on a background thread)
       

        }

        /// <summary>
        /// This function allows multiple.
        /// </summary>
        public bool AllowMultiple
        {
            get { return false; }
        }

        #endregion

        #region Private Methods

        private string GetUserIP()
        {
            System.Web.HttpContext c = System.Web.HttpContext.Current;
            string ipList = c.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return c.Request.ServerVariables["REMOTE_ADDR"];
        }

        #endregion
    }
}