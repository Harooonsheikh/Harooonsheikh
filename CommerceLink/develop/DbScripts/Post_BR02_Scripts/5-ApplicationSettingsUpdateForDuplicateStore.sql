--Adding new column in ApplicationSettings table of type BIT with default values set to 0

ALTER TABLE [dbo].[ApplicationSetting] ADD IsUserForDuplicateStore BIT NOT NULL DEFAULT 0;
GO



--Updating selective rows of ApplicationSettings Table for every store
UPDATE [dbo].[ApplicationSetting] set IsUserForDuplicateStore = 1 where [Key] = 'APPLICATION.ERP_AX_OUN'
GO
UPDATE [dbo].[ApplicationSetting] set IsUserForDuplicateStore = 1 where [Key] = 'APPLICATION.Enviornment'
GO
UPDATE [dbo].[ApplicationSetting] set IsUserForDuplicateStore = 1 where [Key] = 'APPLICATION.Local_Base_Path'
GO
UPDATE [dbo].[ApplicationSetting] set IsUserForDuplicateStore = 1 where [Key] = 'APPLICATION.Remote_Base_Path'
GO
UPDATE [dbo].[ApplicationSetting] set IsUserForDuplicateStore = 1 where [Key] = 'CUSTOMER.Default_ThreeLetterISORegionNamet'
GO
UPDATE [dbo].[ApplicationSetting] set IsUserForDuplicateStore = 1 where [Key] = 'APPLICATION.Default_Currency_Code'
GO