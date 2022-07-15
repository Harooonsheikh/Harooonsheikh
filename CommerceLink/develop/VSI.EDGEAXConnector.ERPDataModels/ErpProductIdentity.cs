namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductIdentity
	{
		public ErpProductIdentity()
		{
		}
		public long RecordId	{ get; set; }//;
		public long LookupId	{ get; set; }//;
		public bool IsMasterProduct	{ get; set; }//;
		public bool IsKitProduct	{ get; set; }//;
		public bool IsRemoteProduct	{ get; set; }//;
		public long MasterProductId	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string InventDimId	{ get; set; }//;
		public string ProductSearchName	{ get; set; }//;
		public string ProductDisplayNumber	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
