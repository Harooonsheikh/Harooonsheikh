namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpIncomeExpenseAccount
	{
		public ErpIncomeExpenseAccount()
		{
		}
		public decimal Amount	{ get; set; }//;
		public string AccountNumber	{ get; set; }//;
		public string AccountName	{ get; set; }//;
		public string AccountNameAlias	{ get; set; }//;
		public ErpIncomeExpenseAccountType AccountType	{ get; set; }//;
		public int AccountTypeValue	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
		public string Message1Line	{ get; set; }//;
		public string Message2Line	{ get; set; }//;
		public string SlipText1Line	{ get; set; }//;
		public string SlipText2Line	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
