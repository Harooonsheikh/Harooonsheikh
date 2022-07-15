namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpStockCountJournal
	{
		public ErpStockCountJournal()
		{
		}
		public string RecordId	{ get; set; }//;
		public string JournalId	{ get; set; }//;
		public string Description	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpStockCountJournalTransaction> StockCountTransactionLines	{ get; set; }//;
		public string Worker	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
