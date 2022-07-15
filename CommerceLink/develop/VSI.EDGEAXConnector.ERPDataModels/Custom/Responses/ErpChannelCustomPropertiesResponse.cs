using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpChannelCustomPropertiesResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErpChannelCustomProperties CustomProperties { get; set; }

        public ErpChannelCustomPropertiesResponse(bool success, string message, ErpChannelCustomProperties customProperties)
        {
            this.CustomProperties = customProperties;
            this.Success = success;
            this.Message = message;
        }
    }
}
