namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpBarcode
	{
		public ErpBarcode()
		{
		}
		public string BarcodeId	{ get; set; }//;
		public string InventoryColorId	{ get; set; }//;
		public string InventorySizeId	{ get; set; }//;
		public string InventoryStyleId	{ get; set; }//;
		public string InventoryConfigId	{ get; set; }//;
		public decimal BarcodePrice	{ get; set; }//;
		public decimal BarcodeQuantity	{ get; set; }//;
		public string CustomerId	{ get; set; }//;
		public string EmployeeId	{ get; set; }//;
		public string SalespersonId	{ get; set; }//;
		public string EANLicenseId	{ get; set; }//;
		public string DataEntry	{ get; set; }//;
		public string DiscountCode	{ get; set; }//;
		public string GiftCardNumber	{ get; set; }//;
		public string LoyaltyCardNumber	{ get; set; }//;
		public string InventoryDimensionId	{ get; set; }//;
		public string ItemBarcode	{ get; set; }//;
		public ErpBarcodeEntryMethodType EntryMethodType	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public int ItemType	{ get; set; }//;
		public string ColorName	{ get; set; }//;
		public string SizeName	{ get; set; }//;
		public string StyleName	{ get; set; }//;
		public string ConfigName	{ get; set; }//;
		public bool EnterDimensions	{ get; set; }//;
		public string VariantId	{ get; set; }//;
		public string ItemGroupId	{ get; set; }//;
		public string ItemDepartmentId	{ get; set; }//;
		public bool ScaleItem	{ get; set; }//;
		public bool ZeroPriceValid	{ get; set; }//;
		public bool NegativeQuantity	{ get; set; }//;
		public bool DiscountNotAllowed	{ get; set; }//;
		public string Prefix	{ get; set; }//;
		public string MaskId	{ get; set; }//;
		public int Decimals	{ get; set; }//;
		public string UnitId	{ get; set; }//;
		public bool Blocked	{ get; set; }//;
		public decimal QuantitySold	{ get; set; }//;
		public System.DateTimeOffset DateToBeBlocked	{ get; set; }//;
		public System.DateTimeOffset DateToBeActivated	{ get; set; }//;
		public string Description	{ get; set; }//;
		public bool CheckDigitValidated	{ get; set; }//;
		public ErpBarcodeMaskType MaskType	{ get; set; }//;
		public string CouponId	{ get; set; }//;
		public string Message	{ get; set; }//;
		public System.DateTimeOffset TimeStarted	{ get; set; }//;
		public System.DateTimeOffset TimeFinished	{ get; set; }//;
		public System.TimeSpan TimeElapsed	{ get; set; }//;
		public string LineDiscountGroup	{ get; set; }//;
		public string MultilineDiscountGroup	{ get; set; }//;
		public bool IncludedInTotalDiscount	{ get; set; }//;
		public ErpKeyInPrices KeyInPrice	{ get; set; }//;
		public ErpKeyInQuantities KeyInQuantity	{ get; set; }//;
		public decimal CostPrice	{ get; set; }//;
		public string BatchId	{ get; set; }//;
		public int InternalType	{ get; set; }//;
		public int EntryType	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
