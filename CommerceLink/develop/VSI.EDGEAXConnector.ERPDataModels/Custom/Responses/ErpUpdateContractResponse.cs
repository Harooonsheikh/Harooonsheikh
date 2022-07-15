﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpUpdateContractResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }

        public ErpUpdateContractResponse(bool success, string message, string result)
        {
            this.Success = success;
            this.Message = message;
            this.Result = result;
        }
    }
}
