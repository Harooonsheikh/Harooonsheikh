using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ERPAppRebuyContractResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string SalesOrderId { get; set; }

        public ERPAppRebuyContractResponse(bool success, string message, string salesOrderId)
        {
            this.Success = success;
            this.Message = message;
            this.SalesOrderId = salesOrderId;
        }
    }
}
