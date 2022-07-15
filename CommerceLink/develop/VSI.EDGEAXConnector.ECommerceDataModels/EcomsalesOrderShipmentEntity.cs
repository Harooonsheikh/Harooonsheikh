namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomsalesOrderShipmentEntity
	{
		public EcomsalesOrderShipmentEntity()
		{
		}
		public string increment_id	{ get; set; }//;
		public string parent_id	{ get; set; }//;
		public string store_id	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string is_active	{ get; set; }//;
		public string shipping_address_id	{ get; set; }//;
		public string shipping_firstname	{ get; set; }//;
		public string shipping_lastname	{ get; set; }//;
		public string order_id	{ get; set; }//;
		public string order_increment_id	{ get; set; }//;
		public string order_created_at	{ get; set; }//;
		public string total_qty	{ get; set; }//;
		public string shipment_id	{ get; set; }//;
		public EcomsalesOrderShipmentItemEntity[] items	{ get; set; }//;
		public EcomsalesOrderShipmentTrackEntity[] tracks	{ get; set; }//;
		public EcomsalesOrderShipmentCommentEntity[] comments	{ get; set; }//;
	}
}
