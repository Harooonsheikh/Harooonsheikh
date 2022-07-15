namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogProductCustomOptionToAdd
	{
		public EcomcatalogProductCustomOptionToAdd()
		{
		}
		public string title	{ get; set; }//;
		public string type	{ get; set; }//;
		public string sort_order	{ get; set; }//;
		public int is_require	{ get; set; }//;
		public bool is_requireSpecified	{ get; set; }//;
		public EcomcatalogProductCustomOptionAdditionalFieldsEntity[] additional_fields	{ get; set; }//;
	}
}
