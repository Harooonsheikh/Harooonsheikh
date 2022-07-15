namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpLinkedProduct
	{
		public ErpLinkedProduct()
		{
		}
		public long ProductRecordId	{ get; set; }//;
		public long LinkedProductRecordId	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
