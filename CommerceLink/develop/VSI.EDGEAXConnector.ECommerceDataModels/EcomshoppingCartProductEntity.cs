namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomshoppingCartProductEntity
	{
		public EcomshoppingCartProductEntity()
		{
		}
		public string product_id	{ get; set; }//;
		public string sku	{ get; set; }//;
		public double qty	{ get; set; }//;
		public bool qtySpecified	{ get; set; }//;
		public EcomassociativeEntity[] options	{ get; set; }//;
		public EcomassociativeEntity[] bundle_option	{ get; set; }//;
		public EcomassociativeEntity[] bundle_option_qty	{ get; set; }//;
		public string[] links	{ get; set; }//;
	}
}
