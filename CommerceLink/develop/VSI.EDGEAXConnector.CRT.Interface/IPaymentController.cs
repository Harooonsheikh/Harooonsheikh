using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IPaymentController
    {
        string GenerateCardBlob(ErpPaymentCard cardProperties, string authTransactionId, PaymentMethod paymentMethod, string requestId, PaymentConnectorData paymentConnectorData);
        string GenerateAuthBlob(ErpPaymentCard cardProperties, string authTransactionId, string approvalCode, decimal approvalAmount, DateTime transactionDate,string currencyCode, PaymentConnectorData paymentConnectorData);
        string GenerateAuthBlobAllPago(ErpPaymentCard cardProperties, string authTransactionId, string approvalCode, decimal approvalAmount, DateTime transactionDate, string currencyCode, PaymentConnectorData paymentConnectorData);
        List<ERPPaymentConnectorResponse> GetPaymentConnectorInfo(string requestId);
    }
}
