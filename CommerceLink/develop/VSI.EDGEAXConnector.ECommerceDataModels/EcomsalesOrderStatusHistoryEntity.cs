namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomsalesOrderStatusHistoryEntity
	{
		public EcomsalesOrderStatusHistoryEntity()
		{
		}
		public string increment_id	{ get; set; }//;
		public string parent_id	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string is_active	{ get; set; }//;
		public string is_customer_notified	{ get; set; }//;
		public string status	{ get; set; }//;
		public string comment	{ get; set; }//;
	}
}
