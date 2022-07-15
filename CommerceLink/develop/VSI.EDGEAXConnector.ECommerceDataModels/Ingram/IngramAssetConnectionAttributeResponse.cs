namespace VSI.EDGEAXConnector.ECommerceDataModels.Ingram
{
    public class IngramAssetConnectionAttributeResponse
    {
        public IngramAssetConnectionAttributeResponse()
        {
        }

        public string id { get; set; }
        public string type { get; set; }

        public IngramConnectionProviderAttributeResponse provider { get; set; }
        public IngramConnectionVendorAttributeResponse vendor { get; set; }
    }
}
