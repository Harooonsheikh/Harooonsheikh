--Just run for 1 time
INSERT [dbo].[Job] ([JobID], [JobName], [Enabled], [JobTypeId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (5, N'QuantityDiscountSync', 1, 0, CAST(N'2018-03-12T11:46:47.890' AS DateTime), N'1', CAST(N'2018-03-12T11:46:47.890' AS DateTime), N'1')
GO

INSERT [dbo].[Job] ([JobID], [JobName], [Enabled], [JobTypeId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (115, N'UploadQuantityDiscountSync', 1, 1, CAST(N'2018-03-12T11:46:47.890' AS DateTime), N'1', CAST(N'2018-03-12T11:46:47.890' AS DateTime), N'1')
GO

update MappingTemplate set [SourceEntity] = 'ErpItemQuantityDiscount', [Name] = 'CREATE.ErpItemQuantityDiscount', [XML] =   
N'<Items>
  <item repeat="true" data-source="ErpItemQuantityDiscount~ProductItemQuantityDiscounts">
    <sku>ProductItemQuantityDiscounts~SKU</sku>
    <tier_price_website>base</tier_price_website>
    <tier_price_customer_group>ALL GROUPS</tier_price_customer_group>
    <tier_price_qty>ProductItemQuantityDiscounts~TierPriceQuantity</tier_price_qty>
    <tier_price>ProductItemQuantityDiscounts~TierPrice</tier_price>
    <tier_price_value_type expression="true">sourceObject.TierPriceValueType == ErpMultiBuyDiscountType.UnitPrice ? "Fixed" : "Discount"</tier_price_value_type>
  </item>
</Items>'
where [SourceEntity] = 'ErpQuantityDiscount'

-------------------------------------------------------------------------------------------------------------------
-- Repeat for every store
DECLARE @StoreId INT = 1;

-- Run for every store
INSERT [dbo].[JobSchedule] ([JobId], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (5, 60, 1, CAST(N'13:10:05.1473707' AS Time), 1, 1, @StoreId, CAST(N'2018-09-17T15:53:36.0852004' AS DateTime2), N'System', CAST(N'2018-09-18T08:04:19.0370627' AS DateTime2), NULL)
INSERT [dbo].[JobSchedule] ([JobId], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (115, 60, 1, CAST(N'13:10:05.1473707' AS Time), 1, 1, @StoreId, CAST(N'2018-09-17T15:53:36.0852004' AS DateTime2), N'System', CAST(N'2018-09-18T08:04:19.0370627' AS DateTime2), NULL)

-- Run for every store
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
VALUES ( N'QUANTITYDISCOUNT.Local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\QuantityDiscount', N'Quantity Discount Local Output Path', N'Quantity Discount', 1, 1, @StoreId, CAST(N'2018-03-09T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')

INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
VALUES ( N'QUANTITYDISCOUNT.Remote_Path', N'{APPLICATION.Remote_Base_Path}/QuantityDiscount', N'Quantity Discount FTP Path', N'Quantity Discount', 2, 1, @StoreId, CAST(N'2018-03-09T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')

INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
VALUES ( N'QUANTITYDISCOUNT.Filename_Prefix',  N'QuantityDiscount-', N'Quantity Discount File Tag', N'Quantity Discount', 3, 1, @StoreId, CAST(N'2018-03-09T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')

INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
VALUES ( N'QUANTITYDISCOUNT.Data_Area_Id',  N'TVDE', N'Quantity Discount Data Area Id', N'Quantity Discount', 0, 1, @StoreId, CAST(N'2018-03-09T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')

INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
VALUES ( N'ECOM.Quantity_Discount_Output_Type',  N'XML', N'Quantity Discount Output Type', N'ECOM Settings', 0, 1, @StoreId, CAST(N'2018-03-09T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')

-- Run for every store
INSERT [dbo].[EmailTemplate] ([EmailTemplateId],[Name], [Subject], [Body], [Footer], [IsActive], [StoreId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (27,N'QuantityDiscount', N'Quantity Discount : {0} Failed on Commerce Link Dev09', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>




', N'Take Care.', 1, @StoreId, CAST(N'2018-09-17T15:53:36.2101576' AS DateTime2), N'System', NULL, NULL)
GO