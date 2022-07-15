namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpKitConfigToComponentAssociation
	{
		public ErpKitConfigToComponentAssociation()
		{
		}
		public long KitLineIdentifier	{ get; set; }//;
		public long ComponentProductId	{ get; set; }//;
		public long KitProductVariantId	{ get; set; }//;
		public string KitVariantInventDimId	{ get; set; }//;
		public string KitConfigId	{ get; set; }//;
		public long KitProductMasterId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
