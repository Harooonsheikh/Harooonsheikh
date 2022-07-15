namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomshoppingCartPaymentEntity
	{
		public EcomshoppingCartPaymentEntity()
		{
		}
		public string payment_id	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string method	{ get; set; }//;
		public string cc_type	{ get; set; }//;
		public string cc_number_enc	{ get; set; }//;
		public string cc_last4	{ get; set; }//;
		public string cc_cid_enc	{ get; set; }//;
		public string cc_owner	{ get; set; }//;
		public string cc_exp_month	{ get; set; }//;
		public string cc_exp_year	{ get; set; }//;
		public string cc_ss_owner	{ get; set; }//;
		public string cc_ss_start_month	{ get; set; }//;
		public string cc_ss_start_year	{ get; set; }//;
		public string cc_ss_issue	{ get; set; }//;
		public string po_number	{ get; set; }//;
		public string additional_data	{ get; set; }//;
		public string additional_information	{ get; set; }//;
	}
}
