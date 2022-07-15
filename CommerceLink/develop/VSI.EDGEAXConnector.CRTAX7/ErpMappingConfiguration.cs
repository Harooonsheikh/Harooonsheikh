using AutoMapper;
using EdgeAXCommerceLink.Commerce.RetailProxy;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRTAX7
{

    public class ErpMappingConfiguration : Profile
    {
        public ErpMappingConfiguration()
        {
            Configure();
        }

        protected override void Configure()
        {
            //=============================
            //Mapping for CRT to ERP
            //=============================
            #region Mapping for CRT to ERP
            Mapper.CreateMap<ErpSalesOrder, SalesOrder>()
                //.ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode)) // CurrencyCode does not exists in dest
                .ForMember(dest => dest.DocumentStatusValue, opt => opt.MapFrom(src => src.DocumentStatusValue))
                .ForMember(dest => dest.RecordId, opt => opt.MapFrom(src => src.RecordId))
                .ForMember(dest => dest.StatusValue, opt => opt.MapFrom(src => src.StatusValue))
                //.ForMember(dest => dest.OrderPlacedDate, opt => opt.MapFrom(src => src.OrderPlacedDate)) // OrderPlacedDate does not exists in dest
                .ForMember(dest => dest.AmountDue, opt => opt.MapFrom(src => src.AmountDue))
                .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(src => src.AmountPaid))
                .ForMember(dest => dest.AvailableDepositAmount, opt => opt.MapFrom(src => src.AvailableDepositAmount))
                .ForMember(dest => dest.BeginDateTime, opt => opt.MapFrom(src => src.BeginDateTime))
                .ForMember(dest => dest.BusinessDate, opt => opt.MapFrom(src => src.BusinessDate))
                .ForMember(dest => dest.CalculatedDepositAmount, opt => opt.MapFrom(src => src.CalculatedDepositAmount))
                .ForMember(dest => dest.CancellationCharge, opt => opt.MapFrom(src => src.CancellationCharge))
                .ForMember(dest => dest.ChannelId, opt => opt.MapFrom(src => src.ChannelId))
                .ForMember(dest => dest.ChannelReferenceId, opt => opt.MapFrom(src => src.ChannelReferenceId))
                .ForMember(dest => dest.ChargeAmount, opt => opt.MapFrom(src => src.ChargeAmount))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.CustomerOrderTypeValue, opt => opt.MapFrom(src => src.CustomerOrderTypeValue))
                .ForMember(dest => dest.DeliveryMode, opt => opt.MapFrom(src => src.DeliveryMode))
                .ForMember(dest => dest.DeliveryModeChargeAmount, opt => opt.MapFrom(src => src.DeliveryModeChargeAmount))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount))
                .ForMember(dest => dest.DiscountCodes, opt => opt.MapFrom(src => src.DiscountCodes))
                .ForMember(dest => dest.EntryStatusValue, opt => opt.MapFrom(src => src.EntryStatusValue))
                .ForMember(dest => dest.GrossAmount, opt => opt.MapFrom(src => src.GrossAmount))
                .ForMember(dest => dest.HasLoyaltyPayment, opt => opt.MapFrom(src => src.HasLoyaltyPayment))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IncomeExpenseTotalAmount, opt => opt.MapFrom(src => src.IncomeExpenseTotalAmount))
                .ForMember(dest => dest.InventoryLocationId, opt => opt.MapFrom(src => src.InventoryLocationId))
                .ForMember(dest => dest.IsCreatedOffline, opt => opt.MapFrom(src => src.IsCreatedOffline))
                //.ForMember(dest => dest.IsDepositOverridden, opt => opt.MapFrom(src => src.IsDepositOverridden)) // IsDepositOverridden does not exists in dest
                .ForMember(dest => dest.IsReturnByReceipt, opt => opt.MapFrom(src => src.IsReturnByReceipt))
                //.ForMember(dest => dest.IsSales, opt => opt.MapFrom(src => src.IsSales)) // IsSales does not exists in des
                .ForMember(dest => dest.IsSuspended, opt => opt.MapFrom(src => src.IsSuspended))
                .ForMember(dest => dest.IsTaxIncludedInPrice, opt => opt.MapFrom(src => src.IsTaxIncludedInPrice))
                .ForMember(dest => dest.LineDiscount, opt => opt.MapFrom(src => src.LineDiscount))
                .ForMember(dest => dest.LineDiscountCalculationTypeValue, opt => opt.MapFrom(src => src.LineDiscountCalculationTypeValue))
                .ForMember(dest => dest.LoyaltyCardId, opt => opt.MapFrom(src => src.LoyaltyCardId))
                .ForMember(dest => dest.LoyaltyDiscountAmount, opt => opt.MapFrom(src => src.LoyaltyDiscountAmount))
                .ForMember(dest => dest.LoyaltyManualDiscountAmount, opt => opt.MapFrom(src => src.LoyaltyManualDiscountAmount))
                .ForMember(dest => dest.ModifiedDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.NetAmountWithNoTax, opt => opt.MapFrom(src => src.NetAmountWithNoTax))
                .ForMember(dest => dest.NetAmountWithTax, opt => opt.MapFrom(src => src.NetAmountWithTax))
                .ForMember(dest => dest.NumberOfItems, opt => opt.MapFrom(src => src.NumberOfItems))
                .ForMember(dest => dest.OverriddenDepositAmount, opt => opt.MapFrom(src => src.OverriddenDepositAmount))
                .ForMember(dest => dest.PeriodicDiscountAmount, opt => opt.MapFrom(src => src.PeriodicDiscountAmount))
                //.ForMember(dest => dest.PermanentTransactionId, opt => opt.MapFrom(src => src.PermanentTransactionId)) // PermanentTransactionId does not exists in dest
                .ForMember(dest => dest.PrepaymentAmountAppliedOnPickup, opt => opt.MapFrom(src => src.PrepaymentAmountAppliedOnPickup))
                .ForMember(dest => dest.PrepaymentAmountInvoiced, opt => opt.MapFrom(src => src.PrepaymentAmountInvoiced))
                .ForMember(dest => dest.PrepaymentAmountPaid, opt => opt.MapFrom(src => src.PrepaymentAmountPaid))
                .ForMember(dest => dest.QuotationExpiryDate, opt => opt.MapFrom(src => src.QuotationExpiryDate))
                .ForMember(dest => dest.ReceiptEmail, opt => opt.MapFrom(src => src.ReceiptEmail))
                .ForMember(dest => dest.ReceiptId, opt => opt.MapFrom(src => src.ReceiptId))
                .ForMember(dest => dest.RequestedDeliveryDate, opt => opt.MapFrom(src => src.RequestedDeliveryDate))
                .ForMember(dest => dest.RequiredDepositAmount, opt => opt.MapFrom(src => src.RequiredDepositAmount))
                .ForMember(dest => dest.ReturnTransactionHasLoyaltyPayment, opt => opt.MapFrom(src => src.ReturnTransactionHasLoyaltyPayment))
                .ForMember(dest => dest.SalesId, opt => opt.MapFrom(src => src.SalesId))
                .ForMember(dest => dest.ShiftId, opt => opt.MapFrom(src => src.ShiftId))
                .ForMember(dest => dest.ShiftTerminalId, opt => opt.MapFrom(src => src.ShiftTerminalId))
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => src.StaffId))
                .ForMember(dest => dest.StatementCode, opt => opt.MapFrom(src => src.StatementCode))
                .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StoreId))
                .ForMember(dest => dest.SubtotalAmount, opt => opt.MapFrom(src => src.SubtotalAmount))
                .ForMember(dest => dest.SubtotalAmountWithoutTax, opt => opt.MapFrom(src => src.SubtotalAmountWithoutTax))
                .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.TaxAmount))
                .ForMember(dest => dest.TaxAmountExclusive, opt => opt.MapFrom(src => src.TaxAmountExclusive))
                .ForMember(dest => dest.TaxAmountInclusive, opt => opt.MapFrom(src => src.TaxAmountInclusive))
                .ForMember(dest => dest.TaxOnCancellationCharge, opt => opt.MapFrom(src => src.TaxOnCancellationCharge))
                .ForMember(dest => dest.TaxOverrideCode, opt => opt.MapFrom(src => src.TaxOverrideCode))
                .ForMember(dest => dest.TerminalId, opt => opt.MapFrom(src => src.TerminalId))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.TotalDiscount, opt => opt.MapFrom(src => src.TotalDiscount))
                .ForMember(dest => dest.TotalManualDiscountAmount, opt => opt.MapFrom(src => src.TotalManualDiscountAmount))
                .ForMember(dest => dest.TotalManualDiscountPercentage, opt => opt.MapFrom(src => src.TotalManualDiscountPercentage))
                .ForMember(dest => dest.TransactionTypeValue, opt => opt.MapFrom(src => src.TransactionTypeValue))
                //.ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.EntityName)) // EntityName does not exists in dest
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData)) // ExtensionData does not exists in dest
                //.ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item)) // Item does not exists in dest
                //.ForMember(dest => dest.isInernationalOrder, opt => opt.MapFrom(src => src.isInernationalOrder)) // isInernationalOrder does not exists in dest
                //.ForMember(dest => dest.isBorderFree, opt => opt.MapFrom(src => src.isBorderFree)) // isBorderFree does not exists in dest
                //
                //.ForMember(dest => dest.DocumentStatus, opt => opt.MapFrom(src => src.DocumentStatus)) // DocumentStatus does not exists in dest
                //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)) // Status does not exists in dest
                //.ForMember(dest => dest.ActiveSalesLines, opt => opt.MapFrom(src => src.ActiveSalesLines)) // ActiveSalesLines does not exists in dest
                //.ForMember(dest => dest.ActiveTenderLines, opt => opt.MapFrom(src => src.ActiveTenderLines)) // ActiveTenderLines does not exists in dest
                .ForMember(dest => dest.AffiliationLoyaltyTierLines, opt => opt.MapFrom(src => src.AffiliationLoyaltyTierLines))
                .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.AttributeValues))
                //.ForMember(dest => dest.CartType, opt => opt.MapFrom(src => src.CartType)) // CartType does not exists in dest
                //.ForMember(dest => dest.ChargeCalculableSalesLines, opt => opt.MapFrom(src => src.ChargeCalculableSalesLines)) // ChargeCalculableSalesLines does not exists in dest
      //          .ForMember(dest => dest.ChargeLines, opt => opt.MapFrom(src => src.ChargeLines))
                .ForMember(dest => dest.ContactInformationCollection, opt => opt.MapFrom(src => src.ContactInformationCollection))
                //.ForMember(dest => dest.CustomerOrderMode, opt => opt.MapFrom(src => src.CustomerOrderMode)) // CustomerOrderMode does not exists in dest
                //.ForMember(dest => dest.CustomerOrderType, opt => opt.MapFrom(src => src.CustomerOrderType)) // CustomerOrderType does not exists in dest
                //.ForMember(dest => dest.EntryStatus, opt => opt.MapFrom(src => src.EntryStatus)) // EntryStatus does not exists in dest
                .ForMember(dest => dest.IncomeExpenseLines, opt => opt.MapFrom(src => src.IncomeExpenseLines))
                //.ForMember(dest => dest.InventorySalesLines, opt => opt.MapFrom(src => src.InventorySalesLines)) // InventorySalesLines does not exists in dest
                //.ForMember(dest => dest.LineDiscountCalculationType, opt => opt.MapFrom(src => src.LineDiscountCalculationType)) // LineDiscountCalculationType does not exists in dest
                .ForMember(dest => dest.LoyaltyRewardPointLines, opt => opt.MapFrom(src => src.LoyaltyRewardPointLines))
                //.ForMember(dest => dest.PriceCalculableSalesLines, opt => opt.MapFrom(src => src.PriceCalculableSalesLines)) // PriceCalculableSalesLines does not exists in dest
                //.ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Properties)) // Properties does not exists in dest
                .ForMember(dest => dest.ReasonCodeLines, opt => opt.MapFrom(src => src.ReasonCodeLines))
                .ForMember(dest => dest.SalesLines, opt => opt.MapFrom(src => src.SalesLines))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.TaxLines, opt => opt.MapFrom(src => src.TaxLines))
                .ForMember(dest => dest.TenderLines, opt => opt.MapFrom(src => src.TenderLines))
                //.ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType)) // TransactionType  does not exists in dest
                .ForMember(dest => dest.ExtensionProperties, opt => opt.MapFrom(src => src.ExtensionProperties))
                ;
            //Mapper.CreateMap<System.Collections.ObjectModel.ObservableCollection<ChargeLine>, IList<ErpChargeLine>>()
            Mapper.CreateMap<ChargeLine, ErpChargeLine>()
                .ForMember(dest => dest.BeginDateTime, opt => opt.MapFrom(src => src.BeginDateTime))
                .ForMember(dest => dest.CalculatedAmount, opt => opt.MapFrom(src => src.CalculatedAmount))
                .ForMember(dest => dest.ChargeCode, opt => opt.MapFrom(src => src.ChargeCode))
                .ForMember(dest => dest.ChargeMethodValue, opt => opt.MapFrom(src => src.ChargeMethodValue))
                .ForMember(dest => dest.ChargeTypeValue, opt => opt.MapFrom(src => src.ChargeTypeValue))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.EndDateTime, opt => opt.MapFrom(src => src.EndDateTime))
                .ForMember(dest => dest.ExtensionProperties, opt => opt.MapFrom(src => src.ExtensionProperties))
                .ForMember(dest => dest.FromAmount, opt => opt.MapFrom(src => src.FromAmount))
                .ForMember(dest => dest.GrossAmount, opt => opt.MapFrom(src => src.GrossAmount))
                .ForMember(dest => dest.IsReturnByReceipt, opt => opt.MapFrom(src => src.IsReturnByReceipt))
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.ItemTaxGroupId, opt => opt.MapFrom(src => src.ItemTaxGroupId))
                .ForMember(dest => dest.Keep, opt => opt.MapFrom(src => src.Keep))
                .ForMember(dest => dest.ModuleTypeValue, opt => opt.MapFrom(src => src.ModuleTypeValue))
                .ForMember(dest => dest.NetAmount, opt => opt.MapFrom(src => src.NetAmount))
                .ForMember(dest => dest.NetAmountPerUnit, opt => opt.MapFrom(src => src.NetAmountPerUnit))
                .ForMember(dest => dest.NetAmountWithAllInclusiveTax, opt => opt.MapFrom(src => src.NetAmountWithAllInclusiveTax))
                //.ForMember(dest => dest.OriginalItemTaxGroupId, opt => opt.MapFrom(src => src.OriginalItemTaxGroupId)) // Not in dest
                //.ForMember(dest => dest.OriginalSalesTaxGroupId, opt => opt.MapFrom(src => src.OriginalSalesTaxGroupId)) // Not in dest
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.SaleLineNumber, opt => opt.MapFrom(src => src.SaleLineNumber))
                .ForMember(dest => dest.SalesOrderUnitOfMeasure, opt => opt.MapFrom(src => src.SalesOrderUnitOfMeasure))
                .ForMember(dest => dest.SalesTaxGroupId, opt => opt.MapFrom(src => src.SalesTaxGroupId))
                .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.TaxAmount))
                .ForMember(dest => dest.TaxAmountExclusive, opt => opt.MapFrom(src => src.TaxAmountExclusive))
                .ForMember(dest => dest.TaxAmountExemptInclusive, opt => opt.MapFrom(src => src.TaxAmountExemptInclusive))
                .ForMember(dest => dest.TaxAmountInclusive, opt => opt.MapFrom(src => src.TaxAmountInclusive))
                .ForMember(dest => dest.TaxLines, opt => opt.MapFrom(src => src.TaxLines))
                .ForMember(dest => dest.TaxRatePercent, opt => opt.MapFrom(src => src.TaxRatePercent))
                .ForMember(dest => dest.ToAmount, opt => opt.MapFrom(src => src.ToAmount))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))

                ;






            Mapper.CreateMap<SalesOrder, ErpSalesOrder>()
               .ForMember(dest => dest.DocumentStatusValue, opt => opt.MapFrom(src => src.DocumentStatusValue))
               .ForMember(dest => dest.RecordId, opt => opt.MapFrom(src => src.RecordId))
               .ForMember(dest => dest.StatusValue, opt => opt.MapFrom(src => src.StatusValue))
               .ForMember(dest => dest.AmountDue, opt => opt.MapFrom(src => src.AmountDue))
               .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(src => src.AmountPaid))
               .ForMember(dest => dest.AvailableDepositAmount, opt => opt.MapFrom(src => src.AvailableDepositAmount))
               .ForMember(dest => dest.BeginDateTime, opt => opt.MapFrom(src => src.BeginDateTime))
               .ForMember(dest => dest.BusinessDate, opt => opt.MapFrom(src => src.BusinessDate))
               .ForMember(dest => dest.CalculatedDepositAmount, opt => opt.MapFrom(src => src.CalculatedDepositAmount))
               .ForMember(dest => dest.CancellationCharge, opt => opt.MapFrom(src => src.CancellationCharge))
               .ForMember(dest => dest.ChannelId, opt => opt.MapFrom(src => src.ChannelId))
               .ForMember(dest => dest.ChannelReferenceId, opt => opt.MapFrom(src => src.ChannelReferenceId))
               .ForMember(dest => dest.ChargeAmount, opt => opt.MapFrom(src => src.ChargeAmount))
               .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
               .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
               .ForMember(dest => dest.CustomerOrderTypeValue, opt => opt.MapFrom(src => src.CustomerOrderTypeValue))
               .ForMember(dest => dest.DeliveryMode, opt => opt.MapFrom(src => src.DeliveryMode))
               .ForMember(dest => dest.DeliveryModeChargeAmount, opt => opt.MapFrom(src => src.DeliveryModeChargeAmount))
               .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount))
               .ForMember(dest => dest.DiscountCodes, opt => opt.MapFrom(src => src.DiscountCodes))
               .ForMember(dest => dest.EntryStatusValue, opt => opt.MapFrom(src => src.EntryStatusValue))
               .ForMember(dest => dest.GrossAmount, opt => opt.MapFrom(src => src.GrossAmount))
               .ForMember(dest => dest.HasLoyaltyPayment, opt => opt.MapFrom(src => src.HasLoyaltyPayment))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.IncomeExpenseTotalAmount, opt => opt.MapFrom(src => src.IncomeExpenseTotalAmount))
               .ForMember(dest => dest.InventoryLocationId, opt => opt.MapFrom(src => src.InventoryLocationId))
               .ForMember(dest => dest.IsCreatedOffline, opt => opt.MapFrom(src => src.IsCreatedOffline))
               .ForMember(dest => dest.IsReturnByReceipt, opt => opt.MapFrom(src => src.IsReturnByReceipt))
               .ForMember(dest => dest.IsSuspended, opt => opt.MapFrom(src => src.IsSuspended))
               .ForMember(dest => dest.IsTaxIncludedInPrice, opt => opt.MapFrom(src => src.IsTaxIncludedInPrice))
               .ForMember(dest => dest.LineDiscount, opt => opt.MapFrom(src => src.LineDiscount))
               .ForMember(dest => dest.LineDiscountCalculationTypeValue, opt => opt.MapFrom(src => src.LineDiscountCalculationTypeValue))
               .ForMember(dest => dest.LoyaltyCardId, opt => opt.MapFrom(src => src.LoyaltyCardId))
               .ForMember(dest => dest.LoyaltyDiscountAmount, opt => opt.MapFrom(src => src.LoyaltyDiscountAmount))
               .ForMember(dest => dest.LoyaltyManualDiscountAmount, opt => opt.MapFrom(src => src.LoyaltyManualDiscountAmount))
               .ForMember(dest => dest.ModifiedDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.NetAmountWithNoTax, opt => opt.MapFrom(src => src.NetAmountWithNoTax))
               .ForMember(dest => dest.NetAmountWithTax, opt => opt.MapFrom(src => src.NetAmountWithTax))
               .ForMember(dest => dest.NumberOfItems, opt => opt.MapFrom(src => src.NumberOfItems))
               .ForMember(dest => dest.OverriddenDepositAmount, opt => opt.MapFrom(src => src.OverriddenDepositAmount))
               .ForMember(dest => dest.PeriodicDiscountAmount, opt => opt.MapFrom(src => src.PeriodicDiscountAmount))
               .ForMember(dest => dest.PrepaymentAmountAppliedOnPickup, opt => opt.MapFrom(src => src.PrepaymentAmountAppliedOnPickup))
               .ForMember(dest => dest.PrepaymentAmountInvoiced, opt => opt.MapFrom(src => src.PrepaymentAmountInvoiced))
               .ForMember(dest => dest.PrepaymentAmountPaid, opt => opt.MapFrom(src => src.PrepaymentAmountPaid))
               .ForMember(dest => dest.QuotationExpiryDate, opt => opt.MapFrom(src => src.QuotationExpiryDate))
               .ForMember(dest => dest.ReceiptEmail, opt => opt.MapFrom(src => src.ReceiptEmail))
               .ForMember(dest => dest.ReceiptId, opt => opt.MapFrom(src => src.ReceiptId))
               .ForMember(dest => dest.RequestedDeliveryDate, opt => opt.MapFrom(src => src.RequestedDeliveryDate))
               .ForMember(dest => dest.RequiredDepositAmount, opt => opt.MapFrom(src => src.RequiredDepositAmount))
               .ForMember(dest => dest.ReturnTransactionHasLoyaltyPayment, opt => opt.MapFrom(src => src.ReturnTransactionHasLoyaltyPayment))
               .ForMember(dest => dest.SalesId, opt => opt.MapFrom(src => src.SalesId))
               .ForMember(dest => dest.ShiftId, opt => opt.MapFrom(src => src.ShiftId))
               .ForMember(dest => dest.ShiftTerminalId, opt => opt.MapFrom(src => src.ShiftTerminalId))
               .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => src.StaffId))
               .ForMember(dest => dest.StatementCode, opt => opt.MapFrom(src => src.StatementCode))
               .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StoreId))
               .ForMember(dest => dest.SubtotalAmount, opt => opt.MapFrom(src => src.SubtotalAmount))
               .ForMember(dest => dest.SubtotalAmountWithoutTax, opt => opt.MapFrom(src => src.SubtotalAmountWithoutTax))
               .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.TaxAmount))
               .ForMember(dest => dest.TaxAmountExclusive, opt => opt.MapFrom(src => src.TaxAmountExclusive))
               .ForMember(dest => dest.TaxAmountInclusive, opt => opt.MapFrom(src => src.TaxAmountInclusive))
               .ForMember(dest => dest.TaxOnCancellationCharge, opt => opt.MapFrom(src => src.TaxOnCancellationCharge))
               .ForMember(dest => dest.TaxOverrideCode, opt => opt.MapFrom(src => src.TaxOverrideCode))
               .ForMember(dest => dest.TerminalId, opt => opt.MapFrom(src => src.TerminalId))
               .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
               .ForMember(dest => dest.TotalDiscount, opt => opt.MapFrom(src => src.TotalDiscount))
               .ForMember(dest => dest.TotalManualDiscountAmount, opt => opt.MapFrom(src => src.TotalManualDiscountAmount))
               .ForMember(dest => dest.TotalManualDiscountPercentage, opt => opt.MapFrom(src => src.TotalManualDiscountPercentage))
               .ForMember(dest => dest.TransactionTypeValue, opt => opt.MapFrom(src => src.TransactionTypeValue))
               .ForMember(dest => dest.AffiliationLoyaltyTierLines, opt => opt.MapFrom(src => src.AffiliationLoyaltyTierLines))
               .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.AttributeValues))
               .ForMember(dest => dest.ChargeLines, opt => opt.MapFrom(src => src.ChargeLines))
               .ForMember(dest => dest.ContactInformationCollection, opt => opt.MapFrom(src => src.ContactInformationCollection))
               .ForMember(dest => dest.IncomeExpenseLines, opt => opt.MapFrom(src => src.IncomeExpenseLines))
               .ForMember(dest => dest.LoyaltyRewardPointLines, opt => opt.MapFrom(src => src.LoyaltyRewardPointLines))
               .ForMember(dest => dest.ReasonCodeLines, opt => opt.MapFrom(src => src.ReasonCodeLines))
               .ForMember(dest => dest.SalesLines, opt => opt.MapFrom(src => src.SalesLines))
               .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
               .ForMember(dest => dest.TaxLines, opt => opt.MapFrom(src => src.TaxLines))
               .ForMember(dest => dest.TenderLines, opt => opt.MapFrom(src => src.TenderLines))
               .ForMember(dest => dest.ExtensionProperties, opt => opt.MapFrom(src => src.ExtensionProperties))
               ;


            #endregion
        }
    }
}
