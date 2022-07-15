using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSI.EDGEAXConnector.ERPDataModels.AosClasses
{
    public class UserSessionInfo
    {
        [JsonProperty("$id")]
        [System.Runtime.Serialization.DataMember(Name = "$id")]
        public string Id { get; set; }
        public string AOSLocaleName { get; set; }
        public string AXLanguage { get; set; }
        public string Company { get; set; }
        public int CompanyTimeZone { get; set; }
        public CurrencyInfo CurrencyInfo { get; set; }
        public bool IsSysAdmin { get; set; }
        public string UserId { get; set; }
        public int UserPreferredCalendar { get; set; }
        public int UserPreferredTimeZone { get; set; }
    }
}
