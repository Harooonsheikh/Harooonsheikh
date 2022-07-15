using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class AppReactivateContractRequest
    {
        public string ChannelReferenceId { get; set; }
        public string SubscriptionExpiryDate { get; set; }
    }
}
