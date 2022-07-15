namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpShiftAccountLine
	{
		public ErpShiftAccountLine()
		{
		}
		public ErpIncomeExpenseAccountType AccountType	{ get; set; }//;
		public int AccountTypeValue	{ get; set; }//;
		public string AccountNumber	{ get; set; }//;
		public decimal Amount	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
