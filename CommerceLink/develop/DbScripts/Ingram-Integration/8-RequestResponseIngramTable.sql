CREATE TABLE [dbo].[RequestResponseIngram](
	[RequestResponseIngramId] [int] IDENTITY(1,1) NOT NULL,
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
	[TotalProcessingDuration] [nvarchar](max) NULL,
	[Url] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[RequestResponseIngramId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


