namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpKitComponent
	{
		public ErpKitComponent()
		{
		}
		public long KitLineIdentifier	{ get; set; }//;
		public long KitLineProductId	{ get; set; }//;
		public long KitLineProductMasterId	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public string Unit	{ get; set; }//;
		public decimal Charge	{ get; set; }//;
		public bool IsComponent	{ get; set; }//;
		public long KitProductMasterId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
