using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
   public class ErpUpdateCustomerPortalLinkResponse
    {
        
            public bool Success { get; set; }
            public string Message { get; set; }
            public string SalesId { get; set; }

        public ErpUpdateCustomerPortalLinkResponse(bool success, string message,string salesId)
        {
            this.Success = success;
            this.Message = message;
            this.SalesId = salesId;
        }
    }
    
}
