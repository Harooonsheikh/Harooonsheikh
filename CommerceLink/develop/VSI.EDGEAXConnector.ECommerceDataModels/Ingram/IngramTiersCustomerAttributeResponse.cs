namespace VSI.EDGEAXConnector.ECommerceDataModels.Ingram
{
    public class IngramTiersCustomerAttributeResponse
    {
        public IngramTiersCustomerAttributeResponse()
        {
        }

        public string external_id { get; set; }
        public string external_uid { get; set; }
        public string id { get; set; }
        public string name { get; set; }

        public IngramTiersContact_infoAttributeResponse contact_info { get; set; }
    }
}
