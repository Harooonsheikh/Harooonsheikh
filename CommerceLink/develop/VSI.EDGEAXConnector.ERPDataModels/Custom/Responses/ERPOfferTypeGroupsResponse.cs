using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
  public  class ERPOfferTypeGroupsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ERPOfferTypeGroup> OfferTypeGroups { get; set; }

        public ERPOfferTypeGroupsResponse(bool success, string message, List<ERPOfferTypeGroup> offerTypeGroup)
        {
            this.OfferTypeGroups = offerTypeGroup;
            this.Success = success;
            this.Message = message;
        }
    }
}
