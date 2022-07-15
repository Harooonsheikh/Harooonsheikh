namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogProductDownloadableLinkAddEntity
	{
		public EcomcatalogProductDownloadableLinkAddEntity()
		{
		}
		public string title	{ get; set; }//;
		public string price	{ get; set; }//;
		public int is_unlimited	{ get; set; }//;
		public bool is_unlimitedSpecified	{ get; set; }//;
		public int number_of_downloads	{ get; set; }//;
		public bool number_of_downloadsSpecified	{ get; set; }//;
		public int is_shareable	{ get; set; }//;
		public bool is_shareableSpecified	{ get; set; }//;
		public EcomcatalogProductDownloadableLinkAddSampleEntity sample	{ get; set; }//;
		public string type	{ get; set; }//;
		public EcomcatalogProductDownloadableLinkFileEntity file	{ get; set; }//;
		public string link_url	{ get; set; }//;
		public string sample_url	{ get; set; }//;
		public int sort_order	{ get; set; }//;
		public bool sort_orderSpecified	{ get; set; }//;
	}
}
