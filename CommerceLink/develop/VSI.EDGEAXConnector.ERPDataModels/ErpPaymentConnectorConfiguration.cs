namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpPaymentConnectorConfiguration
	{
		public ErpPaymentConnectorConfiguration()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string Properties	{ get; set; }//;
		public bool IsTestMode	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
