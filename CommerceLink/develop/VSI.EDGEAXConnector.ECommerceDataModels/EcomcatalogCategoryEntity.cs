namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogCategoryEntity
	{
		public EcomcatalogCategoryEntity()
		{
		}
		public int category_id	{ get; set; }//;
		public int parent_id	{ get; set; }//;
		public string name	{ get; set; }//;
		public int is_active	{ get; set; }//;
		public int position	{ get; set; }//;
		public int level	{ get; set; }//;
		public EcomcatalogCategoryEntity[] children	{ get; set; }//;
	}
}
