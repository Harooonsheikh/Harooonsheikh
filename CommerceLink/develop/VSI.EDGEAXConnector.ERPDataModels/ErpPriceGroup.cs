namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpPriceGroup
	{
		public ErpPriceGroup()
		{
		}
		public long RecordId	{ get; set; }//;
		public long PriceGroupId	{ get; set; }//;
		public string GroupId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
