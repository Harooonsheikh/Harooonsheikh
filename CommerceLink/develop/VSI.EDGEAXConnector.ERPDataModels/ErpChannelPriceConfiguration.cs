namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpChannelPriceConfiguration
	{
		public ErpChannelPriceConfiguration()
		{
		}
		public string Company	{ get; set; }//;
		public string CompanyCurrency	{ get; set; }//;
		public string ChannelTimeZoneId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
