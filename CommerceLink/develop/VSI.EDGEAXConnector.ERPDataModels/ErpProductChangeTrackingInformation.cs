namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductChangeTrackingInformation
	{
		public ErpProductChangeTrackingInformation()
		{
		}
		public System.DateTimeOffset ModifiedDateTime	{ get; set; }//;
		public ErpChangeAction ChangeAction	{ get; set; }//;
		public int ChangeActionValue	{ get; set; }//;
		public ErpPublishingAction RequestedAction	{ get; set; }//;
		public int RequestedActionValue	{ get; set; }//;
	}
}
