using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Enums
{
    public enum PaymentCon
    {
        ALLPAGO_CC,
        ADYEN_CC,
        PAYPAL_EXPRESS,
        BASIC_CREDIT,
        PURCHASEORDER,
        SEPA,
        BOLETO = 101,
        ADYEN_HPP
    }

    public enum CardType 
    {
        ALIPAY,
        ALIPAY_HK
    }
}

