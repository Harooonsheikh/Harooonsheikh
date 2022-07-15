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
VALUES ( N'APPLICATION.Mongo_Connection', N'mongodb://localhost:27017', N'Mongo DB Host', N'Mongo DB Setting', 0, 1, @StoreId, CAST(N'2019-05-14T13:11:48.1900000' AS DateTime2),NULL, 1, N'system',null),
( N'APPLICATION.Mongo_DBName', N'EdgeAXCommerceLink', N'Mongo DB Name', N'Mongo DB Setting', 0, 1, @StoreId, CAST(N'2019-05-14T13:11:48.1900000' AS DateTime2), NULL, 1, N'system',null),
( N'APPLICATION.Mongo_ChunkSize', N'500', N'Mongo Chuck Size', N'Mongo Chunk Size', 0, 0, @StoreId, CAST(N'2019-05-14T13:11:48.1900000' AS DateTime2),NULL, 1, N'system',null)
--------------------------------

    DELETE #ControlTable
    WHERE StoreId = @StoreId

END

DROP TABLE #ControlTable

-------------------------------------------------------------
