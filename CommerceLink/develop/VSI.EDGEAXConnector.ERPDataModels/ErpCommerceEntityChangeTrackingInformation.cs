namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCommerceEntityChangeTrackingInformation
	{
		public ErpCommerceEntityChangeTrackingInformation()
		{
		}
		public long RecordId	{ get; set; }//;
		public long Sequence	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
