namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomshoppingCartLicenseEntity
	{
		public EcomshoppingCartLicenseEntity()
		{
		}
		public string agreement_id	{ get; set; }//;
		public string name	{ get; set; }//;
		public string content	{ get; set; }//;
		public int is_active	{ get; set; }//;
		public bool is_activeSpecified	{ get; set; }//;
		public int is_html	{ get; set; }//;
		public bool is_htmlSpecified	{ get; set; }//;
	}
}
