namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomshoppingCartInfoEntity
	{
		public EcomshoppingCartInfoEntity()
		{
		}
		public string store_id	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string converted_at	{ get; set; }//;
		public int quote_id	{ get; set; }//;
		public bool quote_idSpecified	{ get; set; }//;
		public int is_active	{ get; set; }//;
		public bool is_activeSpecified	{ get; set; }//;
		public int is_virtual	{ get; set; }//;
		public bool is_virtualSpecified	{ get; set; }//;
		public int is_multi_shipping	{ get; set; }//;
		public bool is_multi_shippingSpecified	{ get; set; }//;
		public double items_count	{ get; set; }//;
		public bool items_countSpecified	{ get; set; }//;
		public double items_qty	{ get; set; }//;
		public bool items_qtySpecified	{ get; set; }//;
		public string orig_order_id	{ get; set; }//;
		public string store_to_base_rate	{ get; set; }//;
		public string store_to_quote_rate	{ get; set; }//;
		public string base_currency_code	{ get; set; }//;
		public string store_currency_code	{ get; set; }//;
		public string quote_currency_code	{ get; set; }//;
		public string grand_total	{ get; set; }//;
		public string base_grand_total	{ get; set; }//;
		public string checkout_method	{ get; set; }//;
		public string customer_id	{ get; set; }//;
		public string customer_tax_class_id	{ get; set; }//;
		public int customer_group_id	{ get; set; }//;
		public bool customer_group_idSpecified	{ get; set; }//;
		public string customer_email	{ get; set; }//;
		public string customer_prefix	{ get; set; }//;
		public string customer_firstname	{ get; set; }//;
		public string customer_middlename	{ get; set; }//;
		public string customer_lastname	{ get; set; }//;
		public string customer_suffix	{ get; set; }//;
		public string customer_note	{ get; set; }//;
		public string customer_note_notify	{ get; set; }//;
		public string customer_is_guest	{ get; set; }//;
		public string applied_rule_ids	{ get; set; }//;
		public string reserved_order_id	{ get; set; }//;
		public string password_hash	{ get; set; }//;
		public string coupon_code	{ get; set; }//;
		public string global_currency_code	{ get; set; }//;
		public double base_to_global_rate	{ get; set; }//;
		public bool base_to_global_rateSpecified	{ get; set; }//;
		public double base_to_quote_rate	{ get; set; }//;
		public bool base_to_quote_rateSpecified	{ get; set; }//;
		public string customer_taxvat	{ get; set; }//;
		public string customer_gender	{ get; set; }//;
		public double subtotal	{ get; set; }//;
		public bool subtotalSpecified	{ get; set; }//;
		public double base_subtotal	{ get; set; }//;
		public bool base_subtotalSpecified	{ get; set; }//;
		public double subtotal_with_discount	{ get; set; }//;
		public bool subtotal_with_discountSpecified	{ get; set; }//;
		public double base_subtotal_with_discount	{ get; set; }//;
		public bool base_subtotal_with_discountSpecified	{ get; set; }//;
		public string ext_shipping_info	{ get; set; }//;
		public string gift_message_id	{ get; set; }//;
		public string gift_message	{ get; set; }//;
		public double customer_balance_amount_used	{ get; set; }//;
		public bool customer_balance_amount_usedSpecified	{ get; set; }//;
		public double base_customer_balance_amount_used	{ get; set; }//;
		public bool base_customer_balance_amount_usedSpecified	{ get; set; }//;
		public string use_customer_balance	{ get; set; }//;
		public string gift_cards_amount	{ get; set; }//;
		public string base_gift_cards_amount	{ get; set; }//;
		public string gift_cards_amount_used	{ get; set; }//;
		public string use_reward_points	{ get; set; }//;
		public string reward_points_balance	{ get; set; }//;
		public string base_reward_currency_amount	{ get; set; }//;
		public string reward_currency_amount	{ get; set; }//;
		public EcomshoppingCartAddressEntity shipping_address	{ get; set; }//;
		public EcomshoppingCartAddressEntity billing_address	{ get; set; }//;
		public EcomshoppingCartItemEntity[] items	{ get; set; }//;
		public EcomshoppingCartPaymentEntity payment	{ get; set; }//;
	}
}
