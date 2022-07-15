using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
  public  class ERPQuotationReasonGroupsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ERPQuotationReasonGroup> QuotationReasonGroup { get; set; }

        public ERPQuotationReasonGroupsResponse(bool success, string message, List<ERPQuotationReasonGroup> qotationReasonGroup)
        {
            this.QuotationReasonGroup = qotationReasonGroup;
            this.Success = success;
            this.Message = message;
        }
    }
}
