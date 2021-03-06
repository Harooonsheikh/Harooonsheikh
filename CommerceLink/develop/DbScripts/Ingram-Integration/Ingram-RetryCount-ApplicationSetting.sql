
declare @ingramStoreId int = (select StoreId from Store where name = 'Ingram')
IF NOT EXISTS (SELECT * FROM ApplicationSetting WHERE [Key] = 'INGRAM.Status_Update_Retry_Count' AND StoreId = @ingramStoreId)
BEGIN
INSERT INTO [dbo].[ApplicationSetting]
           ([Key]
           ,[Value]
           ,[Name]
           ,[ScreenName]
           ,[SortOrder]
           ,[IsActive]
           ,[StoreId]
           ,[CreatedOn]
           ,[ModifiedOn]
           ,[FieldTypeId]
           ,[CreatedBy]
           ,[ModifiedBy]
           ,[IsUserForDuplicateStore])
VALUES ('INGRAM.Status_Update_Retry_Count', '3',	'Ingram Status Update Retry Count',	'Sales Order',201,0,@ingramStoreId,GetDate(),NULL,	1,	NULL,	NULL,	0)
END

