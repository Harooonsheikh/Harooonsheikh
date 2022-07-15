namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogProductEntity
	{
		public EcomcatalogProductEntity()
		{
		}
		public string product_id	{ get; set; }//;
		public string sku	{ get; set; }//;
		public string name	{ get; set; }//;
		public string set	{ get; set; }//;
		public string type	{ get; set; }//;
		public string[] category_ids	{ get; set; }//;
		public string[] website_ids	{ get; set; }//;
	}
}
