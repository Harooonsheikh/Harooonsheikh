namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCountryRegionInfo
	{
		public ErpCountryRegionInfo()
		{
		}
		public string CountryRegionId	{ get; set; }//;
		public string ShortName	{ get; set; }//;
		public string LongName	{ get; set; }//;
		public string ISOCode	{ get; set; }//;
		public int TimeZone	{ get; set; }//;
		public string LanguageId	{ get; set; }//;
		public System.Collections.Generic.IList<ErpAddressFormattingInfo> AddressFormatLines	{ get; set; }//;
		public string AddressFormatId	{ get; set; }//;
		public string AddressFormatName	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
