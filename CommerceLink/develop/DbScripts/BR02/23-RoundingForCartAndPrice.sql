SELECT StoreId
INTO #ControlTable 
FROM dbo.Store

DECLARE @StoreId INT

WHILE exists (SELECT * FROM #ControlTable)
BEGIN

    SELECT @StoreId = (SELECT TOP 1 StoreId
                       FROM #ControlTable
                       ORDER BY StoreId ASC)

-- Run for every store
--------------------------------
INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
VALUES ( N'APPLICATION.ERP_AX_SalesOrderPriceRounding', N'-1', N'Sales Order Price Rounding', N'ERP Settings', 1, 1, @StoreId, CAST(N'2018-03-09T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')

INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
VALUES ( N'APPLICATION.ERP_AX_DiscountPriceRounding', N'2', N'Discount Price Rounding', N'ERP Settings', 1, 1, @StoreId, CAST(N'2018-03-09T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')

INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
VALUES ( N'APPLICATION.ERP_AX_PriceRounding', N'-1', N'Price Rounding', N'ERP Settings', 1, 1, @StoreId, CAST(N'2018-03-09T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')

--------------------------------

    DELETE #ControlTable
    WHERE StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------

-- For Japan Store set values to 0 for APPLICATION.ERP_AX_SalesOrderPriceRounding, APPLICATION.ERP_AX_DiscountPriceRounding and APPLICATION.ERP_AX_PriceRounding
UPDATE [dbo].[ApplicationSetting] SET [Value] = 0 
WHERE [Key] in ('APPLICATION.ERP_AX_SalesOrderPriceRounding', 'APPLICATION.ERP_AX_DiscountPriceRounding', 'APPLICATION.ERP_AX_PriceRounding')
and [StoreId] = (SELECT StoreId FROM Store WHERE [Name] = 'Japan')