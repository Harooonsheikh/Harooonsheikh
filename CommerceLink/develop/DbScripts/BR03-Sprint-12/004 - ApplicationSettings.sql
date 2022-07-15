

SELECT	StoreId
INTO	#ControlTable 
FROM	dbo.Store

DECLARE @StoreId INT

WHILE EXISTS (SELECT * FROM #ControlTable)
BEGIN

    SELECT	@StoreId = (	SELECT		TOP 1 StoreId
							FROM		#ControlTable
							ORDER BY	StoreId ASC)

-- Run for every store
-------------------------------------------------------------
IF NOT EXISTS (Select * FROM ApplicationSetting WHERE [Key] = 'PRODUCT.Enable_Catalog_Delta' AND StoreId = @StoreId)
BEGIN
	INSERT [dbo].[ApplicationSetting]
		([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [FieldTypeId]) 
	VALUES
		( N'PRODUCT.Enable_Catalog_Delta', 'FALSE', N'Enable Catalog Delta', N'Product', 1, 1, @StoreId, 1)
END

IF NOT EXISTS (Select * FROM ApplicationSetting WHERE [Key] = 'PRODUCT.Enable_Region_Catalog' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[ApplicationSetting]
	([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [FieldTypeId]) 
VALUES
	( N'PRODUCT.Enable_Region_Catalog', 'FALSE', N'Region Wise Catalog', N'Product', 1, 1, @StoreId, 1)
END

IF NOT EXISTS (Select * FROM ApplicationSetting WHERE [Key] = 'PRODUCT.Catalog_Delta_Batch_Size' AND StoreId = @StoreId)
BEGIN
INSERT [dbo].[ApplicationSetting]
	([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [FieldTypeId]) 
VALUES
	( N'PRODUCT.Catalog_Delta_Batch_Size', '5000', N'Catalog Delta Batch Size', N'Product', 1, 1, @StoreId, 1)
END

--------------------------------

    DELETE	#ControlTable
    WHERE	StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------

