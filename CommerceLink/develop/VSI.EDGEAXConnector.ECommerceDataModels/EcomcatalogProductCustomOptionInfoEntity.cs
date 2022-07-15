namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogProductCustomOptionInfoEntity
	{
		public EcomcatalogProductCustomOptionInfoEntity()
		{
		}
		public string title	{ get; set; }//;
		public string type	{ get; set; }//;
		public string sort_order	{ get; set; }//;
		public int is_require	{ get; set; }//;
		public EcomcatalogProductCustomOptionAdditionalFieldsEntity[] additional_fields	{ get; set; }//;
	}
}
