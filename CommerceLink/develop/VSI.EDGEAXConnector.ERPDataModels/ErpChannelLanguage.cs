namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpChannelLanguage
	{
		public ErpChannelLanguage()
		{
		}
		public long Channel	{ get; set; }//;
		public string LanguageId	{ get; set; }//;
		public bool IsDefault	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
