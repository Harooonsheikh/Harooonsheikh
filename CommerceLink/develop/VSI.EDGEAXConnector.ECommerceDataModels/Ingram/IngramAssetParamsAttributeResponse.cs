namespace VSI.EDGEAXConnector.ECommerceDataModels.Ingram
{
    public class IngramAssetParamsAttributeResponse
    {
        public IngramAssetParamsAttributeResponse()
        {
        }

        public string description { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string value_error { get; set; }

        public string[] value_choices { get; set; }
    }
}
