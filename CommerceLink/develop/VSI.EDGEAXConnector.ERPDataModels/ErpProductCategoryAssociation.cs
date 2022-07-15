namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductCategoryAssociation
	{
		public ErpProductCategoryAssociation()
		{
		}
		public long ProductRecordId	{ get; set; }//;
		public long CategoryRecordId	{ get; set; }//;
		public long CatalogRecordId	{ get; set; }//;
		public bool IsPrimaryCategory	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
