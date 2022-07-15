namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpKitLineProductProperty
	{
		public ErpKitLineProductProperty()
		{
		}
		public long KitLineIdentifier	{ get; set; }//;
		public long ProductId	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public string Unit	{ get; set; }//;
		public decimal Charge	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
