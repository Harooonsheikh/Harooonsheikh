namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpLocalizedString
	{
		public ErpLocalizedString()
		{
		}
		public long RecordId	{ get; set; }//;
		public string LanguageId	{ get; set; }//;
		public int TextId	{ get; set; }//;
		public string Text	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
