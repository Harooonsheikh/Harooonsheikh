namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpChargeConfiguration
	{
		public ErpChargeConfiguration()
		{
		}
		public long RecordId	{ get; set; }//;
		public ErpChargeLevel ChargeLevel	{ get; set; }//;
		public ErpChargeModule ConfigurationModule	{ get; set; }//;
		public ErpChargeAccountType AccountCode	{ get; set; }//;
		public string AccountRelation	{ get; set; }//;
		public ErpChargeItemType ItemCode	{ get; set; }//;
		public string ItemRelation	{ get; set; }//;
		public ErpChargeDeliveryType DeliveryModeCode	{ get; set; }//;
		public string DeliveryModeRelation	{ get; set; }//;
		public decimal Value	{ get; set; }//;
		public string CurrencyCode	{ get; set; }//;
		public string ChargeCode	{ get; set; }//;
		public ErpChargeMethod ChargeMethod	{ get; set; }//;
		public string SalesTaxGroup	{ get; set; }//;
		public string ItemTaxGroup	{ get; set; }//;
		public decimal FromAmount	{ get; set; }//;
		public decimal ToAmount	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
