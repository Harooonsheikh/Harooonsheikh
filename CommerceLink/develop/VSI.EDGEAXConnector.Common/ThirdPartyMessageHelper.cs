using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.Common
{
    public static class ThirdPartyMessageHelper
    {
        static ThirdPartyMessageHelper()
        {
            ThirdPartyDistributorDetails = new List<ThirdPartyDistributorDetail>();
        }

        public static List<ThirdPartyDistributorDetail> ThirdPartyDistributorDetails { get; set; }
    }

    public class ThirdPartyDistributorDetail
    {
        public ThirdPartyDistributorDetail(string distributorKey, string storeKey, string currency)
        {
            this.DistributorKey = distributorKey;
            this.StoreKey = storeKey;
            this.Currency = currency;
        }
        public string DistributorKey { get; set; }
        public string StoreKey { get; set; }
        public string Currency { get; set; }
    }
}
