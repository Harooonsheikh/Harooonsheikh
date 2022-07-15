using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class PaymentCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public PaymentCRTManager()
        {
            _crtFactory = new CRTFactory();
        }
        public string GenerateCardBlob(ErpPaymentCard cardProperties, string authTransactionId, PaymentMethod paymentMethod, string storeKey, string requestId, PaymentConnectorData paymentConnectorData)
        {
            var paymentController = _crtFactory.CreatePaymentController(storeKey);
            return paymentController.GenerateCardBlob(cardProperties, authTransactionId, paymentMethod, requestId, paymentConnectorData);
        }

        public string GenerateAuthBlob(ErpPaymentCard cardProperties, string authTransactionId, string approvalCode, decimal approvalAmount, DateTime transactionDate, string currencyCode, string storeKey, PaymentConnectorData paymentConnectorData)
        {
            var paymentController = _crtFactory.CreatePaymentController(storeKey);
            return paymentController.GenerateAuthBlob(cardProperties, authTransactionId, approvalCode, approvalAmount, transactionDate, currencyCode, paymentConnectorData);
        }

        public List<ERPPaymentConnectorResponse> GetPaymentConnectorInfo(string storeKey, string requestId)
        {
            var paymentController = _crtFactory.CreatePaymentController(storeKey);
            return paymentController.GetPaymentConnectorInfo(requestId);
        }
    }
}
