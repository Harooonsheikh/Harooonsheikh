namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpQuantityDiscountLevel
	{
		public ErpQuantityDiscountLevel()
		{
		}
		public long RecordId	{ get; set; }//;
		public string OfferId	{ get; set; }//;
		public decimal MinimumQuantity	{ get; set; }//;
		public decimal DiscountPriceOrPercent	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
