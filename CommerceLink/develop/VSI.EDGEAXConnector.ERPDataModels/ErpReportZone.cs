namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReportZone
	{
		public ErpReportZone()
		{
		}
		public long ReportRecordId	{ get; set; }//;
		public string ZoneId	{ get; set; }//;
		public string ZoneName	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
