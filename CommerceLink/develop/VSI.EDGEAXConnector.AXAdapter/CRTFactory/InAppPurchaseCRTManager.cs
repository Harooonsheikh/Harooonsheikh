using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class InAppPurchaseCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public InAppPurchaseCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public ERPAppAutoRenewContractResponse AutoRenewContract(AppAutoRenewContractRequest request, string storeKey)
        {
            var inAppPurchaseController = _crtFactory.CreateInAppPurchaseController(storeKey);
            return inAppPurchaseController.AutoRenewContract(request);
        }
        public ERPAppCancelContractResponse CancelContract(AppCancelContractRequest request, string storeKey)
        {
            var inAppPurchaseController = _crtFactory.CreateInAppPurchaseController(storeKey);
            return inAppPurchaseController.CancelContract(request);
        }

        public ERPAppReactivateContractResponse ReactivateContract(AppReactivateContractRequest request, string storeKey)
        {
            var inAppPurchaseController = _crtFactory.CreateInAppPurchaseController(storeKey);
            return inAppPurchaseController.ReactivateContract(request);
        }

        public ERPAppTransferContractResponse TransferContract(AppTransferContractRequest request, string storeKey)
        {
            var inAppPurchaseController = _crtFactory.CreateInAppPurchaseController(storeKey);
            return inAppPurchaseController.TransferContract(request);
        }

        public ERPAppRebuyContractResponse RebuyContract(AppRebuyContractRequest request, string storeKey)
        {
            var inAppPurchaseController = _crtFactory.CreateInAppPurchaseController(storeKey);
            return inAppPurchaseController.RebuyContract(request);
        }
    }
}
