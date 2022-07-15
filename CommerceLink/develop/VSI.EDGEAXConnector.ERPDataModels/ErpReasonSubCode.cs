namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReasonSubCode
	{
		public ErpReasonSubCode()
		{
		}
		public string ReasonCodeId	{ get; set; }//;
		public string SubCodeId	{ get; set; }//;
		public string Description	{ get; set; }//;
		public ErpTriggerFunctionType TriggerFunctionType	{ get; set; }//;
		public int TriggerFunctionTypeValue	{ get; set; }//;
		public string TriggerCode	{ get; set; }//;
		public bool NewSalesLine	{ get; set; }//;
		public ErpPriceType PriceType	{ get; set; }//;
		public int PriceTypeValue	{ get; set; }//;
		public decimal AmountPercent	{ get; set; }//;
		public string LanguageId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
