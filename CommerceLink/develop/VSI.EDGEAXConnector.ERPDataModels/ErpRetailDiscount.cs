namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpRetailDiscount
	{
		public ErpRetailDiscount()
		{
		}
		public string OfferId	{ get; set; }//;
		public string CurrencyCode	{ get; set; }//;
		public long PriceDiscountGroup	{ get; set; }//;
		public string Name	{ get; set; }//;
		public ErpPeriodicDiscountOfferType PeriodicDiscountType	{ get; set; }//;
		public ErpConcurrencyMode ConcurrencyMode	{ get; set; }//;
		public bool IsDiscountCodeRequired	{ get; set; }//;
		public string ValidationPeriodId	{ get; set; }//;
		public int DateValidationType	{ get; set; }//;
		public System.DateTimeOffset ValidFromDate	{ get; set; }//;
		public System.DateTimeOffset ValidToDate	{ get; set; }//;
		public int DiscountType	{ get; set; }//;
		public decimal MixAndMatchDealPrice	{ get; set; }//;
		public decimal MixAndMatchDiscountPercent	{ get; set; }//;
		public decimal MixAndMatchDiscountAmount	{ get; set; }//;
		public int MixAndMatchNumberOfLeastExpensiveLines	{ get; set; }//;
		public int MixAndMatchNumberOfTimeApplicable	{ get; set; }//;
		public int ShouldCountNonDiscountItems	{ get; set; }//;
		public System.Collections.Generic.IList<ErpRetailDiscountLine> DiscountLines	{ get; set; }//;
		public System.Collections.Generic.IList<ErpRetailDiscountPriceGroup> PriceGroups	{ get; set; }//;
		public System.Collections.Generic.IList<ErpDiscountCode> DiscountCodes	{ get; set; }//;
		public System.Collections.Generic.IList<ErpQuantityDiscountLevel> MultibuyQuantityTiers	{ get; set; }//;
		public System.Collections.Generic.IList<ErpThresholdDiscountTier> ThresholdDiscountTiers	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
