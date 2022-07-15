using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels.BoletoPayment;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomTenderLine
    {
        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }
        public string CardOrAccount { get; set; }
        public string CardHolder { get; set; }
        public int? ExpMonth { get; set; }
        public int? ExpYear { get; set; }
        public string UniqueCardId { get; set; }
        public string PayerId { get; set; }
        public string ParentTransactionId { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public EcomAddress BillingAddress { get; set; }
        public string ProcessorId { get; set; }
        public string CreditCardProcessors { get; set; }
        public long RecId { get; set; }
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
        public string TransactionId { get; set; }
        public Boleto Boleto { get; set; }
        public string LocalTaxId { get; set; }
        public string IP { get; set; }

        public ErpAlipay Alipay { get; set; }

        public string PspReference { get; set; }
    }
}
