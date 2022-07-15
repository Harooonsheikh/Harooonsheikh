using VSI.CommerceLink.EcomDataModel.Enum;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomCustomer
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public EcomCustomerType CustomerType { get; set; }
        public string Language { get; set; }
        public string IdentificationNumber { get; set; }
        public string VatNumber { get; set; }
        public System.Collections.Generic.IList<EcomAddress> Addresses { get; set; }
        //public EcomRichMediaLocations Image { get; set; }
        public string Image { get; set; }
        public string EntityName { get; set; }
        //public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }
        public string ExtensionData { get; set; }
        public System.Collections.Generic.ICollection<EcomCommerceProperty> ExtensionProperties { get; set; }
        //public object ExtensionProperties { get; set; }
        public object Item { get; set; }
        public string EcomCustomerId { get; set; }
        public string SLBirthMonth { get; set; }
        //public List<EcomAddress> CustomerAddresses { get; set; }
        public string CustomerAddresses { get; set; }
        public bool? IsAsyncCustomer { get; set; }
        public System.Collections.Generic.IList<EcomCustomerAttribute> Attributes { get; set; }

        // [MB] - TV - BR 3.0 - 15007 - Start
        public bool SwapLanguage { get; set; }
        public string CurrencyCode { get; set; }
        public string Url { get; set; }//;
        // [MB] - TV - BR 3.0 - 15007 - End
        // [MB] - TV - BR 3.0 - 158787 - Start
        public long UrlRecordId { get; set; }
        // [MB] - TV - BR 3.0 - 158787 - End

        public EcomCustomer()
        {
            this.CurrencyCode = null;
            this.SwapLanguage = false;
        }

    }
}
