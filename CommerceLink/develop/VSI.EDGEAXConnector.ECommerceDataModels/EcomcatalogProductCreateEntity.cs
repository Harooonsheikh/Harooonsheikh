namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogProductCreateEntity
	{
		public EcomcatalogProductCreateEntity()
		{
		}
		public string[] categories	{ get; set; }//;
		public string[] websites	{ get; set; }//;
		public string name	{ get; set; }//;
		public string description	{ get; set; }//;
		public string short_description	{ get; set; }//;
		public string weight	{ get; set; }//;
		public string status	{ get; set; }//;
		public string url_key	{ get; set; }//;
		public string url_path	{ get; set; }//;
		public string visibility	{ get; set; }//;
		public string[] category_ids	{ get; set; }//;
		public string[] website_ids	{ get; set; }//;
		public string has_options	{ get; set; }//;
		public string gift_message_available	{ get; set; }//;
		public string price	{ get; set; }//;
	
		public string tax_class_id	{ get; set; }//;
		public EcomcatalogProductTierPriceEntity[] tier_price	{ get; set; }//;
		public string meta_title	{ get; set; }//;
		public string meta_keyword	{ get; set; }//;
		public string meta_description	{ get; set; }//;
		public string custom_design	{ get; set; }//;
		public string custom_layout_update	{ get; set; }//;
		public string options_container	{ get; set; }//;
     
		public EcomcatalogProductAdditionalAttributesEntity additional_attributes	{ get; set; }//;
		public EcomcatalogInventoryStockItemUpdateEntity stock_data	{ get; set; }//;
	}
}
