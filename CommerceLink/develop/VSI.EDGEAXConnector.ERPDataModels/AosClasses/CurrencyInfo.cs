using Newtonsoft.Json;

namespace VSI.EDGEAXConnector.ERPDataModels.AosClasses
{
    public class CurrencyInfo
    {
        [JsonProperty("$id")]
        [System.Runtime.Serialization.DataMember(Name = "$id")]
        public string Id { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public double ExchangeRate { get; set; }
        public string ISOCurrencyCode { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
    }
}