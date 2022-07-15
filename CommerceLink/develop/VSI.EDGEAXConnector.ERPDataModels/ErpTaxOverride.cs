namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTaxOverride
	{
		public ErpTaxOverride()
		{
		}
		public string Code	{ get; set; }//;
		public string SourceTaxGroup	{ get; set; }//;
		public string DestinationTaxGroup	{ get; set; }//;
		public ErpTaxOverrideType OverrideType	{ get; set; }//;
		public string SourceItemTaxGroup	{ get; set; }//;
		public string DestinationItemTaxGroup	{ get; set; }//;
		public ErpTaxOverrideBy OverrideBy	{ get; set; }//;
		public long ChannelId	{ get; set; }//;
		public string AdditionalDescription	{ get; set; }//;
		public int Status	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
