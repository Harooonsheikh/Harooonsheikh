namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpBarcodeMask
	{
		public ErpBarcodeMask()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string Mask	{ get; set; }//;
		public string MaskId	{ get; set; }//;
		public string Prefix	{ get; set; }//;
		public int BarcodeStandardType	{ get; set; }//;
		public ErpBarcodeMaskType MaskType	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
