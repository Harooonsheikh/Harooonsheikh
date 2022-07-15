namespace VSI.EDGEAXConnector.ECommerceDataModels.Ingram
{
    public class IngramContact_infoContactAttributeResponse
    {
        public IngramContact_infoContactAttributeResponse()
        {
        }

        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        public IngramContactPhone_numberAttributeResponse phone_number { get; set; }
    }
}
