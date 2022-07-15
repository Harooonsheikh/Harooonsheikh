using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpConfirmQuotationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }

        public ErpConfirmQuotationResponse(bool success, string message, string errorCode)
        {
            Success = success;
            Message = message;
            ErrorCode = errorCode;
        }
    }
}
