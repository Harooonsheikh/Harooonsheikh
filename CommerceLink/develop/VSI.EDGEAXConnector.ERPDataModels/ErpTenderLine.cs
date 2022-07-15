using System;
using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpTenderLine
    {
        public ErpTenderLine()
        {
        }
        public decimal? Amount { get; set; }
        public decimal? AmountInCompanyCurrency { get; set; }
        public decimal? AmountInTenderedCurrency { get; set; }
        public string Authorization { get; set; }
        public string CardToken { get; set; }
        public int? NumberOfInstallments { get; set; }
        public string BankIdentificationNumberStart { get; set; }
        public string ApprovalCode { get; set; }
        public string shopperReference { get; set; }
        public string IssuerCountry { get; set; }

        public string CardTypeId { get; set; }
        public decimal? CashBackAmount { get; set; }
        public decimal? CompanyCurrencyExchangeRate { get; set; }
        public string CreditMemoId { get; set; }
        public string Currency { get; set; }
        public string CustomerId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
        public string GiftCardId { get; set; }
        public int? IncomeExpenseAccountTypeValue { get; set; }
        public bool? IsChangeLine { get; set; }
        public bool? IsDeposit { get; set; }
        public bool? IsHistorical { get; set; }
        public bool? IsPreProcessed { get; set; }
        public bool? IsVoidable { get; set; }
        public decimal? LineNumber { get; set; }
        public string LoyaltyCardId { get; set; }
        public string MaskedCardNumber { get; set; }
        public ObservableCollection<ErpReasonCodeLine> ReasonCodeLines { get; set; }
        public string SignatureData { get; set; }
        public int? StatusValue { get; set; }
        public DateTimeOffset? TenderDate { get; set; }
        public string TenderLineId { get; set; }
        public string TenderTypeId { get; set; }
        public int? TransactionStatusValue { get; set; }
        public int? VoidStatusValue { get; set; }
        public string ThreeDSecure { get; set; }
        public string IBAN { get; set; }
        public string SwiftCode { get; set; }
        public string BankName { get; set; }
    }
}