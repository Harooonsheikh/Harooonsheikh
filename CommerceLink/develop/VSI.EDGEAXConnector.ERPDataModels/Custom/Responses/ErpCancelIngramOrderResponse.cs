using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Enums.Enums;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCancelIngramOrderResponse
    {
        public bool Success { get; set; }
        public IngramCancelTerminationCodes Code { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }

        public ErpCancelIngramOrderResponse(bool success, IngramCancelTerminationCodes code, string message, string result)
        {
            this.Success = success;
            this.Code = code;
            this.Message = message;
            this.Result = result;
        }
    }
}
