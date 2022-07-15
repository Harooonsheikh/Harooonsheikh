
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
IF NOT EXISTS (Select * FROM ApplicationSetting WHERE [Key] = 'STORE.DISABLE_CACHE' AND StoreId = @StoreId)
BEGIN
	INSERT [dbo].[ApplicationSetting]
		([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [FieldTypeId]) 
	VALUES -- Value should be comma-separated Custom Attribute Keys
		( N'STORE.DISABLE_CACHE', 'FALSE', N'Disable AppSettings Cache', N'Store', 1, 1, @StoreId, 1)
END
--------------------------------

    DELETE	#ControlTable
    WHERE	StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------


