using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface IPaymentController
    {
        string GenerateCardBlob(ErpPaymentCard cardProperties);
        string GenerateAuthBlob(ErpPaymentCard cardProperties, string authTransactionId, string approvalCode, decimal approvalAmount, DateTime transactionDate);
        List<ERPPaymentConnectorResponse> GetPaymentConnectorInfo(string requestId);
    }
}
