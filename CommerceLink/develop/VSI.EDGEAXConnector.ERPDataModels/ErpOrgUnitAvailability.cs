namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpOrgUnitAvailability
	{
		public ErpOrgUnitAvailability()
		{
		}
		public ErpOrgUnitLocation OrgUnitLocation	{ get; set; }//;
		public System.Collections.Generic.IEnumerable<ErpItemAvailability> ItemAvailabilities	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
