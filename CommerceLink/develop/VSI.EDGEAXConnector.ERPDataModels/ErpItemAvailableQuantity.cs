namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpItemAvailableQuantity
	{
		public ErpItemAvailableQuantity()
		{
		}
		public string ItemId	{ get; set; }//;
		public string VariantInventoryDimensionId	{ get; set; }//;
		public decimal AvailableQuantity	{ get; set; }//;
		public string UnitOfMeasure	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
