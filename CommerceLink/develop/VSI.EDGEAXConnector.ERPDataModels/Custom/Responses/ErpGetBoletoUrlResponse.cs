using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpGetBoletoUrlResponse
    {
        public bool Status { get; set; }

        public string TMVBoletoUrl { get; set; }

        public string Message { get; set; }

        public ErpGetBoletoUrlResponse(bool status, string message, string tmvBoletoUrl)
        {
            Status = status;
            Message = message;
            TMVBoletoUrl = tmvBoletoUrl;
        }
    }
}
