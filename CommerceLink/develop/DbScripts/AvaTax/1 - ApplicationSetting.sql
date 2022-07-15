SELECT	StoreId
INTO	#ControlTable 
FROM	dbo.Store

DECLARE @StoreId INT
DECLARE @IsAvaTaxEnabled NVARCHAR(20)

WHILE EXISTS (SELECT * FROM #ControlTable)
BEGIN

    SELECT	@StoreId = (	SELECT		TOP 1 StoreId
							FROM		#ControlTable
							ORDER BY	StoreId ASC)

	SET @IsAvaTaxEnabled = (CASE WHEN (SELECT STOREID FROM dbo.Store WHERE StoreId = @StoreId AND Name IN ('United States', 'Canada')) IS NOT NULL THEN 'TRUE' ELSE 'FALSE' END)
-- Run for every store
-------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM [dbo].[ApplicationSetting] WHERE STOREID = @STOREID AND [KEY] = 'PRODUCT.IsAvaTaxEnabled')
BEGIN
	INSERT [dbo].[ApplicationSetting]
		([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [FieldTypeId]) 
	VALUES
		( N'PRODUCT.IsAvaTaxEnabled', @IsAvaTaxEnabled, N'Is AvaTax Enabled', N'Product', 223, 1, @StoreId, 2)
END

IF @IsAvaTaxEnabled = 'TRUE'
BEGIN
	UPDATE ConfigurableObject SET ErpValue = 'AVATAX' WHERE StoreId = @StoreId AND ComValue = 'SalesTaxGroup'
	UPDATE ConfigurableObject SET ErpValue = 'FULL' WHERE StoreId = @StoreId AND ComValue = 'ItemTaxGroup'
END

--------------------------------

    DELETE	#ControlTable
    WHERE	StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------