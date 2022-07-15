namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpUnitOfMeasureConversion
	{
		public ErpUnitOfMeasureConversion()
		{
		}
		public string ItemId	{ get; set; }//;
		public string FromUnitOfMeasureId	{ get; set; }//;
		public string ToUnitOfMeasureId	{ get; set; }//;
		public string FromUnitOfMeasureSymbol	{ get; set; }//;
		public string ToUnitOfMeasureSymbol	{ get; set; }//;
		public bool IsBackward	{ get; set; }//;
		public long RecordId	{ get; set; }//;
		public long ProductRecordId	{ get; set; }//;
		public decimal Factor	{ get; set; }//;
		public int Numerator	{ get; set; }//;
		public int Denominator	{ get; set; }//;
		public decimal InnerOffset	{ get; set; }//;
		public decimal OuterOffset	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
