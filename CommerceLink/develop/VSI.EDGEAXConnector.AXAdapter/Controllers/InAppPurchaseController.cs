//using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Enums.Enums.TMV;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class InAppPurchaseController : BaseController, IInAppPurchaseController
    {
        #region Public Methods

        public InAppPurchaseController(string storyKey) : base(storyKey)
        {
        }

        /// <summary>
        /// AutoRenew Contract
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ERPAppAutoRenewContractResponse AutoRenewContract(AppAutoRenewContractRequest request)
        {

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var inAppPurchaseManager = new InAppPurchaseCRTManager();
            return inAppPurchaseManager.AutoRenewContract(request, currentStore.StoreKey);

        }

        /// Cancel Contract
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ERPAppCancelContractResponse CancelContract(AppCancelContractRequest request)
        {

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var inAppPurchaseManager = new InAppPurchaseCRTManager();
            return inAppPurchaseManager.CancelContract(request, currentStore.StoreKey);

        }

        /// <summary>
        /// AutoRenew Contract
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ERPAppReactivateContractResponse ReactivateContract(AppReactivateContractRequest request)
        {

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var inAppPurchaseManager = new InAppPurchaseCRTManager();
            return inAppPurchaseManager.ReactivateContract(request, currentStore.StoreKey);

        }

        /// <summary>
        /// AutoRenew Contract
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ERPAppTransferContractResponse TransferContract(AppTransferContractRequest request)
        {

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var inAppPurchaseManager = new InAppPurchaseCRTManager();
            return inAppPurchaseManager.TransferContract(request, currentStore.StoreKey);

        }

        public ERPAppRebuyContractResponse RebuyContract(AppRebuyContractRequest request)
        {

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var inAppPurchaseManager = new InAppPurchaseCRTManager();
            return inAppPurchaseManager.RebuyContract(request, currentStore.StoreKey);

        }
        #endregion

    }
}
