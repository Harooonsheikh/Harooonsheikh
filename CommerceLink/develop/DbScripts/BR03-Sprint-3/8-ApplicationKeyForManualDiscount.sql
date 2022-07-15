SELECT StoreId
INTO #ControlTable 
FROM dbo.Store

DECLARE @StoreId INT

WHILE exists (SELECT * FROM #ControlTable)
BEGIN

    SELECT @StoreId = (SELECT TOP 1 StoreId
                       FROM #ControlTable
                       ORDER BY StoreId ASC)

	IF NOT exists( SELECT ApplicationSettingId FROM ApplicationSetting WHERE StoreId = @StoreId AND [Key] = 'SALESORDER.TMV_ManualDiscountReasonCode')
	BEGIN
	-- Run for every store
	--------------------------------
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
	VALUES ( N'SALESORDER.TMV_ManualDiscountReasonCode', N'20', N'Sales Order Manual Discount Reason Code', N'ERP Settings', 1, 1, @StoreId, CAST(N'2019-04-01T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')
	--------------------------------
	END
    
	DELETE #ControlTable
    WHERE StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------
