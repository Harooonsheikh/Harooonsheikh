
BEGIN Try
BEGIN TRANSACTION T
delete from  ApplicationSetting where [Key] = 'PRODUCT.Log_Data'
SELECT StoreId Into #TempStore
FROM Store
WHERE IsActive = 1
declare @storeId int
while exists (select * from #TempStore)
Begin
	select top 1 @storeId = StoreId
    from #TempStore
IF not exists (select * from [dbo].[ApplicationSetting] where StoreId = @storeId and [Key] = 'PRODUCT.Log_Data')   
BEGIN
	INSERT [dbo].[ApplicationSetting] ([Key], [Value], [Name], [ScreenName], [SortOrder], [IsActive], [StoreId], [CreatedOn], [ModifiedOn], [FieldTypeId], [CreatedBy], [ModifiedBy]) 
								VALUES ( N'PRODUCT.Log_Data', N'TRUE', N'Catalog', N'Product', 1, 1, @storeId,  CAST(N'2019-11-09' AS DateTime2), NULL, 1, N'1', N'1')

PRINT CONCAT('Log_Data Application Setting added against StoreId :' , @storeId);
END
ELSE
BEGIN
PRINT CONCAT('Log_Data Application Setting already exists against StoreId :' , @storeId);
END
	delete from #TempStore where StoreId = @storeId
End
drop table #TempStore
COMMIT TRANSACTION T 
End Try
BEGIN CATCH
IF (@@TRANCOUNT) > 0 
BEGIN
PRINT 'ROLLBACK'
ROLLBACK TRANSACTION T
END
ELSE
BEGIN
PRINT 'No, Transaction Happend'   
END

END CATCH

