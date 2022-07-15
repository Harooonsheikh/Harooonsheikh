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
VALUES ( N'APPLICATION.ERP_Default_Customer', N'', N'Default Customer', N'ERP Settings', 1, 1, @StoreId, CAST(N'2018-11-16T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')

INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
VALUES ( N'SALESORDER.Use_Default_Customer', N'FALSE', N'Use Default Customer', N'Sales Order', 1, 1, @StoreId, CAST(N'2018-11-16T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')
--------------------------------

    DELETE #ControlTable
    WHERE StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------

-- Set default customer id for Apple App online store only
UPDATE [dbo].[ApplicationSetting] SET [Value] = '20002390' 
WHERE [Key] = 'APPLICATION.ERP_Default_Customer'
and [StoreId] = (SELECT StoreId FROM Store WHERE [Name] = 'Apple App')
-------------------------------------------------------------
-- Update use default customer key to true for Apple App store only
UPDATE [dbo].[ApplicationSetting] SET [Value] = 'TRUE' 
WHERE [Key] = 'SALESORDER.Use_Default_Customer'
and [StoreId] = (SELECT StoreId FROM Store WHERE [Name] = 'Apple App')