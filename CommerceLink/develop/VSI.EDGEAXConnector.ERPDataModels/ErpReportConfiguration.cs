namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReportConfiguration
	{
		public ErpReportConfiguration()
		{
		}
		public string ReportId	{ get; set; }//;
		public string DataSourceType	{ get; set; }//;
		public string Query	{ get; set; }//;
		public ErpParameterSet Parameters	{ get; set; }//;
		public bool IsUserBasedReport	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<string> RolesAllowed	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
