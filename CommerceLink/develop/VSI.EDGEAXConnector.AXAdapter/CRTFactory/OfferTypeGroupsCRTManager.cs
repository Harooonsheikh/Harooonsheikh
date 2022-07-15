using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    /// <summary>
    /// OfferTypeGroupsCRTManager class implements the CRT Manager to manage crt ax controller.
    /// </summary>
    public class OfferTypeGroupsCRTManager
    {
        #region Properties
        private readonly ICRTFactory _crtFactory;
        #endregion Properties

        #region Constructor      
        public OfferTypeGroupsCRTManager()
        {
            _crtFactory = new CRTFactory();
        }
        #endregion Constructor

        #region Public Methods
        /// <summary>
        /// GetOfferTypeGroups resolves CRT Controller to call it's GetOfferTypeGroups method.
        /// </summary>
        /// <returns></returns>
        public ERPOfferTypeGroupsResponse GetOfferTypeGroups(string storeKey)
        {
            var offerTypeGroupController = _crtFactory.CreateOfferTypeGroupController(storeKey);
            return offerTypeGroupController.GetOfferTypeGroups();
        }

     
        #endregion
    }
}
