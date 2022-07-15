using System;

namespace VSI.EDGEAXConnector.ERPDataModels.BoletoPayment
{
    public class Boleto
    {
        public string Id { get; set; }
        public string RegistrationId { get; set; }
        public string PaymentType { get; set; }
        public string PaymentBrand { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Descriptor { get; set; }
        public string MerchantTransactionId { get; set; }
        public string BuildNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public string NDC { get; set; }

        public BoletoResult Result { get; set; }
        public BoletoResultDetail ResultDetails { get; set; }
        public BoletoCustomer Customer { get; set; }
        public BoletoBilling Billing { get; set; }
        public BoletoCustomParameter CustomParameters { get; set; }
    }
}
