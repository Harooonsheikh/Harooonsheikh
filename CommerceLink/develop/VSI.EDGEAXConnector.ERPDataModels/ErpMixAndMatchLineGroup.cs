namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpMixAndMatchLineGroup
	{
		public ErpMixAndMatchLineGroup()
		{
		}
		public long RecordId	{ get; set; }//;
		public string OfferId	{ get; set; }//;
		public string LineGroup	{ get; set; }//;
		public int NumberOfItemsNeeded	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
