using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRT.Interface
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
