namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public class EcomcustomerAddressEntityItem
	{
		public EcomcustomerAddressEntityItem()
		{
		}

        public int customer_id { get; set; }//;
		public int customer_address_id	{ get; set; }//;

        public long edgeaxintegrationkey { get; set; }

		public bool customer_address_idSpecified	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string increment_id	{ get; set; }//;
		public string city	{ get; set; }//;
		public string company	{ get; set; }//;
		public string country_id	{ get; set; }//;
		public string fax	{ get; set; }//;
		public string firstname	{ get; set; }//;
		public string lastname	{ get; set; }//;
		public string middlename	{ get; set; }//;
		public string postcode	{ get; set; }//;
		public string prefix	{ get; set; }//;
		public string region	{ get; set; }//;
		public int region_id	{ get; set; }//;
		public bool region_idSpecified	{ get; set; }//;
		public string street	{ get; set; }//;
		public string suffix	{ get; set; }//;
		public string telephone	{ get; set; }//;
		public bool is_default_billing	{ get; set; }//;
		public bool is_default_billingSpecified	{ get; set; }//;
		public bool is_default_shipping	{ get; set; }//;
		public bool is_default_shippingSpecified	{ get; set; }//;
	}
}
