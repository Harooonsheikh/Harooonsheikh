namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomshoppingCartCustomerEntity
	{
		public EcomshoppingCartCustomerEntity()
		{
		}
		public string mode	{ get; set; }//;
		public int customer_id	{ get; set; }//;
		public bool customer_idSpecified	{ get; set; }//;
		public string email	{ get; set; }//;
		public string firstname	{ get; set; }//;
		public string lastname	{ get; set; }//;
		public string password	{ get; set; }//;
		public string confirmation	{ get; set; }//;
		public int website_id	{ get; set; }//;
		public bool website_idSpecified	{ get; set; }//;
		public int store_id	{ get; set; }//;
		public bool store_idSpecified	{ get; set; }//;
		public int group_id	{ get; set; }//;
		public bool group_idSpecified	{ get; set; }//;
	}
}
