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
VALUES ( N'PAYMENTCONNECTOR.Default_Locale', N'en-US', N'Default Locale', N'Payment Connector', 1, 1, @StoreId, CAST(N'2018-03-09T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')

INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
VALUES ( N'PAYMENTCONNECTOR.PayPal_Ecom_Value', N'PAYPAL_EXPRESS', N'PayPal Ecom Value', N'Payment Connector', 1, 1, @StoreId, CAST(N'2018-03-09T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')

INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
VALUES ( N'PAYMENTCONNECTOR.Supported_Card_Types', N'Visa;MasterCard;Amex;Discover;Debit;Jcb;DinersClub', N'Supported Card Types', N'Payment Connector', 1, 1, @StoreId, CAST(N'2018-03-09T13:11:48.1900000' AS DateTime2), NULL, 1, N'1', N'1')

--------------------------------

    DELETE #ControlTable
    WHERE StoreId = @StoreId

END

DROP TABLE #ControlTable
