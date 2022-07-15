using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpTriggerDataSyncResponse
    {
        public bool Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }

        public ErpTriggerDataSyncResponse(bool status, string code, string message, string email)
        {
            Status = status;
            Code = code;
            Message = message;
            Email = email;
        }


    }
}
