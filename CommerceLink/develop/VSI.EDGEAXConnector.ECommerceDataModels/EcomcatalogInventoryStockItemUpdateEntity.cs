namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogInventoryStockItemUpdateEntity
	{
		public EcomcatalogInventoryStockItemUpdateEntity()
		{
		}
		public string qty	{ get; set; }//;
		public int is_in_stock	{ get; set; }//;
		public bool is_in_stockSpecified	{ get; set; }//;
		public int manage_stock	{ get; set; }//;
		public bool manage_stockSpecified	{ get; set; }//;
		public int use_config_manage_stock	{ get; set; }//;
		public bool use_config_manage_stockSpecified	{ get; set; }//;
		public int min_qty	{ get; set; }//;
		public bool min_qtySpecified	{ get; set; }//;
		public int use_config_min_qty	{ get; set; }//;
		public bool use_config_min_qtySpecified	{ get; set; }//;
		public int min_sale_qty	{ get; set; }//;
		public bool min_sale_qtySpecified	{ get; set; }//;
		public int use_config_min_sale_qty	{ get; set; }//;
		public bool use_config_min_sale_qtySpecified	{ get; set; }//;
		public int max_sale_qty	{ get; set; }//;
		public bool max_sale_qtySpecified	{ get; set; }//;
		public int use_config_max_sale_qty	{ get; set; }//;
		public bool use_config_max_sale_qtySpecified	{ get; set; }//;
		public int is_qty_decimal	{ get; set; }//;
		public bool is_qty_decimalSpecified	{ get; set; }//;
		public int backorders	{ get; set; }//;
		public bool backordersSpecified	{ get; set; }//;
		public int use_config_backorders	{ get; set; }//;
		public bool use_config_backordersSpecified	{ get; set; }//;
		public int notify_stock_qty	{ get; set; }//;
		public bool notify_stock_qtySpecified	{ get; set; }//;
		public int use_config_notify_stock_qty	{ get; set; }//;
		public bool use_config_notify_stock_qtySpecified	{ get; set; }//;
	}
}
