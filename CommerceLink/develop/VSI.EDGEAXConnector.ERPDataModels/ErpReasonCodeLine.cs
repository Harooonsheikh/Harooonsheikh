using System;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReasonCodeLine
	{
		public ErpReasonCodeLine()
		{
		}
		public string LineId	{ get; set; }//;
		public decimal Amount	{ get; set; }//;
		public string Information	{ get; set; }//;
		public decimal InformationAmount	{ get; set; }//;
		public ErpReasonCodeInputType InputType	{ get; set; }//;
		public int InputTypeValue	{ get; set; }//;
		public string ItemTender	{ get; set; }//;
		public decimal LineNumber	{ get; set; }//;
		public ErpReasonCodeLineType LineType	{ get; set; }//;
		public int LineTypeValue	{ get; set; }//;
		public string ParentLineId	{ get; set; }//;
		public string ReasonCodeId	{ get; set; }//;
		public string StatementCode	{ get; set; }//;
		public string SubReasonCodeId	{ get; set; }//;
		public string SourceCode	{ get; set; }//;
		public string SourceCode2	{ get; set; }//;
		public string SourceCode3	{ get; set; }//;
		public string TransactionId	{ get; set; }//;
		public bool IsChanged	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
//HK: D365 Update 10.0 Application change start
        public Guid FiscalTransactionParentGuid { get; set; }
//HK: D365 Update 10.0 Application change end
    }
}
