using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpCartResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErpCart Cart { get; set; }

        public ErpCartResponse(bool success, string message, ErpCart cart)
        {
            this.Cart = cart;
            this.Success = success;
            this.Message = message;
        }
    }
}
