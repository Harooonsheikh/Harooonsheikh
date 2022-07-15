namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpImageZone
	{
		public ErpImageZone()
		{
		}
		public int PictureId	{ get; set; }//;
		public string PictureAsBase64	{ get; set; }//;
		public string ZoneId	{ get; set; }//;
		public string ZoneName	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
