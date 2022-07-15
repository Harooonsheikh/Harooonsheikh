--First update Sales Order mapping template
GO
IF NOT EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'CUSTOMER.Default_ThreeLetterISORegionName')			
BEGIN
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'CUSTOMER.Default_ThreeLetterISORegionName', N'NLD', N'Default Three Letter ISO Region Name', N'ERPAdapterCustomer', 1, 1, NULL, NULL, 0)
END
GO

GO
IF NOT EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'SALESORDER.Get_SalesTransaction_Id_From_Ecom')			
BEGIN
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Get_SalesTransaction_Id_From_Ecom', N'TRUE', N'Get SalesTransaction Id From Ecom', N'ERPAdapterSalesOrder', 1, 1, NULL, NULL, 0)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'SALESORDER.Create_Customer_With_SalesOrder')			
BEGIN
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Create_Customer_With_SalesOrder', N'FALSE', N'Create Customer with SalesOrder', N'ERPAdapterSalesOrder', 1, 1, NULL, NULL, 0)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'SALESORDER.Disable_Shippment_Process')			
BEGIN
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.Disable_Shippment_Process', N'TRUE', N'Disable Shippment Process', N'ERPAdapterSalesOrder', 1, 1, NULL, NULL, 0)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'SALESORDER.AX_Default_Delivery_Mode')			
BEGIN
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.AX_Default_Delivery_Mode', N'10', N'AX Default Delivery Mode', N'ERPAdapterSalesOrder', 1, 1, NULL, NULL, 0)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'SALESORDER.AX_Default_UnitofMeasure')			
BEGIN
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'SALESORDER.AX_Default_UnitofMeasure', N'EA', N'AX_Default_UnitofMeasure', N'ERPAdapterSalesOrder', 1, 1, NULL, NULL, 0)
END
GO
Update [dbo].[ApplicationSetting]
set [value] = 'pcs'
where [key] = 'SALESORDER.AX_Default_UnitofMeasure'
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'APPLICATION.ERP_AX_InferPeriodicDiscount')			
BEGIN
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'APPLICATION.ERP_AX_InferPeriodicDiscount', N'TRUE', N'ERP AX Infer Periodic Discount', N'ERPAdapterSalesOrder', 50, 1, NULL, NULL, 0)
END
GO
Update [dbo].[ApplicationSetting]
set [value] = 'FALSE'
where [key] = 'APPLICATION.ERP_AX_InferPeriodicDiscount'
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'INVENTORY.LocationId')			
BEGIN
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) VALUES (N'INVENTORY.LocationId', N'DC-CENTRAL', N'Invent Location Id', N'ERPAdapterGeneral', 40, 1, NULL, NULL, 0)
END
GO
Update [dbo].[ApplicationSetting]
set [value] = 'TVDE'
where [key] = 'INVENTORY.LocationId'
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ConfigurableObjects] WHERE [ComValue] = 'SalesTaxGroup' and [EntityType] = 3 and [ConnectorKey] = 1)			
BEGIN
	INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES (N'SalesTaxGroup', N'NY', 3, 1)
END
GO
Update [dbo].[ConfigurableObjects]
set [ErpValue] = ''
where [ComValue] = 'SalesTaxGroup' and [EntityType] = 3 and [ConnectorKey] = 1
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ConfigurableObjects] WHERE [ComValue] = 'ItemTaxGroup' and [EntityType] = 3 and [ConnectorKey] = 2)			
BEGIN
	INSERT [dbo].[ConfigurableObjects] ([ComValue], [ErpValue], [EntityType], [ConnectorKey]) VALUES ( N'ItemTaxGroup', N'RP', 3, 2)
END
GO
Update [dbo].[ConfigurableObjects]
set [ErpValue] = ''
where [ComValue] = 'ItemTaxGroup' and [EntityType] = 3 and [ConnectorKey] = 2
GO

