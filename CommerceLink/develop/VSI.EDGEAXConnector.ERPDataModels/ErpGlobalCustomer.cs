namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpGlobalCustomer
	{
		public ErpGlobalCustomer()
		{
		}
		public string PartyNumber	{ get; set; }//;
		public long RecordId	{ get; set; }//;
		public string AccountNumber	{ get; set; }//;
		public string FullName	{ get; set; }//;
		public string FullAddress	{ get; set; }//;
		public string Phone	{ get; set; }//;
		public string Email	{ get; set; }//;
		public ErpCustomerType CustomerType	{ get; set; }//;
		public int CustomerTypeValue	{ get; set; }//;
		public ErpRichMediaLocations Image	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
