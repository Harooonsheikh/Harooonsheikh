namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpChannelProfile
	{
		public ErpChannelProfile()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public int ProfileTypeIdentifier	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpChannelProfileProperty> ProfileProperties	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
