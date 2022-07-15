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
	INSERT INTO
		[dbo].[ApplicationSetting]
			([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
		VALUES (
			N'SALESORDER.ValidateOrderPrice',
			N'FALSE',
			N'Validate Order Price',
			N'Sales Order',
			100,
			1,
			@StoreId,
			CAST(GETDATE() AS DateTime2),
			NULL,
			1,
			N'System',
			null)
	--------------------------------

    DELETE #ControlTable
    WHERE StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------
