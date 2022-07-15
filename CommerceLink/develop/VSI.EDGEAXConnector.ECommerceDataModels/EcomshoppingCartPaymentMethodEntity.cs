namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomshoppingCartPaymentMethodEntity
	{
		public EcomshoppingCartPaymentMethodEntity()
		{
		}
		public string po_number	{ get; set; }//;
		public string method	{ get; set; }//;
		public string cc_cid	{ get; set; }//;
		public string cc_owner	{ get; set; }//;
		public string cc_number	{ get; set; }//;
		public string cc_type	{ get; set; }//;
		public string cc_exp_year	{ get; set; }//;
		public string cc_exp_month	{ get; set; }//;
	}
}
