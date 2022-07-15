using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.AosClasses;
using VSI.EDGEAXConnector.ERPDataModels.CalculateContract;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class CustomerPortalCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public CustomerPortalCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public UserSessionInfo TestCall(string requestId, string storeKey)
        {
            var subscriptionController = _crtFactory.CreateSubscriptionController(storeKey);
            return subscriptionController.TestCall(requestId);
        }

        public ErpCreateContractNewPaymentMethodResponse CreateContractNewPaymentMethod(ErpCreateContractNewPaymentMethod request, string storeKey, string requestId)
        {
            var customerController = _crtFactory.CreateCustomerPortalController(storeKey);
            return customerController.CreateContractNewPaymentMethod(request, requestId);
        }

        public UpdateBillingAddressResponse UpdateBillingAddress(UpdateBillingAddressRequest updateBillingAddressRequest, string requestId, string storeKey)
        {
            var customerPortalController = _crtFactory.CreateCustomerPortalController(storeKey);
            return customerPortalController.UpdateBillingAddress(updateBillingAddressRequest, requestId);
        }

        public ErpUpdateSubscriptionContractResponse UpdateSubscriptionContract(ErpUpdateSubscriptionContract request, string storeKey, string requestId)
        {
            var subscriptionController = _crtFactory.CreateSubscriptionController(storeKey);
            return subscriptionController.UpdateSubscriptionContract(request, requestId);
        }

        public ContractCalculationResponse CalculateSubscriptionChange(string cartId, long affiliationId, List<CLContractCartLine> cartLines, CLDeliverySpecification deliverySpecification, List<string> couponCodes, string storeKey, string requestId)
        {
            var customerPortalController = _crtFactory.CreateCustomerPortalController(storeKey);

            return customerPortalController.CalculateSubscriptionChange(cartId, affiliationId, cartLines, deliverySpecification, couponCodes, requestId);
        }

        public ProcessContractTerminateResponse ProcessContractTerminate(ErpProcessContractTerminateRequest processContractTerminateRequest, string requestId, string storeKey)
        {
            var subscriptionController = _crtFactory.CreateSubscriptionController(storeKey);
            return subscriptionController.ProcessContractTerminate(processContractTerminateRequest, requestId);
        }

        public ProcessContractReactivateResponse ProcessContractReactivate(ErpProcessContractReactivateRequest processContractReactivateRequest, string requestId, string storeKey)
        {
            var subscriptionController = _crtFactory.CreateSubscriptionController(storeKey);
            return subscriptionController.ProcessContractReactivate(processContractReactivateRequest, requestId);
        }
        public UnblockContractResponse UnblockContract(UnblockContractRequest unblockContractRequest, string requestId, string currentStoreStoreKey)
        {
            var customerPortalController = _crtFactory.CreateCustomerPortalController(currentStoreStoreKey);
            return customerPortalController.UnblockContract(unblockContractRequest, requestId);
        }

        public PromiseToPayResponse PromiseToPay(PromiseToPayRequest request, string requestId, string currentStoreStoreKey)
        {
            var customerPortalController = _crtFactory.CreateCustomerPortalController(currentStoreStoreKey);
            return customerPortalController.PromiseToPay(request, requestId);
        }

        public AssignCustomerPortalAccessResponse AssignCustomerPortalAccess(AssignCustomerPortalAccessRequest assignCustomerPortalAccessRequest, string requestId, string currentStoreStoreKey)
        {

            var customerPortalController = _crtFactory.CreateCustomerPortalController(currentStoreStoreKey);
            return customerPortalController.AssignCustomerPortalAccess(assignCustomerPortalAccessRequest, requestId);
        }

        public ContractActivationLogResponse ContractActivationLog(ContractActivationLogRequest request, string requestId, string currentStoreStoreKey)
        {

            var customerPortalController = _crtFactory.CreateCustomerPortalController(currentStoreStoreKey);
            return customerPortalController.ContractActivationLog(request, requestId);
        }
    }
}
