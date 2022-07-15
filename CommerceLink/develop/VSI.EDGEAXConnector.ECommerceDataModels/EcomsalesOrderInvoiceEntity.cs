namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomsalesOrderInvoiceEntity
	{
		public EcomsalesOrderInvoiceEntity()
		{
		}
		public string increment_id	{ get; set; }//;
		public string parent_id	{ get; set; }//;
		public string store_id	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string is_active	{ get; set; }//;
		public string global_currency_code	{ get; set; }//;
		public string base_currency_code	{ get; set; }//;
		public string store_currency_code	{ get; set; }//;
		public string order_currency_code	{ get; set; }//;
		public string store_to_base_rate	{ get; set; }//;
		public string store_to_order_rate	{ get; set; }//;
		public string base_to_global_rate	{ get; set; }//;
		public string base_to_order_rate	{ get; set; }//;
		public string subtotal	{ get; set; }//;
		public string base_subtotal	{ get; set; }//;
		public string base_grand_total	{ get; set; }//;
		public string discount_amount	{ get; set; }//;
		public string base_discount_amount	{ get; set; }//;
		public string shipping_amount	{ get; set; }//;
		public string base_shipping_amount	{ get; set; }//;
		public string tax_amount	{ get; set; }//;
		public string base_tax_amount	{ get; set; }//;
		public string billing_address_id	{ get; set; }//;
		public string billing_firstname	{ get; set; }//;
		public string billing_lastname	{ get; set; }//;
		public string order_id	{ get; set; }//;
		public string order_increment_id	{ get; set; }//;
		public string order_created_at	{ get; set; }//;
		public string state	{ get; set; }//;
		public string grand_total	{ get; set; }//;
		public string invoice_id	{ get; set; }//;
		public EcomsalesOrderInvoiceItemEntity[] items	{ get; set; }//;
		public EcomsalesOrderInvoiceCommentEntity[] comments	{ get; set; }//;
	}
}
