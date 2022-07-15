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
VALUES ( N'PRODUCT.Single_Consolidated_Catalog', N'', N'Single Consolidated Catalog', N'Product', 1, 1, @StoreId, GETUTCDATE(), NULL, 1, N'1', N'1')

--------------------------------

    DELETE #ControlTable
    WHERE StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------

-- Set Consolidated Catalog to TRUE for Netherland (discuss which store needs to be configured this way)
UPDATE [dbo].[ApplicationSetting] SET [Value] = 'TRUE' 
WHERE [Key] = 'PRODUCT.Single_Consolidated_Catalog'
and [StoreId] in (SELECT StoreId FROM Store WHERE [Name] in ('Netherland'))
-------------------------------------------------------------
-- Set Consolidated Catalog to False for stores other than Netherland (discuss which store needs to be configured this way)
UPDATE [dbo].[ApplicationSetting] SET [Value] = 'FALSE' 
WHERE [Key] = 'PRODUCT.Single_Consolidated_Catalog'
and [StoreId] in (SELECT StoreId FROM Store WHERE [Name] not in ('Netherland'))
