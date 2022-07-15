namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCustomerLoyaltyCard
	{
		public ErpCustomerLoyaltyCard()
		{
		}
		public long RecordId	{ get; set; }//;
		public string CardNumber	{ get; set; }//;
		public decimal IssuedPoints	{ get; set; }//;
		public decimal UsedPoints	{ get; set; }//;
		public decimal ExpiredPoints	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
