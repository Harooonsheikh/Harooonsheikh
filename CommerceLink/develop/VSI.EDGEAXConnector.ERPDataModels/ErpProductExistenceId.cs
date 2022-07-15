namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductExistenceId
	{
		public ErpProductExistenceId()
		{
		}
		public long ProductId	{ get; set; }//;
		public string LanguageId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
