BEGIN Try
BEGIN TRANSACTION T
IF EXISTS( SELECT		*
					FROM		[sys].[schemas] [s]
					INNER JOIN	[sys].[tables] [t] ON [s].[schema_id] = [t].[schema_id]
					INNER JOIN	[sys].[columns] [c] ON [t].[object_id] = [c].[object_id]
					WHERE		[s].[name] = 'dbo'
					AND			[t].[name] = 'Log')
BEGIN
		declare @date nvarchar(max)
		select  @date = FORMAT(GETDATE(), 'MMMddyyyy', 'en-US')
		declare @newTableName nvarchar(max) 
		select @newTableName = Concat('Archive_Log_',  @date)
		EXEC sp_rename 'Log', @newTableName
		PRINT Concat(@newTableName, ' table created successfully')

CREATE TABLE [dbo].[Log](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[EventLevel] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[MachineName] [nvarchar](100) NOT NULL,
	[EventMessage] [nvarchar](max) NOT NULL,
	[ErrorSource] [nvarchar](3000) NULL,
	[ErrorClass] [nvarchar](500) NULL,
	[ErrorMethod] [nvarchar](max) NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[InnerErrorMessage] [nvarchar](max) NULL,
	[IdentityId] [varchar](50) NULL,
	[ErrorModule] [nvarchar](50) NULL,
	[StoreId] [int] NOT NULL,
 CONSTRAINT [PK_CLLog] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
		PRINT  'Log table re-created successfully'
END
ELSE
BEGIN
PRINT 'Log table does not exists.';
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
