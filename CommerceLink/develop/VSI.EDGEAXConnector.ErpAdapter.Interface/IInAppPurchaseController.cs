using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface IInAppPurchaseController
    {
        ERPAppAutoRenewContractResponse AutoRenewContract(AppAutoRenewContractRequest request);
        ERPAppCancelContractResponse CancelContract(AppCancelContractRequest request);

        ERPAppReactivateContractResponse ReactivateContract(AppReactivateContractRequest request);
        ERPAppTransferContractResponse TransferContract(AppTransferContractRequest request);
        ERPAppRebuyContractResponse RebuyContract(AppRebuyContractRequest request);
    }
}
