namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomsalesOrderPaymentEntity
	{
		public EcomsalesOrderPaymentEntity()
		{
		}
		public string increment_id	{ get; set; }//;
		public string parent_id	{ get; set; }//;
		public string created_at	{ get; set; }//;
		public string updated_at	{ get; set; }//;
		public string is_active	{ get; set; }//;
		public string amount_ordered	{ get; set; }//;
		public string shipping_amount	{ get; set; }//;
		public string base_amount_ordered	{ get; set; }//;
		public string base_shipping_amount	{ get; set; }//;
		public string method	{ get; set; }//;
		public string po_number	{ get; set; }//;
		public string cc_type	{ get; set; }//;
		public string cc_number_enc	{ get; set; }//;
		public string cc_last4	{ get; set; }//;
		public string cc_owner	{ get; set; }//;
		public string cc_exp_month	{ get; set; }//;
		public string cc_exp_year	{ get; set; }//;
		public string cc_ss_start_month	{ get; set; }//;
		public string cc_ss_start_year	{ get; set; }//;
		public string payment_id	{ get; set; }//;
	}
}
