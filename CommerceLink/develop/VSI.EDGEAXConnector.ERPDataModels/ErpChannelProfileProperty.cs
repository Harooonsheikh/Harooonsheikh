namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpChannelProfileProperty
	{
		public ErpChannelProfileProperty()
		{
		}
		public int Key	{ get; set; }//;
		public string Value	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
