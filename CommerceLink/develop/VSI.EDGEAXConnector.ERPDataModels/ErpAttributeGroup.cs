namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpAttributeGroup
	{
		public ErpAttributeGroup()
		{
		}
		public long RecordId	{ get; set; }//;
		public long Id	{ get; set; }//;
		public string Name	{ get; set; }//;
		public ErpAttributeGroupType GroupType	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
