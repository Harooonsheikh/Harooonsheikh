namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpBarcodeMaskSegment
	{
		public ErpBarcodeMaskSegment()
		{
		}
		public long RecordId	{ get; set; }//;
		public int Decimals	{ get; set; }//;
		public int Length	{ get; set; }//;
		public string MaskId	{ get; set; }//;
		public int SegmentNumber	{ get; set; }//;
		public int MaskType	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
