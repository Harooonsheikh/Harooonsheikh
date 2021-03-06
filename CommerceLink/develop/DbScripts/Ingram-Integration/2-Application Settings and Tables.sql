/* BEGIN: Create Application Settings for all stores */

SELECT StoreId
INTO #ControlTable 
FROM dbo.Store

DECLARE @StoreId INT

WHILE exists (SELECT * FROM #ControlTable)
BEGIN

    SELECT @StoreId = (SELECT TOP 1 StoreId
                       FROM #ControlTable
                       ORDER BY StoreId ASC)

	INSERT INTO
		[dbo].[ApplicationSetting]
			([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [IsUserForDuplicateStore]) 
		VALUES
			( 'INGRAM.API_URL', 'https://api.connect.cloud.im/public/v1/requests', 'Ingram API Endpoint', 'Sales Order', 180, 0, @StoreId, CAST(GETDATE() AS DateTime2), NULL, 1002, '0'),
			( 'INGRAM.API_Data_Limit', '1000', 'Ingram API Limit', 'Sales Order', 190, 0, @StoreId, CAST(GETDATE() AS DateTime2), NULL, 1, '0'),
			( 'INGRAM.API_Key', 'ApiKey SU-531-251-782:2f4c0100323dd86c7560d4863e3059ab5016c15e', 'Ingram API Key', 'Sales Order', 200, 0, @StoreId, CAST(GETDATE() AS DateTime2), NULL, 1, '0'),
			( 'SALESORDER.Is_Load_Sales_Order_From_DB', 'TRUE', 'Is load sales order from database', 'Sales Order', 210, 0, @StoreId, CAST(GETDATE() AS DateTime2), NULL, 2, '0'),
			( 'SALESORDER.Sales_Order_Processing_Thread_Count', '25', 'Sales order processing thread count', 'Sales Order', 220, 0, @StoreId, CAST(GETDATE() AS DateTime2), NULL, 1, '0'),
			( 'CUSTOMER.Is_Ecom_Id_String', 'TRUE', 'Is Ecom Id String', 'Sales Order', 222, 0, @StoreId, CAST(GETDATE() AS DateTime2), NULL, 2, '0'),
			( 'SALESORDER.IsCreateResellerCustomer', 'TRUE', 'Is Ceate Reseller Customer', 'Sales Order', 221, 0, @StoreId, CAST(GETDATE() AS DateTime2), NULL, 2, '0')

    DELETE #ControlTable
    WHERE StoreId = @StoreId

END

DROP TABLE #ControlTable

/* END: Create Application Settings for all stores */

/* BEGIN: Update Data Direction Table with new values */

IF EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'SALESORDER.IsCreateResellerCustomer')
BEGIN
	IF NOT EXISTS (SELECT * FROM [dbo].[DataDirection] WHERE DataDirectionName = 'CLRequestToThirdParty')
	BEGIN
		INSERT INTO [dbo].[DataDirection](DataDirectionId, DataDirectionName, IsActive)
		VALUES (3, 'CLRequestToThirdParty', 1)
	END
END

IF EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'SALESORDER.IsCreateResellerCustomer')
BEGIN
	IF NOT EXISTS (SELECT * FROM [dbo].[DataDirection] WHERE DataDirectionName = 'ThirdPartyResponseToCL')
	BEGIN
		INSERT INTO [dbo].[DataDirection](DataDirectionId, DataDirectionName, IsActive)
		VALUES (4, 'ThirdPartyResponseToCL', 1)
	END
END

/* END: Update Data Direction Table with new values */

/* BEGIN: Create Third Party Tables */

CREATE TABLE [dbo].[ThirdPartyMessage](
	[ThirdPartyMessageId] [bigint] IDENTITY(1,1) NOT NULL,
	[EntityId] [int] NULL,
	[ThirdPartyId] [nvarchar](50) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[ThirdPartyStatus] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[TransactionStatus] [int] NOT NULL,
	[DestinationStoreKey] [nvarchar](max) NULL,
 CONSTRAINT [PK_ThirdPartyMessage] PRIMARY KEY CLUSTERED 
(
	[ThirdPartyMessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[ThirdPartyEnvironmentWithStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnvironmentName] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

INSERT INTO
	ThirdPartyEnvironmentWithStatus
		(EnvironmentName, [Status], IsActive)
	VALUES
		('preview','pending',1),
		('preview','inquiring',1),
		('preview','change',0),
		('production','pending',0),
		('production','inquiring',0),
		('production','change',0),
		('test','pending',0),
		('test','inquiring',0),
		('test','change',0);

IF NOT EXISTS (	SELECT	*
				FROM	INFORMATION_SCHEMA.COLUMNS
				WHERE	TABLE_NAME = 'RequestResponse'
				AND		COLUMN_NAME = 'Url')
BEGIN
	ALTER TABLE RequestResponse
		ADD Url	NVARCHAR(250)
END

IF NOT EXISTS (	SELECT	*
				FROM	INFORMATION_SCHEMA.COLUMNS
				WHERE	TABLE_NAME = 'Archive_RequestResponse'
				AND		COLUMN_NAME = 'Url')
BEGIN
	ALTER TABLE Archive_RequestResponse
		ADD Url	NVARCHAR(250)
END

/* END: Create Third Party Tables */