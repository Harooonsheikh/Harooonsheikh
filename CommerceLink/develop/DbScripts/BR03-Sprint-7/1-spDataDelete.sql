CREATE TABLE [dbo].[DataDelete](
	[DataDeleteId] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestId] [nvarchar](50) NOT NULL,
	[CustomerAccountNumber] [nvarchar](50) NOT NULL,
	[ContactPersonEmail] [nvarchar](50) NOT NULL,
	[ContactPersonId] [nvarchar](50) NULL,
	[Status] [smallint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_DataDelete] PRIMARY KEY CLUSTERED 
(
	[DataDeleteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE PROCEDURE spDataDelete
	@DataDeleteId bigint, 
	@RequestId nvarchar(50),
	@CustomerAccountNumber nvarchar(50),
	@ContactPersonEmail varchar(50) 
AS
BEGIN     
	DELETE FROM RequestResponse
	WHERE MethodName IN ('UpdateContactPerson', 'CreateContactPerson', 'SaveContactPerson', 'MergeCreateCustomerContactPerson', 'MergeUpdateCustomerContactPerson')
	AND DataPacket LIKE '%' + @ContactPersonEmail + '%'
    
	DELETE FROM Archive_RequestResponse
	WHERE MethodName IN ('UpdateContactPerson', 'CreateContactPerson', 'SaveContactPerson', 'MergeCreateCustomerContactPerson', 'MergeUpdateCustomerContactPerson')
	AND DataPacket LIKE '%' + @ContactPersonEmail + '%'

	UPDATE DataDelete
	SET
		Status = 2
	WHERE  DataDeleteId = @DataDeleteId

	SELECT 1 AS Status
END   
GO