namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpCustomer
	{
		public ErpCustomer()
		{
		}
		public string AccountNumber	{ get; set; }//;
		public long RecordId	{ get; set; }//;
		public System.DateTimeOffset CreatedDateTime	{ get; set; }//;
		public string ChargeGroup	{ get; set; }//;
		public string PriceGroup	{ get; set; }//;
		public bool IsCustomerTaxInclusive	{ get; set; }//;
		public string Phone	{ get; set; }//;
		public long PhoneRecordId	{ get; set; }//;
		public string PhoneExt	{ get; set; }//;
		public string Cellphone	{ get; set; }//;
		public string Email	{ get; set; }//;
		public long EmailRecordId	{ get; set; }//;
		public string Url	{ get; set; }//;
		public long UrlRecordId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public long PersonNameId	{ get; set; }//;
		public string FirstName	{ get; set; }//;
		public string MiddleName	{ get; set; }//;
		public string LastName	{ get; set; }//;
		public long DirectoryPartyRecordId	{ get; set; }//;
		public string PartyNumber	{ get; set; }//;
		public ErpCustomerType CustomerType	{ get; set; }//;
		public int CustomerTypeValue	{ get; set; }//;
		public string Language	{ get; set; }//;
		public string TaxGroup	{ get; set; }//;
		public string CustomerGroup	{ get; set; }//;
		public string CurrencyCode	{ get; set; }//;
		public string CNPJCPFNumber	{ get; set; }//;
		public string IdentificationNumber	{ get; set; }//;
		public string InvoiceAccount	{ get; set; }//;
		public bool MandatoryCreditLimit	{ get; set; }//;
		public string CreditRating	{ get; set; }//;
		public decimal CreditLimit	{ get; set; }//;
		public decimal Balance	{ get; set; }//;
		public bool Blocked	{ get; set; }//;
		public bool UseOrderNumberReference	{ get; set; }//;
		public string OrganizationId	{ get; set; }//;
		public bool UsePurchaseRequest	{ get; set; }//;
		public string MultilineDiscountGroup	{ get; set; }//;
		public string TotalDiscountGroup	{ get; set; }//;
		public string LineDiscountGroup	{ get; set; }//;
		public string SalesTaxGroup	{ get; set; }//;
		public string TaxExemptNumber	{ get; set; }//;
		public string VatNumber	{ get; set; }//;
		public string TaxOffice	{ get; set; }//;
		public bool NonChargeableAccount	{ get; set; }//;
		public string Tag	{ get; set; }//;
		public int ReceiptSettings	{ get; set; }//;
		public string ReceiptEmail	{ get; set; }//;
		public long RetailCustomerTableRecordId	{ get; set; }//;
		public string NewCustomerPartyNumber	{ get; set; }//;
		public long PhoneLogisticsLocationRecordId	{ get; set; }//;
		public string PhoneLogisticsLocationId	{ get; set; }//;
		public long PhonePartyLocationRecId	{ get; set; }//;
		public long EmailLogisticsLocationRecordId	{ get; set; }//;
		public string EmailLogisticsLocationId	{ get; set; }//;
		public long EmailPartyLocationRecId	{ get; set; }//;
		public long UrlLogisticsLocationRecordId	{ get; set; }//;
		public string UrlLogisticsLocationId	{ get; set; }//;
		public long UrlPartyLocationRecId	{ get; set; }//;
		public long CellphoneRecordId	{ get; set; }//;
		public long CellphoneLogisticsLocationRecordId	{ get; set; }//;
		public string CellphoneLogisticsLocationId	{ get; set; }//;
		public long CellphonePartyLocationRecId	{ get; set; }//;
		public System.Collections.Generic.IList<ErpCustomerAffiliation> CustomerAffiliations	{ get; set; }//;
		public System.Collections.Generic.IList<ErpAddressBookPartyData> AddressBooks	{ get; set; }//;
		public System.Collections.Generic.IList<ErpAddress> Addresses	{ get; set; }//;
		public ErpRichMediaLocations Image	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start

        public int BlockedType { get; set; }

        //NS: D365 Update 12 Platform change end

        public System.Collections.Generic.IList<ErpCustomerAttribute> Attributes { get; set; }
    }
}
