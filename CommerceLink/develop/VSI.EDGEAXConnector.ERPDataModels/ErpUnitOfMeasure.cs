namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpUnitOfMeasure
	{
		public ErpUnitOfMeasure()
		{
		}
		public long RecordId	{ get; set; }//;
		public int DecimalPrecision	{ get; set; }//;
		public string Symbol	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
