using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCustomerCreateUpdateResponse
    {
        public bool Status { get; set; }
        public CustomerInformation CustomerInfo { get; set; }
        public string Message { get; set; }

        public ErpCustomerCreateUpdateResponse(bool status, string message, CustomerInformation customerInfo)
        {
            this.CustomerInfo = customerInfo;
            this.Status = status;
            this.Message = message;
        }
    }
    public class CustomerInformation
    {
        public string AccountNumber { get; set; }
        public long DirectoryPartyRecordId { get; set; }
        public long RecordId { get; set; }
    }
}
