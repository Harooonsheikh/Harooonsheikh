﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IQuantityDiscountWithAffiliationController
    {
        List<ErpProductQuantityDiscountWithAffiliation> GetQuantityDiscountWithAffiliation();
    }
}
