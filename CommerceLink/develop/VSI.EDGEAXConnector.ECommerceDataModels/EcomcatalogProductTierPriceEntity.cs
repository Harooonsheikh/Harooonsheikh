namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogProductTierPriceEntity
	{
		public EcomcatalogProductTierPriceEntity()
		{
		}
		public string customer_group_id	{ get; set; }//;
		public string website	{ get; set; }//;
		public int qty	{ get; set; }//;
		public bool qtySpecified	{ get; set; }//;
		public double price	{ get; set; }//;
		public bool priceSpecified	{ get; set; }//;
	}
}
