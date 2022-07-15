
using VSI.EDGEAXConnector.ERPDataModels;

public class ErpSalesOrderSearchCriteria
{
	public ErpSalesOrderSearchCriteria()
	{
	}
	public System.Collections.Generic.IEnumerable<string> TransactionIds	{ get; set; }//;
	public string SalesId	{ get; set; }//;
	public string ReceiptId	{ get; set; }//;
	public string ChannelReferenceId	{ get; set; }//;
	public string CustomerAccountNumber	{ get; set; }//;
	public string CustomerFirstName	{ get; set; }//;
	public string CustomerLastName	{ get; set; }//;
	public string StoreId	{ get; set; }//;
	public string TerminalId	{ get; set; }//;
	public string ItemId	{ get; set; }//;
	public string Barcode	{ get; set; }//;
	public string SerialNumber	{ get; set; }//;
	public string StaffId	{ get; set; }//;
	public System.Nullable<System.DateTimeOffset> StartDateTime	{ get; set; }//;
	public System.Nullable<System.DateTimeOffset> EndDateTime	{ get; set; }//;
	public bool IncludeDetails	{ get; set; }//;
	public string ReceiptEmailAddress	{ get; set; }//;
	public string SearchIdentifiers	{ get; set; }//;
	public System.Collections.Generic.IEnumerable<ErpSalesTransactionType> SalesTransactionTypes	{ get; set; }//;
	public System.Collections.Generic.IEnumerable<int> SalesTransactionTypeValues	{ get; set; }//;
	public int SearchLocationTypeValue	{ get; set; }//;
	public ErpSearchLocation SearchLocationType	{ get; set; }//;
	public System.Collections.Generic.IEnumerable<ErpTransactionStatus> TransactionStatusTypes	{ get; set; }//;
	public System.Collections.Generic.IEnumerable<int> TransactionStatusTypeValues	{ get; set; }//;
}
