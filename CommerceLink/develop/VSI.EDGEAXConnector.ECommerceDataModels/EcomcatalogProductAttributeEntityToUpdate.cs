namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogProductAttributeEntityToUpdate
	{
		public EcomcatalogProductAttributeEntityToUpdate()
		{
		}
		public string scope	{ get; set; }//;
		public string default_value	{ get; set; }//;
		public int is_unique	{ get; set; }//;
		public bool is_uniqueSpecified	{ get; set; }//;
		public int is_required	{ get; set; }//;
		public bool is_requiredSpecified	{ get; set; }//;
		public string[] apply_to	{ get; set; }//;
		public int is_configurable	{ get; set; }//;
		public bool is_configurableSpecified	{ get; set; }//;
		public int is_searchable	{ get; set; }//;
		public bool is_searchableSpecified	{ get; set; }//;
		public int is_visible_in_advanced_search	{ get; set; }//;
		public bool is_visible_in_advanced_searchSpecified	{ get; set; }//;
		public int is_comparable	{ get; set; }//;
		public bool is_comparableSpecified	{ get; set; }//;
		public int is_used_for_promo_rules	{ get; set; }//;
		public bool is_used_for_promo_rulesSpecified	{ get; set; }//;
		public int is_visible_on_front	{ get; set; }//;
		public bool is_visible_on_frontSpecified	{ get; set; }//;
		public int used_in_product_listing	{ get; set; }//;
		public bool used_in_product_listingSpecified	{ get; set; }//;
		public EcomassociativeEntity[] additional_fields	{ get; set; }//;
		public EcomcatalogProductAttributeFrontendLabelEntity[] frontend_label	{ get; set; }//;
	}
}
