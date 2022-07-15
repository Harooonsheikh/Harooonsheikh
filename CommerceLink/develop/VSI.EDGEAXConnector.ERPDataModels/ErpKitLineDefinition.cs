namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpKitLineDefinition
	{
		public ErpKitLineDefinition()
		{
		}
		public long ComponentProductId	{ get; set; }//;
		public System.Collections.Generic.ICollection<long> SubstituteProductIds	{ get; set; }//;
		public ErpKitLineProductPropertyDictionary IndexedComponentProperties	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpKitLineProductProperty> ComponentProperties	{ get; set; }//;
		public long KitLineIdentifier	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
