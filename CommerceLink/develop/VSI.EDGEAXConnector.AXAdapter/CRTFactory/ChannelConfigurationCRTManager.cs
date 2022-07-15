using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    /// <summary>
    /// ChannelConfigurationCRTManager class implements the CRT Manager to manage crt ax controller.
    /// </summary>
    public class ChannelConfigurationCRTManager
    {
        #region Properties
        private readonly ICRTFactory _crtFactory;
        #endregion Properties

        #region Constructor      
        public ChannelConfigurationCRTManager()
        {
            _crtFactory = new CRTFactory();
        }
        #endregion Constructor

        #region Public Methods
        /// <summary>
        /// GetRetailServiceProfile resolves CRT Controller to call it's GetRetailServiceProfile method.
        /// </summary>
        /// <returns></returns>
        public ErpRetailServiceProfileResponse GetRetailServiceProfile(string storeKey)
        {
            var configurationController = _crtFactory.CreateConfigurationController(storeKey);

            return configurationController.GetRetailServiceProfile();
        }

        /// <summary>
        /// GetRetailChannelProfile resolves CRT Controller to call it's GetRetailChannelProfile method.
        /// </summary>
        /// <returns></returns>
        public ErpRetailChannelProfileResponse GetRetailChannelProfile(string storeKey)
        {
            var configurationController = _crtFactory.CreateConfigurationController(storeKey);

            return configurationController.GetRetailChannelProfile();
        }

        /// <summary>
        /// GetChannelInformation get channel information of currently configured store.
        /// </summary>
        /// <returns></returns>
        public ErpChannel GetChannelInformation(string storeKey)
        {
            var configurationController = _crtFactory.CreateConfigurationController(storeKey);

            return configurationController.GetChannelInformation();
        }
        #endregion
    }
}
