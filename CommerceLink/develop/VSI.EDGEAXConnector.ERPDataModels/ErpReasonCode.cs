namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpReasonCode
	{
		public ErpReasonCode()
		{
		}
		public string ReasonCodeId	{ get; set; }//;
		public string Description	{ get; set; }//;
		public string Prompt	{ get; set; }//;
		public bool OncePerTransaction	{ get; set; }//;
		public bool PrintPromptToReceipt	{ get; set; }//;
		public bool PrintInputToReceipt	{ get; set; }//;
		public bool PrintInputNameOnReceipt	{ get; set; }//;
		public ErpReasonCodeInputType InputType	{ get; set; }//;
		public int InputTypeValue	{ get; set; }//;
		public decimal MinimumValue	{ get; set; }//;
		public decimal MaximumValue	{ get; set; }//;
		public int MinimumLength	{ get; set; }//;
		public int MaximumLength	{ get; set; }//;
		public bool InputRequired	{ get; set; }//;
		public string LinkedReasonCodeId	{ get; set; }//;
		public decimal RandomFactor	{ get; set; }//;
		public bool RetailUseReasonCode	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpReasonSubCode> ReasonSubCodes	{ get; set; }//;
		public string LanguageId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start
        public int ActivityValue { get; set; }
        //NS: D365 Update 12 Platform change end

        //NS: D365 Update 8.1 Application change start
        public bool IsMultiLineText { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}
