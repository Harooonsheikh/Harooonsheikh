namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpDiscountCode
	{
		public ErpDiscountCode()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Barcode	{ get; set; }//;
		public string Code	{ get; set; }//;
		public int ConcurrencyMode	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string Disclaimer	{ get; set; }//;
		public bool IsDiscountCodeRequired	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string OfferId	{ get; set; }//;
		public bool IsEnabled	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> ValidFrom	{ get; set; }//;
		public System.Nullable<System.DateTimeOffset> ValidTo	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
