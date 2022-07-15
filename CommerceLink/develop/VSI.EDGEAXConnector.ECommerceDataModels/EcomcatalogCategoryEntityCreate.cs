namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogCategoryEntityCreate
	{
		public EcomcatalogCategoryEntityCreate()
		{
		}
		public string name	{ get; set; }//;
		public int is_active	{ get; set; }//;
		public bool is_activeSpecified	{ get; set; }//;
		public int position	{ get; set; }//;
		public bool positionSpecified	{ get; set; }//;
		public string[] available_sort_by	{ get; set; }//;
		public string custom_design	{ get; set; }//;
		public int custom_design_apply	{ get; set; }//;
		public bool custom_design_applySpecified	{ get; set; }//;
		public string custom_design_from	{ get; set; }//;
		public string custom_design_to	{ get; set; }//;
		public string custom_layout_update	{ get; set; }//;
		public string default_sort_by	{ get; set; }//;
		public string description	{ get; set; }//;
		public string display_mode	{ get; set; }//;
		public int is_anchor	{ get; set; }//;
		public bool is_anchorSpecified	{ get; set; }//;
		public int landing_page	{ get; set; }//;
		public bool landing_pageSpecified	{ get; set; }//;
		public string meta_description	{ get; set; }//;
		public string meta_keywords	{ get; set; }//;
		public string meta_title	{ get; set; }//;
		public string page_layout	{ get; set; }//;
		public string url_key	{ get; set; }//;
		public int include_in_menu	{ get; set; }//;
        public bool include_in_menuSpecified { get; set; }//;
        public int is_core { get; set; }//;
        public bool is_coreSpecified { get; set; }//;
	}
}
