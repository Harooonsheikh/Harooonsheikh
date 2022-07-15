namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTenderType
	{
		public ErpTenderType()
		{
		}
		public long RecordId	{ get; set; }//;
		public string TenderTypeId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public int OperationId	{ get; set; }//;
		public ErpRetailOperation OperationType	{ get; set; }//;
		public string OperationName	{ get; set; }//;
		public int Function	{ get; set; }//;
		public string ChangeTenderTypeId	{ get; set; }//;
		public bool OpenDrawer	{ get; set; }//;
		public bool UseSignatureCaptureDevice	{ get; set; }//;
		public decimal MinimumSignatureCaptureAmount	{ get; set; }//;
		public bool IsOvertenderAllowed	{ get; set; }//;
		public bool IsUndertenderAllowed	{ get; set; }//;
		public decimal MaximumOvertenderAmount	{ get; set; }//;
		public decimal MaximumUndertenderAmount	{ get; set; }//;
		public decimal MaximumAmountPerTransaction	{ get; set; }//;
		public decimal MaximumAmountPerLine	{ get; set; }//;
		public decimal MinimumAmountPerTransaction	{ get; set; }//;
		public decimal MinimumAmountPerLine	{ get; set; }//;
		public decimal MinimumChangeAmount	{ get; set; }//;
		public ErpRoundingMethod RoundingMethod	{ get; set; }//;
		public decimal RoundOff	{ get; set; }//;
		public int CountingRequired	{ get; set; }//;
		public int TakenToBank	{ get; set; }//;
		public int TakenToSafe	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start
        public string ConnectorId { get; set; }
        public string GiftCardItem { get; set; }
        public bool HideCardInputDetails { get; set; }
        //NS: D365 Update 12 Platform change end

        //NS: D365 Update 8.1 Application change start
        public string ChangeLineOnReceipt { get; set; }
        public bool CashDrawerLimitEnabled { get; set; }
        public decimal QuantityReturnable { get; set; }

        //NS: D365 Update 8.1 Application change end
//HK: D365 Update 10.0 Application change start
        public decimal GiftCardCashoutOutThreshold { get; set; }
        public bool RestrictReturnsWithoutReceipt { get; set; }
//HK: D365 Update 10.0 Application change end

    }
}
