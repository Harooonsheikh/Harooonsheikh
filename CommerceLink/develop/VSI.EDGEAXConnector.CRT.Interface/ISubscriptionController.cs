using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.AosClasses;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface ISubscriptionController
    {
        UserSessionInfo TestCall(string requestId);

        ErpUpdateSubscriptionContractResponse UpdateSubscriptionContract(ErpUpdateSubscriptionContract request, string requestId);
        ProcessContractTerminateResponse ProcessContractTerminate(ErpProcessContractTerminateRequest processContractTerminateRequest, string requestId);
        ProcessContractReactivateResponse ProcessContractReactivate(ErpProcessContractReactivateRequest processContractReactivateRequest, string requestId);
    }
}
