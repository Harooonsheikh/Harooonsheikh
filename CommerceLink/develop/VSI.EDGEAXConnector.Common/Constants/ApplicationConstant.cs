namespace VSI.EDGEAXConnector.Common.Constants
{
    public static class ApplicationConstant
    {
        public const string ECOM_DEMANDWARE_ADAPTER_ASSEMBLY = "VSI.CommerceLink.DemandwareAdapter";
        public const string ECOM_MAGENTO_ADAPTER_ASSEMBLY = "VSI.CommerceLink.MagentoAdapter";

        public const string FILE_TYPE_CSV = "CSV";
        public const string FILE_TYPE_XML = "XML";

        public const string UserName = "System";

        #region Ingram constants
        public const string IngramOrderTypePurchase = "purchase";
        public const string IngramOrderTypeChange = "change";
        public const string IngramOrderTypeCancel = "cancel";
        public const string IngramOrderTypeTransfer = "transfer";

        public const string IngramD365OrderStatusCompleted = "Completed";
        public const string IngramD365OrderStatusInvoiced = "Invoiced";
        public const string IngramD365OrderStatusCanceled = "Canceled";
        public const string IngramD365OrderStatusDelivered = "Delivered";
        public const string IngramD365OrderStatusRenewal = "Renewal";

        public const string IngramOrderStatusApprove = "approve";
        public const string IngramOrderStatusFail = "fail";
        public const string IngramOrderStatusInquire = "inquire";


        public const string IngramResponseOrderStatusApproved = "approved";
        public const string IngramResponseOrderStatusFailed = "failed";
        public const string IngramResponseOrderStatusInquiring = "inquiring";
        public const string IngramResponseOrderStatusPending = "pending";



        public const string IngramPacLicenseParameterName = "PAC_ID_for_license_transfer";
        public const string IngramEndcustomerAdminEmail = "Endcustomer_Admin_Email";
        public const string IngramEndcustomerAdminEmailErrorMessage = "Please provide a valid admin email address";

        public const string IngramMissingParameterStatus = "MissingParameter";

        #endregion

        #region Sales order Custom Attribute
        public const string SalesOrderCustomAttributeTmvSalesOrigion = "TMVSALESORIGIN";
        public const string SalesOrderCustomAttributeRelationshipType = "RelationshipType";

        public const string SalesOrderCustomAtrributeSalesOriginIngram = "Ingram";
        public const string SalesOrderCustomAttributeRelationshipTypeReseller = "Reseller";

        #endregion

    }
}
