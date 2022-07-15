namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogCategoryTree
	{
		public EcomcatalogCategoryTree()
		{
		}
		public int category_id	{ get; set; }//;
		public int parent_id	{ get; set; }//;
		public string name	{ get; set; }//;
		public int position	{ get; set; }//;
		public int level	{ get; set; }//;
		public EcomcatalogCategoryEntity[] children	{ get; set; }//;
	}
}
