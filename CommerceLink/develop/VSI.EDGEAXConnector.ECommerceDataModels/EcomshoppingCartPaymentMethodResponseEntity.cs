namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomshoppingCartPaymentMethodResponseEntity
	{
		public EcomshoppingCartPaymentMethodResponseEntity()
		{
		}
		public string code	{ get; set; }//;
		public string title	{ get; set; }//;
		public EcomassociativeEntity[] cc_types	{ get; set; }//;
	}
}
