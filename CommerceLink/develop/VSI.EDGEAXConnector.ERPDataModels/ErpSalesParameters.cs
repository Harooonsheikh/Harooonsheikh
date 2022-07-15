namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpSalesParameters
	{
		public ErpSalesParameters()
		{
		}
		public long RecordId	{ get; set; }//;
		public bool UseHeaderCharges	{ get; set; }//;
		public bool UseLineCharges	{ get; set; }//;
		public ErpLineDiscountCalculationType LineMultilineMethod	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
