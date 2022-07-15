namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductCatalogAssociation
	{
		public ErpProductCatalogAssociation()
		{
		}
		public long ProductRecordId	{ get; set; }//;
		public long CatalogRecordId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
