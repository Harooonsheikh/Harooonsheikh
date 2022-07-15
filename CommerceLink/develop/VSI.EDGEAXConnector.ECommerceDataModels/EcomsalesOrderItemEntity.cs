namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomsalesOrderItemEntity
	{
		public EcomsalesOrderItemEntity()
		{
		}
		public string item_id	{ get; set; }//;
		public string order_id	{ get; set; }//;
		public string quote_item_id	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string product_id	{ get; set; }//;
		public string product_type	{ get; set; }//;
		public string product_options	{ get; set; }//;
		public string weight	{ get; set; }//;
		public string is_virtual	{ get; set; }//;
		public string sku	{ get; set; }//;
		public string name	{ get; set; }//;
		public string applied_rule_ids	{ get; set; }//;
		public string free_shipping	{ get; set; }//;
		public string is_qty_decimal	{ get; set; }//;
		public string no_discount	{ get; set; }//;
		public string qty_canceled	{ get; set; }//;
		public string qty_invoiced	{ get; set; }//;
		public string qty_ordered	{ get; set; }//;
		public string qty_refunded	{ get; set; }//;
		public string qty_shipped	{ get; set; }//;
		public string cost	{ get; set; }//;
		public string price	{ get; set; }//;
		public string base_price	{ get; set; }//;
		public string original_price	{ get; set; }//;
		public string base_original_price	{ get; set; }//;
		public string tax_percent	{ get; set; }//;
		public string tax_amount	{ get; set; }//;
		public string base_tax_amount	{ get; set; }//;
		public string tax_invoiced	{ get; set; }//;
		public string base_tax_invoiced	{ get; set; }//;
		public string discount_percent	{ get; set; }//;
		public string discount_amount	{ get; set; }//;
		public string base_discount_amount	{ get; set; }//;
		public string discount_invoiced	{ get; set; }//;
		public string base_discount_invoiced	{ get; set; }//;
		public string amount_refunded	{ get; set; }//;
		public string base_amount_refunded	{ get; set; }//;
		public string row_total	{ get; set; }//;
		public string base_row_total	{ get; set; }//;
		public string row_invoiced	{ get; set; }//;
		public string base_row_invoiced	{ get; set; }//;
		public string row_weight	{ get; set; }//;
		public string gift_message_id	{ get; set; }//;
		public string gift_message	{ get; set; }//;
		public string gift_message_available	{ get; set; }//;
		public string base_tax_before_discount	{ get; set; }//;
		public string tax_before_discount	{ get; set; }//;
		public string weee_tax_applied	{ get; set; }//;
		public string weee_tax_applied_amount	{ get; set; }//;
		public string weee_tax_applied_row_amount	{ get; set; }//;
		public string base_weee_tax_applied_amount	{ get; set; }//;
		public string base_weee_tax_applied_row_amount	{ get; set; }//;
		public string weee_tax_disposition	{ get; set; }//;
		public string weee_tax_row_disposition	{ get; set; }//;
		public string base_weee_tax_disposition	{ get; set; }//;
		public string base_weee_tax_row_disposition	{ get; set; }//;
	}
}
