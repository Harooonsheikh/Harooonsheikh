namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpShipmentLineMapping
	{
		public ErpShipmentLineMapping()
		{
		}
		public string SalesId	{ get; set; }//;
		public string ShipmentId	{ get; set; }//;
		public decimal SalesLineNumber	{ get; set; }//;
		public decimal ShipmentLineNumber	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
