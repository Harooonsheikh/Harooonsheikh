namespace VSI.EDGEAXConnector.ERPDataModels
{

    public class ErpPaymentCard
    {
        public ErpPaymentCard()
        {
        }

        //NS:
        //Adding flag to identify PayPal
        public bool IsPayPal { get; set; }

        public bool UseShippingAddress { get; set; }//;
        public string Country { get; set; }//;
        public string BillingName { get; set; }
        public string Address1 { get; set; }//;
        public string Address2 { get; set; }//;
        public string City { get; set; }//;
        public string State { get; set; }//;
        public string Zip { get; set; }//;
        public string Phone { get; set; }//;
        public string NameOnCard { get; set; }//;
        public string CardToken { get; set; }
        public string UniqueCardId { get; set; }
        public string CardTypes { get; set; }//;
        public string CardNumber { get; set; }//;
        public string CCID { get; set; }//;
        public int? ExpirationMonth { get; set; }//;
        public int? ExpirationYear { get; set; }//;
        public string Track1 { get; set; }//;
        public string Track2 { get; set; }//;
        public string Track3 { get; set; }//;
        public bool IsSwipe { get; set; }//;
        public string EncryptedPin { get; set; }//;
        public string AdditionalSecurityData { get; set; }//;
        public string VoiceAuthorizationCode { get; set; }//;
        public string EntityName { get; set; }//;
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }//;
        public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties { get; set; }//;
        public object Item { get; set; }//;
        public string ECommerceValue { get; set; }
        //++ For TV PayPal
        public string PayerId { get; set; }
        public string ParentTransactionId { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        //++ For TV AllPago
        public int? NumberOfInstallments { get; set; }
        //++ For TV Adyen
        public string BankIdentificationNumberStart { get; set; }
        public string ApprovalCode { get; set; }
        public string shopperReference { get; set; }

        public string TransactionId { get; set; }
        public string IssuerCountry { get; set; }

        public string ThreeDSecure { get; set; }

        public string IP { get; set; }

        public string LocalTaxId { get; set; }
    }
}
