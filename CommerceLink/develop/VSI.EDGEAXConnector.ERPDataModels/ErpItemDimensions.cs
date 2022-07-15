namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpItemDimensions
	{
		public ErpItemDimensions()
		{
		}
		public string ItemId	{ get; set; }//;
		public decimal GrossHeight	{ get; set; }//;
		public decimal GrossWidth	{ get; set; }//;
		public decimal GrossDepth	{ get; set; }//;
		public decimal GrossWeight	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
