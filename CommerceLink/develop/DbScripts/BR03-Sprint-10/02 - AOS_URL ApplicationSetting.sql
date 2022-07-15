INSERT INTO
	[dbo].[ApplicationSetting]
		([Key],[Value],[Name],[ScreenName],[SortOrder],[IsActive],[StoreId],[CreatedOn],[ModifiedOn],[FieldTypeId],[CreatedBy],[ModifiedBy],[IsUserForDuplicateStore])
SELECT
        'APPLICATION.AOS_Url'
        ,'https://tv-d365o-int-01aos.sandbox.ax.dynamics.com'
        ,'AOS Service URL'
        ,'D365FO Settings'
        ,100
        ,1
        ,StoreId
        ,GETDATE()
        ,GETDATE()
        ,4
        ,1
        ,1
        ,0
FROM Store