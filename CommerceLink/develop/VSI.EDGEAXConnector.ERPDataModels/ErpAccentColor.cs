namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpAccentColor
	{
		public ErpAccentColor()
		{
		}
		public string AccentId	{ get; set; }//;
		public string AccentName	{ get; set; }//;
		public int Color	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
