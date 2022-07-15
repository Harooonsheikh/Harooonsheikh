namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpPriceParameters
	{
		public ErpPriceParameters()
		{
		}
		public long RecordId	{ get; set; }//;
		public bool ApplyPriceForCustomerAndItem	{ get; set; }//;
		public bool ApplyPriceForCustomerGroupAndItem	{ get; set; }//;
		public bool ApplyPriceForAllCustomersAndItem	{ get; set; }//;
		public bool ApplyLineDiscountForCustomerAndItem	{ get; set; }//;
		public bool ApplyLineDiscountForCustomerAndItemGroup	{ get; set; }//;
		public bool ApplyLineDiscountForCustomerAndAllItems	{ get; set; }//;
		public bool ApplyLineDiscountForCustomerGroupAndItem	{ get; set; }//;
		public bool ApplyLineDiscountForCustomerGroupAndItemGroup	{ get; set; }//;
		public bool ApplyLineDiscountForCustomerGroupAndAllItems	{ get; set; }//;
		public bool ApplyLineDiscountForAllCustomersAndItem	{ get; set; }//;
		public bool ApplyLineDiscountForAllCustomersAndItemGroup	{ get; set; }//;
		public bool ApplyLineDiscountForAllCustomersAndAllItems	{ get; set; }//;
		public bool ApplyMultilineDiscountForCustomerAndItemGroup	{ get; set; }//;
		public bool ApplyMultilineDiscountForCustomerAndAllItems	{ get; set; }//;
		public bool ApplyMultilineDiscountForCustomerGroupAndItemGroup	{ get; set; }//;
		public bool ApplyMultilineDiscountForCustomerGroupAndAllItems	{ get; set; }//;
		public bool ApplyMultilineDiscountForAllCustomersAndItemGroup	{ get; set; }//;
		public bool ApplyMultilineDiscountForAllCustomersAndAllItems	{ get; set; }//;
		public bool ApplyTotalDiscountForCustomerAndAllItems	{ get; set; }//;
		public bool ApplyTotalDiscountForCustomerGroupAndAllItems	{ get; set; }//;
		public bool ApplyTotalDiscountForAllCustomersAndAllItems	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
