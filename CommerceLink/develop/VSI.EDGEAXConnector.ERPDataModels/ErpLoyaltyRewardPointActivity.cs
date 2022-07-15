using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
//HK: D365 Update 10.0 Application change start
    public class ErpLoyaltyRewardPointActivity
    {
        public DateTimeOffset? ActivityDate { get; set; }
        public string LoyaltyRewardPointTimelineEntryType { get; set; }
        public int? ExtensibleLoyaltyRewardPointActivityEntryTypeValue { get; set; }
        public decimal? LoyaltyPoints { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
//HK: D365 Update 10.0 Application change end
}
