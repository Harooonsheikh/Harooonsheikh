using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using NewRelic.Api.Agent;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;
using VSI.EDGEAXConnector.Data.DTO;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class AosBaseController
    {
        internal ConfigurationHelper ConfigurationHelper;
        public StoreDto CurrentStore;
        public static AuthenticationResult AuthResult;
        private IRestClient RestClient;
        protected string AosUrl;
        public string RequestId;

        //protected IMapper _mapper;
        public Stopwatch timer;

        public AosBaseController(string storeKey)
        {
            //++initializing store and custom logger
            CurrentStore = StoreService.GetStoreByKey(storeKey);
            ConfigurationHelper = ConfigurationHelper.GetConfigurationHelperInstanceByStore(storeKey);
            AosUrl = ConfigurationHelper.GetSetting(APPLICATION.AOS_Url);

            RestClient = new RestClient
            {
                BaseUrl = new Uri($"{AosUrl.TrimEnd('/')}/"),
                Authenticator = new JwtAuthenticator(GetAuthToken()),
                FailOnDeserializationError = true
            };
        }

        private string GetAuthToken()
        {

            var isAppMode = Boolean.Parse(ConfigurationHelper.GetSetting(APPLICATION.IsApplicationMode));
            if (isAppMode)
            {
                var allowSecurityProtocol = false;
                var clientId = ConfigurationHelper.GetSetting(APPLICATION.Client_Id);
                var clientSecret = ConfigurationHelper.GetSetting(APPLICATION.Client_Secret);
                var azureActiveDirectory = ConfigurationHelper.GetSetting(APPLICATION.Azure_Active_Directory);

                
                AuthenticationResult authResult = null;
                if (AuthResult != null)
                {
                    var expired = DateTimeOffset.Compare(DateTimeOffset.UtcNow, AuthResult.ExpiresOn);
                    if (expired > 0)
                    {
                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, CurrentStore, string.Empty, "AcquireTokenAsync", DateTime.UtcNow);
                        authResult = AcquireTokenAsync(azureActiveDirectory, clientId, clientSecret);
                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, CurrentStore, string.Empty, "AcquireTokenAsync", DateTime.UtcNow);
                        AuthResult = authResult;
                    }
                    authResult = AuthResult;
                }
                else
                {
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, CurrentStore, string.Empty, "AcquireTokenAsync", DateTime.UtcNow);
                    authResult = AcquireTokenAsync(azureActiveDirectory, clientId, clientSecret);
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, CurrentStore, string.Empty, "AcquireTokenAsync", DateTime.UtcNow);

                    AuthResult = authResult;
                }


                allowSecurityProtocol = bool.Parse(ConfigurationHelper.GetSetting(APPLICATION.Allow_Security_Protocols));

                if (allowSecurityProtocol)
                {
                    // Allow different security protocols. Normally this is not required for Dev enviornments but sometimes required for more secure production envviornments
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                | SecurityProtocolType.Tls11
                                | SecurityProtocolType.Tls12
                                | SecurityProtocolType.Ssl3;
                }

                return AuthResult.AccessToken;

            }

            throw new CommerceLinkError("Application Error: Only App Mode is supported in CL");

        }

        protected double GetElapsedTime()
        {
            if (timer != null)
            {
                if (timer.IsRunning)
                {
                    timer.Stop();
                }

                return timer.ElapsedMilliseconds;
            }

            return 0;
        }

        protected async Task<TOut> SendPostRequestAsync<TOut>(AosMethod method, object requestBody = null)
        {
            var path = $"{RestClient.BaseUrl}{AosUrlManager.GetUrl(method)}";
            var request = new RestRequest(path, Method.POST);
            request.AddHeader("Content-Type", "application/json");

            if (requestBody != null)
            {
                request.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), ParameterType.RequestBody);
            }

            timer = Stopwatch.StartNew();

            var response = Execute(request);

            CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, CurrentStore, this.RequestId,
                method.ToString(), GetElapsedTime());

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<TOut>(response.Content);
            }

            string message = string.Format(CultureInfo.InvariantCulture,
                CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL400015),
                $" {response.StatusDescription} | {response.ErrorMessage}. ");

            throw new CommerceLinkError(message);

        }

        protected string SendPostRequest(AosMethod method, object requestBody = null)
        {
            var path = $"{RestClient.BaseUrl}{AosUrlManager.GetUrl(method)}";
            var request = new RestRequest(path, Method.POST);
            request.AddHeader("Content-Type", "application/json");

            if (requestBody != null)
            {
                request.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), ParameterType.RequestBody);
            }

            // timer = Stopwatch.StartNew(); // TODO: Implement calculation for execution time

            var response = Execute(request);
            if (response.IsSuccessful)
            {
                return response.Content;
            }

            string message = string.Format(CultureInfo.InvariantCulture,
                CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL400015),
                $" {response.StatusDescription} | {response.ErrorMessage}. ");

            throw new CommerceLinkError(message);

        }

        #region External API
        [Trace]
        private IRestResponse Execute(RestRequest request)
        {
            var response = RestClient.Execute(request);
            return response;
        }
        [Trace]
        private AuthenticationResult AcquireTokenAsync(string azureActiveDirectory, string clientId, string clientSecret)
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(azureActiveDirectory, false);
            return Task.Run(async () =>
                await authenticationContext.AcquireTokenAsync(AosUrl, new ClientCredential(clientId, clientSecret))).Result;
        }

        #endregion
    }
}
