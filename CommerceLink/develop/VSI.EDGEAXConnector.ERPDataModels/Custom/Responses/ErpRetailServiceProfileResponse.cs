using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    public class ErpRetailServiceProfileResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErpRetailServiceProfile RetailServiceProfile { get; set; }

        public ErpRetailServiceProfileResponse(bool success, string message, ErpRetailServiceProfile retailServiceProfile)
        {
            this.RetailServiceProfile = retailServiceProfile;
            this.Success = success;
            this.Message = message;
        }
    }
}
