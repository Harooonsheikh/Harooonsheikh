namespace VSI.EDGEAXConnector.ECommerceDataModels.Ingram
{
    public class IngramAssetTiersAttributeResponse
    {
        public IngramAssetTiersAttributeResponse()
        {
        }

        public IngramTiersCustomerAttributeResponse customer { get; set; }
        public IngramTiersCustomerAttributeResponse tier1 { get; set; }
        public IngramTiersCustomerAttributeResponse tier2 { get; set; }
    }
}
