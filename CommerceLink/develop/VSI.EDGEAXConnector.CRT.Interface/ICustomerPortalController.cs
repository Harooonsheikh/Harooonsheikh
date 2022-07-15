using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels.CalculateContract;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface ICustomerPortalController
    {
        ErpCreateContractNewPaymentMethodResponse CreateContractNewPaymentMethod(ErpCreateContractNewPaymentMethod request, string requestId);
        UpdateBillingAddressResponse UpdateBillingAddress(UpdateBillingAddressRequest objRequest, string requestId);
        ContractCalculationResponse CalculateSubscriptionChange(string cartId, long affiliationId, List<CLContractCartLine> cartLines, CLDeliverySpecification deliverySpecification, List<string> couponCodes, string requestId);
        UnblockContractResponse UnblockContract(UnblockContractRequest unblockContractRequest, string requestId);
        PromiseToPayResponse PromiseToPay(PromiseToPayRequest request, string requestId);
        AssignCustomerPortalAccessResponse AssignCustomerPortalAccess(AssignCustomerPortalAccessRequest assignCustomerPortalAccessRequest, string requestId);
        ContractActivationLogResponse ContractActivationLog(ContractActivationLogRequest request, string requestId);
    }
}
