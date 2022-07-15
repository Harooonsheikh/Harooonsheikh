using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Enums
{
    public enum DataDirectionType
    {
        EcomRequestToCL = 1,
        CLResponseToEcom = 2,
        CLRequestToThirdParty = 3,
        ThirdPartyResponseToCL = 4,
        ThirdPartyRequestToCL = 5,
    }
}