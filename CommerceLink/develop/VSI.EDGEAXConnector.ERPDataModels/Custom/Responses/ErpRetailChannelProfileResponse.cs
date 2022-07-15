using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpRetailChannelProfileResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErpRetailChannelProfile RetailChannelProfile { get; set; }

        public ErpRetailChannelProfileResponse(bool success, string message, ErpRetailChannelProfile retailChannelProfile)
        {
            this.RetailChannelProfile = retailChannelProfile;
            this.Success = success;
            this.Message = message;
        }
    }
}
