namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomshoppingCartShippingMethodEntity
	{
		public EcomshoppingCartShippingMethodEntity()
		{
		}
		public string code	{ get; set; }//;
		public string carrier	{ get; set; }//;
		public string carrier_title	{ get; set; }//;
		public string method	{ get; set; }//;
		public string method_title	{ get; set; }//;
		public string method_description	{ get; set; }//;
		public double price	{ get; set; }//;
		public bool priceSpecified	{ get; set; }//;
	}
}
