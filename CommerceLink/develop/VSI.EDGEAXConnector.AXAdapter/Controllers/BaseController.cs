using MapsterMapper;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{

    /// <summary>
    /// BaseController class is base controller for AX.
    /// </summary>
    public class BaseController
    {

        #region Data Members
        protected StoreDto currentStore { get; set; }
        internal ConfigurationHelper configurationHelper;
        protected IMapper _mapper;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseController(string storeKey)
        {
            currentStore = StoreService.GetStoreByKey(storeKey);
            this.configurationHelper = ConfigurationHelper.GetConfigurationHelperInstanceByStore(storeKey);
            InitializeMapper();
        }

        #endregion

        #region Private Methods

        private void InitializeMapper()
        {
            _mapper = AutoMapBootstrapper.MapperInstance;
        }
        #endregion

    }
}
