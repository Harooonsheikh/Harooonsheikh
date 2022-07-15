using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Response
{
    public class MergeCustomerResellerResponse
    {
        public MergeCustomerResellerResponse(bool status, object customerInfo, object resellerInfo, string message)
        {
            this.Status = status;
            this.CustomerInfo = customerInfo;
            this.ResellerInfo = resellerInfo;
            this.Message = message;
        }
        public bool Status { get; set; }
        public object CustomerInfo { get; set; }
        public object ResellerInfo { get; set; }
        public string Message { get; set; }
    }
}
