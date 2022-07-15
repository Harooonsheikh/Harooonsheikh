using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpUpdateSubscriptionContractResponse
    {
        public ErpUpdateSubscriptionContractResponse()
        {
            
        }
        public ErpUpdateSubscriptionContractResponse(bool status, string message, string salesId)
        {
            this.Status = status;
            this.Message = message;
            this.SalesId = salesId;
        }

        public bool Status { get; set; }
        public string Message { get; set; }
        public string SalesId { get; set; }
    }
}
