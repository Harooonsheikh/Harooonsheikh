-- Repeat for every store
Declare @StoreId int = 1;

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (N'pt_pt', N'pt-PT', 6, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (N'ja_jp', N'ja', 6, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (N'ko_kr', N'ko', 6, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)

-- Delete PurchaseOrder/Invoice from South Korea and Japan if it exists
DELETE FROM [dbo].[PaymentMethod] where [ECommerceValue] = 'PURCHASEORDER' and StoreId in (select StoreId from Store where [Name] in ('South Korea', 'Japan'))

-- Delete PayPal from South Korea if it exists
DELETE FROM [dbo].[PaymentMethod] where [ECommerceValue] = 'PAYPAL_EXPRESS' and StoreId in (select StoreId from Store where [Name] in ('South Korea'))

-- Add Port number and Time Out for SFTP
INSERT [dbo].[ApplicationSetting] ( [Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [FieldTypeId], [CreatedBy]) VALUES ( N'ECOM.Remote_SFTP_Port', N'22', N'SFTP Port', N'SFTP Settings', 60, 1, @StoreId,  1, N'system');
INSERT [dbo].[ApplicationSetting] ( [Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [FieldTypeId], [CreatedBy]) VALUES ( N'ECOM.Remote_SFTP_Time_Out', N'60', N'SFTP Time Out', N'SFTP Settings', 70, 1, @StoreId,  1, N'system');

