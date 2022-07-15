namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpDiscountLine
	{
		public ErpDiscountLine()
		{
		}
		public decimal SaleLineNumber	{ get; set; }//;
		public string OfferId	{ get; set; }//;
		public string OfferName	{ get; set; }//;
		public decimal Amount	{ get; set; }//;
		public decimal EffectiveAmount	{ get; set; }//;
		public decimal Percentage	{ get; set; }//;
		public decimal DealPrice	{ get; set; }//;
		public ErpDiscountLineType DiscountLineType	{ get; set; }//;
		public int DiscountLineTypeValue	{ get; set; }//;
		public ErpManualDiscountType ManualDiscountType	{ get; set; }//;
		public int ManualDiscountTypeValue	{ get; set; }//;
		public ErpCustomerDiscountType CustomerDiscountType	{ get; set; }//;
		public int CustomerDiscountTypeValue	{ get; set; }//;
		public ErpPeriodicDiscountOfferType PeriodicDiscountType	{ get; set; }//;
		public int PeriodicDiscountTypeValue	{ get; set; }//;
		public string DiscountApplicationGroup	{ get; set; }//;
		public ErpConcurrencyMode ConcurrencyMode	{ get; set; }//;
		public int ConcurrencyModeValue	{ get; set; }//;
		public bool IsCompoundable	{ get; set; }//;
		public string DiscountCode	{ get; set; }//;
		public bool IsDiscountCodeRequired	{ get; set; }//;
		public decimal ThresholdAmountRequired	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
