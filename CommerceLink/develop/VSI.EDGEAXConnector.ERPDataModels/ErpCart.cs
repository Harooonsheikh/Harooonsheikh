using System;
using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCart
	{
		public ErpCart()
		{
		}
        public ObservableCollection<ErpAffiliationLoyaltyTier> AffiliationLines { get; set; }
        public decimal? AmountDue { get; set; }
        public decimal? AmountPaid { get; set; }
        public ObservableCollection<ErpAttributeValueBase> AttributeValues { get; set; }
        public decimal? AvailableDepositAmount { get; set; }
        public DateTimeOffset? BeginDateTime { get; set; }
        public DateTimeOffset? BusinessDate { get; set; }
        public decimal? CancellationChargeAmount { get; set; }
        public ObservableCollection<ErpCartLine> CartLines { get; set; }
        public int? CartStatusValue { get; set; }
        public int? CartTypeValue { get; set; }
        public long? ChannelId { get; set; }
        public decimal? ChargeAmount { get; set; }
        public ObservableCollection<ErpChargeLine> ChargeLines { get; set; }
        public string Comment { get; set; }
        public string CommissionSalesGroup { get; set; }
        public ObservableCollection<ErpCoupon> Coupons { get; set; }
        public ObservableCollection<ErpCustomerAccountDepositLine> CustomerAccountDepositLines { get; set; }
        public string CustomerId { get; set; }
        public int? CustomerOrderModeValue { get; set; }
        public string DeliveryMode { get; set; }
        public decimal? DeliveryModeChargeAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public ObservableCollection<string> DiscountCodes { get; set; }
        public decimal? EstimatedShippingAmount { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
        public string Id { get; set; }
        public ObservableCollection<ErpIncomeExpenseLine> IncomeExpenseLines { get; set; }
        public decimal? IncomeExpenseTotalAmount { get; set; }
        public string InvoiceComment { get; set; }
        public bool? IsCreatedOffline { get; set; }
        public bool? IsDiscountFullyCalculated { get; set; }
        public bool? IsFavorite { get; set; }
        public bool? IsRecurring { get; set; }
        public bool? IsRequiredAmountPaid { get; set; }
        public bool? IsReturnByReceipt { get; set; }
        public bool? IsSuspended { get; set; }
        public string LoyaltyCardId { get; set; }
        public DateTimeOffset? ModifiedDateTime { get; set; }
        public string Name { get; set; }
        public string OrderNumber { get; set; }
        public decimal? OverriddenDepositAmount { get; set; }
        public decimal? PrepaymentAmountPaid { get; set; }
        public decimal? PrepaymentAppliedOnPickup { get; set; }
        public ObservableCollection<string> PromotionLines { get; set; }
        public DateTimeOffset? QuotationExpiryDate { get; set; }
        public ObservableCollection<ErpReasonCodeLine> ReasonCodeLines { get; set; }
        public string ReceiptEmail { get; set; }
        public int? ReceiptTransactionTypeValue { get; set; }
        public DateTimeOffset? RequestedDeliveryDate { get; set; }
        public decimal? RequiredDepositAmount { get; set; }
        public bool? ReturnTransactionHasLoyaltyPayment { get; set; }
        public string SalesId { get; set; }
        public ErpAddress ShippingAddress { get; set; }
        public string StaffId { get; set; }
        public decimal? SubtotalAmount { get; set; }
        public decimal? SubtotalAmountWithoutTax { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TaxOnCancellationCharge { get; set; }
        public string TaxOverrideCode { get; set; }
        public ObservableCollection<ErpTaxViewLine> TaxViewLines { get; set; }
        public ObservableCollection<ErpTenderLine> TenderLines { get; set; }
        public string TerminalId { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalManualDiscountAmount { get; set; }
        public decimal? TotalManualDiscountPercentage { get; set; }
        public int? TransactionTypeValue { get; set; }
        public long? Version { get; set; }
        public string WarehouseId { get; set; }


        //NS: D365 Update 8.1 Application change start

        public decimal OverriddenDepositWithoutCarryoutAmount { get; set; }
        public decimal RequiredDepositWithoutCarryoutAmount { get; set; }
        public decimal SubtotalSalesAmount { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public decimal TotalReturnAmount { get; set; }
        public decimal TotalCarryoutSalesAmount { get; set; }
        public decimal TotalCustomerOrderSalesAmount { get; set; }
        public bool HasTaxCalculationTriggered { get; set; }

        //NS: D365 Update 8.1 Application change end
		//HK: D365 Update 10.0 Application change start
        public bool IgnoreDiscountCalculation { get; set; }
        public decimal CustomerOrderRemainingBalance { get; set; }
        public string SuspendedCartId { get; set; }
        public bool HasChargeCalculationTriggered { get; set; }
       	//HK: D365 Update 10.0 Application change end
    }
}