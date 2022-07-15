using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Enums.Enums.TMV
{
    public enum PaymentConnectorProperties
    {
        TESTCONNECTOR
    }

    public enum PaymentConfigurations
    {
        MerchantAccount,
        SupportedTenderTypes,
        AssemblyName,
        PortableAssemblyName,
        ServiceAccountId,
        PayerId,
        ParentTransactionId,
        Email,
        Note,
        ExpirationYear,
        ExpirationMonth,
        ProviderTransactionType,
        numberOfInstallments,
        registrationId,
        IP,
        FraudResult,
        TransactionId,
        OriginalPSP,
        ThreeDSecure,
        MerchantTransactionId,
        MerchantCustomerId,
        IsRecurringTransaction,
        OriginalProviderTransactionId

    }
}
