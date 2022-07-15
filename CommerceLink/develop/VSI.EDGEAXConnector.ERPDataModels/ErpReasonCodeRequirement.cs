namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReasonCodeRequirement
	{
		public ErpReasonCodeRequirement()
		{
		}
		public string ReasonCodeId	{ get; set; }//;
		public string SourceId	{ get; set; }//;
		public ErpReasonCodeTableRefType TableRefType	{ get; set; }//;
		public int TableRefTypeValue	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
