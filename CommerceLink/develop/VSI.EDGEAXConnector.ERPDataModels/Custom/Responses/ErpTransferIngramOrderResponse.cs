using System;
using VSI.EDGEAXConnector.Enums.Enums;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpTransferIngramOrderResponse
    {
        public bool Success { get; set; }
        public IngramTransferCodes Code { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }
        public DateTimeOffset? RenewalDate { get; set; }

        public ErpTransferIngramOrderResponse(bool success, IngramTransferCodes code, string message, string result, DateTimeOffset? renewalDate)
        {
            Success = success;
            Code = code;
            Message = message;
            Result = result;
            RenewalDate = renewalDate;
        }
    }
}
