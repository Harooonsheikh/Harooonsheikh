using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.CalculateContract
{
    public enum CLSubscriptionOfferType
    {
        //enum does not support "-" thats why using "_"
        PER = 0,
        SUB_2Y = 24,
        SUB_3Y = 36,
        SUB_6M = 6,
        SUB_M = 1,
        SUB_Y = 12
    }
}
