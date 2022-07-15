namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpSalesTransaction
	{
		public ErpSalesTransaction()
		{
		}
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpSalesLine> ActiveSalesLines	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpTenderLine> ActiveTenderLines	{ get; set; }//;
		public System.Collections.Generic.IList<ErpSalesAffiliationLoyaltyTier> AffiliationLoyaltyTierLines	{ get; set; }//;
		public decimal AmountDue	{ get; set; }//;
		public decimal AmountPaid	{ get; set; }//;
		public System.Collections.Generic.IList<ErpAttributeValueBase> AttributeValues	{ get; set; }//;
		public decimal AvailableDepositAmount	{ get; set; }//;
		public System.DateTimeOffset BeginDateTime	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> BusinessDate	{ get; set; }//;
		public decimal CalculatedDepositAmount	{ get; set; }//;
		public System.Nullable<decimal> CancellationCharge	{ get; set; }//;
		public ErpCartType CartType	{ get; set; }//;
		public long ChannelId	{ get; set; }//;
		public string ChannelReferenceId	{ get; set; }//;
		public decimal ChargeAmount	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpSalesLine> ChargeCalculableSalesLines	{ get; set; }//;
		public System.Collections.Generic.IList<ErpChargeLine> ChargeLines	{ get; set; }//;
		public string Comment	{ get; set; }//;
		public System.Collections.Generic.IList<ErpContactInformation> ContactInformationCollection	{ get; set; }//;
		public string CustomerId	{ get; set; }//;
		public ErpCustomerOrderMode CustomerOrderMode	{ get; set; }//;
		public ErpCustomerOrderType CustomerOrderType	{ get; set; }//;
		public int CustomerOrderTypeValue	{ get; set; }//;
		public string DeliveryMode	{ get; set; }//;
		public System.Nullable<decimal> DeliveryModeChargeAmount	{ get; set; }//;
		public decimal DiscountAmount	{ get; set; }//;
		public System.Collections.Generic.IList<string> DiscountCodes	{ get; set; }//;
		public ErpTransactionStatus EntryStatus	{ get; set; }//;
		public int EntryStatusValue	{ get; set; }//;
		public decimal GrossAmount	{ get; set; }//;
		public bool HasLoyaltyPayment	{ get; set; }//;
		public string Id	{ get; set; }//;
		public System.Collections.Generic.IList<ErpIncomeExpenseLine> IncomeExpenseLines	{ get; set; }//;
		public decimal IncomeExpenseTotalAmount	{ get; set; }//;
		public string InventoryLocationId	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpSalesLine> InventorySalesLines	{ get; set; }//;
		public bool IsCreatedOffline	{ get; set; }//;
		public bool IsDepositOverridden	{ get; set; }//;
		public bool IsReturnByReceipt	{ get; set; }//;
		public bool IsSales	{ get; set; }//;
		public bool IsSuspended	{ get; set; }//;
		public bool IsTaxIncludedInPrice	{ get; set; }//;
		public decimal LineDiscount	{ get; set; }//;
		public ErpLineDiscountCalculationType LineDiscountCalculationType	{ get; set; }//;
		public int LineDiscountCalculationTypeValue	{ get; set; }//;
		public string LoyaltyCardId	{ get; set; }//;
		public decimal LoyaltyDiscountAmount	{ get; set; }//;
		public System.Nullable<decimal> LoyaltyManualDiscountAmount	{ get; set; }//;
		public System.Collections.Generic.IList<ErpLoyaltyRewardPointLine> LoyaltyRewardPointLines	{ get; set; }//;
		public System.DateTimeOffset ModifiedDateTime	{ get; set; }//;
		public string Name	{ get; set; }//;
		public decimal NetAmountWithNoTax	{ get; set; }//;
		public decimal NetAmountWithTax	{ get; set; }//;
		public decimal NumberOfItems	{ get; set; }//;
		public decimal OverriddenDepositAmount	{ get; set; }//;
		public decimal PeriodicDiscountAmount	{ get; set; }//;
		public string PermanentTransactionId	{ get; set; }//;
		public decimal PrepaymentAmountAppliedOnPickup	{ get; set; }//;
		public decimal PrepaymentAmountInvoiced	{ get; set; }//;
		public decimal PrepaymentAmountPaid	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpSalesLine> PriceCalculableSalesLines	{ get; set; }//;
		public ErpParameterSet Properties	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> QuotationExpiryDate	{ get; set; }//;
		public System.Collections.Generic.IList<ErpReasonCodeLine> ReasonCodeLines	{ get; set; }//;
		public string ReceiptEmail	{ get; set; }//;
		public string ReceiptId	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> RequestedDeliveryDate	{ get; set; }//;
		public decimal RequiredDepositAmount	{ get; set; }//;
		public bool ReturnTransactionHasLoyaltyPayment	{ get; set; }//;
		public string SalesId	{ get; set; }//;
		public System.Collections.Generic.IList<ErpSalesLine> SalesLines	{ get; set; }//;
		public long ShiftId	{ get; set; }//;
		public string ShiftTerminalId	{ get; set; }//;
		public ErpAddress ShippingAddress	{ get; set; }//;
		public string StaffId	{ get; set; }//;
		public string StatementCode	{ get; set; }//;
		public string StoreId	{ get; set; }//;
		public decimal SubtotalAmount	{ get; set; }//;
		public decimal SubtotalAmountWithoutTax	{ get; set; }//;
		public decimal TaxAmount	{ get; set; }//;
		public decimal TaxAmountExclusive	{ get; set; }//;
		public decimal TaxAmountInclusive	{ get; set; }//;
		public System.Collections.Generic.IList<ErpTaxLine> TaxLines	{ get; set; }//;
		public System.Nullable<decimal> TaxOnCancellationCharge	{ get; set; }//;
		public string TaxOverrideCode	{ get; set; }//;
		public System.Collections.Generic.IList<ErpTenderLine> TenderLines	{ get; set; }//;
		public string TerminalId	{ get; set; }//;
		public decimal TotalAmount	{ get; set; }//;
		public decimal TotalDiscount	{ get; set; }//;
		public decimal TotalManualDiscountAmount	{ get; set; }//;
		public decimal TotalManualDiscountPercentage	{ get; set; }//;
		public ErpSalesTransactionType TransactionType	{ get; set; }//;
		public int TransactionTypeValue	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
