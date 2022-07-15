namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogProductAttributeMediaCreateEntity
	{
		public EcomcatalogProductAttributeMediaCreateEntity()
		{
		}
		public EcomcatalogProductImageFileEntity file	{ get; set; }//;
		public string label	{ get; set; }//;
		public string position	{ get; set; }//;
		public string[] types	{ get; set; }//;
		public string exclude	{ get; set; }//;
		public string remove	{ get; set; }//;
	}
}
