namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomsalesOrderCreditmemoData
	{
		public EcomsalesOrderCreditmemoData()
		{
		}
		public EcomorderItemIdQty[] qtys	{ get; set; }//;
		public double shipping_amount	{ get; set; }//;
		public bool shipping_amountSpecified	{ get; set; }//;
		public double adjustment_positive	{ get; set; }//;
		public bool adjustment_positiveSpecified	{ get; set; }//;
		public double adjustment_negative	{ get; set; }//;
		public bool adjustment_negativeSpecified	{ get; set; }//;
	}
}
