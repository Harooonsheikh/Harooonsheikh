using EdgeAXCommerceLink.Commerce.RetailProxy;
using EdgeAXCommerceLink.Commerce.RetailProxy.Authentication;
using System;
using System.Net;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Configuration;

namespace VSI.EDGEAXConnector.CRTAX7.Controllers
{
    public class BaseController
    {
        internal ManagerFactory RPFactory;
        internal PagingInfo Paging_0_1000;
        internal SortingInfo SortingChannel;
        private Uri retailServerUri;
        private string operatingUnitNumber;
        //private string RetailServerSpn;
        //private object authority;
        //private String clientId;
        //private String clientSecret;

        static protected long baseChannelId = 0;

        public BaseController()
        {
            try
            {
                // Disable the Certificate
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                retailServerUri = new Uri(ConfigurationHelper.GetSetting(APPLICATION.ERP_AX_RetailServerUri));
                operatingUnitNumber = ConfigurationHelper.GetSetting(APPLICATION.ERP_AX_OUN);


                RPFactory = CreateManagerFactory(false); // change to async Result when we get token

                if (baseChannelId == 0)
                {
                    var oum = RPFactory.GetManager<IOrgUnitManager>();
                    baseChannelId = Task.Run(async () => await oum.GetOrgUnitConfiguration()).Result.RecordId;
                }

                Paging_0_1000 = new PagingInfo();
                Paging_0_1000.Skip = 0;
                Paging_0_1000.Top = 1000;

                SortingChannel = new SortingInfo();
                var one = new SortColumn();
                one.ColumnName = "ONLINECHANNELNAME";
                SortingChannel.Columns = new System.Collections.ObjectModel.ObservableCollection<SortColumn> { one };

                // Initialize the Mapper
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTAX7.ErpToRSMappingConfiguration();
            }

            catch(Exception exp)
            {
                throw exp;

            }
        }

        private ManagerFactory CreateManagerFactory(Boolean authenticated)
        {

/*            if (authenticated)  -- change to Async when we get token
            { 
                AuthenticationContext authenticationContext = new AuthenticationContext(this.authority.ToString(), false);
                AuthenticationResult authResult = null;
                authResult = await authenticationContext.AcquireTokenAsync(RetailServerSpn, new ClientCredential(this.clientId, this.clientSecret));
                // System.Console.WriteLine(authResult.AccessToken);
                ClientCredentialsToken clientCredentialsToken = new ClientCredentialsToken(authResult.AccessToken);
                retailServerContext = RetailServerContext.Create(this.retailServerUri, this.operatingUnitNumber, clientCredentialsToken);
                
            } else
            {
                retailServerContext = ;
            }
*/

            return ManagerFactory.Create(RetailServerContext.Create(this.retailServerUri, this.operatingUnitNumber));
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
    

