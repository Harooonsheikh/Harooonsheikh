// Creates groups and enum from default configuration
namespace VSI.EDGEAXConnector.Configuration
{
	internal enum GROUP
	{ 
		APPLICATION,
		PRODUCT,
		CUSTOMER,
		DISCOUNT,
		DISCOUNTWITHAFFILIATION,
		ECOM,
		NOTIFICATION,
		ADDRESS,
		PRICE,
		SALESORDER,
		INVENTORY,
		STORE,
		CHANNELCONFIGURATION,
		OFFERTYPEGROUPS,
		QUOTATIONREASONGROUP,
		EXTERNALWEBAPI,
        QUANTITYDISCOUNT,
        PAYMENTCONNECTOR,
        QUANTITYDISCOUNTWITHAFFILIATION,
        INGRAM,
    }
	public enum APPLICATION
	{
        Mongo_DBName,
        Mongo_Connection,
        Mongo_ChunkSize,
        IsApplicationMode,
		Defualt_SalesTaxGroupId_Constant,
		Enviornment,
		Local_Base_Path,
		Remote_Base_Path,
		XML_Base_Path,
		Magento_XML_Base_Path,
		Default_Culture,
		Channel_Id,
		TimeStamp_Difference,
		ThreadSleep_Time,
		ERP_Default_Customer_Group,
		ERP_Customer_Default_TaxGroup,
		ERP_Default_Address_Type,
		ZipCode_Truncate_Enable,
		Reset_Time_InMinutes,
		Retail_Media_Path,
		Default_Currency_Code,
		ERP_Default_GiftCard_Delivery_Mode,
		ERP_AX_OUN,
		ERP_AX_RetailServerUri,
		ERP_AX_InferPeriodicDiscount,
		ERP_Legal_Company,
		Windows_Service,
		File_Service,
		Client_Secret,
		D365_Machine_Url,
		Azure_Active_Directory,
		Client_Id,
		Allow_Security_Protocols,
		AbandonedCartDays,
		AbandonedCartSalesOriginId,
		AbandonedCartTitle,
		AbandonedCartOrderType,
        ERP_Default_Customer,
        ERP_AX_SalesOrderPriceRounding,
        ERP_AX_DiscountPriceRounding,
        ERP_AX_PriceRounding,
        AOS_Url,
        Mandatory_State_Countries,
        Ingram_Default_Currency_Code
    }

	public enum PRODUCT
	{
		IncludeConfigurationDimension,
		SKU_Prefix,
		SKU_Postfix,
		Local_Output_Path,
		Remote_Path,
		Image_Remote_Path,
		Filename_Prefix,
		ProductImage_FileName_Prefix,
		Image_Mediaserver_URL,
		CSV_Map_Path,
		Attr_Color,
		Attr_Size,
		Attr_Width,
		Delta_Disable,
		Delete_Disable,
		Category_Assignment_Delete,
		Image_Local_Output_Path,
		Attr_Not_For_Variant,
		File_Generate_Retry,
		Attr_PrimaryCategory_Name,
		Flat_Hierarchy_Enable,
		Attr_Not_For_Simple,
		Attr_IsMaster,
		Attr_Flat_Hierarchy_Related,
		Retail_Server_Paging,
		OfferTypeGroupName,
		CrossSellType,
		MasterProductTypeEcomName,
		VariantProductTypeEcomName,
        Single_Consolidated_Catalog,
        Log_Data,
        Catalog_Retail_Server_Paging,
        IsAvaTaxEnabled,
        Enable_Region_Catalog,
        Enable_Catalog_Delta,
        Catalog_Delta_Batch_Size,
        Variant_Attributes_To_Keep,
        Read_Changed_Enabled
    }

	public enum CUSTOMER
	{
		Default_ThreeLetterISORegionName,
		Default_CurrencyCode,
		DefaultCustomer,
		Remote_File_Path,
		Local_Output_Path,
		Remote_Upload_SFTP_Path,
		Local_Input_Path,
		Address_Local_Output_Path,
		Address_Upload_SFTP_Path,
		Job_id,
        TMVSanctionFlag,
        TMVSanctionStatus,
        TMVIsDuplicateUser,
        TMVDuplicateCustomerAccountNumber,
        TMVSanctionFlagDataType,
        TMVSanctionStatusDataType,
        TMVIsDuplicateUserDataType,
        TMVDuplicateCustomerAccountNumberDataType,
        Is_Ecom_Id_String,
		StateToValidateForCountries
	}

	public enum DISCOUNT
	{
		Remote_Path,
		Filename_Prefix,
		Local_Output_Path,
		Required_InFuture,
		Pricebook_Id,
		Parent_Pricebook_Id,
		CSV_Map_Path
	}

	public enum DISCOUNTWITHAFFILIATION
	{
		Filename_Prefix,
		Local_Output_Path,
		CSV_Map_Path,
        Remote_Path
    }

    public enum QUANTITYDISCOUNT
    {
        Remote_Path,
        Filename_Prefix,
        Local_Output_Path,
        CSV_Map_Path
    }

    public enum QUANTITYDISCOUNTWITHAFFILIATION
    {
        Remote_Path,
        Filename_Prefix,
        Local_Output_Path,
        CSV_Map_Path
    }

    public enum ECOM
	{
		Magento_API_URL,
		TimeZone_Difference_InHours,
		Root_Category_Id,
		Magento_API_Username,
		Magento_Store_Id,
		SalesPerson_Id,
		Remote_SFTP_Host,
		Remote_SFTP_UserName,
		Remote_SFTP_Password,
		Remote_SFTP_Extenstions,
        Remote_SFTP_Port,
        Remote_SFTP_Time_Out,
        Constant_Filename_For_SFTP,
		Magento_API_Password,
		Category_Assignment,
		CSV_Column_Delimiter,
		CSV_IsfirstRow_Header,
		CSV_Max_Buffer_Size,
		CSV_Maximum_Rows,
		CSV_Skip_Starting_Rows_inNumbers,
		Product_Output_Type,
		Price_Output_Type,
		Inventory_Output_Type,
		Discount_Output_Type,
        Quantity_Discount_Output_Type,
        Discount_With_Affiliation_Output_Type,
        Quantity_Discount_With_Affiliation_Output_Type
    }

	public enum NOTIFICATION
	{
		Email_Destination,
		Email_CC,
		Email_SSL_Enable,
		Email_Password,
		Email_Port,
		Email_SMTP,
		Email_Source,
		Email_Subject,
		Email_Username
	}

	public enum ADDRESS
	{
		Local_Path,
		Local_Path_Deleted,
		Remote_Path,
		Remote_Path_Deleted
	}

	public enum PRICE
	{
		Remote_Path,
		local_Output_Path,
		Filename_Prefix,
		Pricebook_Id,
		CSV_Map_Path
	}

	public enum SALESORDER
	{
		AX_Default_Delivery_Mode,
		Disable_Shippment_Process,
		Create_Customer_With_SalesOrder,
		Get_SalesTransaction_Id_From_Ecom,
		OrderPrefix,
		Multiplefile_Input_Path,
		Singlefile_Input_Path,
		Status_Remote_Path,
		Header_Discount_Reason_Code,
		Line_Discont_Reason_Code,
		Tax_Exempt_Number,
		AX_Default_UnitofMeasure,
		Update_Status_inDays,
		Multiple_To_Single_File,
		OptionItem_Constant,
		DeliveryItem_Constant,
		OptionItem_None_Constant,
		Delivery_Date_Null_Constant,
		Status_File_Name,
		Status_local_Path,
		Remote_Input_Path,
		Subtract_Discount_NetAmount_Enable,
		Order_Tax_As_Charges,
		SalesLine_Tax_As_Charges_Code,
		SalesLine_Tax_As_Charges_Description,
		Retail_Server_Paging,
		Include_ERP_Order_Number_in_Status,
		Include_Tracking_Info_in_Status,
		Order_Shipping_Tax_As_Charges_Code,
		Order_Shipping_Tax_As_Charges_Description,
		Rebate_Reason_Code,
		OOB_Coupon_Reason_Code,
		Affiliate_Reason_Code,
		Statuses_For_Sync,
		Statuses_For_Tracking,
		AX_Invoice_Address_Type,
		AX_Delivery_Address_Type,
		AX_Address_IsPrivate,
		AX_VW_AvalaraShippingTax_Category,
		AX_VW_VitaminWorldShipping_Category,
		AX_VW_AvalaraProductTax_Category,
		AX_VW_Shipping_Custom_Category,
        Use_Default_Customer,
        Is_Load_Sales_Order_From_DB,
        Sales_Order_Processing_Thread_Count,
        DefaultCurrencyCode,
        TMV_ManualDiscountReasonCode,
        IsCreateResellerCustomer,
        ValidateOrderPrice
    }

	public enum INVENTORY
	{
		Remote_Path,
		Filename_Prefix,
		Local_Output_Path,
		CSV_Map_Path,
		LocationId
	}

	public enum STORE
	{
		Local_Output_Path
	}

	public enum CHANNELCONFIGURATION
	{
		Remote_Path,
		Local_Output_Path,
		Filename_Prefix,
		ChannelConfiguration_Id
	}

	public enum OFFERTYPEGROUPS
	{
		Remote_Path,
		Local_Output_Path,
		Filename_Prefix,
		Offertypegroups_Id
	}

	public enum QUOTATIONREASONGROUP
	{
		Remote_Path,
		Local_Output_Path,
		Filename_Prefix,
		QuotationReason_Id
	}

	public enum EXTERNALWEBAPI
	{
		DQS_User_Name,
		DQS_Password,
		DQS_Workflow_Name,
		DQS_Endpoint
	}

	public enum CATEGORYASSIGNMENT
	{
		NONE,
		ALL,
		SINGLE
    }

    public enum PAYMENTCONNECTOR
    {
        Default_Locale,
        PayPal_Ecom_Value,
        Supported_Card_Types
    }

    public enum INGRAM
    {
        API_URL,
        API_Key,
        API_Data_Limit,
        Status_Update_Retry_Count
    }

}
