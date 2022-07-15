namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReasonCodeSpecific
	{
		public ErpReasonCodeSpecific()
		{
		}
		public string ReasonCodeId	{ get; set; }//;
		public bool InputRequired	{ get; set; }//;
		public ErpReasonCodeTableRefType TableRefType	{ get; set; }//;
		public int TableRefTypeValue	{ get; set; }//;
		public string ReferenceRelation	{ get; set; }//;
		public string ReferenceRelation2	{ get; set; }//;
		public string ReferenceRelation3	{ get; set; }//;
		public int SequenceNumber	{ get; set; }//;
		public ErpReasonCodeInputRequiredType InputRequiredType	{ get; set; }//;
		public int InputRequiredTypeValue	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
