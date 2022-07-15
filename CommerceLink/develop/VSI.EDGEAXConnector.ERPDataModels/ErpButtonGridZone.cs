namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpButtonGridZone
	{
		public ErpButtonGridZone()
		{
		}
		public string ButtonGridId	{ get; set; }//;
		public string ZoneId	{ get; set; }//;
		public string ZoneName	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
