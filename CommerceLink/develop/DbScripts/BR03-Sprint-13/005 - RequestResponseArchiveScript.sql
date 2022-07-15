BEGIN Try
BEGIN TRANSACTION T
IF EXISTS( SELECT		*
					FROM		[sys].[schemas] [s]
					INNER JOIN	[sys].[tables] [t] ON [s].[schema_id] = [t].[schema_id]
					INNER JOIN	[sys].[columns] [c] ON [t].[object_id] = [c].[object_id]
					WHERE		[s].[name] = 'dbo'
					AND			[t].[name] = 'RequestResponse')
BEGIN
		declare @date nvarchar(max)
		select  @date = FORMAT(GETDATE(), 'MMMddyyyy', 'en-US')
		declare @newTableName nvarchar(max) 
		select @newTableName = Concat('Archive_RequestResponse_',  @date)
		EXEC sp_rename 'RequestResponse', @newTableName
		PRINT Concat(@newTableName, ' table created successfully')

		CREATE TABLE [dbo].[RequestResponse](
			[RequestResponseId] [int] IDENTITY(1,1) PRIMARY KEY,
			[StoreId] [int] NOT NULL,
			[DataDirectionId] [int] NULL,
			[EcomTransactionId] [nvarchar](256) NULL,
			[ApplicationName] [nvarchar](50) NOT NULL,
			[MethodName] [nvarchar](max) NOT NULL,
			[Description] [nvarchar](max) NULL,
			[DataPacket] [nvarchar](max) NULL,
			[CreatedOn] [datetime] NOT NULL,
			[CreatedBy] [nvarchar](50) NULL,
			[RequestInitiatedIP] [nvarchar](50) NULL,
			[OutputPacket] [nvarchar](max) NULL,
			[OutputSentAt] [datetime] NULL,
			[IdentifierKey] [nvarchar](max) NULL,
			[IdentifierValue] [nvarchar](max) NULL,
			[IsSuccess] [bit] NULL,
			[TotalProcessingDuration] decimal(20,5) NOT NULL DEFAULT 0
		)
		PRINT  'RequestResponse table re-created successfully'
END
ELSE
BEGIN
PRINT 'RequestResponse table does not exists.';
END
COMMIT TRANSACTION T 
End Try
BEGIN CATCH
IF (@@TRANCOUNT) > 0 
BEGIN
ROLLBACK TRANSACTION T
PRINT Concat (ERROR_MESSAGE() , ' ROLLBACK')
END
ELSE
BEGIN
PRINT 'No, Transaction Happend'   
END
END CATCH
