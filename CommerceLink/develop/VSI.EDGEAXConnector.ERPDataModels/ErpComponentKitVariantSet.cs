namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpComponentKitVariantSet
	{
		public ErpComponentKitVariantSet()
		{
		}
		public long KitLineProductId	{ get; set; }//;
		public System.Collections.Generic.ICollection<long> KitVariantIdList	{ get; set; }//;
	}
}
