namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTextValueTranslation
	{
		public ErpTextValueTranslation()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Language	{ get; set; }//;
		public string Text	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
