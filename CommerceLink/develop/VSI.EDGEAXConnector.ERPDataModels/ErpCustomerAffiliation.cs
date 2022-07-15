namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCustomerAffiliation
	{
		public ErpCustomerAffiliation()
		{
		}
		public long RecordId	{ get; set; }//;
		public string CustAccountNum	{ get; set; }//;
		public long RetailAffiliationId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
