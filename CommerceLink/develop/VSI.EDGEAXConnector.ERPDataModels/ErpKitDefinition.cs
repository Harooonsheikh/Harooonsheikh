namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpKitDefinition
	{
		public ErpKitDefinition()
		{
		}
		public bool DisassembleAtRegister	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpKitLineDefinition> KitLineDefinitions	{ get; set; }//;
		public ErpProductToKitVariantDictionary IndexedProductToKitVariantMap	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpComponentKitVariantSet> KitLineProductToVariantMap	{ get; set; }//;
		public ErpKitVariantToComponentDictionary IndexedKitVariantToComponentMap	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpKitVariantContent> KitVariantToComponentMap	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
