﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class CustomerInvoiceRequest
    {
        public string CustomerAccount { get; set; }
        public string LicenseID { get; set; }
        public string TransactionType { get; set; }
    }
}
