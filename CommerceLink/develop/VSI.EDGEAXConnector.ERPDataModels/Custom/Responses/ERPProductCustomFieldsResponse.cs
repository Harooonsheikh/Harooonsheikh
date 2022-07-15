using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ERPProductCustomFieldsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ErpProductCustomFields> ProductCustomFields { get; set; }

        public ERPProductCustomFieldsResponse(bool success, string message, List<ErpProductCustomFields> productCustomFields)
        {
            this.ProductCustomFields = productCustomFields;
            this.Success = success;
            this.Message = message;
        }
    }
}
