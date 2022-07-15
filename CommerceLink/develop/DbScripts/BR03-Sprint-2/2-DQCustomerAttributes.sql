SET NOCOUNT ON;
GO
SELECT StoreId
INTO #ControlTable 
FROM dbo.Store

DECLARE @StoreId INT
--Give the customer attribute RecId below as on D365 environment
DECLARE @TMVSanctionFlagRecId nvarchar(100) = '5637149076';
DECLARE @TMVSanctionStatusRecId nvarchar(100) = '5637149078';
DECLARE @TMVIsDuplicateUserRecId nvarchar(100) = '5637149077';
DECLARE @TMVDuplicateCustomerAccountNumberRecId nvarchar(100) = '5637149826';
--Give the customer attribute Data Type below as on D365 environment
DECLARE @TMVSanctionFlagDataType nvarchar(100) = '6';
DECLARE @TMVSanctionStatusDataType nvarchar(100) = '5';
DECLARE @TMVIsDuplicateUserDataType nvarchar(100) = '6';
DECLARE @TMVDuplicateCustomerAccountNumberDataType nvarchar(100) = '5';
--ApplicationSetting key names
DECLARE @ApplicationSettingKey1 nvarchar(300) = 'CUSTOMER.TMVDuplicateCustomerAccountNumber';
DECLARE @ApplicationSettingKey2 nvarchar(300) = 'CUSTOMER.TMVDuplicateCustomerAccountNumberDataType';
DECLARE @ApplicationSettingKey3 nvarchar(300) = 'CUSTOMER.TMVSanctionFlag';
DECLARE @ApplicationSettingKey4 nvarchar(300) = 'CUSTOMER.TMVSanctionStatus';
DECLARE @ApplicationSettingKey5 nvarchar(300) = 'CUSTOMER.TMVIsDuplicateUser';
DECLARE @ApplicationSettingKey6 nvarchar(300) = 'CUSTOMER.TMVSanctionFlagDataType';
DECLARE @ApplicationSettingKey7 nvarchar(300) = 'CUSTOMER.TMVSanctionStatusDataType';
DECLARE @ApplicationSettingKey8 nvarchar(300) = 'CUSTOMER.TMVIsDuplicateUserDataType';

WHILE exists (SELECT * FROM #ControlTable)
BEGIN

	SELECT @StoreId = (SELECT TOP 1 StoreId
					   FROM #ControlTable
					   ORDER BY StoreId ASC)

	--Run for every store
	-------------------------------------------------------------

	IF EXISTS(	SELECT	*
				FROM	ApplicationSetting
				WHERE   [Key] = @ApplicationSettingKey1
				AND		[StoreId] = @StoreId)
	BEGIN
		PRINT('Key ' + @ApplicationSettingKey1 + ' already inserted for Store Id: ' + CAST(@StoreId AS nvarchar));
	END
	ELSE
	BEGIN
		INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId],[FieldTypeId]) 
		VALUES (@ApplicationSettingKey1, @TMVDuplicateCustomerAccountNumberRecId, N'TMVDuplicateCustomerAccountNumber RecId', N'Customer', 1, 1, @StoreId, 1);
	END
	-------------------------------
	IF EXISTS(	SELECT	*
				FROM	ApplicationSetting
				WHERE   [Key] = @ApplicationSettingKey2
				AND		[StoreId] = @StoreId)
	BEGIN
		PRINT('Key ' + @ApplicationSettingKey2 + ' already inserted for Store Id: ' + CAST(@StoreId AS nvarchar));
	END
	ELSE
	BEGIN
		INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId],[FieldTypeId]) 
		VALUES (@ApplicationSettingKey2, @TMVDuplicateCustomerAccountNumberDataType, N'TMVDuplicateCustomerAccountNumber DataType', N'Customer', 1, 1, @StoreId, 1);
	END 	
	-------------------------------
	IF EXISTS(	SELECT	*
				FROM	ApplicationSetting
				WHERE   [Key] = @ApplicationSettingKey3
				AND		[StoreId] = @StoreId)
	BEGIN
		PRINT('Key ' + @ApplicationSettingKey3 + ' already inserted for Store Id: ' + CAST(@StoreId AS nvarchar));
	END
	ELSE
	BEGIN
		INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId],[FieldTypeId]) 
		VALUES (@ApplicationSettingKey3, @TMVSanctionFlagRecId, N'TMVSanctionFlag RecId', N'Customer', 1, 1, @StoreId, 1);
	END 
	-------------------------------
	IF EXISTS(	SELECT	*
				FROM	ApplicationSetting
				WHERE   [Key] = @ApplicationSettingKey4
				AND		[StoreId] = @StoreId)
	BEGIN
		PRINT('Key ' + @ApplicationSettingKey4 + ' already inserted for Store Id: ' + CAST(@StoreId AS nvarchar));
	END
	ELSE
	BEGIN
		INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId],[FieldTypeId]) 
		VALUES (@ApplicationSettingKey4, @TMVSanctionStatusRecId, N'TMVSanctionStatus RecId', N'Customer', 1, 1, @StoreId, 1);
	END 
	-------------------------------
	IF EXISTS(	SELECT	*
				FROM	ApplicationSetting
				WHERE   [Key] = @ApplicationSettingKey5
				AND		[StoreId] = @StoreId)
	BEGIN
		PRINT('Key ' + @ApplicationSettingKey5 + ' already inserted for Store Id: ' + CAST(@StoreId AS nvarchar));
	END
	ELSE
	BEGIN
		INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId],[FieldTypeId]) 
		VALUES (@ApplicationSettingKey5, @TMVIsDuplicateUserRecId, N'TMVIsDuplicateUser RecId', N'Customer', 1, 1, @StoreId, 1);
	END 
	-------------------------------
	IF EXISTS(	SELECT	*
				FROM	ApplicationSetting
				WHERE   [Key] = @ApplicationSettingKey6
				AND		[StoreId] = @StoreId)
	BEGIN
		PRINT('Key ' + @ApplicationSettingKey6 + ' already inserted for Store Id: ' + CAST(@StoreId AS nvarchar));
	END
	ELSE
	BEGIN
		INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId],[FieldTypeId]) 
		VALUES (@ApplicationSettingKey6, @TMVSanctionFlagDataType, N'TMVSanctionFlag DataType', N'Customer', 1, 1, @StoreId, 1);
	END 
	-------------------------------
	IF EXISTS(	SELECT	*
				FROM	ApplicationSetting
				WHERE   [Key] = @ApplicationSettingKey7
				AND		[StoreId] = @StoreId)
	BEGIN
		PRINT('Key ' + @ApplicationSettingKey7 + ' already inserted for Store Id: ' + CAST(@StoreId AS nvarchar));
	END
	ELSE
	BEGIN
		INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId],[FieldTypeId]) 
		VALUES (@ApplicationSettingKey7, @TMVSanctionStatusDataType, N'TMVSanctionStatus DataType', N'Customer', 1, 1, @StoreId, 1);
	END 
	-------------------------------
	IF EXISTS(	SELECT	*
				FROM	ApplicationSetting
				WHERE   [Key] = @ApplicationSettingKey8
				AND		[StoreId] = @StoreId)
	BEGIN
		PRINT('Key ' + @ApplicationSettingKey8 + ' already inserted for Store Id: ' + CAST(@StoreId AS nvarchar));
	END
	ELSE
	BEGIN
		INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId],[FieldTypeId]) 
		VALUES (@ApplicationSettingKey8, @TMVIsDuplicateUserDataType, N'TMVIsDuplicateUser DataType', N'Customer', 1, 1, @StoreId, 1);
	END 
	-------------------------------------------------------------

	DELETE #ControlTable
	WHERE StoreId = @StoreId

END -- END WHILE

DROP TABLE #ControlTable

GO
--------------------------------------------------------------------------------------------------------------------