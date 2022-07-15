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
IF NOT EXISTS (Select * FROM ApplicationSetting WHERE [Key] = 'APPLICATION.Ingram_Default_Currency_Code' AND StoreId = @StoreId)
BEGIN
	DECLARE @Currency nvarchar(10)
	select @Currency = [Value] from ApplicationSetting where [Key] = 'APPLICATION.Default_Currency_Code' AND StoreId = @StoreId
	
	INSERT [dbo].[ApplicationSetting]
		([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [CreatedBy], [FieldTypeId]) 
	VALUES 
		( N'APPLICATION.Ingram_Default_Currency_Code', @Currency, N'Ingram Default Currency Code', N'ERP Settings', 1, 1, @StoreId, GETUTCDATE(), N'System', 1)
END
--------------------------------

    DELETE	#ControlTable
    WHERE	StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------


declare @storeid INT
select @storeid = StoreId from store where name = 'brazil'

UPDATE APPLICATIONSETTING
SET
	[VALUE] = 'USD'
WHERE [KEY] = 'APPLICATION.Ingram_Default_Currency_Code' AND STOREID = @STOREID

SELECT * FROM APPLICATIONSETTING WHERE [KEY] = 'APPLICATION.Ingram_Default_Currency_Code' AND STOREID = @STOREID




