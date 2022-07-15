using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Logging;

namespace VSI.CommerceLink.ThirdPartyAdapter.Controllers
{
    public class BaseController
    {
        #region Data Members
        internal ConfigurationHelper configurationHelper;
        public StoreDto currentStore = null;
        public IntegrationManager IntegrationManager = null;

        public string thirdPartyApiUrl = string.Empty;
        public string thirdPartyApiKey = string.Empty;
        public string thirdPartyApiLimit = string.Empty;
        #endregion

        #region Constructor
        public BaseController(string storeKey)
        {
            this.configurationHelper = new ConfigurationHelper(storeKey);
            this.currentStore = StoreService.GetStoreByKey(storeKey);
            IntegrationManager = new IntegrationManager(storeKey);
            thirdPartyApiUrl = this.configurationHelper.GetSetting(INGRAM.API_URL);
            thirdPartyApiKey = this.configurationHelper.GetSetting(INGRAM.API_Key);
            thirdPartyApiLimit = this.configurationHelper.GetSetting(INGRAM.API_Data_Limit);
        }
        #endregion

        public void Dispose()
        {
        }
    }
}
