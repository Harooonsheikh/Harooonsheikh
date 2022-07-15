DECLARE @StoreId int = (SELECT StoreId FROM Store WHERE NAME = 'Ingram')

IF(@StoreId IS NULL)
BEGIN
print 'Ingram store not found'
raiserror('Ingram store not found', 20, -1) with log
END


---------------------------- DDL scripts ----------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ThirdPartyMessage')
BEGIN
CREATE TABLE [dbo].[ThirdPartyMessage](
[ThirdPartyMessageId] [bigint] IDENTITY(1,1) NOT NULL,
[EntityId] [int] NULL,
[ThirdPartyId] [nvarchar](50) NOT NULL,
[Content] [nvarchar](max) NOT NULL,
[ThirdPartyStatus] [nvarchar](50) NOT NULL,
[CreatedOn] [datetime] NOT NULL,
[CreatedBy] [nvarchar](50) NOT NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [nvarchar](50) NULL,
[TransactionStatus] [int] NOT NULL,
CONSTRAINT [PK_ThirdPartyMessage] PRIMARY KEY CLUSTERED
(
[ThirdPartyMessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS(
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE
TABLE_NAME = 'RequestResponse'
AND COLUMN_NAME = 'Url')
BEGIN

ALTER TABLE RequestResponse
ADD Url	nvarchar(250)
END

---------------------------- Application Settings ----------------------------
IF NOT EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'INGRAM.API_URL' AND StoreId = @StoreId)
BEGIN
INSERT INTO ApplicationSetting ([Key], [Value], [Name], ScreenName, SortOrder, isActive, storeId, FieldTypeId)
VALUES ('INGRAM.API_URL', 'https://api.connect.cloud.im/public/v1/requests', 'Ingram API endpoint', 'Sales Order', 180, 1, @StoreId, 1002);
END

IF NOT EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'INGRAM.API_Data_Limit' AND StoreId = @StoreId)
BEGIN
INSERT INTO ApplicationSetting ([Key], [Value], [Name], ScreenName, SortOrder, isActive, storeId, FieldTypeId)
VALUES ('INGRAM.API_Data_Limit', '1000', 'Ingram API Limit', 'SalesOrder', 190, 1, @StoreId, 1);
END

IF NOT EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'INGRAM.API_Key' AND StoreId = @StoreId)
BEGIN
INSERT INTO ApplicationSetting ([Key], [Value], [Name], ScreenName, SortOrder, isActive, storeId, FieldTypeId)
VALUES ('INGRAM.API_Key', 'ApiKey SU-531-251-782:2f4c0100323dd86c7560d4863e3059ab5016c15e', 'Ingram API Key', 'Sales Order', 200, 1, @StoreId, 1);
END

IF NOT EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'SALESORDER.Is_Load_Sales_Order_From_DB' AND StoreId = @StoreId)
BEGIN
INSERT INTO ApplicationSetting ([Key], [Value], [Name], ScreenName, SortOrder, isActive, storeId, FieldTypeId)
VALUES ('SALESORDER.Is_Load_Sales_Order_From_DB', 'TRUE', 'Is load sales order from database', 'Sales Order', 210, 1, @StoreId, 2);
END

IF NOT EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'SALESORDER.Sales_Order_Processing_Thread_Count' AND StoreId = @StoreId)
BEGIN
INSERT INTO ApplicationSetting ([Key], [Value], [Name], ScreenName, SortOrder, isActive, storeId, FieldTypeId)
VALUES ('SALESORDER.Sales_Order_Processing_Thread_Count', '25', 'Sales order processing thread count', 'Sales Order', 220, 1, @StoreId, 1);
END

IF NOT EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'SALESORDER.DefaultCurrencyCode' AND StoreId = @StoreId)
BEGIN
INSERT INTO ApplicationSetting ([Key] , Value, Name, ScreenName, SortOrder, IsActive , StoreId, CreatedOn, FieldTypeId)
VALUES ('SALESORDER.DefaultCurrencyCode', 'USD', 'Default Currency Code', 'Sales Order', 221, 1, @StoreId, GETDATE(), 1)
END

IF NOT EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'CUSTOMER.Is_Ecom_Id_String' AND StoreId = @StoreId)
BEGIN
INSERT INTO ApplicationSetting ([Key], [Value], [Name], ScreenName, SortOrder, isActive, storeId, FieldTypeId)
VALUES ('CUSTOMER.Is_Ecom_Id_String', 'TRUE', 'Is Ecom Id String', 'Sales Order', 222, 1, @StoreId, 2);
END

IF NOT EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'SALESORDER.IsCreateResellerCustomer' AND StoreId = @StoreId)
BEGIN
INSERT INTO ApplicationSetting ([Key], [Value], [Name], ScreenName, SortOrder, isActive, storeId, FieldTypeId)
VALUES ('SALESORDER.IsCreateResellerCustomer', 'TRUE', 'Is Ceate Reseller Customer', 'Sales Order', 221, 1, @StoreId, 2);
END

---------------------------- Data Direction ----------------------------
IF NOT EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'SALESORDER.IsCreateResellerCustomer')
BEGIN
INSERT INTO [dbo].[DataDirection](DataDirectionId, DataDirectionName, IsActive)
VALUES (3, 'CLRequestToThirdParty', 1)
END

IF NOT EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'SALESORDER.IsCreateResellerCustomer')
BEGIN
INSERT INTO [dbo].[DataDirection](DataDirectionId, DataDirectionName, IsActive)
VALUES (4, 'ThirdPartyResponseToCL', 1)
END

---------------------------- Scheduling ----------------------------
DECLARE @NextJobId int = (SELECT MAX(JobID) FROM Job) + 1;

IF NOT EXISTS (SELECT * FROM Job WHERE [JobName] = 'DownloadThirdPartySalesOrderSync')
BEGIN
INSERT INTO [dbo].[Job](JobID, [JobName],[Enabled],[JobTypeId],[CreatedOn],[CreatedBy])
VALUES (@NextJobId, 'DownloadThirdPartySalesOrderSync',1,0,GETDATE(),1)
END

DECLARE @JobId int = (SELECT JobID FROM Job WHERE JobName = 'DownloadThirdPartySalesOrderSync')

IF NOT EXISTS (SELECT * FROM JobSchedule WHERE JobId = @JobId AND StoreId = @StoreId)
BEGIN
INSERT INTO JobSchedule(JobId, JobInterval, IsRepeatable, StartTime, IsActive, JobStatus, StoreId, CreatedOn, CreatedBy)
VALUES (@JobId, 180, 1, '00:00:00.0000000', 1, 1, @StoreId, GETDATE(), 'Admin')
END

SET @JobId = (SELECT JobID FROM Job WHERE JobName = 'SalesOrderSyncJob')

IF NOT EXISTS (SELECT * FROM Job WHERE JobId = @JobId AND Enabled = 1)
BEGIN
UPDATE Job
SET
Enabled = 1
WHERE JobId = @JobId
END

IF NOT EXISTS (SELECT * FROM JobSchedule WHERE JobId = @JobId AND StoreId = @StoreId AND IsActive = 1)
BEGIN
UPDATE JobSchedule
SET
IsRepeatable = 1,
IsActive = 1,
JobStatus = 1
WHERE StoreId = @StoreId
AND JobId = @JobId
END

SET @JobId = (SELECT JobID FROM Job WHERE JobName = 'SyncSalesOrderStatus')

IF NOT EXISTS (SELECT * FROM Job WHERE JobId = @JobId AND Enabled = 1)
BEGIN
UPDATE Job
SET
Enabled = 1
WHERE JobId = @JobId
END

IF NOT EXISTS (SELECT * FROM JobSchedule WHERE JobId = @JobId AND StoreId = @StoreId AND IsActive = 1)
BEGIN
UPDATE JobSchedule
SET
IsRepeatable = 1,
IsActive = 1,
JobStatus = 1
WHERE StoreId = @StoreId
AND JobId = @JobId
END


-------------------------------- Products Mapping --------------------------------
IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637144664' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637144664', N'PRD-374-638-432-0001', N'TVB0001:000000089', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637144668' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637144668', N'PRD-374-638-432-0002', N'TVP0001:000000093', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637144666' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637144666', N'PRD-374-638-432-0003', N'TVC0001:000000091', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637144661' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637144661', N'PRD-374-638-432-0004', N'TVAD001:000000086', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637144663' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637144663', N'PRD-374-638-432-0005', N'TVAD003:000000088', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637144679' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637144679', N'PRD-374-638-432-0006', N'SSC0001:000000104', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637151326' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637151326', N'PRD-813-128-715-0001', N'TVPI001:000081', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637144660' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637144660', N'PRD-440-847-641-0001', N'ITBMA0001:000000085', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637144653' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637144653', N'PRD-440-847-641-0002', N'ITBMA0001:000000078', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637144658' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637144658', N'PRD-440-847-641-0003', N'ITBAM0001:000000083', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637144651' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637144651', N'PRD-440-847-641-0004', N'ITBAM0001:000000076', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637144659' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637144659', N'PRD-440-847-641-0005', N'ITBB0001:000000084', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM IntegrationKey WHERE [ErpKey] = '5637144652' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[IntegrationKey] ([EntityId], [ErpKey], [ComKey], [Description], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (2, N'5637144652', N'PRD-440-847-641-0006', N'ITBB0001:000000077', @StoreId, CAST(N'2019-01-30 15:43:42.1479723' AS DateTime2), N'1', NULL, NULL)
END

-------------------------------- Mapping Template --------------------------------
IF EXISTS (SELECT * FROM MappingTemplate WHERE StoreId = @StoreId AND SourceEntity = 'ErpSalesOrder' AND Name = 'READ.ErpSalesOrder')
BEGIN
UPDATE MappingTemplate
SET
[XML] = '<Targets>
	<!--ErpSalesOrder Properties-->
	<Target property="ErpSalesOrder~Id" source-path="//orders/id" />
	<Target property="ErpSalesOrder~OrderPlacedDate" source-path="//orders/created" />
	<Target property="ErpSalesOrder~ChannelReferenceId" source-path="//orders/id" />
	<Target property="ErpSalesOrder~CurrencyCode" source-path="//orders/currency_code" />
	<Target property="ErpSalesOrder~Status" constant-value="Created" />
	<Target property="ErpSalesOrder~ChannelReferenceId" source-path="//orders/id" />
	<Target property="ErpSalesOrder~TotalAmount" constant-value="0" />
	<Target property="ErpSalesOrder~TaxAmount" constant-value="0" />
	<Target property="ErpSalesOrder~NetAmountWithNoTax" constant-value="0" />
	<Target property="ErpSalesOrder~NetAmountWithTax" constant-value="0" />
	<!--Reading discount of Order-->
	<Target property="ErpSalesOrder~OrderDiscounts" source-path="//orders/order/totals/merchandize-total/price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true">
		<Properties>
			<Target property="ErpDiscountLine~EffectiveAmount" source-path="net-price" />
			<Target property="ErpDiscountLine~Tax" source-path="tax" />
			<Target property="ErpDiscountLine~DiscountCode" source-path="promotion-id" />
			<Target property="ErpDiscountLine~OfferId" source-path="campaign-id" />
			<Target property="ErpDiscountLine~CouponId" source-path="coupon-id" />
			<Target property="ErpDiscountLine~RebateCode" source-path="custom-attributes/custom-attribute[@attribute-id=''rebate-code'']" />
			<Target property="ErpDiscountLine~Amount" source-path="custom-attributes/custom-attribute[@attribute-id=''discount'']" />
			<Target property="ErpDiscountLine~SppNumber" source-path="custom-attributes/custom-attribute[@attribute-id=''spp-no'']" />
			<Target property="ErpDiscountLine~OfferName" source-path="custom-attributes/custom-attribute[@attribute-id=''reason-code'']" />
		</Properties>
	</Target>
	<!--ErpSalesOrder Products-->
	<Target property="ErpSalesOrder~SalesLines" source-path="//orders/asset/items" target-source="ErpSalesLine" repeat="true">
		<Properties>
			<Target property="ErpSalesLine~UnitOfMeasureSymbol" constant-value="" />
			<Target property="ErpSalesLine~SalesOrderUnitOfMeasure" constant-value="" />
			<Target property="ErpSalesLine~NetAmount" constant-value="0" />
			<Target property="ErpSalesLine~TaxAmount" constant-value="0" />
			<Target property="ErpSalesLine~TotalAmount" constant-value="0" />
			<Target property="ErpSalesLine~BasePrice" constant-value="0" />
			<Target property="ErpSalesLine~Price" constant-value="0" />
			<Target property="ErpSalesLine~LineNumber" source-path="position" />
			<Target property="ErpSalesLine~Description" source-path="global_id" />
			<Target property="ErpSalesLine~ItemId" source-path="global_id" />
			<Target property="ErpSalesLine~Quantity" source-path="quantity" />
			<Target property="ErpSalesLine~QuantityOrdered" source-path="quantity" />
			<Target property="ErpSalesLine~TaxRatePercent" constant-value="0" />
			<Target property="ErpSalesLine~IsGiftCardLine" constant-value="false" />
			<Target property="ErpSalesLine~ShipmentId" constant-value="0" />
			<Target property="ErpSalesLine~InventoryLocationId" source-path="custom-attributes/custom-attribute[@attribute-id=''fromStoreId'']" />
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''SKU'']" is-custom-attribute="true" attribute-id="SKU" />
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''bogoPromotionId'']" is-custom-attribute="true" attribute-id="bogoPromotionId" />
			<Target property="ErpSalesLine~SalesTaxGroupId" source-path="custom-attributes/custom-attribute[@attribute-id=''taxJurisdictionID'']" />
			<!--<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''SKU'']" is-custom-attribute="true" attribute-id="SKU" />-->
			<!--TVW FDD-007-->
			<!--Set in the "Synchronize orders" batch flow based Shipping Date Requested field on the SalesLin-->
			<Target property="ErpSalesLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDFROM'']" is-custom-attribute="true" attribute-id="TMVCONTRACTVALIDFROM" />
			<!--Set in the "Synchronize orders" batch flow and should be equal to TMVContractValidFrom in the happy flow.-->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTCALCULATEFROM'']" default-value="" is-custom-attribute="true" attribute-id="TMVCONTRACTCALCULATEFROM" />
			<!--Set in the "Synchronize orders" batch flow and should be equal to TMVContractValidFrom + the length of the offer type from the deimension value - could also be empty if perpetual i.e. same logic implemented in Basic Contract FDD should be automatically triggerred. -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDTO'']" default-value="" is-custom-attribute="true" attribute-id="TMVCONTRACTVALIDTO" />
			<!--No logic implemented at time of writing - leave default empty value-->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTPOSSTERMDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTPOSSTERMDATE" />
			<!--No logic implemented at time of writing - leave default empty value -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTCANCELDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTCANCELDATE" />
			<!--No logic implemented at time of writing - leave default empty value -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTPOSSCANCELDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTPOSSCANCELDATE" />
			<!--No logic implemented at time of writing - leave default empty value -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTTERMDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTTERMDATE" />
			<!--No logic implemented at time of writing - leave default empty value -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTTERMDATEEFFECTIVE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTTERMDATEEFFECTIVE" />
			<!--Ensure that is set to false (i.e. the default value). This is only used in more advanced scenarios and not the happy flow. -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVAUTOPROLONGATION'']" default-value="0" is-custom-attribute="true" attribute-id="TMVAUTOPROLONGATION" />
			<!--No logic implemented at time of writing - leave default empty value -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVPURCHORDERFORMNUM'']" is-custom-attribute="true" attribute-id="TMVPURCHORDERFORMNUM" />
			<!--No logic implemented at time of writing - leave default empty value -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCUSTOMERREF'']" is-custom-attribute="true" attribute-id="TMVCUSTOMERREF" />
			<!--Set in the "Synchronize orders" batch flow to value of "created". -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTSTATUSLINE'']" default-value="10" is-custom-attribute="true" attribute-id="TMVCONTRACTSTATUSLINE" />
			<!--This field is required in order to post the confirmation but no logic implemented at time of writing - fill in with dummy value. -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVEULAVERSION'']" default-value="v1" is-custom-attribute="true" attribute-id="TMVEULAVERSION" />
			<!--Set in the "Synchronize orders" batch flow to value of "year". Note there is no logic tied to this field at the moment. -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVBILLINGPERIOD'']" default-value="1" is-custom-attribute="true" attribute-id="TMVBILLINGPERIOD" />
			<!--Random GUID passed from Magento/Postman but is inserted in a related table to the SalesLine hence the field is an Int64 RefRecId field. -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''PACLICENSE'']" default-value="" is-custom-attribute="true" attribute-id="PACLICENSE" />
			<!--Set in the "Synchronize orders" batch flow. Copy value from SalesLine.LineAmount-->
			<Target property="ErpSalesLine~CustomAttributes" source-path="gross-price" is-custom-attribute="true" attribute-id="TMVORIGINALLINEAMOUNT" />
			<!--Set in the "Synchronize orders" batch flow to value of "yes"-->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVLINEMODIFIED'']" default-value="1" is-custom-attribute="true" attribute-id="TMVLINEMODIFIED" />
			<!--Set in the "Synchronize orders" batch flow to value of "none". -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVREVERSEDLINE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVREVERSEDLINE" />
			<!--Set in the "Synchronize orders" batch flow to value of "". -->
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVMIGRATEDSALESLINENUMBER'']" default-value="0" is-custom-attribute="true" attribute-id="TMVMIGRATEDSALESLINENUMBER" />
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''AffiliationRecId'']" default-value="0" is-custom-attribute="true" attribute-id="TMVAFFILIATIONRECID" />
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVOLDSALESLINENUMBER'']" default-value="0" is-custom-attribute="true" attribute-id="TMVOLDSALESLINENUMBER" />
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVOLDSALESLINEACTION'']" default-value="" is-custom-attribute="true" attribute-id="TMVOLDSALESLINEACTION" />
			<Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVSOURCEID'']" default-value="" is-custom-attribute="true" attribute-id="TMVSOURCEID" />
			<!--Reading discount of product-->
			<Target property="ErpSalesLine~DiscountLines" source-path="price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true">
				<Properties>
					<Target property="ErpDiscountLine~Amount" source-path="net-price" />
					<Target property="ErpDiscountLine~Tax" source-path="tax" />
					<Target property="ErpDiscountLine~DiscountCode" source-path="promotion-id" />
					<Target property="ErpDiscountLine~OfferId" source-path="campaign-id" />
					<Target property="ErpDiscountLine~CouponId" source-path="coupon-id" />
					<Target property="ErpDiscountLine~OfferName" source-path="OfferName" />
					<Target property="ErpDiscountLine~EffectiveAmount" source-path="EffectiveAmount" />
					<Target property="ErpDiscountLine~Percentage" source-path="Percentage" />
					<Target property="ErpDiscountLine~DiscountLineTypeValue" source-path="DiscountLineTypeValue" />
					<Target property="ErpDiscountLine~ManualDiscountTypeValue" source-path="ManualDiscountTypeValue" />
					<Target property="ErpDiscountLine~CustomerDiscountTypeValue" source-path="CustomerDiscountTypeValue" />
					<Target property="ErpDiscountLine~PeriodicDiscountTypeValue" source-path="PeriodicDiscountTypeValue" />
				</Properties>
			</Target>
		</Properties>
	</Target>
	<Target property="ErpSalesOrder~Customer" source-path="//orders/asset/tiers/customer" target-source="ErpCustomer" repeat="false">
		<Properties>
			<Target property="ErpCustomer~EcomCustomerId" source-path="id" />
			<Target property="ErpCustomer~Name" source-path="name" />
			<Target property="ErpCustomer~Email" source-path="contact_info/contact/email" />
			<Target property="ErpCustomer~FirstName" source-path="contact_info/contact/first_name" />
			<Target property="ErpCustomer~LastName" source-path="contact_info/contact/last_name" />
			<Target property="ErpCustomer~Street" source-path="contact_info/address_line1" />
			<Target property="ErpCustomer~City" source-path="contact_info/city" />
			<Target property="ErpCustomer~ZipCode" source-path="postal_code" />
			<Target property="ErpCustomer~State" source-path="state" />
			<Target property="ErpCustomer~TwoLetterISORegionName" source-path="country" />
			<Target property="ErpCustomer~ThreeLetterISORegionName" source-path="country" />
			<Target property="ErpCustomer~Phone" source-path="contact_info/contact/phone_number/country_code" />
			<Target property="ErpCustomer~Phone" source-path="contact_info/contact/phone_number/area_code" concatenate="true" />
			<Target property="ErpCustomer~Phone" source-path="contact_info/contact/phone_number/phone_number" concatenate="true" />
			<Target property="ErpCustomer~Address" source-path="//orders/asset/tiers/customer/contact_info" target-source="ErpAddress" repeat="false">
				<Properties>
					<Target property="ErpAddress~Name" source-path="contact/first_name" />
					<Target property="ErpAddress~Name" source-path="contact/last_name" concatenate="true" />
					<Target property="ErpAddress~Street" source-path="address_line1" />
					<Target property="ErpAddress~City" source-path="city" />
					<Target property="ErpAddress~ZipCode" source-path="postal_code" />
					<Target property="ErpAddress~State" source-path="state" />
					<Target property="ErpAddress~StateName" source-path="state" />
					<Target property="ErpAddress~TwoLetterISORegionName" source-path="country" />
					<Target property="ErpAddress~ThreeLetterISORegionName" source-path="country" />
					<Target property="ErpAddress~Phone" source-path="contact/phone_number/country_code" />
					<Target property="ErpAddress~Phone" source-path="contact/phone_number/area_code" concatenate="true" />
					<Target property="ErpAddress~Phone" source-path="contact/phone_number/phone_number" concatenate="true" />
				</Properties>
			</Target>
		</Properties>
	</Target>
	<Target property="ErpSalesOrder~Reseller" source-path="//orders/asset/tiers/tier1" target-source="ErpCustomer" repeat="false">
		<Properties>
			<Target property="ErpCustomer~EcomCustomerId" source-path="id" />
			<Target property="ErpCustomer~Name" source-path="name" />
			<Target property="ErpCustomer~Email" source-path="contact_info/contact/email" />
			<Target property="ErpCustomer~FirstName" source-path="contact_info/contact/first_name" />
			<Target property="ErpCustomer~LastName" source-path="contact_info/contact/last_name" />
			<Target property="ErpCustomer~Street" source-path="contact_info/address_line1" />
			<Target property="ErpCustomer~City" source-path="contact_info/city" />
			<Target property="ErpCustomer~ZipCode" source-path="postal_code" />
			<Target property="ErpCustomer~State" source-path="state" />
			<Target property="ErpCustomer~TwoLetterISORegionName" source-path="country" />
			<Target property="ErpCustomer~ThreeLetterISORegionName" source-path="country" />
			<Target property="ErpCustomer~Phone" source-path="contact_info/contact/phone_number/country_code" />
			<Target property="ErpCustomer~Phone" source-path="contact_info/contact/phone_number/area_code" concatenate="true" />
			<Target property="ErpCustomer~Phone" source-path="contact_info/contact/phone_number/phone_number" concatenate="true" />
			<Target property="ErpCustomer~Address" source-path="//orders/asset/tiers/tier1/contact_info" target-source="ErpAddress" repeat="false">
				<Properties>
					<Target property="ErpAddress~Name" source-path="contact/first_name" />
					<Target property="ErpAddress~Name" source-path="contact/last_name" concatenate="true" />
					<Target property="ErpAddress~Street" source-path="address_line1" />
					<Target property="ErpAddress~City" source-path="city" />
					<Target property="ErpAddress~ZipCode" source-path="postal_code" />
					<Target property="ErpAddress~State" source-path="state" />
					<Target property="ErpAddress~StateName" source-path="state" />
					<Target property="ErpAddress~TwoLetterISORegionName" source-path="country" />
					<Target property="ErpAddress~ThreeLetterISORegionName" source-path="country" />
					<Target property="ErpAddress~Phone" source-path="contact/phone_number/country_code" />
					<Target property="ErpAddress~Phone" source-path="contact/phone_number/area_code" concatenate="true" />
					<Target property="ErpAddress~Phone" source-path="contact/phone_number/phone_number" concatenate="true" />
				</Properties>
			</Target>
		</Properties>
	</Target>
	<!--ErpSalesOrder Payments-->
	<Target property="ErpSalesOrder~TenderLines" source-path="//orders" target-source="ErpTenderLine" repeat="true">
		<Properties>
			<!--Common for all Payment methods-->
			<Target property="ErpTenderLine~TenderTypeId" constant-value="PURCHASEORDER" />
		</Properties>
	</Target>
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Fraud_Status'']" is-custom-attribute="true" attribute-id="Fraud_Status" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Realtime_auth_and_fraud_check_done'']" is-custom-attribute="true" attribute-id="Realtime_auth_and_fraud_check_done" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authStatus'']" is-custom-attribute="true" attribute-id="authStatus" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authorizationCode'']" is-custom-attribute="true" attribute-id="authorizationCode" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paymentProfileID'']" is-custom-attribute="true" attribute-id="paymentProfileID" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''responseDateTime'']" is-custom-attribute="true" attribute-id="responseDateTime" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceIndex'']" is-custom-attribute="true" attribute-id="transactionReferenceIndex" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceNumber'']" is-custom-attribute="true" attribute-id="transactionReferenceNumber" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''webOrderID'']" is-custom-attribute="true" attribute-id="webOrderID" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paypalPaymentMethod'']" is-custom-attribute="true" attribute-id="paypalPaymentMethod" />
	<!--TVW FDD-007-->
	<Target property="ErpSalesOrder~RequestedDeliveryDate" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDFROM'']" />
	<!--Should remain empty: only applicable for use in call-center orders in BR1 -->
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVRESELLERACCOUNT'']" is-custom-attribute="true" attribute-id="TMVRESELLERACCOUNT" />
	<!--Should remain empty: only applicable for use in call-center orders in BR1 -->
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVDISTRIBUTORACCOUNT'']" is-custom-attribute="true" attribute-id="TMVDISTRIBUTORACCOUNT" />
	<!--Should remain empty: only applicable for use in call-center orders in BR1 -->
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVINDIRECTCUSTOMER'']" is-custom-attribute="true" attribute-id="TMVINDIRECTCUSTOMER" />
	<!--Set in the "Synchronize orders" batch flow based on the TMVMainOfferType of the Offer Type Group dimension line. -->
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVMAINOFFERTYPE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVMAINOFFERTYPE" />
	<!--Set in the "Synchronize orders" batch flow - more information to be received about this field. Contract confirmation fails if this field has no value. -->
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPRODUCTFAMILY'']" is-custom-attribute="true" attribute-id="TMVPRODUCTFAMILY" />
	<!--This should be mapped to value ‚1‘ i.e. contract in the commerce link mapper. Orders created from the RetailSynchOrdersSchedulerTask class should use the value coming from the mapper and ignore the default value set on Accounts Receivable. This is for future-compatabilty reasons. -->
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSALESORDERSUBTYPE'']" default-value="1" is-custom-attribute="true" attribute-id="TMVSALESORDERSUBTYPE" />
	<!--Ensure that is set to false (i.e. the default value). This is only used in Advanced Contract Management -->
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVINVOICESCHEDULECOMPLETE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVINVOICESCHEDULECOMPLETE" />
	<!--Set in the "Synchronize orders" batch flow based on the lowest value of SalesLine.TMVContractStatusLine within all contract lines. -->
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTSTATUSLINE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVCONTRACTSTATUSLINE" />
	<!--This should be passed from Magento when the order is created as part of a marketing campaing (such as email sent to customer from Marketo). Part of work package WP-S07.-->
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSMMCAMPAIGNID'']" default-value="" is-custom-attribute="true" attribute-id="TMVSMMCAMPAIGNID" />
	<!--This should be passed from Magento . Part of work package WP-S07.-->
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPURCHORDERFORMNUM'']" default-value="" is-custom-attribute="true" attribute-id="TMVPURCHORDERFORMNUM" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPIT'']" default-value="" is-custom-attribute="true" attribute-id="TMVPIT" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVQUOTATIONID'']" default-value="" is-custom-attribute="true" attribute-id="TMVQUOTATIONID" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVMIGRATEDORDERNUMBER'']" default-value="" is-custom-attribute="true" attribute-id="TMVMIGRATEDORDERNUMBER" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVOLDSALESORDERNUMBER'']" default-value="" is-custom-attribute="true" attribute-id="TMVOLDSALESORDERNUMBER" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVOriginatingCountry'']" default-value="" is-custom-attribute="true" attribute-id="TMVOriginatingCountry" />
	<Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCOMMENTFORORDER'']" default-value="" is-custom-attribute="true" attribute-id="TMVCOMMENTFORORDER" />
    <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCOMMENTFOREMAIL'']" default-value="" is-custom-attribute="true" attribute-id="TMVCOMMENTFOREMAIL" />
    <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSALESORIGIN'']" default-value="" is-custom-attribute="true" attribute-id="TMVSALESORIGIN" />
    <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPARTNERID'']" default-value="" is-custom-attribute="true" attribute-id="TMVPARTNERID" />
    <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPaymentTerms'']" default-value="" is-custom-attribute="true" attribute-id="TMVPaymentTerms" />
    <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVFraudReviewStatus'']" default-value="" is-custom-attribute="true" attribute-id="TMVFraudReviewStatus" />
</Targets>'
WHERE StoreId = @StoreId AND SourceEntity = 'ErpSalesOrder' AND Name = 'READ.ErpSalesOrder'
END

GO

