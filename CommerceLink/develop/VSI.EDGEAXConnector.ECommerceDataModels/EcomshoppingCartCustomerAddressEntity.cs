namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomshoppingCartCustomerAddressEntity
	{
		public EcomshoppingCartCustomerAddressEntity()
		{
		}
		public string mode	{ get; set; }//;
		public string address_id	{ get; set; }//;
		public string firstname	{ get; set; }//;
		public string lastname	{ get; set; }//;
		public string company	{ get; set; }//;
		public string street	{ get; set; }//;
		public string city	{ get; set; }//;
		public string region	{ get; set; }//;
		public string region_id	{ get; set; }//;
		public string postcode	{ get; set; }//;
		public string country_id	{ get; set; }//;
		public string telephone	{ get; set; }//;
		public string fax	{ get; set; }//;
		public int is_default_billing	{ get; set; }//;
		public bool is_default_billingSpecified	{ get; set; }//;
		public int is_default_shipping	{ get; set; }//;
		public bool is_default_shippingSpecified	{ get; set; }//;
	}
}
