using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTenderDetail
	{
		public ErpTenderDetail()
		{
		}
		public string TenderId	{ get; set; }//;
		public decimal Amount	{ get; set; }//;
		public decimal ForeignCurrencyAmount	{ get; set; }//;
		public decimal ForeignCurrencyExchangeRate	{ get; set; }//;
		public string Currency	{ get; set; }//;
		public decimal CompanyExchangeRate	{ get; set; }//;
		public decimal CompanyAmount	{ get; set; }//;
		public string BankBagNumber	{ get; set; }//;
		public string TenderTypeId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start
        public ObservableCollection<ErpDenominationDetail> DenominationDetails { get; set; }
        //NS: D365 Update 12 Platform change end
    }
}
