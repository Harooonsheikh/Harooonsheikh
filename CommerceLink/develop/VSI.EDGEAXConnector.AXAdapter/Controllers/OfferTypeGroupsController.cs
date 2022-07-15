//using Autofac;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    /// <summary>
    /// OfferTypeGroupsController class provides features to get Offer Type groups  from CRT.
    /// </summary>
    public class OfferTypeGroupsController : BaseController,IOfferTypeGroupController
    {
        #region Public Methods
        public OfferTypeGroupsController(string storeKey) : base(storeKey)
        {

        }

        public ERPOfferTypeGroupsResponse GetERPOfferTypeGroups()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var offerTypeGroupsManager = new OfferTypeGroupsCRTManager();
            var offerTypeGroups = offerTypeGroupsManager.GetOfferTypeGroups(currentStore.StoreKey);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return offerTypeGroups;
        }
        #endregion
    }
}
