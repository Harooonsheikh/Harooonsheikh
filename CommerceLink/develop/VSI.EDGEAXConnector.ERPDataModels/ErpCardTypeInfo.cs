namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpCardTypeInfo
	{
		public ErpCardTypeInfo()
		{
		}
		public long RecordId	{ get; set; }//;
		public string TypeId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string PaymentMethodId	{ get; set; }//;
		public ErpCardType CardType	{ get; set; }//;
		public int CardTypeValue	{ get; set; }//;
		public string Issuer	{ get; set; }//;
		public string NumberFrom	{ get; set; }//;
		public string NumberTo	{ get; set; }//;
		public decimal CashBackLimit	{ get; set; }//;
		public bool AllowManualInput	{ get; set; }//;
		public bool CheckModulus	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
