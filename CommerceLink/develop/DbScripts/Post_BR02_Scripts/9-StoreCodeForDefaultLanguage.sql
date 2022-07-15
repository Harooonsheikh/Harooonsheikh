Declare @StoreId int = (select StoreId from Store where [Name] = 'Netherland')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES (N'en_nl', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Belgium')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_be', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Estonia')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_ee', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Portugal')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_pt', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Malta')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_mt', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Bulgaria')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_bg', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Albania')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_al', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO


Declare @StoreId int = (select StoreId from Store where [Name] = 'South Korea')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_kr', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Japan')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_jp', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Puerto Rico')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_pr', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Costa Rica')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_cr', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Panama')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_pa', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Dominic Republic')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_do', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Uruguay')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_uy', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Ecuador')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_ec', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'Poland')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_pl', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO

Declare @StoreId int = (select StoreId from Store where [Name] = 'China')

INSERT [dbo].[ConfigurableObject] ([ComValue], [ErpValue], [EntityType], [ConnectorKey], [StoreId], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
VALUES (N'en_cn', N'en-US', 7, NULL, @StoreId, '', GETUTCDATE(), N'System', NULL, NULL)
GO