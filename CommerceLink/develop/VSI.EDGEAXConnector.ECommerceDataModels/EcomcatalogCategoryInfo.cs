namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogCategoryInfo
	{
		public EcomcatalogCategoryInfo()
		{
		}
		public string category_id	{ get; set; }//;
		public int is_active	{ get; set; }//;
		public string position	{ get; set; }//;
		public string level	{ get; set; }//;
		public string parent_id	{ get; set; }//;
		public string all_children	{ get; set; }//;
		public string children	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string name	{ get; set; }//;
		public string url_key	{ get; set; }//;
		public string description	{ get; set; }//;
		public string meta_title	{ get; set; }//;
		public string meta_keywords	{ get; set; }//;
		public string meta_description	{ get; set; }//;
		public string path	{ get; set; }//;
		public string url_path	{ get; set; }//;
		public int children_count	{ get; set; }//;
		public bool children_countSpecified	{ get; set; }//;
		public string display_mode	{ get; set; }//;
		public int is_anchor	{ get; set; }//;
		public bool is_anchorSpecified	{ get; set; }//;
		public string[] available_sort_by	{ get; set; }//;
		public string custom_design	{ get; set; }//;
		public string custom_design_apply	{ get; set; }//;
		public string custom_design_from	{ get; set; }//;
		public string custom_design_to	{ get; set; }//;
		public string page_layout	{ get; set; }//;
		public string custom_layout_update	{ get; set; }//;
		public string default_sort_by	{ get; set; }//;
		public int landing_page	{ get; set; }//;
		public bool landing_pageSpecified	{ get; set; }//;
	}
}
