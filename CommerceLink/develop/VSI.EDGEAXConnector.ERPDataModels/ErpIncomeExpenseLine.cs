namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpIncomeExpenseLine
	{
		public ErpIncomeExpenseLine()
		{
		}
		public decimal Amount	{ get; set; }//;
		public string IncomeExpenseAccount	{ get; set; }//;
		public string StoreNumber	{ get; set; }//;
		public ErpIncomeExpenseAccountType AccountType	{ get; set; }//;
		public int AccountTypeValue	{ get; set; }//;
		public string Terminal	{ get; set; }//;
		public string Shift	{ get; set; }//;
		public ErpTransactionStatus TransactionStatus	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
