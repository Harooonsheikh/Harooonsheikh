namespace VSI.EDGEAXConnector.ECommerceDataModels.Ingram
{
    public class IngramTiersContact_infoAttributeResponse
    {
        public IngramTiersContact_infoAttributeResponse()
        {
        }

        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string postal_code { get; set; }
        public string state { get; set; }

        public IngramContact_infoContactAttributeResponse contact { get; set; }
    }
}
