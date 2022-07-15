namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomsalesOrderEntity
	{
		public EcomsalesOrderEntity()
		{
		}
		public string increment_id	{ get; set; }//;
		public string parent_id	{ get; set; }//;
		public string store_id	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string is_active	{ get; set; }//;
		public string customer_id	{ get; set; }//;
		public string tax_amount	{ get; set; }//;
		public string shipping_amount	{ get; set; }//;
		public string discount_amount	{ get; set; }//;
		public string subtotal	{ get; set; }//;
		public string grand_total	{ get; set; }//;
		public string total_paid	{ get; set; }//;
		public string total_refunded	{ get; set; }//;
		public string total_qty_ordered	{ get; set; }//;
		public string total_canceled	{ get; set; }//;
		public string total_invoiced	{ get; set; }//;
		public string total_online_refunded	{ get; set; }//;
		public string total_offline_refunded	{ get; set; }//;
		public string base_tax_amount	{ get; set; }//;
		public string base_shipping_amount	{ get; set; }//;
		public string base_discount_amount	{ get; set; }//;
		public string base_subtotal	{ get; set; }//;
		public string base_grand_total	{ get; set; }//;
		public string base_total_paid	{ get; set; }//;
		public string base_total_refunded	{ get; set; }//;
		public string base_total_qty_ordered	{ get; set; }//;
		public string base_total_canceled	{ get; set; }//;
		public string base_total_invoiced	{ get; set; }//;
		public string base_total_online_refunded	{ get; set; }//;
		public string base_total_offline_refunded	{ get; set; }//;
		public string billing_address_id	{ get; set; }//;
		public string billing_firstname	{ get; set; }//;
		public string billing_lastname	{ get; set; }//;
		public string shipping_address_id	{ get; set; }//;
		public string shipping_firstname	{ get; set; }//;
		public string shipping_lastname	{ get; set; }//;
		public string billing_name	{ get; set; }//;
		public string shipping_name	{ get; set; }//;
		public string store_to_base_rate	{ get; set; }//;
		public string store_to_order_rate	{ get; set; }//;
		public string base_to_global_rate	{ get; set; }//;
		public string base_to_order_rate	{ get; set; }//;
		public string weight	{ get; set; }//;
		public string store_name	{ get; set; }//;
		public string remote_ip	{ get; set; }//;
		public string status	{ get; set; }//;
		public string state	{ get; set; }//;
		public string applied_rule_ids	{ get; set; }//;
		public string global_currency_code	{ get; set; }//;
		public string base_currency_code	{ get; set; }//;
		public string store_currency_code	{ get; set; }//;
		public string order_currency_code	{ get; set; }//;
		public string shipping_method	{ get; set; }//;
		public string shipping_description	{ get; set; }//;
		public string customer_email	{ get; set; }//;
		public string customer_firstname	{ get; set; }//;
		public string customer_lastname	{ get; set; }//;
		public string quote_id	{ get; set; }//;
		public string is_virtual	{ get; set; }//;
		public string customer_group_id	{ get; set; }//;
		public string customer_note_notify	{ get; set; }//;
		public string customer_is_guest	{ get; set; }//;
		public string email_sent	{ get; set; }//;
		public string order_id	{ get; set; }//;
		public string gift_message_id	{ get; set; }//;
		public string gift_message	{ get; set; }//;
        public string order_international { get; set; }//;
        public string order_borderfree { get; set; }//;
        public string is_tax_exempt_customer { get; set; }//;

        public string transtime { get; set; }//;
		public EcomsalesOrderAddressEntity shipping_address	{ get; set; }//;
		public EcomsalesOrderAddressEntity billing_address	{ get; set; }//;
		public EcomsalesOrderItemEntity[] items	{ get; set; }//;
		public EcomsalesOrderPaymentEntity payment	{ get; set; }//;
		public EcomsalesOrderStatusHistoryEntity[] status_history	{ get; set; }//;
	}
}
