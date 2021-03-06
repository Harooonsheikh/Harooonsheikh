SET IDENTITY_INSERT [dbo].[EmailTemplate] ON 


GO
INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (2, N'Product', N'Product : {0} Failed on Commerce Link QA', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>




', N'Take Care.', 1)
GO
INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (3, N'Customer', N'Customer: {0} Failed on Commerce Link QA', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>




', N'Take Care.', 1)
GO
INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (4, N'Store', N'Store: {0} Failed on Commerce Link QA', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>




', N'Take Care.', 1)
GO
INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (5, N'SalesOrder', N'Sales Order : {0} Failed on Commerce Link QA', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>




', N'Take Care.', 1)
GO
INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (6, N'Inventory', N'Inventory : {0} Failed on Commerce Link QA', N'<h2>Hi {0}</h2>
<h4>{1}</h4>
<h3>Exception</h3>

<p>{2}</p>

<h3>{3}</h3>




', N'Take Care.', 1)
GO
INSERT [dbo].[EmailTemplate] ([Id], [Name], [Subject], [Body], [Footer], [IsActive]) VALUES (22, N'SimpleNotification', N'Service Stopped on Commerce Link QA {0}', N'Hi,  Service Stopped  {0}', N'Exception :  {1}', 1)
GO
SET IDENTITY_INSERT [dbo].[EmailTemplate] OFF
GO


GO
INSERT [dbo].[DimensionSet] ( [ErpValue], [ComValue], [IsActive]) VALUES ( N'color', N'color', 1)
GO
INSERT [dbo].[DimensionSet] ([ErpValue], [ComValue], [IsActive]) VALUES ( N'size', N'size', 1)
GO
INSERT [dbo].[DimensionSet] ([ErpValue], [ComValue], [IsActive]) VALUES ( N'style', N'style', 1)
GO
INSERT [dbo].[DimensionSet] ( [ErpValue], [ComValue], [IsActive]) VALUES ( N'configuration', N'configuration', 1)
GO


INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (1, N'InventorySyncJob', 120, 1, CAST(N'02:00:00' AS Time), 0, 1, NULL, 0, 0)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (2, N'ProductSyncJob', 30, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 0, 1)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (3, N'StoreSyncJob', 0, 1, CAST(N'02:45:00' AS Time), 0, 1, NULL, 0, 0)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (4, N'PriceSyncJob', 10, 1, CAST(N'01:00:00' AS Time), 0, 1, NULL, 0, 1)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (11, N'UploadInventorySynch', 3, 1, CAST(N'03:00:00' AS Time), 0, 1, NULL, 1, 0)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (12, N'UploadProductSync', 1, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 1, 1)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (13, N'SyncCustomerJob', 1, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 0, 0)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (14, N'UploadPriceSync', 1, 1, CAST(N'01:00:00' AS Time), 0, 1, NULL, 1, 1)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (15, N'CategorySyncJob', 240, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 0, 1)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (17, N'SalesOrderSyncJob', 3, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 0, 1)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (18, N'SyncCustomerInMagento', 20, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 0, 0)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (19, N'SyncDiscount', 10, 1, CAST(N'01:00:00' AS Time), 0, 1, NULL, 0, 1)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (20, N'SyncSalesOrderStatus', 10, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 0, 1)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (21, N'SyncCustomerDeletedAddressesJob', 60, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 0, 0)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (113, N'DownloadCustomerSync', 1, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 1, 0)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (117, N'DownloadSalesOrderSync', 1, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 1, 1)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (118, N'UploadCustomerSyncInMagento', 1, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 1, 0)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (119, N'UploadDiscountSync', 2, 1, CAST(N'01:00:00' AS Time), 0, 1, NULL, 1, 1)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (121, N'DownloadCustomerDeletedAddressesSync', 60, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 1, 0)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (122, N'UploadSalesOrderStatusSync', 10, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 1, 1)
GO
INSERT [dbo].[Jobs] ([JobID], [JobName], [JobInterval], [IsRepeatable], [StartTime], [IsActive], [JobStatus], [CronExpression], [JobType], [ShowInUI]) VALUES (123, N'ChannelPublishingSync', 5, 1, CAST(N'00:00:00' AS Time), 0, 1, NULL, 0, 1)
GO


GO
INSERT [dbo].[PaymentMethod] ( [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES (NULL, N'BASIC_CREDIT', N'2', 1, N'CreditCard', 1)
INSERT [dbo].[PaymentMethod] ( [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES ( 10, N'AE', N'3', 0, N'AMEX', 1)
INSERT [dbo].[PaymentMethod] ([ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES ( 10, N'VISA', N'12', 0, N'Visa', 1)
INSERT [dbo].[PaymentMethod] ( [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES ( 10, N'MC', N'14', 0, N'MasterCard', 1)

GO


INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (1, N'AL', N'Alabama ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (2, N'AK', N'Alaska ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (4, N'AZ', N'Arizona ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (5, N'AR', N'Arkansas ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (12, N'CA', N'California ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (13, N'CO', N'Colorado ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (14, N'CT', N'Connecticut ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (15, N'DE', N'Delaware ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (16, N'DC', N'District of Columbia ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (18, N'FL', N'Florida ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (19, N'GA', N'Georgia ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (21, N'HI', N'Hawaii ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (22, N'ID', N'Idaho ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (23, N'IL', N'Illinois ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (24, N'IN', N'Indiana ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (25, N'IA', N'Iowa ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (26, N'KS', N'Kansas ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (27, N'KY', N'Kentucky ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (28, N'LA', N'Louisiana ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (29, N'ME', N'Maine ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (31, N'MD', N'Maryland ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (32, N'MA', N'Massachusetts ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (33, N'MI', N'Michigan ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (34, N'MN', N'Minnesota ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (35, N'MS', N'Mississippi ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (36, N'MO', N'Missouri ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (37, N'MT', N'Montana ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (38, N'NE', N'Nebraska ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (39, N'NV', N'Nevada ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (40, N'NH', N'New Hampshire ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (41, N'NJ', N'New Jersey ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (42, N'NM', N'New Mexico ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (43, N'NY', N'New York ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (44, N'NC', N'North Carolina ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (45, N'ND', N'North Dakota ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (47, N'OH', N'Ohio ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (48, N'OK', N'Oklahoma ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (49, N'OR', N'Oregon ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (51, N'PA', N'Pennsylvania ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (53, N'RI', N'Rhode Island ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (54, N'SC', N'South Carolina ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (55, N'SD', N'South Dakota ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (56, N'TN', N'Tennessee ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (57, N'TX', N'Texas ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (58, N'UT', N'Utah ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (59, N'VT', N'Vermont ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (61, N'VA', N'Virginia ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (62, N'WA', N'Washington ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (63, N'WV', N'West Virginia ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (64, N'WI', N'Wisconsin ', N'USA')
GO
INSERT [dbo].[States] ([Id], [Code], [State], [Country]) VALUES (65, N'WY', N'Wyoming', N'USA')
GO


-- Application Settings

TRUNCATE TABLE [dbo].[ApplicationSetting]
GO
SET IDENTITY_INSERT [dbo].[ApplicationSetting] ON 

GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'INVENTORY.CSV_Map_Path', N'{APPLICATION.Local_Base_Path}\Templates\MagentoXMLMappingFiles\CREATE.ErpInventoryProducts.xml', N'CSV_Map_Path', N'abc', 0, 1, NULL, NULL, 1, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'STORE.Local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\Stores', N'Store Local Output Path', N'abc', 0, 1, NULL, NULL, 2, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.Remote_Base_Path', N'/', N'Remote Directory Base Path', N'SFTPConfiguration', 1, 1, NULL, NULL, 3, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.ERP_AX_OUN', N'077', N'AX Channel Org Unit Number', N'ERPAdapterGeneral', 10, 1, NULL, NULL, 4, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.ERP_AX_RetailServerUri', N'https://d365demovsi27cf5668d64f14abret.cloudax.dynamics.com/Commerce/', N'AX Retail Server URL', N'ERPAdapterGeneral', 0, 1, NULL, NULL, 5, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.ERP_Default_Customer_Group', N'Default', N'ERP Default Customer Group', N'ERPAdapterCustomer', 0, 1, NULL, NULL, 6, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.Retail_Media_Path', N'/images/2017/', N'Retail Media Directory', N'ERPAdapterProduct', 130, 1, NULL, NULL, 7, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.ERP_AX_InferPeriodicDiscount', N'TRUE', N'ERP AX Infer Periodic Discount', N'ERPAdapterSalesOrder', 50, 1, NULL, NULL, 8, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.ERP_Legal_Company', N'077', N'ERP Legal Company', N'ERPAdapterGeneral', 30, 1, NULL, NULL, 9, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRODUCT.SKU_Prefix', N'', N'SKU Prefix', N'ERPAdapterProduct', 80, 1, NULL, NULL, 10, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRODUCT.Retail_Server_Paging', N'1000', N'AX Retail Server Paging', N'ERPAdapterGeneral', 20, 1, NULL, NULL, 11, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRODUCT.Attr_Flat_Hierarchy_Related', N'Child Variant', N'Product Flat Hierarchy Relation Attribute', N'ERPAdapterProduct', 110, 1, NULL, NULL, 12, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRODUCT.Attr_IsMaster', N'Is Master', N'Product Is Master Attribute', N'ERPAdapterProduct', 120, 1, NULL, NULL, 13, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRODUCT.Flat_Hierarchy_Enable', N'FALSE', N'Product Flat Hierarchy Enable', N'ERPAdapterProduct', 100, 1, NULL, NULL, 14, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRODUCT.SKU_Postfix', N'M', N'SKU Postfix', N'ERPAdapterProduct', 90, 1, NULL, NULL, 15, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRODUCT.Local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\Product', N'Product Local Output Path', N'Product/Inventory', 10, 1, NULL, NULL, 16, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRODUCT.Remote_Path', N'{APPLICATION.Remote_Base_Path}/files/DEM_Files/catalog', N'Product Remote Path', N'Product/Inventory', 20, 1, NULL, NULL, 17, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRODUCT.Filename_Prefix', N'Catalog-', N'Product File Name Prefix', N'Product/Inventory', 30, 1, NULL, NULL, 18, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'DISCOUNT.Remote_Path', N'{APPLICATION.Remote_Base_Path}/files/DEM_Files/discounts/', N'Discount Remote Path', N'Price/Discount', 50, 1, NULL, NULL, 19, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'DISCOUNT.Filename_Prefix', N'Discount-', N'Discount File Name Prefix', N'Price/Discount', 60, 1, NULL, NULL, 20, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'DISCOUNT.Local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\Discount', N'Discount Local Output Path', N'Price/Discount', 40, 1, NULL, NULL, 21, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'ECOM.Root_Category_Id', N'root', N'Root Category Id', N'ERPAdapterProduct', 70, 1, NULL, NULL, 22, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'ECOM.Category_Assignment', N'ALL', N'Category Assignment', N'ERPAdapterProduct', 60, 1, NULL, NULL, 23, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'ECOM.SalesPerson_Id', N'', N'Sales Person Id', N'ERPAdapterGeneral', 50, 1, NULL, NULL, 24, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'ECOM.Remote_SFTP_Host', N'119.63.133.156', N'SFTP Host', N'SFTPConfiguration', 1, 1, NULL, NULL, 25, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'ECOM.Remote_SFTP_UserName', N'ftp01', N'SFTP User Name', N'SFTPConfiguration', 2, 1, NULL, NULL, 26, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'ECOM.Remote_SFTP_Password', N'Just4Now#', N'SFTP Password', N'SFTPConfiguration', 3, 1, NULL, NULL, 27, 1)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'ECOM.Remote_SFTP_Extenstions', N'csv|xml', N'SFTP Extension', N'SFTPConfiguration', 4, 1, NULL, NULL, 28, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRICE.Remote_Path', N'{APPLICATION.Remote_Base_Path}/pricebook', N'Price Remote Path', N'Price/Discount', 20, 1, NULL, NULL, 29, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRICE.local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\Price', N'Price Local Output Path', N'Price/Discount', 10, 1, NULL, NULL, 30, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRICE.Filename_Prefix', N'PriceBook-', N'Price File Name Prefix', N'Price/Discount', 30, 1, NULL, NULL, 31, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Multiplefile_Input_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\SalesOrder\MultiSalesOrderFile', N'Sales Order Multiple File Input Path', N'SalesOrder', 10, 1, NULL, NULL, 32, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Singlefile_Input_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\SalesOrder', N'Sales Order Single File Input Path', N'SalesOrder', 20, 1, NULL, NULL, 33, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Status_Remote_Path', N'{APPLICATION.Remote_Base_Path}/orderStatusImport/vitaminworld_us', N'Sales Order Status Remote Path', N'SalesOrder', 50, 1, NULL, NULL, 34, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Header_Discount_Reason_Code', N'HEADER_DISCOUNT', N'Header Discount Reason Code', N'ERPAdapterSalesOrder', 60, 1, NULL, NULL, 35, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Line_Discont_Reason_Code', N'LINE_DISCOUNT', N'Line Discont Reason Code', N'ERPAdapterSalesOrder', 70, 1, NULL, NULL, 36, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Update_Status_inDays', N'7', N'Update Status of Previous Days', N'ERPAdapterSalesOrder', 0, 1, NULL, NULL, 37, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Status_File_Name', N'orderstatus-', N'Sales Order Status File Name', N'SalesOrder', 60, 1, NULL, NULL, 38, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Status_local_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\SalesOrderStatus', N'Sales Order Status Local Output Path', N'SalesOrder', 40, 1, NULL, NULL, 39, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Remote_Input_Path', N'{APPLICATION.Remote_Base_Path}/files/demo/VSI-DES-DemoLab/export/orders', N'Sales Order Remote Input Path', N'SalesOrder', 30, 1, NULL, NULL, 40, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Include_ERP_Order_Number_in_Status', N'TRUE', N'Include ERP Order Number in Status', N'ERPAdapterSalesOrder', 10, 1, NULL, NULL, 41, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Include_Tracking_Info_in_Status', N'TRUE', N'Include Tracking Info in Status', N'ERPAdapterSalesOrder', 20, 1, NULL, NULL, 42, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Order_Shipping_Tax_As_Charges_Code', N'C74', N'Order Shipping Tax As Charges Code', N'ERPAdapterSalesOrder', 80, 1, NULL, NULL, 43, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Rebate_Reason_Code', N'REBATE', N'Rebate Reason Code', N'ERPAdapterSalesOrder', 120, 1, NULL, NULL, 44, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.OOB_Coupon_Reason_Code', N'COUPON', N'OOB Coupon Reason Code', N'ERPAdapterSalesOrder', 121, 1, NULL, NULL, 45, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Affiliate_Reason_Code', N'SOURCECODE', N'SourceCode Reason Code', N'ERPAdapterSalesOrder', 122, 1, NULL, NULL, 46, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Statuses_For_Sync', N'Invoiced, Canceled', N'Order Status to Sync', N'ERPAdapterSalesOrder', 30, 1, NULL, NULL, 47, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Statuses_For_Tracking', N'Invoiced', N'Order Status for Tracking URL', N'ERPAdapterSalesOrder', 40, 1, NULL, NULL, 48, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Order_Tax_As_Charges', N'FALSE', N'Order Tax As Charges', N'ERPAdapterSalesOrder', 79, 1, NULL, NULL, 49, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.SalesLine_Tax_As_Charges_Code', N'C73', N'Sale Line Tax As Charges Code', N'ERPAdapterSalesOrder', 100, 1, NULL, NULL, 50, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.SalesLine_Tax_As_Charges_Description', N'SALESLINE_TAX', N'Sale Line Tax As Charges Description', N'ERPAdapterSalesOrder', 110, 1, NULL, NULL, 51, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Order_Shipping_Tax_As_Charges_Description', N'SHIPPING_TAX', N'Order Shipping Tax As Charges Description', N'ERPAdapterSalesOrder', 90, 1, NULL, NULL, 52, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.AX_Invoice_Address_Type', N'1', N'AX Invoice Address Type', N'ERPAdapterCustomer', 10, 1, NULL, NULL, 53, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.AX_Delivery_Address_Type', N'2', N'AX Delivery Address Type', N'ERPAdapterCustomer', 20, 1, NULL, NULL, 54, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.Multiple_To_Single_File', N'TRUE', N'Break Multiple SalesOrders File To Single File', N'SalesOrder', 25, 1, NULL, NULL, 55, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.AX_VW_AvalaraShippingTax_Category', N'7', N'VW Avalara ShippingTax Category', N'ERPAdapterSalesOrder', 91, 1, NULL, NULL, 56, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.AX_VW_AvalaraProductTax_Category', N'8', N'VW Avalara Sales Line Tax Category', N'ERPAdapterSalesOrder', 111, 1, NULL, NULL, 57, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.AX_VW_VitaminWorldShipping_Category', N'6', N'VW Shipping Category', N'ERPAdapterSalesOrder', 92, 1, NULL, NULL, 58, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.AX_VW_Shipping_Custom_Category', N'Standard,USPS Posta,Express,Expedited,Internatio', N'VW Shipping Custom Category', N'ERPAdapterSalesOrder', 93, 1, NULL, NULL, 59, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'INVENTORY.Remote_Path', N'{APPLICATION.Remote_Base_Path}/inventory', N'Inventory Remote Path', N'Product/Inventory', 50, 1, NULL, NULL, 60, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'INVENTORY.Filename_Prefix', N'ProductInventory-', N'Inventory File Name Prefix', N'Product/Inventory', 60, 1, NULL, NULL, 61, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'INVENTORY.Local_Output_Path', N'{APPLICATION.Local_Base_Path}\DataFiles\Inventory', N'Inventory Local Output Path', N'Product/Inventory', 40, 1, NULL, NULL, 62, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'INVENTORY.LocationId', N'DC-CENTRAL', N'Invent Location Id', N'ERPAdapterGeneral', 40, 1, NULL, NULL, 63, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.Enviornment', N'DEV', N'Enviornment', N'EmailSetting', 0, 1, NULL, NULL, 64, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'NOTIFICATION.Email_SMTP', N'smtp.gmail.com', N'Email SMTP', N'EmailSetting', 0, 1, NULL, NULL, 65, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'NOTIFICATION.Email_Source', N'emailaxconnector@gmail.com', N'Email Source', N'EmailSetting', 0, 1, NULL, NULL, 66, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'NOTIFICATION.Email_Port', N'587', N'Email Port', N'EmailSetting', 0, 1, NULL, NULL, 67, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'NOTIFICATION.Email_Username', N'emailaxconnector@gmail.com', N'Email User Name', N'EmailSetting', 0, 1, NULL, NULL, 68, 1)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'NOTIFICATION.Email_Password', N'axconnector@1234', N'Email Password', N'EmailSetting', 0, 1, NULL, NULL, 69, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'NOTIFICATION.Email_SSL_Enable', N'TRUE', N'Email SSL Enable', N'EmailSetting', 0, 1, NULL, NULL, 70, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.Windows_Service', N'EdgeAX CommerceLink Sync Service', N'Windows Service', N'Services', 1, 1, NULL, NULL, 71, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.File_Service', N'EdgeAX CommerceLink File Service', N'File Service', N'Services', 1, 1, NULL, NULL, 72, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.XML_Base_Path', N'{APPLICATION.Local_Base_Path}\Templates', N'Template XML Base Path', N'MapsLocation', 1, 1, NULL, NULL, 73, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.Reset_Time_InMinutes', N'60', N'Service Reset Time in Minutes', N'Services', 1, 1, NULL, NULL, 74, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.AX_Default_UnitofMeasure', N'EA', N'AX_Default_UnitofMeasure', N'ERPAdapterSalesOrder', 1, 1, NULL, NULL, 75, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.Local_Base_Path', N'\EdgeAXDemardWare101', N'Local Base Path', N'abcd', 1, 1, NULL, NULL, 76, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRODUCT.IncludeConfigurationDimension', N'FALSE', N'Include Configuration Dimention', N'abcd', 1, 1, NULL, NULL, 77, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.IsApplicationMode', N'TRUE', N'Application Mode', N'ApplicationMode', 1, 1, NULL, NULL, 78, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.Client_Secret', N'jvaLTmzodPjsC9Rvbh2Dq3vjGNQlkLDT8qBOpQR7JPA=', N'Client Secret', N'ClientSecret', 1, 1, NULL, NULL, 79, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.D365_Machine_Url', N'https://d365demovsi27cf5668d64f14abret.cloudax.dynamics.com/', N'D365 Machine Url', N'D365MachineUrl', 1, 1, NULL, NULL, 80, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.Azure_Active_Directory', N'https://sts.windows.net/75668f36-65d3-4d90-a990-4bb8ec16728f/', N'Azure Active Directory', N'AzureActiveDirectoryUri', 1, 1, NULL, NULL, 81, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.Client_Id', N'cea731e7-2713-4c5f-a065-0ac15e3633bf', N'Client Id', N'ClientId', 1, 1, NULL, NULL, 82, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'SALESORDER.OrderPrefix', N'TMV_', N'Order Prefix', N'SalesOrder', 1, 1, NULL, NULL, 83, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'CUSTOMER.DefaultCustomer', N'004109', N'DefaultCustomer', N'DefaultCustomer', 1, 1, NULL, NULL, 84, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'PRICE.CSV_Map_Path', N'{APPLICATION.Local_Base_Path}\Templates\MagentoXMLMappingFiles\CREATE.ErpPrice.xml', N'CSV_Map_Path', N'abc', 0, 1, NULL, NULL, 85, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'Discount.CSV_Map_Path', N'{APPLICATION.Local_Base_Path}\Templates\MagentoXMLMappingFiles\CREATE.ErpDiscount.xml', N'CSV_Map_Path', N'abc', 0, 1, NULL, NULL, 86, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'Product.CSV_Map_Path', N'{APPLICATION.Local_Base_Path}\Templates\MagentoXMLMappingFiles\CREATE.ErpCatalog.xml', N'CSV_Map_Path', N'abc', 0, 1, NULL, NULL, 87, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'ECOM.Product_Output_Type', N'XML', N'Product Output Type', N'EComAdapterGeneral', 0, 1, NULL, NULL, 88, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'ECOM.Inventory_Output_Type', N'XML', N'Inventory Output Type', N'EComAdapterGeneral', 0, 1, NULL, NULL, 89, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'ECOM.Discount_Output_Type', N'XML', N'Discount Output Type', N'EComAdapterGeneral', 0, 1, NULL, NULL, 90, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'ECOM.Price_Output_Type', N'XML', N'Price Output Type', N'EComAdapterGeneral', 0, 1, NULL, NULL, 91, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'APPLICATION.Allow_Security_Protocols', N'TRUE', N'Allow Security Protocols', N'AllowSecurityProtocols', 0, 1, NULL, NULL, 92, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'EXTERNALWEBAPI.DQS_User_Name', N'admin', N'DQS User Name', N'ERPAdapterExternalWebAPI', 0, 1, NULL, NULL, 93, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'EXTERNALWEBAPI.DQS_Password', N'admin', N'DQS Password', N'ERPAdapterExternalWebAPI', 0, 1, NULL, NULL, 94, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'EXTERNALWEBAPI.DQS_Workflow_Name', N'RBWF_SanctionListCheck_Web', N'DQS Workflow Name', N'ERPAdapterExternalWebAPI', 0, 1, NULL, NULL, 95, 0)
GO
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [Id], [IsPassword]) VALUES (N'EXTERNALWEBAPI.DQS_Endpoint', N'https://dataquality01.teamviewer.com/DQServer/DQServer.asmx', N'DQS Endpoint', N'ERPAdapterExternalWebAPI', 0, 1, NULL, NULL, 96, 0)
GO

SET IDENTITY_INSERT [dbo].[ApplicationSetting] OFF
GO

-- Add Configurable Objects

TRUNCATE TABLE [dbo].[ConfigurableObjects]
GO
SET IDENTITY_INSERT [dbo].[ConfigurableObjects] ON 

GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (29, N'Ground', N'UPS-03', 1, 1)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (30, N'2 Day', N'UPS-02', 1, 1)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (31, N'Next Day', N'UPS-01', 1, 1)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (32, N'Store Pickup', N'60', 1, 2)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (33, N'SalesTaxGroup', N'NY', 3, 1)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (34, N'DiscountCharges', N'E-PROMOCOD', 4, 1)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (35, N'ShippingCharges', N'FREIGHT', 4, 2)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (36, N'ItemTaxGroup', N'RP', 3, 2)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (37, N'Visa', N'3', 2, 2)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (38, N'American Express', N'7', 2, 2)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (39, N'MasterCard', N'10', 2, 2)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (40, N'Discover', N'11', 2, 2)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (41, N'MonogramCharges', N'E-MONOGRAM', 4, 3)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (42, N'SR 2DAY', N'UPS-02', 1, 4)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (43, N'giftcard', N'GIFTCARD', 5, 4)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (44, N'Email', N'MOD-02', 1, 3)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (45, N'99', N'99', 1, 1)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (46, N'003', N'99', 1, 1)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (47, N'001', N'11', 1, 1)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (48, N'005', N'60', 1, 1)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (50, N'002', N'99', 1, 1)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (52, N'nl_be', N'nl-BE', 6, NULL)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (54, N'fr_be', N'fr-BE', 6, NULL)
GO
INSERT [dbo].[ConfigurableObjects] ([Id], [ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (55, N'nl_nl', N'nl', 6, NULL)
GO
SET IDENTITY_INSERT [dbo].[ConfigurableObjects] OFF
GO

-- Default Payment Methods

TRUNCATE TABLE [dbo].[PaymentMethod]
SET IDENTITY_INSERT [dbo].[PaymentMethod] ON 

GO
INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES (10, NULL, N'BASIC_CREDIT', N'2', 1, N'CreditCard',0)
GO
INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES (11, 10, N'AE', N'12', 0, N'AMEX',0)
GO
INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES (12, 10, N'VISA', N'3', 0, N'Visa',1)
GO
INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES (13, 10, N'MC', N'14', 0, N'MasterCard',0)
GO
INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment]) VALUES (14, 10, N'DI', N'15', 0, N'Discover',0)
GO
SET IDENTITY_INSERT [dbo].[PaymentMethod] OFF
GO

-- Activate Jobs
UPDATE [dbo].[Jobs] SET IsActive = 1 WHERE [JobID] = 1 OR [JobID] = 2 OR [JobID] = 3 OR [JobID] = 4 OR [JobID] = 11 
									   OR [JobID] = 12 OR [JobID] = 14 OR [JobID] = 17 OR [JobID] = 19 OR [JobID] = 20 
									   OR [JobID] = 117 OR [JobID] = 119 OR [JobID] = 122