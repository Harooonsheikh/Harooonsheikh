namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomshoppingCartItemEntity
	{
		public EcomshoppingCartItemEntity()
		{
		}
		public string item_id	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string product_id	{ get; set; }//;
		public string store_id	{ get; set; }//;
		public string parent_item_id	{ get; set; }//;
		public int is_virtual	{ get; set; }//;
		public bool is_virtualSpecified	{ get; set; }//;
		public string sku	{ get; set; }//;
		public string name	{ get; set; }//;
		public string description	{ get; set; }//;
		public string applied_rule_ids	{ get; set; }//;
		public string additional_data	{ get; set; }//;
		public string free_shipping	{ get; set; }//;
		public string is_qty_decimal	{ get; set; }//;
		public string no_discount	{ get; set; }//;
		public double weight	{ get; set; }//;
		public bool weightSpecified	{ get; set; }//;
		public double qty	{ get; set; }//;
		public bool qtySpecified	{ get; set; }//;
		public double price	{ get; set; }//;
		public bool priceSpecified	{ get; set; }//;
		public double base_price	{ get; set; }//;
		public bool base_priceSpecified	{ get; set; }//;
		public double custom_price	{ get; set; }//;
		public bool custom_priceSpecified	{ get; set; }//;
		public double discount_percent	{ get; set; }//;
		public bool discount_percentSpecified	{ get; set; }//;
		public double discount_amount	{ get; set; }//;
		public bool discount_amountSpecified	{ get; set; }//;
		public double base_discount_amount	{ get; set; }//;
		public bool base_discount_amountSpecified	{ get; set; }//;
		public double tax_percent	{ get; set; }//;
		public bool tax_percentSpecified	{ get; set; }//;
		public double tax_amount	{ get; set; }//;
		public bool tax_amountSpecified	{ get; set; }//;
		public double base_tax_amount	{ get; set; }//;
		public bool base_tax_amountSpecified	{ get; set; }//;
		public double row_total	{ get; set; }//;
		public bool row_totalSpecified	{ get; set; }//;
		public double base_row_total	{ get; set; }//;
		public bool base_row_totalSpecified	{ get; set; }//;
		public double row_total_with_discount	{ get; set; }//;
		public bool row_total_with_discountSpecified	{ get; set; }//;
		public double row_weight	{ get; set; }//;
		public bool row_weightSpecified	{ get; set; }//;
		public string product_type	{ get; set; }//;
		public double base_tax_before_discount	{ get; set; }//;
		public bool base_tax_before_discountSpecified	{ get; set; }//;
		public double tax_before_discount	{ get; set; }//;
		public bool tax_before_discountSpecified	{ get; set; }//;
		public double original_custom_price	{ get; set; }//;
		public bool original_custom_priceSpecified	{ get; set; }//;
		public double base_cost	{ get; set; }//;
		public bool base_costSpecified	{ get; set; }//;
		public double price_incl_tax	{ get; set; }//;
		public bool price_incl_taxSpecified	{ get; set; }//;
		public double base_price_incl_tax	{ get; set; }//;
		public bool base_price_incl_taxSpecified	{ get; set; }//;
		public double row_total_incl_tax	{ get; set; }//;
		public bool row_total_incl_taxSpecified	{ get; set; }//;
		public double base_row_total_incl_tax	{ get; set; }//;
		public bool base_row_total_incl_taxSpecified	{ get; set; }//;
		public string gift_message_id	{ get; set; }//;
		public string gift_message	{ get; set; }//;
		public string gift_message_available	{ get; set; }//;
		public double weee_tax_applied	{ get; set; }//;
		public bool weee_tax_appliedSpecified	{ get; set; }//;
		public double weee_tax_applied_amount	{ get; set; }//;
		public bool weee_tax_applied_amountSpecified	{ get; set; }//;
		public double weee_tax_applied_row_amount	{ get; set; }//;
		public bool weee_tax_applied_row_amountSpecified	{ get; set; }//;
		public double base_weee_tax_applied_amount	{ get; set; }//;
		public bool base_weee_tax_applied_amountSpecified	{ get; set; }//;
		public double base_weee_tax_applied_row_amount	{ get; set; }//;
		public bool base_weee_tax_applied_row_amountSpecified	{ get; set; }//;
		public double weee_tax_disposition	{ get; set; }//;
		public bool weee_tax_dispositionSpecified	{ get; set; }//;
		public double weee_tax_row_disposition	{ get; set; }//;
		public bool weee_tax_row_dispositionSpecified	{ get; set; }//;
		public double base_weee_tax_disposition	{ get; set; }//;
		public bool base_weee_tax_dispositionSpecified	{ get; set; }//;
		public double base_weee_tax_row_disposition	{ get; set; }//;
		public bool base_weee_tax_row_dispositionSpecified	{ get; set; }//;
		public string tax_class_id	{ get; set; }//;
	}
}
