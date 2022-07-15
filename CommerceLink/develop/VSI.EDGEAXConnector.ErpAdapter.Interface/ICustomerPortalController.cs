using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.AosClasses;
using VSI.EDGEAXConnector.ERPDataModels.CalculateContract;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface ICustomerPortalController
    {
        UserSessionInfo TestCall(string requestId);

        ErpCreateContractNewPaymentMethodResponse CreateNewContractPaymentMethod(ErpCreateContractNewPaymentMethod request, string requestId);

        UpdateBillingAddressResponse UpdateBillingAddress(UpdateBillingAddressRequest objRequest, string requestId);

        ErpUpdateSubscriptionContractResponse UpdateSubscriptionContract(ErpUpdateSubscriptionContract request, string requestId);

        ContractCalculationResponse CalculateSubscriptionChange(bool useOldContractDates, DateTimeOffset contractStartDate, DateTimeOffset contractEndDate, CLSubscriptionOfferType subWeight, DateTimeOffset requestDate, long affiliationId, List<CLContractCartLine> cartLines, CLDeliverySpecification deliverySpecification, List<string> couponCodes, string requestId);

        ProcessContractTerminateResponse ProcessContractTerminate(ErpProcessContractTerminateRequest processContractTerminateRequest, string requestId);

        UnblockContractResponse UnblockContract(UnblockContractRequest unblockContractRequest, string requestId);

        ProcessContractReactivateResponse ProcessContractReactivate(ErpProcessContractReactivateRequest processContractReactivateRequest, string requestId);
    }
}
