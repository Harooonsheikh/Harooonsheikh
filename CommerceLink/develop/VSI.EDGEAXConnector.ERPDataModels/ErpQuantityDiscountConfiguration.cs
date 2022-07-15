﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpQuantityDiscountConfiguration
    {
        public decimal MinimumQuantity { get; set; }
        public decimal UnitPriceOrDiscountPercentage { get; set; }
    }
}