namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpThresholdDiscountTier
	{
		public ErpThresholdDiscountTier()
		{
		}
		public long RecordId	{ get; set; }//;
		public string OfferId	{ get; set; }//;
		public decimal AmountThreshold	{ get; set; }//;
		public ErpThresholdDiscountMethod DiscountMethod	{ get; set; }//;
		public decimal DiscountValue	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
