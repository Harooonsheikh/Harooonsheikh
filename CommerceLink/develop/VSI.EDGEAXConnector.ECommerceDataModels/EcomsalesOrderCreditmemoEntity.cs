namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomsalesOrderCreditmemoEntity
	{
		public EcomsalesOrderCreditmemoEntity()
		{
		}
		public string updated_at	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string increment_id	{ get; set; }//;
		public string transaction_id	{ get; set; }//;
		public string global_currency_code	{ get; set; }//;
		public string base_currency_code	{ get; set; }//;
		public string order_currency_code	{ get; set; }//;
		public string store_currency_code	{ get; set; }//;
		public string cybersource_token	{ get; set; }//;
		public string invoice_id	{ get; set; }//;
		public string billing_address_id	{ get; set; }//;
		public string shipping_address_id	{ get; set; }//;
		public string state	{ get; set; }//;
		public string creditmemo_status	{ get; set; }//;
		public string email_sent	{ get; set; }//;
		public string order_id	{ get; set; }//;
		public string tax_amount	{ get; set; }//;
		public string shipping_tax_amount	{ get; set; }//;
		public string base_tax_amount	{ get; set; }//;
		public string base_adjustment_positive	{ get; set; }//;
		public string base_grand_total	{ get; set; }//;
		public string adjustment	{ get; set; }//;
		public string subtotal	{ get; set; }//;
		public string discount_amount	{ get; set; }//;
		public string base_subtotal	{ get; set; }//;
		public string base_adjustment	{ get; set; }//;
		public string base_to_global_rate	{ get; set; }//;
		public string store_to_base_rate	{ get; set; }//;
		public string base_shipping_amount	{ get; set; }//;
		public string adjustment_negative	{ get; set; }//;
		public string subtotal_incl_tax	{ get; set; }//;
		public string shipping_amount	{ get; set; }//;
		public string base_subtotal_incl_tax	{ get; set; }//;
		public string base_adjustment_negative	{ get; set; }//;
		public string grand_total	{ get; set; }//;
		public string base_discount_amount	{ get; set; }//;
		public string base_to_order_rate	{ get; set; }//;
		public string store_to_order_rate	{ get; set; }//;
		public string base_shipping_tax_amount	{ get; set; }//;
		public string adjustment_positive	{ get; set; }//;
		public string store_id	{ get; set; }//;
		public string hidden_tax_amount	{ get; set; }//;
		public string base_hidden_tax_amount	{ get; set; }//;
		public string shipping_hidden_tax_amount	{ get; set; }//;
		public string base_shipping_hidden_tax_amnt	{ get; set; }//;
		public string shipping_incl_tax	{ get; set; }//;
		public string base_shipping_incl_tax	{ get; set; }//;
		public string base_customer_balance_amount	{ get; set; }//;
		public string customer_balance_amount	{ get; set; }//;
		public string bs_customer_bal_total_refunded	{ get; set; }//;
		public string customer_bal_total_refunded	{ get; set; }//;
		public string base_gift_cards_amount	{ get; set; }//;
		public string gift_cards_amount	{ get; set; }//;
		public string gw_base_price	{ get; set; }//;
		public string gw_price	{ get; set; }//;
		public string gw_items_base_price	{ get; set; }//;
		public string gw_items_price	{ get; set; }//;
		public string gw_card_base_price	{ get; set; }//;
		public string gw_card_price	{ get; set; }//;
		public string gw_base_tax_amount	{ get; set; }//;
		public string gw_tax_amount	{ get; set; }//;
		public string gw_items_base_tax_amount	{ get; set; }//;
		public string gw_items_tax_amount	{ get; set; }//;
		public string gw_card_base_tax_amount	{ get; set; }//;
		public string gw_card_tax_amount	{ get; set; }//;
		public string base_reward_currency_amount	{ get; set; }//;
		public string reward_currency_amount	{ get; set; }//;
		public string reward_points_balance	{ get; set; }//;
		public string reward_points_balance_refund	{ get; set; }//;
		public string creditmemo_id	{ get; set; }//;
		public EcomsalesOrderCreditmemoItemEntity[] items	{ get; set; }//;
		public EcomsalesOrderCreditmemoCommentEntity[] comments	{ get; set; }//;
	}
}
