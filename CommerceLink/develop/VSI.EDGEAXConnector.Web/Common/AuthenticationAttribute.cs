using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Web.Common
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
            AuthenticationHeaderValue authorization = context.Request.Headers.Authorization;

            String staticKeyValue = "";
            String apiKeyValue = "";
            StoreDto axStore = null;

            // 2. If there are no credentials, do nothing.
            if (authorization == null)
            {
                try
                {
                    IEnumerable<String> apiKey = null ;
                    if (context.Request.Headers.TryGetValues("x-api-key", out apiKey))
                    {
                        apiKeyValue = apiKey.FirstOrDefault();
                    }
                    else
                    {
                        return; // no auth
                    }
                }
                catch (System.InvalidOperationException)
                {
                    return;
                }

                try
                {

                    //++ Veriyfing Store Id exists
                     axStore = StoreService.GetStoreByKey(apiKeyValue);
                    // lookup from static access to web.config
                    if (axStore != null && apiKeyValue == axStore.StoreKey)
                    {
                        IIdentity identity = new GenericIdentity(GetUserIP(), "ClientSystem");  // remote IP address of eCom system
                        IPrincipal principal = new GenericPrincipal(identity, new string[] { "eCommerce" });
                        context.Principal = principal;
                        return;
                    }
                    else
                    {
                        CustomLogger.LogFatal("Wrong API key received:" + apiKeyValue, 1 , "System");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
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
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", context.Request);
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