namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpKitVariantContent
	{
		public ErpKitVariantContent()
		{
		}
		public long KitVariantId	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpKitComponentKey> KitComponentKeyList	{ get; set; }//;
	}
}
