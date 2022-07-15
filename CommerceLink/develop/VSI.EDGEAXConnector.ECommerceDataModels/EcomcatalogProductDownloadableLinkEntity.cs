namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogProductDownloadableLinkEntity
	{
		public EcomcatalogProductDownloadableLinkEntity()
		{
		}
		public string link_id	{ get; set; }//;
		public string title	{ get; set; }//;
		public string price	{ get; set; }//;
		public int number_of_downloads	{ get; set; }//;
		public bool number_of_downloadsSpecified	{ get; set; }//;
		public int is_unlimited	{ get; set; }//;
		public bool is_unlimitedSpecified	{ get; set; }//;
		public int is_shareable	{ get; set; }//;
		public string link_url	{ get; set; }//;
		public string link_type	{ get; set; }//;
		public string sample_file	{ get; set; }//;
		public string sample_url	{ get; set; }//;
		public string sample_type	{ get; set; }//;
		public int sort_order	{ get; set; }//;
		public EcomcatalogProductDownloadableLinkFileInfoEntity[] file_save	{ get; set; }//;
		public EcomcatalogProductDownloadableLinkFileInfoEntity[] sample_file_save	{ get; set; }//;
	}
}
