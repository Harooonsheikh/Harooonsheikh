using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCloseExistingOrderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool Result { get; set; }

        public ErpCloseExistingOrderResponse(bool success, string message, bool result)
        {
            this.Success = success;
            this.Message = message;
            this.Result = result;
        }
    }
}
