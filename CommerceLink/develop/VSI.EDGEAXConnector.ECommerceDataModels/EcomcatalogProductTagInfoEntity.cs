namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogProductTagInfoEntity
	{
		public EcomcatalogProductTagInfoEntity()
		{
		}
		public string name	{ get; set; }//;
		public string status	{ get; set; }//;
		public string base_popularity	{ get; set; }//;
		public EcomassociativeEntity[] products	{ get; set; }//;
	}
}
