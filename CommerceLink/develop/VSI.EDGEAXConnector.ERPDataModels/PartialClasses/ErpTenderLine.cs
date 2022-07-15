using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels.BoletoPayment;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpTenderLine
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
        public ErpAddress BillingAddress { get; set; }
        public string ProcessorId { get; set; }
        public string CreditCardProcessors { get; set; }
        public long RecId { get; set; }
        public string TransactionId { get; set; }
        public Boleto Boleto { get; set; }
        public string BoletoXml { get; set; }
        public string LocalTaxId { get; set; }
        public string IP { get; set; }

        public ErpAlipay Alipay { get; set; }

        public string PspReference { get; set; }
    }

}