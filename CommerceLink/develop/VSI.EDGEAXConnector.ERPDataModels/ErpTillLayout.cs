namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpTillLayout
	{
		public ErpTillLayout()
		{
		}
		public string CashChangerLayoutXml	{ get; set; }//;
		public string CustomerLayoutId	{ get; set; }//;
		public string CustomerLayoutXml	{ get; set; }//;
		public int Height	{ get; set; }//;
		public string LayoutId	{ get; set; }//;
		public string LayoutXml	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string ReceiptId	{ get; set; }//;
		public string ReceiptItemsLayoutXml	{ get; set; }//;
		public string ReceiptPaymentLayoutXml	{ get; set; }//;
		public string TotalId	{ get; set; }//;
		public string TotalsLayoutXml	{ get; set; }//;
		public int Width	{ get; set; }//;
		public long RecordId	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpButtonGridZone> ButtonGridZones	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpImageZone> ImageZones	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpReportZone> ReportZones	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
