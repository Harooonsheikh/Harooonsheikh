using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Enums
{
    /// <summary>
    /// This enum maps to EntityType of Configurable Objects
    /// </summary>
    public enum ConfigObjecsType : short
    {
        DeliveryModes = 1,
        PaymentMethods = 2,
        TaxCodes = 3,
        Charges = 4,
        GiftCards = 5,
        LanguageCode = 6,
        StoreCode = 7
    }
}
