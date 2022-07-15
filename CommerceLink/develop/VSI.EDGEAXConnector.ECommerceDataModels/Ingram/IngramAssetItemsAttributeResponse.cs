namespace VSI.EDGEAXConnector.ECommerceDataModels.Ingram
{
    public class IngramAssetItemsAttributeResponse
    {
        public IngramAssetItemsAttributeResponse()
        {
        }

        public string global_id { get; set; }
        public string id { get; set; }
        public string mpn { get; set; }
        public int old_quantity { get; set; }
        public int quantity { get; set; }
        public int line_number { get; set; }
    }
}
