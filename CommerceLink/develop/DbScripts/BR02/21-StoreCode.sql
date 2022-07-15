Declare @StoreId int = (select StoreId from Store where [Name] = 'Netherland')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (N'nl_nl', N'nl', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Belgium')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'nl_be', N'nl-BE', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'fr_be', N'fr-BE', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Portugal')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'pt_pt', N'pt-PT', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
GO


Declare @StoreId int = (select StoreId from Store where [Name] = 'South Korea')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'ko_kr', N'ko', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Japan')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'ja_jp', N'ja', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Puerto Rico')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'es_pr', N'es', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Costa Rica')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'es_cr', N'es', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Panama')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'es_pa', N'es', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Dominic Republic')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'es_do', N'es', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Uruguay')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'es_uy', N'es', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Ecuador')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'es_ec', N'es', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Poland')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'pl_pl', N'pl', 7, NULL, @StoreId, '', CAST(N'2018-03-19T16:03:59.3433333' AS DateTime2), N'System', NULL, NULL)
GO
