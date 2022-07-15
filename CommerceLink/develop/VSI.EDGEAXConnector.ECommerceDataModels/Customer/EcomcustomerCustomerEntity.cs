namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcustomerCustomerEntity
	{
		public EcomcustomerCustomerEntity()
		{
		}
		public int customer_id	{ get; set; }//;
		public bool customer_idSpecified	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string increment_id	{ get; set; }//;
		public int store_id	{ get; set; }//;
		public bool store_idSpecified	{ get; set; }//;
		public int website_id	{ get; set; }//;
		public bool website_idSpecified	{ get; set; }//;
		public string created_in	{ get; set; }//;
		public string email	{ get; set; }//;
		public string firstname	{ get; set; }//;
		public string middlename	{ get; set; }//;
		public string lastname	{ get; set; }//;
		public int group_id	{ get; set; }//;
		public bool group_idSpecified	{ get; set; }//;
		public string prefix	{ get; set; }//;
		public string suffix	{ get; set; }//;
		public string dob	{ get; set; }//;
		public string taxvat	{ get; set; }//;
		public bool confirmation	{ get; set; }//;
		public bool confirmationSpecified	{ get; set; }//;
		public string password_hash	{ get; set; }//;
	}
}
