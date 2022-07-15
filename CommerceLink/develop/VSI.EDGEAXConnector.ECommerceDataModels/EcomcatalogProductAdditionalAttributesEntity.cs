namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogProductAdditionalAttributesEntity
	{
		public EcomcatalogProductAdditionalAttributesEntity()
		{
		}
		public EcomassociativeMultiEntity[] multi_data	{ get; set; }//;
		public EcomassociativeEntity[] single_data	{ get; set; }//;
	}
}
