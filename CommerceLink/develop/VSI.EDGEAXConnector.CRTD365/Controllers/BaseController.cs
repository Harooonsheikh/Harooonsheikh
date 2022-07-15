using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using VSI.EDGEAXConnector.AXCommon;
using System.Net;
using System.Text;
using VSI.EDGEAXConnector.Logging;
using Microsoft.Dynamics.Commerce.RetailProxy;
using Microsoft.Dynamics.Commerce.RetailProxy.Authentication;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.Data.DTO;
using NewRelic.Api.Agent;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class BaseController
    {
        internal ManagerFactory RPFactory;
        internal static PagingInfo Paging_0_1000 = new PagingInfo();
        internal static SortingInfo SortingChannel = new SortingInfo();
        protected string operatingUnitNumber;
        protected long baseChannelId = 0;
        protected string baseCompany = "";
        protected string baseInventLocation = "";
        protected string ChannelCurrencyCode = string.Empty;
        protected List<String> companyLanguagesList;
        protected string defaultLanguage;
        internal ConfigurationHelper configurationHelper;
        public StoreDto currentStore = null;
        public string ChannelNaturalId = string.Empty;
        public static Dictionary<string, ChannelConfiguration> channelConfigurationsDict = new Dictionary<string, ChannelConfiguration>();
        public static AuthenticationResult AuthResult = null;
        private static IEdmModelExtension[] edmModelExtension = null;
        private static readonly System.Collections.ObjectModel.ObservableCollection<SortColumn> column =
            new System.Collections.ObjectModel.ObservableCollection<SortColumn> { new SortColumn { ColumnName = "ONLINECHANNELNAME" } };

        protected MapsterMapper.IMapper _mapper;
        public Stopwatch timer;
        public BaseController(string storeKey)
        {
            RetailServerContext.Initialize(new IEdmModelExtension[] { new EdgeAXCommerceLink.RetailProxy.Extensions.EdmModel() });

            currentStore = Data.StoreService.GetStoreByKey(storeKey);

            configurationHelper = ConfigurationHelper.GetConfigurationHelperInstanceByStore(storeKey);
            var retailServerUri = new Uri(configurationHelper.GetSetting(APPLICATION.ERP_AX_RetailServerUri));
            operatingUnitNumber = configurationHelper.GetSetting(APPLICATION.ERP_AX_OUN);

            RPFactory = CreateManagerFactory(false, retailServerUri);

            if (channelConfigurationsDict.ContainsKey(operatingUnitNumber))
            {
                var channelConfigurations = channelConfigurationsDict[operatingUnitNumber];
                SetChannelConfigurations(channelConfigurations);
            }
            else
            {
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, string.Empty, "GetOrgUnitConfiguration", DateTime.UtcNow);
                ChannelConfiguration channelConfigurations;
                try
                {
                    channelConfigurations = GetOrgUnitConfiguration();
                }
                catch (Exception e)
                {
                    throw new Exception(GetInnerErrors(e));
                }

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, string.Empty, "GetOrgUnitConfiguration", DateTime.UtcNow);

                lock (channelConfigurationsDict)
                {
                    if (!channelConfigurationsDict.ContainsKey(operatingUnitNumber))
                    {
                        channelConfigurationsDict.Add(operatingUnitNumber, channelConfigurations);
                    }
                }

                SetChannelConfigurations(channelConfigurations);
            }

            Paging_0_1000.Skip = 0;
            Paging_0_1000.Top = 1000;
            SortingChannel.Columns = column;

        }

        private void SetChannelConfigurations(ChannelConfiguration channelConfigurations)
        {
            baseChannelId = channelConfigurations.RecordId;
            baseCompany = channelConfigurations.InventLocationDataAreaId;
            baseInventLocation = channelConfigurations.InventLocation;
            ChannelCurrencyCode = channelConfigurations.Currency;
            ChannelNaturalId = channelConfigurations.ChannelNaturalId;
            companyLanguagesList = new List<String>();

            if (channelConfigurations.Languages != null)
            {
                for (int i = 0; i < channelConfigurations.Languages.Count; i++)
                {
                    companyLanguagesList.Add(channelConfigurations.Languages[i].LanguageId);
                }
            }
            _mapper = AutoMapBootstrapper.MapsterInstance;

            defaultLanguage = channelConfigurations.DefaultLanguageId;
        }
        private ManagerFactory CreateManagerFactory(Boolean authenticated, Uri retailServerUri)
        {

            var isAppMode = Boolean.Parse(configurationHelper.GetSetting(APPLICATION.IsApplicationMode));
            if (isAppMode)
            {
                var clientId = configurationHelper.GetSetting(APPLICATION.Client_Id);
                var clientSecret = configurationHelper.GetSetting(APPLICATION.Client_Secret);
                var azureActiveDirectory = configurationHelper.GetSetting(APPLICATION.Azure_Active_Directory);
                var D365Url = configurationHelper.GetSetting(APPLICATION.D365_Machine_Url);
                AuthenticationResult authResult = null;
                if (AuthResult != null)
                {
                    bool.TryParse(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["isCacheDisable"]), out bool isCacheDisable);
                    var expireMints = (AuthResult.ExpiresOn - DateTime.UtcNow).Minutes;

                    // Generate new token if Chche Disabled or token expiration before 10 mints
                    if (isCacheDisable || expireMints < 10)
                    {
                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, string.Empty, "AcquireTokenAsync", DateTime.UtcNow);
                        authResult = AcquireToken(azureActiveDirectory, D365Url, clientId, clientSecret);
                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, string.Empty, "AcquireTokenAsync", DateTime.UtcNow);
                        AuthResult = authResult;
                    }
                    authResult = AuthResult;
                }
                else
                {
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, string.Empty, "AcquireTokenAsync", DateTime.UtcNow);
                    authResult = AcquireToken(azureActiveDirectory, D365Url, clientId, clientSecret);
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, string.Empty, "AcquireTokenAsync", DateTime.UtcNow);

                    AuthResult = authResult;
                }


                var allowSecurityProtocol = bool.Parse(configurationHelper.GetSetting(APPLICATION.Allow_Security_Protocols));

                if (allowSecurityProtocol)
                {
                    // Allow different security protocols. Normally this is not required for Dev enviornments but sometimes required for more secure production envviornments
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                | SecurityProtocolType.Tls11
                                | SecurityProtocolType.Tls12
                                | SecurityProtocolType.Ssl3;
                }

                ClientCredentialsToken clientCredentialsToken = new ClientCredentialsToken(authResult.AccessToken);
                RetailServerContext retailServerContext = RetailServerContext.Create(retailServerUri, this.operatingUnitNumber, clientCredentialsToken);
                return ManagerFactory.Create(retailServerContext);
            }
            else
            {
                return ManagerFactory.Create(RetailServerContext.Create(retailServerUri, this.operatingUnitNumber));
            }
        }
        [Trace]
        public AuthenticationResult AcquireToken(string azureActiveDirectory, string D365Url, string clientId, string clientSecret)
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(azureActiveDirectory, false);
            return Task.Run(async () => await authenticationContext.AcquireTokenAsync(D365Url, new ClientCredential(clientId, clientSecret))).Result;
        }
        [Trace]
        public ChannelConfiguration GetOrgUnitConfiguration()
        {
            var oum = RPFactory.GetManager<IOrgUnitManager>();
            return Task.Run(async () =>
            {
                try
                {
                    return await oum.GetOrgUnitConfiguration();
                }
                catch (RetailProxyException e)
                {
                    var errorJson = e.InnerException?.InnerException?.Message;
                    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(errorJson);
                    dynamic errorObj = Newtonsoft.Json.JsonConvert.DeserializeObject((string)jsonObj.Exception);
                    throw new Exception((string)errorObj.LocalizedMessage);
                }
                catch (Exception e)
                {
                    throw new Exception(GetInnerErrors(e));
                }

            }).Result;
        }

        protected string GetInnerErrors(Exception e)
        {
            Exception exp = e;
            StringBuilder sb = new StringBuilder();
            while (exp != null)
            {
                sb.Append(exp.Message);
                exp = exp.InnerException;
            }

            return sb.ToString();
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
            else
            {
                return 0;
            }
        }
    }


    /**
     * To be used when authentication is needed
     */
    public class ClientCredentialsToken : UserToken
    {
        /// <summary>
        /// The identifier token scheme name.
        /// </summary>
        internal const string ClientCredentialsSchemeName = "bearer";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCredentialsToken"/> class.
        /// </summary>
        /// <param name="clientCredentialsToken">The token corresponding to Client Credentials Grant Flow.</param>
        public ClientCredentialsToken(string clientCredentialsToken) : base(clientCredentialsToken, ClientCredentialsSchemeName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCredentialsToken"/> class.
        /// </summary>
        protected ClientCredentialsToken() : base(ClientCredentialsSchemeName)
        {
        }
    }
}


