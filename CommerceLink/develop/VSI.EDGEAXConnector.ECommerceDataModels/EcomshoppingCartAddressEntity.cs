namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomshoppingCartAddressEntity
	{
		public EcomshoppingCartAddressEntity()
		{
		}
		public string address_id	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string customer_id	{ get; set; }//;
		public int save_in_address_book	{ get; set; }//;
		public bool save_in_address_bookSpecified	{ get; set; }//;
		public string customer_address_id	{ get; set; }//;
		public string address_type	{ get; set; }//;
		public string email	{ get; set; }//;
		public string prefix	{ get; set; }//;
		public string firstname	{ get; set; }//;
		public string middlename	{ get; set; }//;
		public string lastname	{ get; set; }//;
		public string suffix	{ get; set; }//;
		public string company	{ get; set; }//;
		public string street	{ get; set; }//;
		public string city	{ get; set; }//;
		public string region	{ get; set; }//;
		public string region_id	{ get; set; }//;
		public string postcode	{ get; set; }//;
		public string country_id	{ get; set; }//;
		public string telephone	{ get; set; }//;
		public string fax	{ get; set; }//;
		public int same_as_billing	{ get; set; }//;
		public bool same_as_billingSpecified	{ get; set; }//;
		public int free_shipping	{ get; set; }//;
		public bool free_shippingSpecified	{ get; set; }//;
		public string shipping_method	{ get; set; }//;
		public string shipping_description	{ get; set; }//;
		public double weight	{ get; set; }//;
		public bool weightSpecified	{ get; set; }//;
	}
}
