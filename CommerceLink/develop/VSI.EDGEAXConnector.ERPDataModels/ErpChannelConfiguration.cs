namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpChannelConfiguration
	{
		public ErpChannelConfiguration()
		{
		}
		public long RecordId	{ get; set; }//;
		public ErpRetailChannelType ChannelType	{ get; set; }//;
		public string InventLocation	{ get; set; }//;
		public string InventLocationDataAreaId	{ get; set; }//;
		public string Currency	{ get; set; }//;
		public string CompanyCurrency	{ get; set; }//;
		public bool PriceIncludesSalesTax	{ get; set; }//;
		public string CountryRegionId	{ get; set; }//;
		public string ChannelCountryRegionISOCode	{ get; set; }//;
		public ErpCountryRegionISOCode CountryRegionISOCode	{ get; set; }//;
		public string DefaultLanguageId	{ get; set; }//;
		public long ExchangeRateType	{ get; set; }//;
		public string StaffPasswordHash	{ get; set; }//;
		public string TransactionServicePasswordEncryptionType	{ get; set; }//;
		public string TransactionServiceDeviceTokenAlgorithm	{ get; set; }//;
		public string TransactionServiceProfileId	{ get; set; }//;
		public ErpLogOnConfiguration TransactionServiceStaffLogOnConfiguration	{ get; set; }//;
		public string TransactionServiceHost	{ get; set; }//;
		public int TransactionServicePort	{ get; set; }//;
		public string TransactionServiceEncryptedPassword	{ get; set; }//;
		public string TransactionServiceServerCertificateDns	{ get; set; }//;
		public string TransactionServiceServiceName	{ get; set; }//;
		public int TransactionServiceProtocol	{ get; set; }//;
		public bool TransactionServiceSecurityOff	{ get; set; }//;
		public string PickupDeliveryModeCode	{ get; set; }//;
		public decimal CancellationChargePercentage	{ get; set; }//;
		public string CancellationChargeCode	{ get; set; }//;
		public string ShippingChargeCode	{ get; set; }//;
		public int QuoteExpirationDays	{ get; set; }//;
		public decimal MinimumDepositPercentage	{ get; set; }//;
		public string BingMapsApiKey	{ get; set; }//;
		public string TimeZoneInfoId	{ get; set; }//;
		public string EmailDeliveryModeCode	{ get; set; }//;
		public string GiftCardItemId	{ get; set; }//;
		public bool EarnLoyaltyOffline	{ get; set; }//;
		public string CatalogDefaultImageTemplate	{ get; set; }//;
		public string EmployeeDefaultImageTemplate	{ get; set; }//;
		public string CustomerDefaultImageTemplate	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpChannelProfileProperty> ProfileProperties	{ get; set; }//;
		public ErpTransactionServiceProfile TransactionServiceProfile	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start
        public bool BingMapsEnabled { get; set; }
        public int NotificationRefreshInterval { get; set; }
        //NS: D365 Update 12 Platform change end

        //NS: D365 Update 8.1 Application change start
        public bool AllowExchangeOnReturnOrders { get; set; }

        //NS: D365 Update 8.1 Application change end
		//HK: D365 Update 10.0 Application change start
        public bool UseAdvancedAutoCharges { get; set; }
        public bool EnableReturnsForMultipleOrderInvoices { get; set; }
        public int VoidSuspendedTransactionsOnCloseShift { get; set; }
        public bool EnableOmniChannelPayments { get; set; }
        public bool UseAdvancedCashManagement { get; set; }
        public int SalesOrderHeaderAttributeGroups { get; set; }
        public int SalesOrderLinesAttributeGroups { get; set; }
		//HK: D365 Update 10.0 Application change end

    }
}
