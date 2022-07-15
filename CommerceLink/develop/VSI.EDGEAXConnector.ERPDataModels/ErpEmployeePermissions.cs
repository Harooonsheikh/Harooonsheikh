namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpEmployeePermissions
	{
		public ErpEmployeePermissions()
		{
		}
		public bool ContinueOnTSErrors	{ get; set; }//;
		public bool AllowBlindClose	{ get; set; }//;
		public bool AllowKitDisassembly	{ get; set; }//;
		public bool AllowChangeNoVoid	{ get; set; }//;
		public bool HasManagerPrivileges	{ get; set; }//;
		public bool AllowCreateOrder	{ get; set; }//;
		public bool AllowEditOrder	{ get; set; }//;
		public bool AllowFloatingTenderDeclaration	{ get; set; }//;
		public bool AllowMultipleLogins	{ get; set; }//;
		public bool AllowMultipleShiftLogOn	{ get; set; }//;
		public bool AllowOpenDrawer	{ get; set; }//;
		public int AllowPriceOverride	{ get; set; }//;
		public bool AllowRetrieveOrder	{ get; set; }//;
		public bool AllowSalesTaxChange	{ get; set; }//;
		public bool AllowTenderDeclaration	{ get; set; }//;
		public bool AllowTransactionSuspension	{ get; set; }//;
		public bool AllowTransactionVoiding	{ get; set; }//;
		public bool AllowXReportPrinting	{ get; set; }//;
		public bool AllowZReportPrinting	{ get; set; }//;
		public bool AllowPasswordChange	{ get; set; }//;
		public bool AllowResetPassword	{ get; set; }//;
		public decimal MaximumDiscountPercentage	{ get; set; }//;
		public decimal MaximumLineDiscountAmount	{ get; set; }//;
		public decimal MaximumLineReturnAmount	{ get; set; }//;
		public decimal MaximumTotalDiscountAmount	{ get; set; }//;
		public decimal MaximumTotalDiscountPercentage	{ get; set; }//;
		public decimal MaxTotalReturnAmount	{ get; set; }//;
		public bool AllowUseHandheld	{ get; set; }//;
		public bool AllowViewTimeClockEntries	{ get; set; }//;
		public bool AllowChangePeripheralStation	{ get; set; }//;
		public bool ManageDevice	{ get; set; }//;
		public string StaffId	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<string> Roles	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start

        public bool AllowCreateTransferOrder { get; set; }
        public bool AllowAcceptOrder { get; set; }
        public bool AllowRejectOrder { get; set; }

        //NS: D365 Update 12 Platform change end

        //NS: D365 Update 8.1 Application change start
        public bool AllowMassActivation { get; set; }

        //NS: D365 Update 8.1 Application change end
		//HK: D365 Update 10.0 Application change start
        public bool MyPAllowSkipRegistrationOrMarkAsRegisteredroperty { get; set; }
		//HK: D365 Update 10.0 Application change end
    }
}
