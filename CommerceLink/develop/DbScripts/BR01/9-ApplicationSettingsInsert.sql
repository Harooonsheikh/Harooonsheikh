IF EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'PRODUCT.OfferTypeGroupName')   
Begin 
	Update [dbo].[ApplicationSetting]
		set [value] = 'Offer Type'
	where [key] = 'PRODUCT.OfferTypeGroupName'
End
BEGIN
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) 
		VALUES (N'PRODUCT.OfferTypeGroupName', N'Offer Type', N'Offer Type Group Name', N'ERPAdapterProduct', 1, 1, NULL, NULL, 0);
END
GO

IF EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'PRODUCT.CrossSellType')   
Begin 
	Update [dbo].[ApplicationSetting]
		set [value] = '41'
	where [key] = 'PRODUCT.CrossSellType'
End
BEGIN
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) 
		VALUES (N'PRODUCT.CrossSellType', N'41', N'Cross Sell Type Value', N'ERPAdapterProduct', 1, 1, NULL, NULL, 0);
END
GO

IF EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'PRODUCT.MasterProductTypeEcomName')   
Begin 
	Update [dbo].[ApplicationSetting]
		set [value] = 'configurable'
	where [key] = 'PRODUCT.MasterProductTypeEcomName'
End
BEGIN
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) 
		VALUES (N'PRODUCT.MasterProductTypeEcomName', N'configurable', N'Master Product Type Ecom Name', N'Product', 51, 1, NULL, NULL, 0);
END
GO

IF EXISTS(SELECT * FROM [dbo].[ApplicationSetting] WHERE [key] = 'PRODUCT.VariantProductTypeEcomName')   
Begin 
	Update [dbo].[ApplicationSetting]
		set [value] = 'virtual'
	where [key] = 'PRODUCT.VariantProductTypeEcomName'
End
BEGIN
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [CreatedOn], [UpdatedOn], [IsPassword]) 
		VALUES (N'PRODUCT.VariantProductTypeEcomName', N'virtual', N'Variant Product Type Ecom Name', N'Product', 52, 1, NULL, NULL, 0);
END
GO
