namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomsalesOrderInvoiceItemEntity
	{
		public EcomsalesOrderInvoiceItemEntity()
		{
		}
		public string increment_id	{ get; set; }//;
		public string parent_id	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string is_active	{ get; set; }//;
		public string weee_tax_applied	{ get; set; }//;
		public string qty	{ get; set; }//;
		public string cost	{ get; set; }//;
		public string price	{ get; set; }//;
		public string tax_amount	{ get; set; }//;
		public string row_total	{ get; set; }//;
		public string base_price	{ get; set; }//;
		public string base_tax_amount	{ get; set; }//;
		public string base_row_total	{ get; set; }//;
		public string base_weee_tax_applied_amount	{ get; set; }//;
		public string base_weee_tax_applied_row_amount	{ get; set; }//;
		public string weee_tax_applied_amount	{ get; set; }//;
		public string weee_tax_applied_row_amount	{ get; set; }//;
		public string weee_tax_disposition	{ get; set; }//;
		public string weee_tax_row_disposition	{ get; set; }//;
		public string base_weee_tax_disposition	{ get; set; }//;
		public string base_weee_tax_row_disposition	{ get; set; }//;
		public string sku	{ get; set; }//;
		public string name	{ get; set; }//;
		public string order_item_id	{ get; set; }//;
		public string product_id	{ get; set; }//;
		public string item_id	{ get; set; }//;
	}
}
